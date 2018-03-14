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
        /**
         * 添加训练信息
         */
        public void AddTraininfo(TrainInfo trainInfo, List<DevicePrescription> devicePrescriptions)
        {
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            {
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                DevicePrescriptionDAO devicePrescriptionDao = new DevicePrescriptionDAO();

                //插入症状信息返回主键
                int id = (int)new TrainInfoDAO().Insert(trainInfo);

                int deviceId;
                uploadManagementDao.Insert(new UploadManagement(id, "bdl_traininfo"));
                //插入设备处方
                foreach (DevicePrescription devicePrescription in devicePrescriptions)
                {
                    devicePrescription.Fk_TI_Id = id;
                    deviceId = (int) devicePrescriptionDao.Insert(devicePrescription);
                    //插入至上传表
                    uploadManagementDao.Insert(new UploadManagement(deviceId, "bdl_deviceprescription"));
                }
                ts.Complete();
            }
        }
        
        /**
         * 添加训练结果，返回主键
         */
        public void AddPrescriptionResult()
        {
            
        }
    }
}
