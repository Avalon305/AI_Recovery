using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using spms.constant;
using spms.dao;
using spms.entity;
using spms.util;
using spms.view.dto;

namespace spms.service
{
    class TrainService
    {
        static UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
        static DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
        static TrainInfoDAO trainInfoDao = new TrainInfoDAO();

        /// <summary>
        /// 保存训练信息
        /// </summary>
        /// <param name="trainInfo"></param>
        /// <param name="devicePrescriptions"></param>
        public void SaveTraininfo(TrainInfo trainInfo, List<DevicePrescription> devicePrescriptions)
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
                            trainInfoFromDB.Status = (int)TrainInfoStatus.Abandon;
                            trainInfoDao.UpdateByPrimaryKey(trainInfoFromDB);
                            break;
                    }
                }

                //插入症状信息、插入上传表
                int tiId = (int)trainInfoDao.Insert(trainInfo);
                uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo"));

                int dpId;
                //插入设备处方
                if (devicePrescriptions != null)
                {
                    foreach (DevicePrescription devicePrescription in devicePrescriptions)
                    {
                        devicePrescription.Fk_TI_Id = tiId;
                        dpId = (int)devicePrescriptionDao.Insert(devicePrescription);
                        //插入至上传表
                        uploadManagementDao.Insert(new UploadManagement(dpId, "bdl_deviceprescription"));
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

        /// <summary>
        /// 添加训练结果，返回主键
        /// </summary>
        public void AddPrescriptionResult(TrainInfo trainInfo,
            Dictionary<DevicePrescription, PrescriptionResult> prescriptionDic)
        {
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
            PrescriptionResultDAO prescriptionResultDao = new PrescriptionResultDAO();

            using (TransactionScope ts = new TransactionScope()) //使整个代码块成为事务性代码
            {
                //插入训练信息和上传表
                int tiId = (int) new TrainInfoDAO().Insert(trainInfo);
                uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo"));

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
                        dpId = (int) devicePrescriptionDao.Insert(devicePrescription);
                        uploadManagementDao.Insert(new UploadManagement(dpId, "bdl_deviceprescription"));

                        //插入设备处方结果、上传表
                        prescriptionResult.Fk_DP_Id = dpId;
                        prId = (int) prescriptionResultDao.Insert(prescriptionResult);
                        uploadManagementDao.Insert(new UploadManagement(prId, "bdl_prescriptionresult"));
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
        public List<DevicePrescription> GetSaveDevicePrescriptionsByUser(User user)
        {
            TrainInfo trainInfoFromDB = GetTrainInfoByUserIdAndStatus(user.Pk_User_Id, (int)TrainInfoStatus.Save);
            List<DevicePrescription> devicePrescriptions = null;
            if (trainInfoFromDB != null)
            {
                devicePrescriptions = new DevicePrescriptionDAO().GetByTIId(trainInfoFromDB.Pk_TI_Id);
            }

            return devicePrescriptions;
        }

        /// <summary>
        /// 根据用户身份证号和设备类型查询处方
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DevicePrescription GetDevicePrescriptionByIdCardDeviceType(string idcard, DeviceType deviceType)
        {
            return new DevicePrescriptionDAO().GetByUserIdDeviceType(idcard, deviceType);
        }

        /// <summary>
        /// 根据用户id获取扩展类
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Dictionary<string, List<TrainDTO>> getTrainDTOByUser(User user)
        {
            TrainInfoDAO trainInfoDao = new TrainInfoDAO();
            DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
            PrescriptionResultDAO prescriptionResultDao = new PrescriptionResultDAO();
            DeviceSortDAO deviceSortDao = new DeviceSortDAO();

            Dictionary<string, List<TrainDTO>> dic = new Dictionary<string, List<TrainDTO>>();

            //找到该用户所有已训练记录
            List<TrainInfo> trainInfos = trainInfoDao.GetFinishTrainInfoByUserId(user.Pk_User_Id);
            foreach (TrainInfo trainInfo in trainInfos)
            {
                //根据训练信息id查找处方
                List<DevicePrescription> devicePrescriptions = devicePrescriptionDao.GetByTIId(trainInfo.Pk_TI_Id);
                foreach (DevicePrescription devicePrescription in devicePrescriptions)
                {
                    string devName = deviceSortDao.Load(devicePrescription.Fk_DS_Id).DS_name;
                    
                    //根据处方查找训练结果
                    PrescriptionResult prescriptionResult =
                        prescriptionResultDao.GetByDPId(devicePrescription.Pk_DP_Id);
                    if (prescriptionResult == null)
                    {
                        //如果没有训练结果，
                        continue;
                    }

                    //查找字典是否有以此设备名字命名的key,不存在则先创建
                    if (!dic.ContainsKey(devName))
                    {
                        List<TrainDTO> trainDtos = new List<TrainDTO>();
                        dic.Add(devName, trainDtos);
                    }
//                    Console.WriteLine(prescriptionResult.PR_Evaluate);
                    //PR_Evaluate 总是1？？？？
                    dic[devName].Add(new TrainDTO(trainInfo, devicePrescription, prescriptionResult));
                    
                }
            }
            
            return dic;
        }
        /// <summary>
        /// 根据训练结果id获取扩展类
        /// </summary>
        /// <param name="prescriptionResultPkPrId"></param>
        public List<TrainDTO> GetTrainDTOByPRId(int prId)
        {
            DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
            PrescriptionResultDAO prescriptionResultDao = new PrescriptionResultDAO();

            List<TrainDTO> trainDtos = new List<TrainDTO>();

            //根据训练结果id查询所在处方id
            int tiId = devicePrescriptionDao.GetTIIdByPRId(prId);

            //根据处方id查询处方+结果
            List<DevicePrescription> devicePrescriptions = devicePrescriptionDao.GetByTIId(tiId);
            foreach (DevicePrescription devicePrescription in devicePrescriptions)
            {
                //根据处方查找训练结果
                PrescriptionResult prescriptionResult = prescriptionResultDao.GetByDPId(devicePrescription.Pk_DP_Id);
                
                trainDtos.Add(new TrainDTO(null, devicePrescription, prescriptionResult));
            }

            return trainDtos;
        }
    }
}