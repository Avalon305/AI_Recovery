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
        /// <summary>
        /// 根据权限级别获得用户
        /// 一名普通管理员，一名超级管理员
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public Auther GetByAuthLevel(byte? Auth_Level)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_auth where Auth_Level = @Auth_Level";

                return conn.QueryFirst<Auther>(query, new { Auth_Level = Auth_Level });
            }
        }
    }
}
