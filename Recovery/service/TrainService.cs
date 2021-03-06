﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using NLog;
using Recovery.bean;
using Recovery.constant;
using Recovery.dao;
using Recovery.entity;
using Recovery.entity.newEntity;
using Recovery.util;
using Recovery.view.dto;

namespace Recovery.service
{
    class TrainService
    {
        static UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
        static NewDevicePrescriptionDAO devicePrescriptionDao = new NewDevicePrescriptionDAO();
        static TrainInfoDAO trainInfoDao = new TrainInfoDAO();
        static SymptomInfoDao symptomInfoDao = new SymptomInfoDao();
        static DevicePrescriptionDAO devicePrescriptionDAO = new DevicePrescriptionDAO();
        static PrescriptionResultDAO prescriptionResultDAO = new PrescriptionResultDAO();
        static PrescriptionResultTwoDao prescriptionResultTwoDao = new PrescriptionResultTwoDao();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 保存训练信息
        /// DevicePrescription
        /// </summary>
        /// <param name="trainInfo"></param>
        /// <param name="devicePrescriptions"></param>
        public void SaveTraininfo(object siId, TrainInfo trainInfo, List<NewDevicePrescription> devicePrescriptions)
        {
            using (TransactionScope ts = new TransactionScope()) //使整个代码块成为事务性代码
            {

                TrainInfo trainInfoFromDB = GetTrainInfoByUserIdAndStatus(trainInfo.FK_User_Id, trainInfo.Status);
                if (trainInfoFromDB != null)
                {
                    switch (trainInfo.Status)
                    {
                        case (int)TrainInfoStatus.Save:
                            //如果保存处方，删除原来的记录和关联的处方
                            trainInfoDao.DeleteByPrimaryKey(trainInfoFromDB);
                            devicePrescriptionDao.DeleteByTiId(trainInfoFromDB.Pk_TI_Id);
                            break;
                        case (int)TrainInfoStatus.Normal:
                            //如果是写卡后的插入，废弃原来的记录
                            trainInfoDao.UpdateStatusByUserId(trainInfo.FK_User_Id);


                            break;
                        case (int)TrainInfoStatus.Temp:
                            //如果是暂存状态的数据，废弃之前的该用户的暂存训练信息。在此处并不废弃已经有的nomal状态的信息，写卡成功了，才会废弃。
                            trainInfoDao.AbandonAllTempTrainInfo(trainInfo.FK_User_Id);

                            break;
                    }
                }

                //插入训练信息表
                int tiId = (int)trainInfoDao.Insert(trainInfo);
                //插入上传表
                uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo", 0));
                //插入症状信息，如果有症状信息的话
                if (trainInfo.Status == (int)TrainInfoStatus.Normal && siId != null)
                {
                    //如果是写卡，并且选了症状记录，改变症状表外键关联
                    var symptomInfo = symptomInfoDao.Load((int)siId);
                    symptomInfo.Fk_TI_Id = tiId;
                    symptomInfoDao.UpdateByPrimaryKey(symptomInfo);
                    //插入至上传表
                    uploadManagementDao.Insert(new UploadManagement(symptomInfo.Pk_SI_Id, "bdl_symptominfo", 1));

                }

                int dpId;
                //插入设备处方
                if (devicePrescriptions != null)
                {
                    foreach (NewDevicePrescription devicePrescription in devicePrescriptions)
                    {
                        devicePrescription.Fk_ti_id = tiId;
                        dpId = (int)devicePrescriptionDao.Insert(devicePrescription);
                        //插入至上传表
                        uploadManagementDao.Insert(new UploadManagement(dpId, "bdl_deviceprescription", 0));
                    }
                }

                ts.Complete();
            }
        }
        /// <summary>
        /// 根据用户id和状态查询训练信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public TrainInfo GetTrainInfoByUserIdAndStatus(int userId, int status)
        {
            List<TrainInfo> trainInfos = trainInfoDao.GetTrainInfoByUserIdAndStatus(userId, status);
            if (trainInfos == null || trainInfos.Count == 0)
            {
                return null;
            }
            else
            {
                return trainInfos[0];
            }
        }

