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
    class UserDAO : BaseDAO<User>
    {
        public User GetByIdCard(string idCard)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_user where user_idcard = @IdCard";

                return conn.QueryFirst<User>(query, new { IdCard = idCard });
            }
        }
    }
}
