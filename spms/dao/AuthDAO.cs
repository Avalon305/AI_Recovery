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
                return conn.QueryFirstOrDefault<Auther>(query, new { Auth_Level = Auth_Level });
            }
        }
        public Auther GetAuther(byte Auth_Level)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_auth where Auth_Level = @Auth_Level";

                return conn.QueryFirst<Auther>(query, new { Auth_Level = Auth_Level });
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Auth_Level"></param>
        /// <returns></returns>
        public Auther Login(string name,string password)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_auth where Auth_UserName = @Auth_UserName and Auth_UserPass = @Auth_UserPass";

                return conn.QueryFirstOrDefault<Auther>(query, new { Auth_UserName = name, Auth_UserPass = password });
            }
        }
        public int GetAuthCount()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select count(*) from bdl_auth";

                return conn.QueryFirstOrDefault<int>(query);
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Auth_Level"></param>
        /// <returns></returns>
        public Auther GetByName(string name)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_auth where Auth_UserName = @Auth_UserName ";

                return conn.QueryFirstOrDefault<Auther>(query, new { Auth_UserName = name});
            }
        }

        public void UpdateByUserName(string username, int status)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "UPDATE bdl_auth SET user_status = @User_Status WHERE auth_username = @Auth_UserName";

                conn.Execute(query, new { User_Status = status, Auth_UserName = username });
            }
        }
        public int getIdByMaxId()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT Max(pk_auth_id) FROM bdl_auth";
                return conn.QueryFirstOrDefault<int>(query);
            }
        }
    }
}
