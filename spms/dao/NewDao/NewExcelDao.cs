using Dapper;
using spms.entity.newEntity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.dao.NewDao
{
    class NewExcelDao
    {
        /// <summary>
        /// 根据用户id，查询用户的综合报告
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<NewTrainingAndSymptomBean> ListTrainingAndSymptomByUserId(int userId)
        {

            using (var conn = DbUtil.getConn())
            {
                const string query =
                    "SELECT " +
                    "ti.gmt_create," +
                    "pr.finish_time," +
                    "pr.energy," +
                    "si.si_pre_highpressure," +
                    "si.si_pre_lowpressure," +
                    "si.si_suf_highpressure," +
                    "si.si_suf_lowpressure," +
                    "si.si_waterinput," +
                    "si.si_careinfo " +
                    "FROM bdl_traininfo ti " +
                    "JOIN bdl_deviceprescription dp ON ti.pk_ti_id = dp.fk_ti_id " +
                    "JOIN bdl_prescriptionresult pr ON dp.pk_dp_id = pr.fk_dp_id " +
                    "LEFT JOIN bdl_symptominfo si ON ti.pk_ti_id = si.fk_ti_id " +
                    "WHERE ti.fk_user_id=@User_Id AND ti.status!=3 " +
                    "ORDER BY ti.gmt_create";

                return conn.Query<NewTrainingAndSymptomBean>(query, new { User_Id = userId }).ToList();
            }

        }
    }
}
