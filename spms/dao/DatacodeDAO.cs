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
    }
}
