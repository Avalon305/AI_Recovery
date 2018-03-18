using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using spms.constant;
using spms.dao;
using spms.entity;

namespace spms.service
{
    class TrainService
    {
        /// <summary>
        /// 添加训练信息
        /// </summary>
        /// <param name="trainInfo"></param>
        /// <param name="devicePrescriptions"></param>
        public void AddTraininfo(TrainInfo trainInfo, List<DevicePrescription> devicePrescriptions)
        {
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
            TrainInfoDAO trainInfoDao = new TrainInfoDAO();
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            {
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
                int tiId = (int)new TrainInfoDAO().Insert(trainInfo);
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
                        dpId = (int)devicePrescriptionDao.Insert(devicePrescription);
                        uploadManagementDao.Insert(new UploadManagement(dpId, "bdl_deviceprescription"));

                        //插入设备处方结果、上传表
                        prescriptionResult.Fk_DP_Id = dpId;
                        prId = (int)prescriptionResultDao.Insert(prescriptionResult);
                        uploadManagementDao.Insert(new UploadManagement(prId, "bdl_prescriptionresult"));
                    }
                }
                ts.Complete();
            }
        }


        /// <summary>
        /// 获取该用户最后一次训练处方
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DevicePrescription> GetLastDevicePrescriptionsByUser(User user)
        {
            TrainInfoDAO trainInfoDao = new TrainInfoDAO();
            TrainInfo trainInfoFromDB = trainInfoDao.GetLastByUserId(user.Pk_User_Id);
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
    }
}
