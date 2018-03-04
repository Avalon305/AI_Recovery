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
    class AuthDAO : BaseDAO<Auth>
    {

        public List<Auth> ListByUserStatus(byte status)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_auth where user_status = @Status";

                return (List<Auth>)conn.Query<Auth>(query, new { Status = status });
            }
        }
    }
}
