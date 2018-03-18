using Dapper;
using spms.entity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static spms.entity.CustomData;

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
               const string query = "select if(isnull(MAX(code_xh)),0,MAX(code_xh)) maxXh from bdl_datacode WHERE code_type_id = @TypeId ";
               return conn.QueryFirst<int>(query, new { TypeId = typeId });
            }
            
        }

        

       

    }
}
