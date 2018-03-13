using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spms.http.entity;

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

            }
            else if (uploadManagement.UM_DataTable == "")
            {

            }

            return serviceResult;
        }
        
    }
}
