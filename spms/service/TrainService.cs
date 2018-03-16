using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
            int tiId;
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            {
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();
                TrainInfoDAO trainInfoDao = new TrainInfoDAO();

                //插入症状信息返回主键
                tiId = (int)trainInfoDao.Insert(trainInfo);

                //插入上传表
                uploadManagementDao.Insert(new UploadManagement(tiId, "bdl_traininfo"));

                int dsId;
                //插入设备处方
                if (devicePrescriptions != null)
                {
                    foreach (DevicePrescription devicePrescription in devicePrescriptions)
                    {
                        devicePrescription.Fk_TI_Id = tiId;
                        dsId = (int)devicePrescriptionDao.Insert(devicePrescription);
                        //插入至上传表
                        uploadManagementDao.Insert(new UploadManagement(dsId, "bdl_deviceprescription"));
                    }
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 添加训练结果，返回主键
        /// </summary>
        public void AddPrescriptionResult()
        {

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
    }
}
