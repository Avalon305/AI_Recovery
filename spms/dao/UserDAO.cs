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
        /// 根据身份证号获取用户
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public User GetByIdCard(string idCard)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_user where user_idcard = @IdCard";

                return conn.QueryFirst<User>(query, new { IdCard = idCard });

            }
        }
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
