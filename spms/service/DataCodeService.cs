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
        }

        public void UpdateDataCode(DataCode dataCode)
        {
            dataCodeDAO.UpdateByPrimaryKey(dataCode);
        }

        public void DeleteByPrimaryKey(int primaryKey)
        {
            var d = new DataCode();
            d.Pk_Code_Id = primaryKey;
            dataCodeDAO.DeleteByPrimaryKey(d);
        }
        /// <summary>
        /// 新增自定义三项
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="dValue"></param>
        /// <param name="sValue"></param>
        /// <param name="state"></param>
        public void AddCustomDataCode(DataCodeTypeEnum typeId, string dValue)
        {
            //新建时默认设置启用
            DataCode dataCode = new DataCode();
            //设置序号
            dataCode.Code_Xh = dataCodeDAO.GetMaxXh(typeId.ToString()) + 1;
            //设置值
            dataCode.Code_D_Value = dValue;
            //设置其他实体持有的值
            dataCode.Code_S_Value = (dataCode.Code_Xh-1).ToString();
            //设置类目
            dataCode.Code_Type_Id = TypeEnumHelper.getByEnum(typeId);
            dataCodeDAO.Insert(dataCode);
        }
        /// <summary>
        /// 获得最新的规定typeId的list<DataCode>列表
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<DataCode> GetDatasByTypeId(DataCodeTypeEnum typeId) {
            return dataCodeDAO.ListByTypeId(TypeEnumHelper.getByEnum(typeId));
        }
        /// <summary>
        /// 获得规定typeId的list<string>列表
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<string> GetDataStrByTypeId(DataCodeTypeEnum typeId)
        {
            var result =  dataCodeDAO.ListByTypeId(TypeEnumHelper.getByEnum(typeId));
            List<string> lists = new List<string>();
            foreach (var i in result) {
                lists.Add(i.Code_D_Value);
            }
            return lists;
        }
    }
}
