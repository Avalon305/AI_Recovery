using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using spms.entity;
using spms.util;


namespace spms.dao
{
    public class PhysicalPowerDAO : BaseDAO<PhysicalPower>
    {
        public List<PhysicalPower> GetByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_physicalpower WHERE fk_user_id = @FK_User_Id";

                return conn.Query<PhysicalPower>(query, new { FK_User_Id = userId }).ToList();
            }
        }
    }
}
