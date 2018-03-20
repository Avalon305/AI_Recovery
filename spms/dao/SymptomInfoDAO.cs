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
    public class SymptomInfoDao : BaseDAO<SymptomInfo>
    {
        /// <summary>
        /// 根据用户id获取症状信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SymptomInfo> GetByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_symptominfo WHERE fk_user_id = @FK_User_Id";

                return conn.Query<SymptomInfo>(query, new { FK_User_Id = userId }).ToList();
            }
        }
    }
}
