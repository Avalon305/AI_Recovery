/// ***********************************************************************
/// 创 建 者    ：张方琛
/// 创建日期    ：2019/8/13 15:56:24
/// 功能描述    ：与设备通信的service
/// ***********************************************************************

using NLog;
using spms.dao;
using spms.entity;
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
		private static UserDAO userDAO = new UserDAO();
		private static SetterService setterService = new SetterService();
		private static SkeletonLengthDAO skeletonLengthDAO = new SkeletonLengthDAO();
		///// <summary>
		/// 处理登录请求
		/// </summary>
		/// <param name="request"></param>
		public LoginResponse HandleLoginRequest(LoginRequest request)
		{
			LoginResponse response = new LoginResponse();

			//根据bind_id查询到Uerid
			UserRelation userRelation = userRelationDao.FindUserRelationByBind_id(request.BindId);
			string uid = (userRelation.Fk_user_id).ToString();
			response.Uid = uid;
			response.ExisitSetting = false;
			response.ClientTime = request.ClientTime;
			response.ServerTime = DateTime.Now.ToString();
			//查询用户是否存在，若不存在 。。。打印日志
			User user = userDAO.GetByPK(uid); 
			if (user != null)
			{
				//Console.WriteLine("收到的UID:{0}在数据库中不存在，自动创建用户及计划", request.Uid);
				logger.Info("用户存在", uid);
				string birth_year = (user.User_Birth.ToString().Split('/'))[0];
				
				int now_year = int.Parse((DateTime.Now.ToString("yyyy")));
				response.Age = now_year - int.Parse(birth_year);
				SkeletonLengthEntity skeletonLengthEntity= skeletonLengthDAO.getSkeletonLengthRecord(int.Parse(uid));
				response.Weight = skeletonLengthEntity.Weigth;
				response.UserName = user.User_Name;
			}
			else
			{
				//Console.WriteLine("用户存在");
				logger.Info("用户不存在");
				return response;
			}

		    //查询用户是否存在个人设置
			var pSetting = personalSettingDAO.GetSettingByMemberId(uid, ((int)request.DeviceType).ToString());
			if (pSetting != null)
			{//存在个人设置
				response.ExisitSetting = true;
				response.TrainMode = (TrainMode)Enum.Parse(typeof(TrainMode), pSetting.Training_mode);
				response.SeatHeight = pSetting.Seat_height == null ? 0 : (int)pSetting.Seat_height;
				response.BackDistance = pSetting.Backrest_distance == null ? 0 : (int)pSetting.Backrest_distance;
				response.FootboardDistance=pSetting.Footboard_distance==null ? 0: (int)pSetting.Backrest_distance;
				response.LeverAngle = pSetting.Lever_angle == null ? 0 : (double)pSetting.Lever_angle;
				response.ForwardLimit = pSetting.Front_limit == null ? 0 : (int)pSetting.Front_limit;
				response.BackLimit = pSetting.Back_limit == null ? 0 : (int)pSetting.Back_limit;
				response.ConsequentForce = pSetting.Consequent_force == null ? 0 : (double)pSetting.Consequent_force;
				response.ReverseForce = pSetting.Reverse_force == null ? 0 : (double)pSetting.Reverse_force;
				response.Power=pSetting.Power == null ? 0 : (double)pSetting.Power;
			}
			else
			{
				logger.Info("用户不存在个人设置");
			    //默认个人设置，男女
				return response;
			}


			//查询此用户是否有未做大处方
			TrainInfo trainInfo =  trainService.GetTrainInfoByUserIdAndStatus(int.Parse(uid), 0);
			if (trainInfo != null) {

				var trainInfo_id = trainInfo.Pk_TI_Id;
				var ds_id = (int)(request.DeviceType);

				//根据traininfo_id和设备id获取处方信息
				entity.newEntity.NewDevicePrescription newDevicePrescription = trainService.GetDevicePrescriptionByTiIdAndDsId(trainInfo_id, ds_id);
				if (newDevicePrescription != null)
				{
					response.DpStatus = newDevicePrescription.Dp_status == entity.newEntity.NewDevicePrescription.UNDO ? 0 : 1;
					response.DpMoveway = (int)newDevicePrescription.Dp_moveway;
					response.DpMemo = newDevicePrescription.Dp_memo;
					response.DpGroupcount = (int)newDevicePrescription.Dp_groupcount;
					response.DpGroupnum = (int)newDevicePrescription.Dp_groupnum;
					response.DpRelaxtime = (int)newDevicePrescription.Dp_relaxtime;
					response.SpeedRank = (int)newDevicePrescription.Speed_rank;
					response.DpId = (int)newDevicePrescription.Pk_dp_id;
				}
				else
				{
					logger.Info("用户不存未完成的处方或没有处方");
					return response;
				}
				
			}
			else {
				logger.Info("用户不存未完成的trainInfo或没有trainInfo");
				return response;
			}

			//当前系统版本
			Setter setter = setterService.getSetter();
			if (setter != null)
			{
				var ver = setter.Set_Version;
				response.SysVersion = ver;
			}
			else
			{
				logger.Info("无系统版本");
				response.SysVersion = "null";
				return response;
			}

			// 待训练列表 
			List<DeviceType> todoDevices = GenToDoDevices(trainInfo.Pk_TI_Id);
			response.DeviceTypeArr.AddRange(todoDevices);
			return response;
		}

		/// <summary>
		/// 获取待训练设备列表 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="setDto"></param>
		/// <returns></returns>
		private List<DeviceType> GenToDoDevices(int tiid)
		{
			List<long> doneList = trainService.dscodelist(tiid);
			var todoDevices = new List<DeviceType>();
			todoDevices.AddRange(new DeviceType[]{
					DeviceType.P00, DeviceType.P01, DeviceType.P02,DeviceType.P03,DeviceType.P04,DeviceType.P05,DeviceType.P06,
					DeviceType.P07,DeviceType.P08,DeviceType.P09
				});			
		
	
			for (int i = 0; i < doneList.Count; i++)
			{
				int ecode = (int)doneList[i];
				todoDevices.Remove((DeviceType)ecode);
			}

			return todoDevices;
		}

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
				DataId = request.DataId
			};

			response.Finished = true;
			response.Success = true;

			var prescriptionResult = new PrescriptionResultTwo
			{
				Fk_dp_id = request.DpId,
				Fk_ds_id = (long)(request.DeviceType),
				Bind_id = request.BindId,
				Device_mode=(int)(request.TrainMode),
				Consequent_force=request.ConsequentForce,
				Reverse_force=request.ReverseForce,
				Power=request.Power,
				Speed_rank=request.SpeedRank,
				Finish_num=request.FinishNum,
				Distance=request.Distance,
				Energy=request.Energy,
				Heart_rate_list=request.HeartRateList,
				pr_userthoughts =request.PrUserthoughts
				
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
				Device_code = ((int)request.DeviceType).ToString(),
				Device_order_number = ((int)request.DeviceType+1),
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
				Success = false
			};

			logger.Error("当前出现错误时间" + request.ErrorStartTime
						 + "用户id" + request.Uid
						 + "设备类型" + request.Uid
						 + "训练模式" + request.TrainMode
						 + "错误信息" + request.Error
				);
			response.Success = true;
			return response;
		}
	}
}
