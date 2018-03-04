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
        /*获得设置者
         */
        public Setter getSetter ()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_set";
                List<Setter> setList = (List<Setter>)conn.Query<Setter>(query);
                Setter setter = null;
                if (setList!=null) {
                    setter = setList.First<Setter>();
                }
                
                return setter;
            }
        }
        
    }
}
