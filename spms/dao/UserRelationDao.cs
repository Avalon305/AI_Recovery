/// ***********************************************************************
/// 创 建 者    ：张方琛
/// 创建日期    ：2019/8/14 15:51:20
/// 功能描述    ：3D扫描dao
/// ***********************************************************************

using Dapper;
using NLog;
using spms.entity.newEntity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.dao
{
	class UserRelationDao
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public void updateMuscle(UserRelation entity)
		{
			string sql = @"update bdl_user_relation set muscle_test_val=@Muscle_test_val,muscle_create_time=@Mtv_create_time
                where fk_user_id = @Fk_user_id
            ";
			using (var conn = DbUtil.getConn())
			{
				try
				{
					conn.Execute(sql, entity);
				}
				catch (Exception ex)
				{
					logger.Error("数据库updateMuscle操作异常：" + ex.ToString());
				}

			}
		}

		public UserRelation FindUserRelationByBind_id(string bind_id)
		{
			using (var conn = DbUtil.getConn())
			{
				const string query = "select * from bdl_user_relation where bind_id = @Bind_id ";

				return conn.QueryFirstOrDefault<UserRelation>(query, new { Bind_id = bind_id });

			}
		}

		public UserRelation FindUserRelationByuserID(int user_id)
		{
			using (var conn = DbUtil.getConn())
			{
				const string query = "select * from bdl_user_relation where fk_user_id = @fk_user_id ";

				return conn.QueryFirstOrDefault<UserRelation>(query, new { fk_user_id = user_id });

			}
		}
	}
}
