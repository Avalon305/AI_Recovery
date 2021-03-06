﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Recovery.entity;
using Recovery.util;

namespace Recovery.dao
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
        /// <summary>
        /// 获取没有关联训练的症状信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<SymptomInfo> GetNullTiIdByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query =
                    "SELECT * FROM bdl_symptominfo WHERE fk_user_id = @FK_User_Id AND (fk_ti_id = 0 OR fk_ti_id IS NULL)";
                return conn.Query<SymptomInfo>(query, new {FK_User_Id = userId }).ToList();
            }
        }
    }
}
