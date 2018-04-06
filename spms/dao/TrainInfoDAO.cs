using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using spms.constant;
using spms.entity;
using spms.util;

namespace spms.dao
{
    public class TrainInfoDAO : BaseDAO<TrainInfo>
    {
        /// <summary>
        /// 根据病人id和状态获取训练信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TrainInfo> GetTrainInfoByUserIdAndStatus(int userId, int status)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_traininfo WHERE fk_user_id = @FK_User_Id AND status = @Status;";

                return conn.Query<TrainInfo>(query, new { FK_User_Id = userId, Status = status }).ToList();
            }
        }

        public void UpdateStatusByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "update bdl_traininfo set status = 3 where fk_user_id = @FK_User_Id and status = 0";

                conn.Execute(query, new { FK_User_Id = userId});
            }
        }
        /// <summary>
        /// 根据用户id获取
        /// </summary>
        /// <param name="userPkUserId"></param>
        /// <returns></returns>
        public List<TrainInfo> GetFinishTrainInfoByUserId(int userId)
        {
            return GetTrainInfoByUserIdAndStatus(userId, (int) TrainInfoStatus.Finish);
        }
        /// <summary>
        /// 查找没有关联症状信息的有效训练信息
        /// </summary>
        /// <returns></returns>
        public List<TrainInfo> GetTrainInfoNoSymp(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query =
                    "SELECT * FROM bdl_traininfo LEFT JOIN bdl_symptominfo ON bdl_traininfo.pk_ti_id = bdl_symptominfo.fk_ti_id WHERE bdl_symptominfo.pk_si_id IS NULL AND bdl_traininfo.fk_user_id = @FK_User_Id AND status in (0, 2)";
                return conn.Query<TrainInfo>(query, new { FK_User_Id = userId }).ToList();
            }
        }
    }

    public class DevicePrescriptionDAO : BaseDAO<DevicePrescription>
    {
        /// <summary>
        /// 根据训练信息id查询处方
        /// </summary>
        /// <param name="tiId"></param>
        /// <returns></returns>
        public List<DevicePrescription> GetByTIId(int tiId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_deviceprescription where fk_ti_id = @Fk_TI_Id";

                return conn.Query<DevicePrescription>(query, new { Fk_TI_Id = tiId }).ToList();
            }
        }
        /// <summary>
        /// 根据训练信息id查询处方
        /// </summary>
        /// <param name="tiId"></param>
        /// <returns></returns>
        public List<DevicePrescription> ListUnDoByTIId(int tiId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_deviceprescription where fk_ti_id = @Fk_TI_Id and dp_status = 0";

                return conn.Query<DevicePrescription>(query, new { Fk_TI_Id = tiId }).ToList();
            }
        }
        /// <summary>
        /// 根据用户身份证号和设备类型查询处方信息，Normal状态
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DevicePrescription GetByUserIdDeviceType(string idcard, DeviceType deviceType)
        {
      
            using (var conn = DbUtil.getConn())
            {
                const string query = "select d.* from bdl_user u join bdl_traininfo t on u.pk_user_id = t.fk_user_id join bdl_deviceprescription d on d.fk_ti_id = t.pk_ti_id where u.user_idcard = @Idcard and d.fk_ds_id = @DeviceType and t.status = @TrainInfoStatus and d.dp_status = @Dp_Status order by t.gmt_create desc";

                return conn.QueryFirstOrDefault<DevicePrescription>(query, new { Idcard = idcard, DeviceType = (byte)deviceType, Dp_Status= DevicePrescription.UNDO , TrainInfoStatus =(byte)TrainInfoStatus.Normal });
            }
        }
        /// <summary>
        /// 通过训练结果表的id获取关联的训练信息id外键
        /// </summary>
        /// <returns></returns>
        public int GetTIIdByPRId(int id)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT fk_ti_id FROM bdl_deviceprescription JOIN bdl_prescriptionresult ON bdl_deviceprescription.pk_dp_id = bdl_prescriptionresult.fk_dp_id WHERE bdl_prescriptionresult.pk_pr_id = @Pk_PR_Id";

                return conn.QueryFirstOrDefault<int>(query, new { Pk_PR_Id = id });
            }
        }
        /// <summary>
        /// 根据训练信息id删除
        /// </summary>
        /// <param name="tiId"></param>
        public void DeleteByTiId(int tiId)
        {
            
            using (var conn = DbUtil.getConn())
            {
                const string sql = "DELETE FROM bdl_deviceprescription WHERE fk_ti_id = @FK_Ti_Id";

                conn.Execute(sql, new { FK_Ti_Id = tiId });
            }
        }
    }
    public class PrescriptionResultDAO : BaseDAO<PrescriptionResult>
    {
        public PrescriptionResult GetByDPId(int devicePrescriptionPkDpId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_prescriptionresult where fk_dp_id=@Fk_DP_Id";

                return conn.QueryFirstOrDefault<PrescriptionResult>(query, new { Fk_DP_Id = devicePrescriptionPkDpId});
            }
        }
    }
    public class DeviceSetDAO : BaseDAO<DeviceSet>
    {
        
    }
    public class DeviceSortDAO : BaseDAO<DeviceSort>
    {
        /// <summary>
        /// 根据设备名查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DeviceSort GetByName(string name)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_devicesort where ds_name=@DS_name";

                return conn.QueryFirstOrDefault<DeviceSort>(query, new { DS_name = name });
            }
        }
        public List<DeviceSort> GetDeviceSortBySet(int Dset_Id)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_devicesort where fk_dset_id = @Fk_Dset_Id";

                return (List<DeviceSort>)conn.Query<DeviceSort>(query, new { Fk_Dset_Id = Dset_Id });
            }
        }
        public void UpdateDeviceSorts(List<DeviceSort> DeviceSetList)
        {
            using (var conn = DbUtil.getConn())
            {
                const string Update = "update bdl_devicesort set DS_Status=@DS_Status where Pk_DS_Id = @Pk_DS_Id";

                conn.Execute(Update, DeviceSetList);
            }
        }
    }

}
