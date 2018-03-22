using Dapper;
using spms.entity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static spms.entity.CustomData;

namespace spms.dao.app
{
    public class CustomDataDAO : BaseDAO<CustomData>
    {
        /// <summary>
        /// 根据类型获得List<CustomData>
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<CustomData> GetListByTypeID(CustomDataEnum typeId)
        {

            using (var conn = DbUtil.getConn())
            {

                const string query = "select * from bdl_customdata where is_deleted = 0 and CD_Type = @CD_Type";

                return (List<CustomData>)conn.Query<CustomData>(query, new { CD_Type = typeId });
            }
        }
        public CustomData GetListByTypeIDAndName(CustomDataEnum typeId, string Name)
        {

            using (var conn = DbUtil.getConn())
            {

                const string query = "select * from bdl_customdata where is_deleted = 0 and CD_Type = @CD_Type and CD_CustomName=@CD_CustomName";

                return conn.QueryFirstOrDefault<CustomData>(query, new { CD_Type = typeId, CD_CustomName = Name });
            }
        }
        public void DeleteCustomDataByPrimaryKey(int ID)
        {

            using (var conn = DbUtil.getConn())
            {

                const string query = "update bdl_customdata set is_deleted=1 where Pk_CD_Id = @Pk_CD_Id";

                conn.Query<CustomData>(query, new { Pk_CD_Id = ID });
            }
        }

        /// <summary>
        ///  模糊查询
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<CustomData> GetExistByValue(CustomDataEnum customDataEnum, string value)
        {

            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_customdata where is_deleted = 0 and CD_Type = @CD_Type and CD_CustomName like @CD_CustomName";

                return (List<CustomData>)conn.Query<CustomData>(query, new { CD_Type = customDataEnum, CD_CustomName = "%" + value + "%" });
            }
        }

    }
}
