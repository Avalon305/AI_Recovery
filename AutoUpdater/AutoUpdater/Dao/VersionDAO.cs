using AutoUpdater.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdater.Dao
{
    class VersionDAO
    {
        public void UpdateVersion(string version)
        {
            const string sql = "update bdl_set set set_version = ?Version";
           using(var conn = DbUtil.getConn())
            {

                conn.Open();
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("?Version", version);

                cmd.ExecuteNonQuery();
            }
        

            
        }
    }
}
