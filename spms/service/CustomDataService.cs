using spms.dao;
using spms.dao.app;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static spms.entity.CustomData;

namespace spms.service
{
    /// <summary>
    /// 针对三个自定义类型（分组，残障，疾病）的service层
    /// </summary>
    public class CustomDataService
    {
        private CustomDataDAO customDataDAO = new CustomDataDAO();
        /// <summary>
        /// 插入自定义类型数据
        /// </summary>
        /// <param name="customDataEnum">自定义枚举类型</param>
        /// <param name="value">插入的值</param>
        public void InsertCustomData(CustomDataEnum customDataEnum, string value) {
            CustomData customData = new CustomData();
            customData.CD_Type = (byte)customDataEnum;
            customData.CD_CustomName = value;
            customDataDAO.Insert(customData);
            //插入至上传表
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            uploadManagementDao.Insert(new UploadManagement(customData.Pk_CD_Id, "bdl_customdata", 0));
        }
        /// <summary>
        /// 根据type获得对应的某种自定义的全部List<string>
        /// </summary>
        /// <param name="customDataEnum">枚举类型type</param>
        /// <returns></returns>
        public List<string> GetAllByType(CustomDataEnum customDataEnum) {
            List<string> queryResult = new List<string>();
            var result = customDataDAO.GetListByTypeID(customDataEnum);
            foreach (var i in result) {
                queryResult.Add(i.CD_CustomName);
            }
            return queryResult;
        }
        public  List<CustomData> GetAllObjectByType(CustomDataEnum customDataEnum)
        {
            List<CustomData> queryResult = new List<CustomData>();
            queryResult = customDataDAO.GetListByTypeID(customDataEnum);
            return queryResult;
        }

        /// <summary>
        /// 根据value模糊查询获得对应的自定义的List<string>
        /// </summary>
        /// <param name="customDataEnum">枚举类型type</param>
        /// <returns></returns>
        public List<string> GetExistByValue(CustomDataEnum customDataEnum ,string value)
        {
            List<string> queryResult = new List<string>();
            var result = customDataDAO.GetExistByValue(customDataEnum, value);
            foreach (var i in result)
            {
                queryResult.Add(i.CD_CustomName);
            }
            return queryResult;
        }
    }
}
