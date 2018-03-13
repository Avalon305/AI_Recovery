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
            //识别是否是用户添加
            if (uploadManagement.UM_DataTable== "bdl_auth") {
                AuthDAO authDAO = new AuthDAO();
                Auther auther = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_MANAGER);
                AutherDTO autherDTO = new AutherDTO(setter, auther);
                serviceResult.URL = "/clientController/addClient.action";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<AutherDTO>(autherDTO);
            }
            else if (uploadManagement.UM_DataTable == "bdl_user")
            {
                UserDAO userDAO = new UserDAO();
                User user = userDAO.Load(uploadManagement.UM_DataId);
                //TODO 编写entityDTO
                UserDTO userDTO = new UserDTO(user,setter);
                serviceResult.URL = "/bigData/BodyStrongUser";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<UserDTO>(userDTO);
            }
           
            
            else if (uploadManagement.UM_DataTable == "bdl_symptominfo")
            {
                SymptomInfoDao symptomInfoDao = new SymptomInfoDao();
                var result = symptomInfoDao.Load(uploadManagement.UM_DataId);
                //TODO SymptomInfoDTO
                SymptomInfoDTO symptomInfoDTO = new SymptomInfoDTO(result,setter);
                serviceResult.URL = "/bigData/SymptomInfo";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<SymptomInfoDTO>(symptomInfoDTO);
            }
            else if (uploadManagement.UM_DataTable == "bdl_symptominfochild")
            {
                SymptomInfoChildDao symptomInfoChildDao = new SymptomInfoChildDao();
                var result = symptomInfoChildDao.Load(uploadManagement.UM_DataId);
                //TODO SymptomInfoDTO
                SymptomInfoChildDTO symptomInfoChildDTO = new SymptomInfoChildDTO(result, setter);
                serviceResult.URL = "/bigData/SymptomInfoChild";
                serviceResult.Data = JsonTools.Obj2JSONStrNew<SymptomInfoDTO>(symptomInfoDTO);
            }
            else if (uploadManagement.UM_DataTable == "bdl_traininfo")
            {

            }
            else if (uploadManagement.UM_DataTable == "bdl_prescriptionresult")
            {

            }
           
            else if (uploadManagement.UM_DataTable == "bdl_deviceprescription")
            {

            }
            else if (uploadManagement.UM_DataTable == "bdl_physicalpower")
            {

            }

            return serviceResult;
        }
        
    }
}
