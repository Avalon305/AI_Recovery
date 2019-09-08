﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.entity.newEntity
{
	[Serializable]
	[Table("bdl_personal_setting")]
    public	class PersonalSettingEntity
	{
		/// 主键自增id
		[ExplicitKey]
		public long Id { get; set; }
		public long Fk_member_id { get; set; }
		public string Member_id { get; set; }
		/// 设备名
		public string Device_code { get; set; }
		/// 设备序号
		public int? Device_order_number { get; set; }
		/// 训练模式
		public string Training_mode { get; set; }
		/// 座位高度cm
		public int? Seat_height { get; set; }
		/// 靠背距离cm
		public int? Backrest_distance { get; set; }
		/// 踏板距离cm
		public int? Footboard_distance { get; set; }
		/// <summary>
		/// 杠杆角度
		/// </summary>
		public double? Lever_angle { get; set; }
		/// 前方限制cm
		public int? Front_limit { get; set; }
		/// 后方限制cm
		public int? Back_limit { get; set; }
		/// 顺向力
		public double? Consequent_force { get; set; }
		/// 反向力
		public double? Reverse_force { get; set; }
		/// 功率
		public double? Power { get; set; }
		/// <summary>
		/// 扩展字段
		/// </summary>
		public string Extra_setting { get; set; }
		/// 创建时间
		public DateTime? Gmt_create { get; set; }
		public DateTime? Gmt_modified { get; set; }
	}
}