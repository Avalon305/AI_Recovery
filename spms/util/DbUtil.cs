
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.util
{
 
   
        class DbUtil
        {

            private const string connstr = "server=127.0.0.1;user id=root; password=hengxingqingdao; database=bdl; pooling=true";

            public static MySqlConnection getConn()
            {
                return new MySqlConnection(connstr);
            }

            public static void close(SqlConnection conn, SqlConnection reader)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }

        }
     

}
