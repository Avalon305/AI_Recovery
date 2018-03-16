using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
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
    }
    public class PrescriptionResultDAO : BaseDAO<PrescriptionResult>
    {
    }
}
