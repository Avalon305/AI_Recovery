
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


   public class DbUtil
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
            
            DbName = ConfigUtil.GetEncrypt("DbName", "");
            DbUserName = ConfigUtil.GetEncrypt("DbUserName", "");  
            DbPassword = ConfigUtil.GetEncrypt("DbPassword","");
            DbUrl = ConfigUtil.GetEncrypt("DbUrl","");

            connstr = string.Format("server={0};user id={1}; password={2}; database={3}; pooling=true", DbUrl, DbUserName, DbPassword, DbName);
        }
 
        public static MySqlConnection getConn()
        {
            return new MySqlConnection(connstr);
        }



    }


}
