using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spms.http.entity;
using spms.http.dto;
using spms.util;
using spms.constant;

namespace spms.service
{
    class UploadManagementService
    {
        /// <summary>
        /// 查询30条数据
        /// </summary>
        /// <returns></returns>
        public List<UploadManagement> ListLimit30()
        {
            Console.WriteLine("ListLimit30()-执行");
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
            mac = Encoding.GetEncoding("GBK").GetString(b);

            ///if识别出表,设置发送路径，select出实体，转化至DTO，json打成string,返回
            //识别是否是权限用户添加
            if (uploadManagement.UM_DataTable == "bdl_auth")
            {
                AuthDAO authDAO = new AuthDAO();
                Auther auther = authDAO.Load(uploadManagement.UM_DataId);
                AutherDTO autherDTO = new AutherDTO(setter, auther, mac);
                if (autherDTO == null)
                {
                    return null;
                }

                serviceResult.URL = "clientController/addClient.action";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<AutherDTO>(autherDTO);
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

                //TODO 编写entityDTO
                UserDTO userDTO = new UserDTO(user, mac);
                serviceResult.URL = "bigData/bodyStrongUser";
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

                //TODO SymptomInfoDTO
                SymptomInfoDTO symptomInfoDTO = new SymptomInfoDTO(result, mac);
                serviceResult.URL = "bigData/symptomInfo";
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

                //todo
                TrainInfoDTO trainInfoDTO = new TrainInfoDTO(result, mac);
                serviceResult.URL = "bigData/trainInfo";
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

                //todo
                DevicePrescriptionDTO devicePrescriptionDTO = new DevicePrescriptionDTO(result, mac);
                serviceResult.URL = "bigData/devicePrescription";
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

                //todo
                PrescriptionResultDTO prescriptionResultDTO = new PrescriptionResultDTO(result, mac);
                serviceResult.URL = "bigData/prescriptionResult";
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
                serviceResult.URL = "bigData/physicalPower";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<PhysicalPowerDTO>(physicalPowerDTO);
            }

            return serviceResult;
        }
    }
}