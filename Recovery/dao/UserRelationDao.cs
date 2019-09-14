/// ***********************************************************************
/// 创 建 者    ：张方琛
/// 创建日期    ：2019/8/14 15:51:20
/// 功能描述    ：3D扫描dao
/// ***********************************************************************

using Dapper;
using NLog;
using Recovery.entity.newEntity;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.dao
{
	class UserRelationDao
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public void updateMuscle(UserRelation entity)
		{
			string sql = @"update bdl_user_relation set muscle_test_val=@Muscle_test_val,mtv_create_time=@Mtv_create_time
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
				try
				{
					return conn.QueryFirstOrDefault<UserRelation>(query, new { Bind_id = bind_id });

				}
				catch (Exception ex)
				{
					logger.Error("数据库跟新操作异常：" + ex.ToString());
					throw;
				}
				
			}
		}

		public UserRelation FindUserRelationByuserID(int user_id)
		{
			using (var conn = DbUtil.getConn())
			{
				const string query = "select * from bdl_user_relation where fk_user_id = @Fk_user_id ";

				return conn.QueryFirstOrDefault<UserRelation>(query, new { Fk_user_id = user_id });

			}
		}

        public int insertUserRelation(UserRelation userRelation)
        {
            using (var conn = DbUtil.getConn())
            {
                const string insert = "INSERT INTO bdl_user_relation (`fk_user_id`, `bind_id`) VALUES (@Fk_user_id, @Bind_id)";

                return conn.Execute(insert, userRelation);

            }
        }

        public void updateBind_idByFk_user_id(UserRelation userRelation)
        {
            string sql = @"update bdl_user_relation set bind_id=@Bind_id ,gmt_modified=@Gmt_modified where fk_user_id = @Fk_user_id";
            using (var conn = DbUtil.getConn())
            {
                try
                {
                    conn.Execute(sql, userRelation);
                }
                catch (Exception ex)
                {
                    logger.Error("数据库updateUserRelation操作异常：" + ex.ToString());
                }

            }
        }

        public List<UserRelation> GetExistUserRelation(string Bind_id)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_user_relation where bind_id = @Bind_id";

                return (List<UserRelation>)conn.Query<UserRelation>(query, new { Bind_id });
            }
        }

        public void updateBind_idToNull(UserRelation userRelation)
        {
            string sql = @"update bdl_user_relation set bind_id=Null ,gmt_modified=@Gmt_modified where fk_user_id = @Fk_user_id";
            using (var conn = DbUtil.getConn())
            {
                try
                {
                    conn.Execute(sql, userRelation);
                }
                catch (Exception ex)
                {
                    logger.Error("数据库updateUserRelation操作异常：" + ex.ToString());
                }

            }
        }
    }
}
