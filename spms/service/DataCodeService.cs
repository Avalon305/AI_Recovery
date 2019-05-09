using spms.constant;
using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.service
{
    /// <summary>
    /// 若要查询请使用DataCodeCache,这个类只提供增加删除修改相关方法
    /// </summary>
    public class DataCodeService
    {
        private DataCodeDAO dataCodeDAO = new DataCodeDAO();
        /// <summary>
        /// 新增编码
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="dValue"></param>
        /// <param name="sValue"></param>
        /// <param name="state"></param>
        public void AddDataCode(DataCodeTypeEnum typeId,string dValue,string sValue,Boolean state)
        {
            DataCode dataCode = new DataCode();
            //
            dataCode.Code_Xh = dataCodeDAO.GetMaxXh(typeId.ToString()) + 1;
            dataCode.Code_D_Value = dValue;
            dataCode.Code_S_Value = sValue;
            dataCodeDAO.Insert(dataCode);
            //插入至上传表
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            uploadManagementDao.Insert(new UploadManagement(dataCode.Pk_Code_Id, "bdl_datacode", 0));
        }

        public void UpdateDataCode(DataCode dataCode)
        {
            dataCodeDAO.UpdateByPrimaryKey(dataCode);
            //插入至上传表
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            uploadManagementDao.Insert(new UploadManagement(dataCode.Pk_Code_Id, "bdl_datacode", 1));
        }

        public void DeleteByPrimaryKey(int primaryKey)
        {
            var d = new DataCode();
            d.Pk_Code_Id = primaryKey;
            dataCodeDAO.DeleteByPrimaryKey(d);
        }
     
    }
}
