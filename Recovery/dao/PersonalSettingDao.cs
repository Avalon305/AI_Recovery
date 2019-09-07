/// ***********************************************************************
/// 创 建 者    ：张方琛
/// 创建日期    ：2019/8/14 15:56:24
/// 功能描述    ：个人设置dao
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
	class PersonalSettingDao:BaseDAO<PersonalSettingEntity>
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// 更新个人设置
		/// </summary>
		/// <param name="entity"></param>
		public void UpdateSetting(PersonalSettingEntity entity)
		{
			string sql = @"update bdl_personal_setting set Seat_height=@Seat_height,Backrest_distance=@Backrest_distance
               ,Footboard_distance=@Footboard_distance,Lever_angle=@Lever_angle,Front_limit=@Front_limit,Back_limit=@Back_limit,Training_mode=@Training_mode
               ,Consequent_force=@Consequent_force,Reverse_force=@Reverse_force
                where fk_member_id = @Fk_member_id and Device_code=@Device_code 
            ";
			using (var conn = DbUtil.getConn())
			{
				try
				{
					conn.Execute(sql, entity);
				}
				catch (Exception ex)
				{
					logger.Error("数据库UpdateSetting操作异常" + ex.ToString());
				}

			}

		}

		/// <summary>
		/// 查询个人设置
		/// </summary>
		/// <param name="member_id"></param>
		/// <param name="deviceType"></param>
		/// <param name="activityType"></param>
		/// <returns></returns>
		public PersonalSettingEntity GetSettingByMemberId(string user_id, string deviceType_code)
		{
			const string query = @"SELECT * FROM bdl_personal_setting WHERE fk_member_id = @user_id and device_code = @DeviceCode 
                             
";
			var para = new { user_id = user_id,  DeviceCode = deviceType_code };
			using (var conn = DbUtil.getConn())
			{
				return conn.Query<PersonalSettingEntity>(query, para).FirstOrDefault();
			}

		}

		/// <summary>
		/// 根据主键id删除个人设置
		/// </summary>
		/// <param name="user_id"></param>
		public void DeleteSettingByUserId(string user_id)
		{
			string sql = @"delete from bdl_personal_setting where fk_member_id = @Fk_member_id";
			using (var conn = DbUtil.getConn())
			{
				try
				{
					conn.Execute(sql, new { Fk_member_id = user_id });
				}
				catch (Exception ex)
				{
					logger.Error("数据库DELETESETTTING操作异常" + ex.ToString());
				}

			}
		}

	}
}
