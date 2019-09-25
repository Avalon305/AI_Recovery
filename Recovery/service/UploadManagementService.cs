using Recovery.dao;
using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recovery.http.entity;
using Recovery.http.dto;
using Recovery.util;
using Recovery.constant;
using Recovery.dao.app;

namespace Recovery.service
{
    class UploadManagementService
    {
        /// <summary>
        /// 查询30条数据
        /// </summary>
        /// <returns></returns>
        public List<UploadManagement> ListLimit30()
        {
            //Console.WriteLine("ListLimit30()-执行");
            return new UploadManagementDAO().ListLimit30();
        }

        /// <summary>
        /// 根据主键ID删除
        /// </summary>
        /// <param name="id">主键</param>
        public void deleteByPrimaryKey(int id)
        {
            new UploadManagementDAO().DeleteByPrimaryKey(new UploadManagement(id));
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="uploadManagement">传入上传管理者实体</param>
        /// <returns>返回可以用来上传的辅助对象</returns>
        public ServiceResult GetServiceResult(UploadManagement uploadManagement)
        {
            //service返回结果对象
            ServiceResult serviceResult = new ServiceResult();
            //提前载入唯一Setter
            SetterDAO setterDAO = new SetterDAO();
            Setter setter = setterDAO.getSetter();
            //需要加入解密逻辑 TODO
            string mac = "";
            //try
            //{
            //    byte[] deBytes = AesUtil.Decrypt(Encoding.GetEncoding("GBK").GetBytes(setter.Set_Unique_Id),
            //        ProtocolConstant.USB_DOG_PASSWORD);
            //    mac = Encoding.GetEncoding("GBK").GetString(deBytes);
            //}
            //catch (Exception ex)
            //{
            //    mac = setter.Set_Unique_Id.Replace(":", "");
            //}
            byte[] a = ProtocolUtil.StringToBcd(setter.Set_Unique_Id);
            byte[] b = AesUtil.Decrypt(a, ProtocolConstant.USB_DOG_PASSWORD);
            mac = Encoding.GetEncoding("GBK").GetString(b).Replace(":", "-");

            ///if识别出表,设置发送路径，select出实体，转化至DTO，json打成string,返回
            //识别是否是权限用户添加
            if (uploadManagement.UM_DataTable == "bdl_auth")
            {
                AuthDAO authDAO = new AuthDAO();
                Auther auther = authDAO.Load(uploadManagement.UM_DataId);
                if (auther == null)
                {
                    return null;
                }
                AutherDTO autherDTO = new AutherDTO(setter, auther, mac);


                //serviceResult.URL = "clientController/addClient.action";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<AutherDTO>(autherDTO);
            }
            //bdl_customdata表
            else if (uploadManagement.UM_DataTable == "bdl_customdata")
            {
                CustomDataDAO customDataDAO = new CustomDataDAO();
                CustomData customData = customDataDAO.Load(uploadManagement.UM_DataId);
                if (customData == null)
                {
                    return null;
                }
                CustomDataDTO customDataDTO = new CustomDataDTO(customData, mac);
                serviceResult.Data = JsonTools.Obj2JSONStrNew<CustomDataDTO>(customDataDTO);

            }
            //bdl_datacode表
            else if (uploadManagement.UM_DataTable == "bdl_datacode")
            {
                DataCodeDAO dataCodedao = new DataCodeDAO();
                DataCode dataCode = dataCodedao.Load(uploadManagement.UM_DataId);

                if (dataCode == null)
                {
                    return null;
                }
                DataCodeDTO dataCodeDTO = new DataCodeDTO(dataCode, mac);
                serviceResult.Data = JsonTools.Obj2JSONStrNew<DataCodeDTO>(dataCodeDTO);
            }
            //bdl_deviceset表
            else if (uploadManagement.UM_DataTable == "bdl_deviceset")
            {
                DeviceSetDAO deviceSetDAO = new DeviceSetDAO();
                DeviceSet deviceSet = deviceSetDAO.Load(uploadManagement.UM_DataId);
                if (deviceSet == null)
                {
                    return null;
                }
                DeviceSetDTO deviceSetDTO = new DeviceSetDTO(deviceSet, mac);

                serviceResult.Data = JsonTools.Obj2JSONStrNew<DeviceSetDTO>(deviceSetDTO);
            }
            //bdl_devicesort表
            else if (uploadManagement.UM_DataTable == "bdl_devicesort")
            {
                DeviceSortDAO deviceSortDAO = new DeviceSortDAO();
                DeviceSort deviceSort = deviceSortDAO.Load(uploadManagement.UM_DataId);
                if (deviceSort == null)
                {
                    return null;
                }
                DeviceSortDTO deviceSortDTO = new DeviceSortDTO(deviceSort, mac);


                serviceResult.Data = JsonTools.Obj2JSONStrNew<DeviceSortDTO>(deviceSortDTO);
            }
            //bdl_onlinedevice表
            else if (uploadManagement.UM_DataTable == "bdl_onlinedevice")
            {
                OnlineDeviceDAO onlineDeviceDAO = new OnlineDeviceDAO();
                OnlineDevice onlineDevice = onlineDeviceDAO.Load(uploadManagement.UM_DataId);

                if (onlineDevice == null)
                {
                    return null;
                }
                OnlineDeviceDTO onlineDeviceDTO = new OnlineDeviceDTO(onlineDevice, mac);

                serviceResult.Data = JsonTools.Obj2JSONStrNew<OnlineDeviceDTO>(onlineDeviceDTO);
            }
            //bdl_set表
            else if (uploadManagement.UM_DataTable == "bdl_set")
            {
                SetterDAO setterDAO1 = new SetterDAO();
                Setter setter1 = setterDAO1.Load(uploadManagement.UM_DataId);

                if (setter1 == null)
                {
                    return null;
                }

                SetterDTO setterDTO = new SetterDTO(setter1, mac);

                serviceResult.Data = JsonTools.Obj2JSONStrNew<SetterDTO>(setterDTO);
            }
            //病人表
            else if (uploadManagement.UM_DataTable == "bdl_user")
            {
                UserDAO userDAO = new UserDAO();
                User user = userDAO.Load(uploadManagement.UM_DataId);
                if (user == null)
                {
                    return null;
                }


                UserDTO userDTO = new UserDTO(user, mac);
                //serviceResult.URL = "bigData/bodyStrongUser";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<UserDTO>(userDTO);
            }

            //症状表
            else if (uploadManagement.UM_DataTable == "bdl_symptominfo")
            {
                SymptomInfoDao symptomInfoDao = new SymptomInfoDao();
                var result = symptomInfoDao.Load(uploadManagement.UM_DataId);
                if (result == null)
                {
                    return null;
                }


                SymptomInfoDTO symptomInfoDTO = new SymptomInfoDTO(result, mac);
                //serviceResult.URL = "bigData/symptomInfo";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<SymptomInfoDTO>(symptomInfoDTO);
            }

            //训练处方总表
            else if (uploadManagement.UM_DataTable == "bdl_traininfo")
            {
                TrainInfoDAO trainInfoDAO = new TrainInfoDAO();
                var result = trainInfoDAO.Load(uploadManagement.UM_DataId);
                if (result == null)
                {
                    return null;
                }


                TrainInfoDTO trainInfoDTO = new TrainInfoDTO(result, mac);
                //serviceResult.URL = "bigData/trainInfo";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<TrainInfoDTO>(trainInfoDTO);
            }

            //总表中的一条数据对某台设备的具体处方
            else if (uploadManagement.UM_DataTable == "bdl_deviceprescription")
            {
                DevicePrescriptionDAO devicePrescriptionDAO = new DevicePrescriptionDAO();
                var result = devicePrescriptionDAO.Load(uploadManagement.UM_DataId);
                if (result == null)
                {
                    return null;
                }


                DevicePrescriptionDTO devicePrescriptionDTO = new DevicePrescriptionDTO(result, mac);
                //serviceResult.URL = "bigData/devicePrescription";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<DevicePrescriptionDTO>(devicePrescriptionDTO);
            }
            //具体处方的具体反馈
            else if (uploadManagement.UM_DataTable == "bdl_prescriptionresult")
            {
                PrescriptionResultDAO prescriptionResultDAO = new PrescriptionResultDAO();
                var result = prescriptionResultDAO.Load(uploadManagement.UM_DataId);
                if (result == null)
                {
                    return null;
                }


                PrescriptionResultDTO prescriptionResultDTO = new PrescriptionResultDTO(result, mac);
                //serviceResult.URL = "bigData/prescriptionResult";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<PrescriptionResultDTO>(prescriptionResultDTO);
            }
            else if (uploadManagement.UM_DataTable == "bdl_physicalpower")
            {
                PhysicalPowerDAO physicalPowerDAO = new PhysicalPowerDAO();
                var result = physicalPowerDAO.Load(uploadManagement.UM_DataId);
                if (result == null)
                {
                    return null;
                }

                PhysicalPowerDTO physicalPowerDTO = new PhysicalPowerDTO(result, mac);
                //serviceResult.URL = "bigData/physicalPower";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<PhysicalPowerDTO>(physicalPowerDTO);
            }
            else if (uploadManagement.UM_DataTable == "bdl_error")
            {
                ErrorDao errorDao = new ErrorDao();
                var result = errorDao.Load(uploadManagement.UM_DataId);
                if (result == null)
                {
                    return null;
                }

                ErrorDTO errorDTO = new ErrorDTO(result, mac);
                //serviceResult.URL = "bigData/physicalPower";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<ErrorDTO>(errorDTO);
            }

            return serviceResult;
        }
    }
}