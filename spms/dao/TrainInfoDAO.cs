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
        public DevicePrescription GetByUserIdDeviceType(string idcard,DeviceType deviceType)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select d.* from bdl_user u join bdl_traininfo t on u.pk_user_id = t.fk_user_id join bdl_deviceprescription d on d.fk_ti_id = t.pk_ti_id where u.user_idcard = @Idcard and d.fk_ds_id = @DeviceType";

                return conn.QueryFirstOrDefault<DevicePrescription>(query, new { Idcard = idcard, DeviceType=(byte)deviceType });
            }
        }
    }
    public class PrescriptionResultDAO : BaseDAO<PrescriptionResult>
    {
    }
    public class DeviceSetDAO : BaseDAO<DeviceSet>
    {
    }

}