        public List<TrainInfo> GetTrainInfosByUserId(int userId)
        {
            List<TrainInfo> trainInfos = trainInfoDao.GetTrainInfoByUserId(userId);
            if (trainInfos == null || trainInfos.Count == 0)
            {
                //Console.WriteLine("用户没有设置大处方信息");
                return null;
            }
            else
            {
                return trainInfos;
            }
        }
        public TrainInfo GetTrainInfoByUserId(int userId)
        {
            List<TrainInfo> trainInfos = trainInfoDao.GetTrainInfoByUserId(userId);
            if (trainInfos == null || trainInfos.Count == 0)
            {
                Console.WriteLine("用户没有设置大处方信息");
                return null;
            }
            else
            {
                return trainInfos[0];
            }
        }
        /// <summary>
        /// 添加训练结果，设备上报上来的结果
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="result"></param>
        public void AddPrescriptionResult(string idCard, PrescriptionResult result, DeviceType deviceType)
        {
            using (TransactionScope ts = new TransactionScope()) //使整个代码块成为事务性代码
            {
                DevicePrescription p = GetDevicePrescriptionByIdCardDeviceType(idCard, deviceType, (byte)DevicePrescription.UNDO);
                if (p == null)
                {
                    return;
                }
                //插入训练结果
                result.Fk_DP_Id = p.Pk_DP_Id;
                if (p.Gmt_Modified != null && result.Gmt_Create != null)
                {
                    TimeSpan ts0 = (DateTime)result.Gmt_Create - (DateTime)p.Gmt_Modified;
                    result.PR_Time2 = ts0.TotalSeconds;
                }

                prescriptionResultDAO.Insert(result);
                //插入至上传表
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                uploadManagementDao.Insert(new UploadManagement(result.Fk_DP_Id, "bdl_prescriptionresult", 0));
                //查询是否还有没完成的训练处方，如果都完成了就更新TrinInfo
                var unDoItemList = devicePrescriptionDAO.ListUnDoByTIId(p.Fk_TI_Id);
                if (unDoItemList.Count == 1)
                {
                    if (unDoItemList[0].Pk_DP_Id == p.Pk_DP_Id)//未完成的项目恰好是一个且为上报上来的这个项目就说明该大处方已经完成了，更新状态
                    {
                        var t = trainInfoDao.Load(p.Fk_TI_Id);
                        t.Pk_TI_Id = p.Fk_TI_Id;
                        t.Status = (byte)TrainInfoStatus.Finish;
                        trainInfoDao.UpdateByPrimaryKey(t);
                        //插入至上传表
                        UploadManagementDAO uploadManagementDao1 = new UploadManagementDAO();
                        uploadManagementDao.Insert(new UploadManagement(t.FK_User_Id, "bdl_traininfo", 1));
                    }
                }

                //更新该设备处方已完成状态
                p.Dp_status = DevicePrescription.DOWN;
                devicePrescriptionDAO.UpdateByPrimaryKey(p);
                //插入至上传表
                UploadManagementDAO uploadManagementDao2 = new UploadManagementDAO();
                uploadManagementDao.Insert(new UploadManagement(p.Pk_DP_Id, "bdl_deviceprescription", 1));

                ts.Complete();
            }
        }

