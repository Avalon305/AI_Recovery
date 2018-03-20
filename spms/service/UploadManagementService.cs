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
            return new UploadManagementDAO().ListLimit30();

        }
        /// <summary>
        /// 根据主键ID删除
        /// </summary>
        /// <param name="id">主键</param>
        public void deleteByPrimaryKey(int id) {
            new UploadManagementDAO().DeleteByPrimaryKey(new UploadManagement(id));
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="uploadManagement">传入上传管理者实体</param>
        /// <returns>返回可以用来上传的辅助对象</returns>
        public ServiceResult GetServiceResult(UploadManagement uploadManagement) {
            //service返回结果对象
            ServiceResult serviceResult = new ServiceResult();
            //提前载入唯一Setter
            SetterDAO setterDAO = new SetterDAO();
            Setter setter = setterDAO.getSetter();
            //if识别出表,设置发送路径，select出实体，转化至DTO，json打成string,返回
            //识别是否是权限用户添加
            if (uploadManagement.UM_DataTable== "bdl_auth") {
                AuthDAO authDAO = new AuthDAO();
                Auther auther = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_MANAGER);
                AutherDTO autherDTO = new AutherDTO(setter, auther);
                serviceResult.URL = "/clientController/addClient.action";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<AutherDTO>(autherDTO);
            }
            //病人表
            else if (uploadManagement.UM_DataTable == "bdl_user")
            {
                UserDAO userDAO = new UserDAO();
                User user = userDAO.Load(uploadManagement.UM_DataId);
                //TODO 编写entityDTO
                UserDTO userDTO = new UserDTO(user,setter);
                serviceResult.URL = "/bigData/BodyStrongUser";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<UserDTO>(userDTO);
            }
           
            //症状表
            else if (uploadManagement.UM_DataTable == "bdl_symptominfo")
            {
                SymptomInfoDao symptomInfoDao = new SymptomInfoDao();
                var result = symptomInfoDao.Load(uploadManagement.UM_DataId);
                //TODO SymptomInfoDTO
                SymptomInfoDTO symptomInfoDTO = new SymptomInfoDTO(result,setter);
                serviceResult.URL = "/bigData/SymptomInfo";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<SymptomInfoDTO>(symptomInfoDTO);
            }
            
             
            //训练处方总表
            else if (uploadManagement.UM_DataTable == "bdl_traininfo")
            {
                TrainInfoDAO trainInfoDAO = new TrainInfoDAO();
                var result = trainInfoDAO.Load(uploadManagement.UM_DataId);
                //todo
                TrainInfoDTO trainInfoDTO = new TrainInfoDTO(result,setter);
                serviceResult.URL = "/bigData/TrainInfo";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<TrainInfoDTO>(trainInfoDTO);

            }
           
           //总表中的一条数据对某台设备的具体处方
            else if (uploadManagement.UM_DataTable == "bdl_deviceprescription")
            {
                DevicePrescriptionDAO devicePrescriptionDAO = new DevicePrescriptionDAO();
                var result = devicePrescriptionDAO.Load(uploadManagement.UM_DataId);
                //todo
                DevicePrescriptionDTO devicePrescriptionDTO = new DevicePrescriptionDTO(result, setter);
                serviceResult.URL = "/bigData/DevicePrescription";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<DevicePrescriptionDTO>(devicePrescriptionDTO);
            }
            //具体处方的具体反馈
            else if (uploadManagement.UM_DataTable == "bdl_prescriptionresult")
            {
                PrescriptionResultDAO prescriptionResultDAO = new PrescriptionResultDAO();
                var result = prescriptionResultDAO.Load(uploadManagement.UM_DataId);
                //todo
                PrescriptionResultDTO prescriptionResultDTO = new PrescriptionResultDTO(result, setter);
                serviceResult.URL = "/bigData/PrescriptionResult";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<PrescriptionResultDTO>(prescriptionResultDTO);
            }
            else if (uploadManagement.UM_DataTable == "bdl_physicalpower")
            {
                PhysicalPowerDAO physicalPowerDAO = new PhysicalPowerDAO();
                var result = physicalPowerDAO.Load(uploadManagement.UM_DataId);
                PhysicalPowerDTO physicalPowerDTO = new PhysicalPowerDTO(result, setter);
                serviceResult.URL = "/bigData/PrescriptionResult";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<PhysicalPowerDTO>(physicalPowerDTO);
            }

            return serviceResult;
        }
        
    }
}
