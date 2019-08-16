/// ***********************************************************************
/// 创 建 者    ：张方琛
/// 创建日期    ：2019/8/13 15:56:24
/// 功能描述    ：与设备通信的service
/// ***********************************************************************

using NLog;
using spms.dao;
using spms.entity.newEntity;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace spms.service
{
	/// <summary>
	/// 与设备通信的service
	/// </summary>
	class DeviceCommService
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		#region
		private PersonalSettingDao personalSettingDAO = new PersonalSettingDao();
		private UserRelationDao userRelationDao = new UserRelationDao();
		private static TrainService trainService = new TrainService();
		private static OnlineDeviceService onlineDeviceService = new OnlineDeviceService();

		//private MemberDAO memberDAO = new MemberDAO();
		//private MemberService memberService = new MemberService();
		//private TrainingActivityRecordDAO trainingActivityRecordDAO = new TrainingActivityRecordDAO();
		//private ActivityDAO activityDAO = new ActivityDAO();
		//private TrainingDeviceRecordDAO trainingDeviceRecordDAO = new TrainingDeviceRecordDAO();
		//private TrainingCourseDAO trainingCourseDAO = new TrainingCourseDAO();
		//private SystemSettingDAO SystemSettingDAO = new SystemSettingDAO();
		///// <summary>
		/// 处理登录请求
		/// </summary>
		/// <param name="request"></param>
		//public LoginResponse LoginRequest(LoginRequest request)
		//{
		//	LoginResponse response = new LoginResponse();
		//	response.Uid = request.Uid;
		//	response.ActivityType = request.ActivityType;

		//	//查询用户是否存在，若不存在 则自动创建用户和一系列训练课程、活动 byCQZ 2019.3.28
		//	MemberEntity memberEntity = memberDAO.GetMember(request.Uid);
		//	if (memberEntity == null)
		//	{
		//		//自动创建用户及计划 保证正常锻炼 接收数据
		//		memberService.AutoInsertUser(request.Uid);
		//		Console.WriteLine("收到的UID:{0}在数据库中不存在，自动创建用户及计划", request.Uid);
		//	}
		//	else
		//	{
		//		Console.WriteLine("用户存在");
		//	}

		//	var pSetting = personalSettingDAO.GetSettingByMemberId(request.Uid, request.DeviceType, request.ActivityType);
		//	if (pSetting != null)
		//	{//存在个人设置
		//		response.ExisitSetting = true;
		//	}
		//	else
		//	{
		//		return response;
		//	}

		//	response.TrainMode = (TrainMode)Enum.Parse(typeof(TrainMode), pSetting.Training_mode);
		//	MemberEntity member = memberDAO.Load(pSetting.Fk_member_id);
		//	response.DefatModeEnable = member.Is_open_fat_reduction;
		//	response.SeatHeight = pSetting.Seat_height == null ? 0 : (int)pSetting.Seat_height;
		//	response.BackDistance = pSetting.Backrest_distance == null ? 0 : (int)pSetting.Backrest_distance;
		//	// 可动杠杆长度cm
		//	response.LeverLength = pSetting.Lever_length == null ? 0 : (int)pSetting.Lever_length;
		//	response.ForwardLimit = pSetting.Front_limit == null ? 0 : (int)pSetting.Front_limit;
		//	response.BackLimit = pSetting.Back_limit == null ? 0 : (int)pSetting.Back_limit;
		//	//杠杆角度
		//	response.LeverAngle = pSetting.Lever_angle == null ? 0 : (double)pSetting.Lever_angle;
		//	response.ForwardForce = pSetting.Consequent_force == null ? 0 : (double)pSetting.Consequent_force;
		//	response.ReverseForce = pSetting.Reverse_force == null ? 0 : (double)pSetting.Reverse_force;
		//	response.Power = pSetting.Power == null ? 0 : (double)pSetting.Power;
		//	//课程ID、训练活动ID、训练活动记录ID

		//	var setDto = personalSettingDAO.GetSettingCourseInfoByMemberId(request.Uid, request.ActivityType);

		//	response.CourseId = setDto.Course_id;
		//	response.ActivityId = setDto.Activity_id;

		//	var recordEntity = trainingActivityRecordDAO.GetByActivityPrimaryKey(setDto.Activity_id, setDto.Current_course_count);
		//	if ((recordEntity == null) || (recordEntity != null && recordEntity.Is_complete == true))
		//	{//没有训练课程记录就插入一条新的
		//		recordEntity = new TrainingActivityRecordEntity
		//		{
		//			Id = KeyGenerator.GetNextKeyValueLong("bdl_training_activity_record"),
		//			Gmt_create = DateTime.Now,
		//			Fk_activity_id = pSetting.Fk_training_activity_id,
		//			Activity_type = ((int)request.ActivityType).ToString(),
		//			Is_complete = false,
		//			Fk_training_course_id = setDto.Course_id,
		//			Course_count = setDto.Current_course_count
		//		};
		//		trainingActivityRecordDAO.Insert(recordEntity);
		//		//插入至上传表
		//		UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
		//		uploadManagementDao.Insert(new UploadManagement(recordEntity.Id, "bdl_training_activity_record", 0));
		//	}
		//	response.ActivityRecordId = recordEntity.Id;
		//	//踏板距离
		//	response.PedalDistance = pSetting.Footboard_distance == null ? 0 : (int)pSetting.Footboard_distance;
		//	//最大心率计算值
		//	response.HeartRateMax = member.Max_heart_rate == null ? 0 : (int)member.Max_heart_rate;
		//	//角色ID
		//	response.RoleId = member.Role_id == null ? 0 : (int)member.Role_id;
		//	response.Weight = member.Weight == null ? 0.0 : (double)member.Weight;
		//	response.Age = member.Age == null ? 0 : (int)member.Age;
		//	//当前系统版本
		//	List<SystemSettingEntity> list = SystemSettingDAO.ListAll();
		//	if (list != null && list.Count > 0)
		//	{
		//		int ver = list[0].System_version == null ? 0 : (int)list[0].System_version;
		//		response.SysVersion = ver;
		//	}
		//	else
		//	{
		//		response.SysVersion = 0;
		//	}

		//	// 待训练列表 修改传入的fk_activity_id和course_count参数为活动记录表主键activityRecordId  --ByCQZ 4.7
		//	List<DeviceType> todoDevices = GenToDoDevices(request.Uid, request.ActivityType, setDto.Is_open_fat_reduction, recordEntity.Id);
		//	response.DeviceTypeArr.AddRange(todoDevices);
		//	return response;
		//}

		/// <summary>
		/// 获取待训练设备列表 修改传入的fk_activity_id和course_count参数为活动记录表主键activityRecordId  --ByCQZ 4.7
		/// </summary>
		/// <param name="request"></param>
		/// <param name="setDto"></param>
		/// <returns></returns>
		//private List<DeviceType> GenToDoDevices(string uid, ActivityType activityType, bool Is_open_fat_reduction, long activityRecordId)
		//{
		//	List<DeviceDoneDTO> doneList = personalSettingDAO.ListDeviceDone(uid, activityType, activityRecordId);
		//	var todoDevices = new List<DeviceType>();

		//	if (activityType == ActivityType.PowerCycle)//力量循环 不包括P09，原来写错了 改到耐力循环中ByCQZ 4.8
		//	{
		//		todoDevices.AddRange(new DeviceType[]{
		//			DeviceType.P00, DeviceType.P01, DeviceType.P02,DeviceType.P03,DeviceType.P04,DeviceType.P05,DeviceType.P06,
		//			DeviceType.P07,DeviceType.P08
		//		});
		//	}
		//	else if (activityType == ActivityType.EnduranceCycle)//力量耐力循环需要考虑减脂模式 增加P09 ByCQZ 4.8
		//	{
		//		if (Is_open_fat_reduction)
		//		{//减脂模式
		//			todoDevices.Add(DeviceType.E16);//动感单车
		//			todoDevices.Add(DeviceType.E12);//椭圆跑步机
		//		}
		//		todoDevices.AddRange(new DeviceType[]{
		//			DeviceType.E09,DeviceType.E10,DeviceType.E11,DeviceType.E12,DeviceType.E13,DeviceType.E14,DeviceType.E15,DeviceType.E16
		//		});
		//		if (Is_open_fat_reduction)
		//		{//减脂模式
		//			todoDevices.Add(DeviceType.E16);//动感单车
		//			todoDevices.Add(DeviceType.E12);//椭圆跑步机
		//		}
		//	}
		//	foreach (var d in doneList)//移除掉已经完成的设备
		//	{
		//		int ecode = int.Parse(d.Device_code);
		//		todoDevices.Remove((DeviceType)ecode);
		//	}

		//	return todoDevices;
		//}
	
		/// <summary>
		/// 处理心跳上传请求
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public KeepaliveResponse HandleKeepaliveResponse(KeepaliveRequest request)
		{
			System.DateTime CurrentTime = new System.DateTime();
			CurrentTime = System.DateTime.Now;

			KeepaliveResponse response = new KeepaliveResponse
			{
				DeviceId = request.DeviceId,
				DeviceType = request.DeviceType,
				ClientTime = request.ClientTime,
				ServerTime = CurrentTime.ToString()
			};

			onlineDeviceService.InsertOrUpDateOnlineDeceive(request);
			return response;
		}

		/// <summary>
		/// 处理上传训练结果请求
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public UploadResponse HandleUploadRequest(UploadRequest request)
		{
			UploadResponse response = new UploadResponse
			{
				Uid = request.Uid,
				DeviceType = request.DeviceType,
				SportMode = request.SportMode,
				DataId = request.DataId
			};

			response.Finished = true;
			response.Success = true;

			var prescriptionResult = new PrescriptionResultTwo
			{
				Fk_dp_id = request.DpId,
				Fk_ds_id = DeceiveTypeConvertTODsId(request.DeviceType),
				Bind_id = request.BindId,
				Sport_mode = SportModeTypeConcertToINT(request.SportMode),
				Device_mode=TrainModeTypeConvertToInt(request.TrainMode),
				Consequent_force=request.ConsequentForce,
				Reverse_force=request.ReverseForce,
				Power=request.Power,
				Speed_rank=request.SpeedRank,
				Finish_num=request.FinishNum,
				Finish_time=request.FinishTime,
				Distance=request.Distance,
				Energy=request.Energy,
				Heart_rate_list=request.HeartRateList
			};
			try
			{
				trainService.InsertPrescriptionResult(request, prescriptionResult);
			}
			catch (Exception ex)
			{
				response.Success = false;
				logger.Error("更新上传处方结果失败" + ex.ToString());
			}
		
			return response;
		}

		/// <summary>
		/// 将接受过来的deceivetype转换化为与数据库设备序号
		/// </summary>
		/// <param name="deviceType"></param>
		/// <returns></returns>
		public int DeceiveTypeConvertTODsId(DeviceType deviceType)
		{
			switch (deviceType)
			{
				case DeviceType.P00:
					return 1;
				case DeviceType.P01:
					return 2;
				case DeviceType.P02:
					return 3;
				case DeviceType.P03:
					return 4;
				case DeviceType.P04:
					return 5;
				case DeviceType.P05:
					return 6;
				case DeviceType.P06:
					return 7;
				case DeviceType.P07:
					return 8;
				case DeviceType.P08:
					return 9;
				case DeviceType.P09:
					return 10;
				default:
					return -1;
			}


		}

		/// <summary>
		/// 将接受过来的deceivetype转换化为与数据库设备名字
		/// </summary>
		/// <param name="deviceType"></param>
		/// <returns></returns>
		public string DeceiveTypeConvertTODsName(DeviceType deviceType)
		{
			switch (deviceType)
			{
				case DeviceType.P00:
					return "坐式划船机";
				case DeviceType.P01:
					return "坐式推胸机";
				case DeviceType.P02:
					return "腿部推蹬机";
				case DeviceType.P03:
					return "腹肌训练机";
				case DeviceType.P04:
					return "三头肌训练机";
				case DeviceType.P05:
					return "腿部外弯机";
				case DeviceType.P06:
					return "腿部内弯机";
				case DeviceType.P07:
					return "蝴蝶机";
				case DeviceType.P08:
					return "反向蝴蝶机";
				case DeviceType.P09:
					return "坐式背部伸展机";
				default:
					return "无此设备";
			}

		}
		/// <summary>
		/// 将接受来的SportModeType转化为int 用户运动模式 0：计数模式，1：计时模式
		/// </summary>
		/// <param name="sportMode"></param>
		/// <returns></returns>
		public int SportModeTypeConcertToINT(SportMode sportMode)
		{
			switch (sportMode)
			{
				case SportMode.CountingMode:
					return 0;
				case SportMode.TimerMode:
					return 1;
				default:
					return -1;
			}

		}

		/// <summary>
		/// 将接受来的TrainModeType转化为int 设备训练模式 0康复模式，1主被动模式,2被动模式
		/// </summary>
		/// <param name="trainMode"></param>
		/// <returns></returns>
		public int TrainModeTypeConvertToInt(TrainMode trainMode)
		{
			switch (trainMode)
			{
				case TrainMode.RehabilitationModel:
					return 0;
				case TrainMode.ActiveModel:
					return 1;
				case TrainMode.PassiveModel:
					return 2;
				default:
					return -1;
			}

		}
		/// <summary>
		/// 处理更新个人设置请求
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public PersonalSetResponse HandlePersonalSetRequest(PersonalSetRequest request)
		{
			PersonalSetResponse response = new PersonalSetResponse
			{
				Uid = request.Uid,
				DeviceType = request.DeviceType,
				TrainMode = request.TrainMode,
				DataId = request.DataId,
				Success = false,

			};
			var setEntity = new PersonalSettingEntity
			{
				Fk_member_id = long.Parse(request.Uid),
				Member_id = request.BindId,
				Device_code = DeceiveTypeConvertTODsName(request.DeviceType),
				Device_order_number = DeceiveTypeConvertTODsId(request.DeviceType),
				Seat_height = request.SeatHeight,
				Backrest_distance = request.BackDistance,
				Footboard_distance = request.FootboardDistance,
				Lever_angle = request.LeverAngle,
				Front_limit = request.ForwardLimit,
				Back_limit = request.BackLimit,
				Training_mode = ((int)request.TrainMode).ToString(),
				Consequent_force = request.ConsequentForce,
				Reverse_force = request.ReverseForce,
				Power = request.Power,
			};
			using (TransactionScope ts = new TransactionScope()) //使整个代码块成为事务性代码
			{
				//更新个人设置
				personalSettingDAO.UpdateSetting(setEntity);
				response.Success = true;
				ts.Complete();
			}
			return response;
		}
		#endregion

		/// <summary>
		/// 处理肌力测试上传请求
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public MuscleStrengthResponse HandleMuscleStrengthRequest(MuscleStrengthRequest request)
		{
			MuscleStrengthResponse response = new MuscleStrengthResponse
			{
				Uid = request.Uid,
				Success = false,
			};

			string muscleCreatTime = request.MuscleCreatTime;
			DateTime dt = DateTime.ParseExact(muscleCreatTime, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
			var userRelationEntity = new UserRelation
			{
				Fk_user_id = int.Parse(request.Uid),
				Muscle_test_val = request.MuscleTestValue,
				Mtv_create_time = dt
			};

			userRelationDao.updateMuscle(userRelationEntity);
			response.Success = true;
			return response;
		}

		/// <summary>
		/// 处理错上传请求
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public ErrorInfoResponse HandleErrorInfoRequest(ErrorInfoRequest request)
		{
			ErrorInfoResponse response = new ErrorInfoResponse
			{
				Uid = request.Uid,
				DeviceType = request.DeviceType,
				TrainMode = request.TrainMode,
				SportMode = request.SportMode,
				Success = false
			};

			logger.Error("当前出现错误时间" + request.ErrorStartTime
						 + "用户id" + request.Uid
						 + "设备类型" + request.Uid
						 + "训练模式" + request.TrainMode
						 + "运动模式" + request.SportMode
						 + "错误信息" + request.Error
				);
			response.Success = true;
			return response;
		}
	}
}
