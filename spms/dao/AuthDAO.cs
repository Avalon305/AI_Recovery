using spms.entity;
using spms.util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.dao
{
    class AuthDAO : BaseDAO<Auther>
    {

        public List<Auther> ListByUserStatus(byte status)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_auth where user_status = @Status";

                return (List<Auther>)conn.Query<Auther>(query, new { Status = status });
            }
        }
    }
}
