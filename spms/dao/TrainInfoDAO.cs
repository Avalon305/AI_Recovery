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
        /// 根据病人id获取最后一次训练信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TrainInfo GetLastByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_traininfo WHERE fk_user_id = @FK_User_Id ORDER BY gmt_modified DESC LIMIT 1";

                return conn.QueryFirstOrDefault<TrainInfo>(query, new { FK_User_Id = userId });
            }
        }
        /// <summary>
        /// 根据用户id获取
        /// </summary>
        /// <param name="userPkUserId"></param>
        /// <returns></returns>
        public List<TrainInfo> GetByUserId(int userPkUserId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_traininfo where fk_user_id=@FK_User_Id";

                return conn.Query<TrainInfo>(query, new { FK_User_Id = userPkUserId }).ToList();
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
        /// 根据用户身份证号和设备类型查询处方信息
        /// </summary>
        /// <param name="idcard"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DevicePrescription GetByUserIdDeviceType(string idcard, DeviceType deviceType)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select d.* from bdl_user u join bdl_traininfo t on u.pk_user_id = t.fk_user_id join bdl_deviceprescription d on d.fk_ti_id = t.pk_ti_id where u.user_idcard = @Idcard and d.fk_ds_id = @DeviceType";

                return conn.QueryFirstOrDefault<DevicePrescription>(query, new { Idcard = idcard, DeviceType = (byte)deviceType });
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
    }

}
