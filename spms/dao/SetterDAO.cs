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
     class SetterDAO : BaseDAO<Setter>
    {
        /*获得唯一设置者
         */
        public Setter getSetter ()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_set";
                conn.Open();
                var result =  conn.QueryFirst<Setter>(query);
                return result;
            }
        }
        
    }
}