        /// <summary>
        /// 添加训练结果，设备上报上来的结果，zfc
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        public void InsertPrescriptionResult(UploadRequest request, entity.newEntity.PrescriptionResultTwo entity)
        {

            //插入处方结果信息
            if (entity != null)
            {
                try
                {
                    long a = prescriptionResultTwoDao.Insert(entity);
                    if (a > 0)
                    {
                        logger.Info("插入训练结果成功");
                    }
                    else
                    {
                        logger.Error("插入训练结果失败");
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            using (TransactionScope ts = new TransactionScope()) //使整个代码块成为事务性代码
            {

                //查询是否还有没完成的训练处方，如果都完成了就更新TrinInfo
                var unDoDevicePrescription = devicePrescriptionDAO.ListUnDoByDpId(request.DpId);

                var unDoItemList = devicePrescriptionDAO.ListUnDoByTIId(unDoDevicePrescription[0].Fk_TI_Id);
                logger.Info("未做的小处方的数量:" + unDoItemList.Count);
                if (unDoItemList.Count == 1)
                {
                    if (unDoItemList[0].Pk_DP_Id == request.DpId)
                    {
                        //未完成的项目恰好是一个且为上报上来的这个项目就说明该大处方已经完成了，更新状态
                        trainInfoDao.UpdateTraninfoStatusByUserId(int.Parse(request.Uid));
                    }
                }
                else if(unDoItemList.Count == 0)
                {
                    logger.Info("数据存储出现错误");
                    return;
                }
                //更新处方表中的status
                devicePrescriptionDAO.updateDpStatus(request.DpId);

                ts.Complete();
            }
        }

        /// <summary>
        /// 添加训练结果，返回主键
        /// </summary>
        public void AddPrescriptionResult(object siId, TrainInfo trainInfo,
            Dictionary<DevicePrescription, PrescriptionResult> prescriptionDic)
        {
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
            PrescriptionResultDAO prescriptionResultDao = new PrescriptionResultDAO();

            using (TransactionScope ts = new TransactionScope()) //使整个代码块成为事务性代码
            {
                //插入训练信息和上传表
                int tiId = (int)new TrainInfoDAO().Insert(trainInfo);
                uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo", 0));
                if (siId != null)
                {
                    //改变症状表外键关联
                    var symptomInfo = symptomInfoDao.Load((int)siId);
                    symptomInfo.Fk_TI_Id = tiId;
                    symptomInfoDao.UpdateByPrimaryKey(symptomInfo);
                    //将更新信息插入至上传表
                    uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo", 1));
                }
                if (prescriptionDic != null)
                {
                    int dpId;
                    int prId;
                    foreach (KeyValuePair<DevicePrescription, PrescriptionResult> prescription in prescriptionDic)
                    {
                        DevicePrescription devicePrescription = prescription.Key;
                        PrescriptionResult prescriptionResult = prescription.Value;
                        //插入设备处方、上传表
                        devicePrescription.Fk_TI_Id = tiId;
                        dpId = (int)devicePrescriptionDao.Insert(devicePrescription);
                        uploadManagementDao.Insert(new UploadManagement(dpId, "bdl_deviceprescription", 0));

                        //插入设备处方结果、上传表
                        prescriptionResult.Fk_DP_Id = dpId;
                        prId = (int)prescriptionResultDao.Insert(prescriptionResult);
                        uploadManagementDao.Insert(new UploadManagement(prId, "bdl_prescriptionresult", 0));
                    }
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 添加训练结果，返回主键,lj
        /// </summary>
        public void AddPrescriptionResultTwo(object siId, TrainInfo trainInfo,
            Dictionary<NewDevicePrescription, PrescriptionResultTwo> prescriptionDic)
        {
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            NewDevicePrescriptionDAO devicePrescriptionDao = new NewDevicePrescriptionDAO();
            NewPrescriptionResultDAO prescriptionResultDao = new NewPrescriptionResultDAO();

            using (TransactionScope ts = new TransactionScope()) //使整个代码块成为事务性代码
            {
                //插入训练信息和上传表
                int tiId = (int)new TrainInfoDAO().Insert(trainInfo);
                uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo", 0));
                if (siId != null)
                {
                    //改变症状表外键关联
                    var symptomInfo = symptomInfoDao.Load((int)siId);
                    symptomInfo.Fk_TI_Id = tiId;
                    symptomInfoDao.UpdateByPrimaryKey(symptomInfo);
                    //将更新信息插入至上传表
                    uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo", 1));
                }
                if (prescriptionDic != null)
                {
                    int dpId;
                    int prId;
                    foreach (KeyValuePair<NewDevicePrescription, PrescriptionResultTwo> prescription in prescriptionDic)
                    {
                        NewDevicePrescription devicePrescription = prescription.Key;
                        PrescriptionResultTwo prescriptionResult = prescription.Value;
                        //插入设备处方、上传表
                        devicePrescription.Fk_ti_id = tiId;
                        dpId = (int)devicePrescriptionDao.Insert(devicePrescription);
                        uploadManagementDao.Insert(new UploadManagement(dpId, "bdl_deviceprescription", 0));

                        //插入设备处方结果、上传表
                        prescriptionResult.Fk_dp_id = dpId;
                        prId = (int)prescriptionResultDao.Insert(prescriptionResult);
                        uploadManagementDao.Insert(new UploadManagement(prId, "bdl_prescriptionresult", 0));
                    }
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 获取该用户保存的训练处方
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<NewDevicePrescription> GetSaveDevicePrescriptionsByUser(User user)
        {
            TrainInfo trainInfoFromDB = GetTrainInfoByUserIdAndStatus(user.Pk_User_Id, (int)TrainInfoStatus.Save);
            List<NewDevicePrescription> devicePrescriptions = null;
            if (trainInfoFromDB != null)
            {
                devicePrescriptions = new NewDevicePrescriptionDAO().GetByTIId(trainInfoFromDB.Pk_TI_Id);
            }

            return devicePrescriptions;
        }

        /// <summary>
        /// 根据用户身份证号查询未完成处方
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public List<DevicePrescription> ListUndoDevicePrescriptionByUserId(string idcard)
        {
            return new DevicePrescriptionDAO().ListUnDoByUserId(idcard);
        }


        /// <summary>
        /// 根据用户身份证号和设备类型查询处方,Normol状态的
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DevicePrescription GetDevicePrescriptionByIdCardDeviceType(string idcard, DeviceType deviceType, byte dp_status)
        {
            return new DevicePrescriptionDAO().GetByUserIdDeviceType(idcard, deviceType, dp_status);
        }

        /// <summary>
        /// 返回以完成的设备号
        /// </summary>
        /// <param name="tiid"></param>
        /// <returns></returns>
        public List<long> dscodelist(int tiid)
        {

            List<entity.newEntity.NewDevicePrescription> devicePrescriptions = new NewDevicePrescriptionDAO().findAllDevicePrescriptionByTiId(tiid);

            List<long> listdscoed = new List<long>();
            for (int i = 0; i < devicePrescriptions.Count; i++)
            {
                listdscoed.Add(devicePrescriptions[i].Fk_ds_id);
            }
            return listdscoed;
        }

        /// <summary>
        /// 返回未完成的设备号
        /// </summary>
        /// <param name="tiid"></param>
        /// <returns></returns>
        public List<long> dscodelistTwo(int tiid)
        {

            List<entity.newEntity.NewDevicePrescription> devicePrescriptions = new NewDevicePrescriptionDAO().findAllDevicePrescriptionByTiIdTwo(tiid);

            List<long> listdscoed = new List<long>();
            for (int i = 0; i < devicePrescriptions.Count; i++)
            {
                listdscoed.Add(devicePrescriptions[i].Fk_ds_id);
            }
            return listdscoed;
        }

        /// <summary>
        /// 根据大处方id和设备id获得处方
        /// </summary>
        /// <param name="tiid"></param>
        /// <param name="dsid"></param>
        /// <returns></returns>
        public entity.newEntity.NewDevicePrescription GetDevicePrescriptionByTiIdAndDsId(int tiid, int dsid)
        {
            return new NewDevicePrescriptionDAO().GetByTIIdAndDsID(tiid, dsid);
        }

        public Dictionary<int, List<TrainDTO>> getTrainDTOByUserA(User user)
        {
            TrainInfoDAO trainInfoDao = new TrainInfoDAO();
            DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
            PrescriptionResultDAO prescriptionResultDao = new PrescriptionResultDAO();
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();

            Dictionary<int, List<TrainDTO>> dic = new Dictionary<int, List<TrainDTO>>();

            //找到该用户训练记录(训练记录状态为0或2)
            List<TrainInfo> trainInfos = trainInfoDao.GetFinishOrNormalTrainInfoByUserId(user.Pk_User_Id);
            foreach (TrainInfo trainInfo in trainInfos)
            {
                //根据训练信息id查找处方
                List<DevicePrescription> devicePrescriptions = devicePrescriptionDao.GetByTIId(trainInfo.Pk_TI_Id);
                foreach (DevicePrescription devicePrescription in devicePrescriptions)
                {
                    int devId = devicePrescription.Fk_DS_Id;

                    //根据处方查找训练结果
                    PrescriptionResult prescriptionResult =
                        prescriptionResultDao.GetByDPId(devicePrescription.Pk_DP_Id);
                    if (prescriptionResult == null)
                    {
                        //如果没有训练结果，
                        continue;
                    }

                    //查找字典是否有以此设备名字命名的key,不存在则先创建
                    if (!dic.ContainsKey(devId))
                    {
                        List<TrainDTO> trainDtos = new List<TrainDTO>();
                        dic.Add(devId, trainDtos);
                    }
                    //                    Console.WriteLine(prescriptionResult.PR_Evaluate);
                    //PR_Evaluate 总是1？？？？
                    dic[devId].Add(new TrainDTO(trainInfo, devicePrescription, prescriptionResult));

                }
            }

            return dic;
            return null;
        }

        /// <summary>
        /// 主页面训练结果
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Dictionary<int, List<NewTrainDTO>> getNewTrainDTOByUserA(User user)
        {
            TrainInfoDAO trainInfoDao = new TrainInfoDAO();
            NewDevicePrescriptionDAO devicePrescriptionDao = new NewDevicePrescriptionDAO();
            NewPrescriptionResultDAO prescriptionResultDao = new NewPrescriptionResultDAO();
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();

            Dictionary<int, List<NewTrainDTO>> dic = new Dictionary<int, List<NewTrainDTO>>();

            //找到该用户训练记录(训练记录状态为0或2)
            List<TrainInfo> trainInfos = trainInfoDao.GetFinishOrNormalTrainInfoByUserId(user.Pk_User_Id);
            foreach (TrainInfo trainInfo in trainInfos)
            {
                //根据训练信息id查找处方
                List<NewDevicePrescription> devicePrescriptions = devicePrescriptionDao.GetByTIId(trainInfo.Pk_TI_Id);
                foreach (NewDevicePrescription devicePrescription in devicePrescriptions)
                {
                    int devId = Convert.ToInt32(devicePrescription.Fk_ds_id);

                    //根据处方查找训练结果
                    PrescriptionResultTwo prescriptionResult =
                        prescriptionResultDao.GetByDPId(Convert.ToInt32(devicePrescription.Pk_dp_id));
                    if (prescriptionResult == null)
                    {
                        //如果没有训练结果，
                        continue;
                    }
                    Console.WriteLine("prescriptionResult = " + prescriptionResult.Energy);
                    //查找字典是否有以此设备名字命名的key,不存在则先创建
                    if (!dic.ContainsKey(devId))
                    {
                        List<NewTrainDTO> trainDtos = new List<NewTrainDTO>();
                        dic.Add(devId, trainDtos);
                    }
                    //                    Console.WriteLine(prescriptionResult.PR_Evaluate);
                    //PR_Evaluate 总是1？？？？
                    dic[devId].Add(new NewTrainDTO(trainInfo, devicePrescription, prescriptionResult));

                }
            }

            return dic;
        }

        /// <summary>
        /// 根据用户id获取扩展类
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Dictionary<int, List<TrainDTO>> getTrainDTOByUser(User user)
        {
            TrainInfoDAO trainInfoDao = new TrainInfoDAO();
            DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
            PrescriptionResultDAO prescriptionResultDao = new PrescriptionResultDAO();
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();

            Dictionary<int, List<TrainDTO>> dic = new Dictionary<int, List<TrainDTO>>();

            //找到该用户所有已训练记录
            List<TrainInfo> trainInfos = trainInfoDao.GetFinishTrainInfoByUserId(user.Pk_User_Id);
            foreach (TrainInfo trainInfo in trainInfos)
            {
                //根据训练信息id查找处方
                List<DevicePrescription> devicePrescriptions = devicePrescriptionDao.GetByTIId(trainInfo.Pk_TI_Id);
                foreach (DevicePrescription devicePrescription in devicePrescriptions)
                {
                    int devId = devicePrescription.Fk_DS_Id;

                    //根据处方查找训练结果
                    PrescriptionResult prescriptionResult =
                        prescriptionResultDao.GetByDPId(devicePrescription.Pk_DP_Id);
                    if (prescriptionResult == null)
                    {
                        //如果没有训练结果，
                        continue;
                    }

                    //查找字典是否有以此设备名字命名的key,不存在则先创建
                    if (!dic.ContainsKey(devId))
                    {
                        List<TrainDTO> trainDtos = new List<TrainDTO>();
                        dic.Add(devId, trainDtos);
                    }
                    //                    Console.WriteLine(prescriptionResult.PR_Evaluate);
                    //PR_Evaluate 总是1？？？？
                    dic[devId].Add(new TrainDTO(trainInfo, devicePrescription, prescriptionResult));

                }
            }

            return dic;
        }

        /// <summary>
        /// 根据训练结果id获取扩展类
        /// </summary>
        /// <param name="prescriptionResultPkPrId"></param>
        public List<NewTrainDTO> GetTrainDTOByPRId(int prId)
        {
            NewDevicePrescriptionDAO devicePrescriptionDao = new NewDevicePrescriptionDAO();
            NewPrescriptionResultDAO prescriptionResultDao = new NewPrescriptionResultDAO();

            List<NewTrainDTO> trainDtos = new List<NewTrainDTO>();

            //根据训练结果id查询所在处方id
            int tiId = devicePrescriptionDao.GetTIIdByPRId(prId);

            //根据处方id查询处方+结果
            List<NewDevicePrescription> devicePrescriptions = devicePrescriptionDao.GetByTIId(tiId);
            foreach (NewDevicePrescription devicePrescription in devicePrescriptions)
            {
                //根据处方查找训练结果
                PrescriptionResultTwo prescriptionResult = prescriptionResultDao.GetByDPId(Convert.ToInt32(devicePrescription.Pk_dp_id));

                trainDtos.Add(new NewTrainDTO(null, devicePrescription, prescriptionResult));
            }

            return trainDtos;
        }
        /// <summary>
        /// 将某用户的暂存状态的信息设置为nomal状态，要处理好不是一条数据的情况
        /// </summary>
        /// <param name="pk_User_Id"></param>
        public void SetTmpToNomal(int pk_User_Id)
        {
            //此时，设置为nomal状态之前，先把某用户的之前的nomal状态的数据废弃，因为界面上已经同意了覆盖。写卡失败不会触发该操作。      
            trainInfoDao.UpdateStatusByUserId(pk_User_Id);
            //将该用户的暂存状态的信息设置为nomal状态
            trainInfoDao.SetTmpToNomal(pk_User_Id);
        }

    }
}