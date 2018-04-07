using spms.bean;
using spms.util;
using Dapper;
using spms.constant;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static spms.bean.TrainExcelVO;

namespace spms.dao
{
    public class ExcelDao
    {
        /// <summary>
        /// 根据用户id，查询用户的综合报告
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TrainingAndSymptomBean> ListTrainingAndSymptomByUserId(int userId)
        {

            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT DISTINCT si.gmt_create,pr.pr_time1,pr.pr_time2,pr.pr_cal,pr.pr_index,si.si_pre_highpressure,si.si_pre_lowpressure,si.si_suf_highpressure,si.si_suf_lowpressure,si.si_waterinput,si.si_careinfo FROM bdl_symptominfo si,bdl_deviceprescription dp,bdl_prescriptionresult pr WHERE si.fk_ti_id = dp.fk_ti_id AND dp.pk_dp_id = pr.fk_dp_id AND si.fk_user_id=@User_Id ORDER BY si.gmt_create";

                return conn.Query<TrainingAndSymptomBean>(query, new { User_Id = userId }).ToList();
            }

        }

        /// <summary>
        /// 根据用户id查询用户训练的详细记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DevicePrescriptionExcel> ListTrainingDetailByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT dp.gmt_create,dp.dp_groupcount,dp.dp_groupnum,dp.dp_relaxtime,dp.dp_moveway,dp.dp_memo,dp.dp_weight,ds.ds_name FROM bdl_traininfo ti, bdl_deviceprescription dp, bdl_devicesort ds WHERE ti.pk_ti_id = dp.fk_ti_id AND dp.fk_ds_id = ds.pk_ds_id AND ti.fk_user_id=@User_Id ORDER BY dp.gmt_create";

                return conn.Query<DevicePrescriptionExcel>(query, new { User_Id = userId }).ToList();
            }
        }

        /// <summary>
        /// 根据用户id查询体力评价记录 - 页面的展示
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PhysicalPowerExcekVO> ListPhysicalPowerExcekVOByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT gmt_create,pp_high,pp_weight,pp_grip,pp_eyeopenstand,pp_functionprotract,pp_sitandreach FROM bdl_physicalpower pp WHERE pp.fk_user_id=@User_Id ORDER BY gmt_create";

                return conn.Query<PhysicalPowerExcekVO>(query, new { User_Id = userId }).ToList();
            }
        }

        /// <summary>
        /// 用户查询当前用户的训练记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TrainComprehensive> ListTrainExcekVOByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT ti.gmt_create, ds.ds_name, dp.dp_groupcount, dp.dp_groupnum, dp.dp_relaxtime, dp.dp_weight,dp.dp_moveway,pr.pr_sportstrength,pr.pr_time1,pr.pr_time2,pr.pr_distance,pr.pr_countworkquantity,pr.pr_cal,pr.pr_index,pr.pr_finishgroup,pr.pr_evaluate,pr.pr_memo,pr.pr_attentionpoint,pr.pr_userthoughts FROM bdl_deviceprescription dp,bdl_prescriptionresult pr,bdl_devicesort ds,bdl_traininfo ti WHERE dp.pk_dp_id = pr.fk_dp_id AND dp.fk_ds_id = ds.pk_ds_id AND dp.fk_ti_id = ti.pk_ti_id AND ti.fk_user_id =@User_Id ORDER BY ti.gmt_create";

                return conn.Query<TrainComprehensive>(query, new { User_Id = userId }).ToList();
            }
        }

        /// <summary>
        /// 根据用户id查询体力评价记录 - 文档导出
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PhysicalPower> ListPhysicalPowerExcelVO(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT gmt_create,pp_high,pp_weight,pp_grip,pp_eyeopenstand,pp_functionprotract,pp_sitandreach,pp_timeupgo,pp_walk5milegeneral,pp_walk5milefast,pp_walk10mile,pp_walk6minute,pp_step2minute,pp_legraise2minute,pp_usermemo,pp_workermemo FROM bdl_physicalpower pp WHERE pp.fk_user_id=@User_Id ORDER BY gmt_create";

                return conn.Query<PhysicalPower>(query, new { User_Id = userId }).ToList();
            }
        }
    }
}
