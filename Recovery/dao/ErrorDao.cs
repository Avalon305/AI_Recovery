using Dapper;
using Recovery.entity;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.dao
{
    class ErrorDao:BaseDAO<Error>
    {
        public int GetErrorLastId()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select last_insert_id()";

                return conn.QueryFirstOrDefault<int>(query);
            }
        }
    }
}
