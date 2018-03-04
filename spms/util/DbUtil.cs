
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace spms.util
{


    class DbUtil
    {

        private static string DbName;
        private static string DbUserName;
        private static string DbPassword;
        private static string DbUrl;
        private static string connstr;

        private DbUtil()
        {

        }
        /// <summary>
        /// 静态代码块初始化
        /// </summary>
        static DbUtil()
        {
            DbName = ConfigurationManager.AppSettings["DbName"];
            DbUserName = ConfigurationManager.AppSettings["DbUserName"];
            DbPassword = ConfigurationManager.AppSettings["DbPassword"];
            DbUrl = ConfigurationManager.AppSettings["DbUrl"];

            connstr = string.Format("server={0};user id={1}; password={2}; database={3}; pooling=true", DbUrl, DbUserName, DbPassword, DbName);
        }
 
        public static MySqlConnection getConn()
        {
            return new MySqlConnection(connstr);
        }



    }


}
