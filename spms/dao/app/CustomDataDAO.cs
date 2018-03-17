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
    public class CustomDataDAO:BaseDAO<CustomData>
    {
        /// <summary>
        /// 根据类型获得List<CustomData>
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<CustomData> GetListByTypeID(CustomDataEnum typeId) {

            using (var conn = DbUtil.getConn())
            {
                //防止查空报错
                const string checkNull = "select count(*) from bdl_customdata where is_deleted = 0 and CD_Type = @CD_Type";
                int count = conn.QueryFirst<int>(checkNull, new { CD_Type = typeId });
                if (count == 0)
                {
                    return new List<CustomData>();
                }
                //非空则正常查询
                const string query = "select * from bdl_customdata where is_deleted = 0 and CD_Type = @CD_Type";

                return (List<CustomData>)conn.Query<CustomData>(query, new { CD_Type = typeId});
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
                //防止查空报错
                const string checkNull = "select count(*) from bdl_customdata where is_deleted = 0 and CD_Type = @CD_Type and CD_CustomName like @CD_CustomName";
                int count = conn.QueryFirst<int>(checkNull, new { CD_Type = customDataEnum, CD_CustomName = "%"+value+"%" });
                if (count == 0)
                {
                    return new List<CustomData>();
                }
                //非空则正常查询
                const string query = "select * from bdl_customdata where is_deleted = 0 and CD_Type = @CD_Type and CD_CustomName like @CD_CustomName";

                return (List<CustomData>)conn.Query<CustomData>(query, new { CD_Type = customDataEnum, CD_CustomName = "%" + value + "%" });
            }
        }

    }
}
