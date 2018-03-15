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
        /// <summary>
        /// 获得所有未删除的用户
        /// </summary>
        /// <returns></returns>
        public List<User> GetExistUsers() {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_user where Is_Deleted = 0";

                return (List<User>)conn.Query<User>(query);
            }
        }
    }
}
