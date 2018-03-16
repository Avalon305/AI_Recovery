using Dapper;
using spms.entity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.dao
{
    class DataCodeDAO :BaseDAO<DataCode>
    {
        public List<DataCode> ListByTypeId(string typeId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_datacode where code_type_id = @Code_type_id and code_state = 1 order by code_xh";

                return (List<DataCode>)conn.Query<DataCode>(query, new { Code_type_id = typeId });
            }
        }
        public int GetMaxXh(string typeId)
        {
            using (var conn = DbUtil.getConn())
            {
                //先验证是不是第一次添加
                string query = "select count(*)  from bdl_datacode WHERE code_type_id = @TypeId ";
                int  count = conn.QueryFirst<int>(query, new { TypeId = typeId });
                if ( count == 0) {
                    return 0;
                }
                //不是第一次添加走正常流程
                query = "select MAX(code_xh)  from bdl_datacode WHERE code_type_id = @TypeId ";
                Console.WriteLine("typeId:" + typeId);
               return conn.QueryFirst<int>(query, new { TypeId = typeId });
            }
            
        }

        

       

    }
}
