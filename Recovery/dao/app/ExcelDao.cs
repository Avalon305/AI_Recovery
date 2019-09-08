using Recovery.bean;
using Recovery.util;
using Dapper;
using Recovery.constant;
using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recovery.bean.TrainExcelVO;
using Recovery.entity.newEntity;

namespace Recovery.dao
{
    public class ExcelDao
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

        /// <summary>
        /// 根据用户id查询用户训练的详细记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<NewDevicePrescriptionExcel> ListTrainingDetailByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                //const string query = "SELECT ti.gmt_create,dp.dp_groupcount,dp.dp_groupnum,dp.dp_relaxtime,dp.dp_moveway,dp.dp_weight,ds.ds_name,pr.pr_evaluate FROM bdl_traininfo ti, bdl_deviceprescription dp, bdl_devicesort ds,bdl_prescriptionresult pr WHERE ti.pk_ti_id = dp.fk_ti_id AND dp.fk_ds_id = ds.pk_ds_id AND dp.pk_dp_id=pr.fk_dp_id AND ti.status!=3 AND ti.fk_user_id=@User_Id ORDER BY dp.gmt_create";
                const string query = 
                    "SELECT " +
                    "ti.gmt_create," +
                    "dp.dp_groupcount," +
                    "dp.dp_groupnum," +
                    "dp.dp_relaxtime," +
                    "dp.dp_moveway," +
                    "dp.device_mode," +
                    "ds.ds_name," +
                    "pr.pr_userthoughts " +
                    "FROM bdl_traininfo ti, bdl_deviceprescription dp, bdl_devicesort ds,bdl_prescriptionresult pr " +
                    "WHERE ti.pk_ti_id = dp.fk_ti_id AND dp.fk_ds_id = ds.pk_ds_id AND dp.pk_dp_id=pr.fk_dp_id AND ti.status!=3 AND ti.fk_user_id=@User_Id " +
                    "ORDER BY dp.gmt_create";

                return conn.Query<NewDevicePrescriptionExcel>(query, new { User_Id = userId }).ToList();
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
                //const string query = "SELECT gmt_create,pp_high,pp_weight,pp_grip,pp_eyeopenstand,pp_functionprotract,pp_sitandreach,pp_timeupgo,pp_walk5milegeneral,pp_walk5milefast,pp_walk10mile,pp_walk6minute,pp_step2minute,pp_legraise2minute FROM bdl_physicalpower pp WHERE pp.fk_user_id=@User_Id ORDER BY gmt_create";
                const string query = 
                    "SELECT " +
                    "gmt_create," +
                    "pp_high," +
                    "pp_weight," +
                    "pp_grip," +
                    "pp_eyeopenstand," +
                    "pp_functionprotract," +
                    "pp_sitandreach," +
                    "pp_timeupgo," +
                    "pp_walk5milegeneral," +
                    "pp_walk5milefast," +
                    "pp_walk10mile," +
                    "pp_walk6minute," +
                    "pp_step2minute," +
                    "pp_legraise2minute " +
                    "FROM bdl_physicalpower pp " +
                    "WHERE pp.fk_user_id=@User_Id " +
                    "ORDER BY gmt_create";

                return conn.Query<PhysicalPowerExcekVO>(query, new { User_Id = userId }).ToList();
            }
        }

        /// <summary>
        /// 用户查询当前用户的训练记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<NewTrainComprehensive> ListTrainExcekVOByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                //const string query = "SELECT ti.gmt_create, ds.ds_name, dp.dp_groupcount, dp.dp_groupnum, dp.dp_relaxtime, dp.dp_weight,dp.dp_moveway,pr.pr_sportstrength,pr.pr_time1,pr.pr_time2,pr.pr_distance,pr.pr_countworkquantity,pr.pr_cal,pr.pr_index,pr.pr_finishgroup,pr.pr_evaluate,pr.pr_memo,pr.pr_attentionpoint,pr.pr_userthoughts FROM bdl_deviceprescription dp,bdl_prescriptionresult pr,bdl_devicesort ds,bdl_traininfo ti WHERE dp.pk_dp_id = pr.fk_dp_id AND dp.fk_ds_id = ds.pk_ds_id AND dp.fk_ti_id = ti.pk_ti_id AND ti.status != 3 AND ti.fk_user_id =@User_Id ORDER BY ti.gmt_create";
                const string query = 
                    "SELECT " +
                    "ti.gmt_create, " +
                    "ds.ds_name, " +
                    "dp.dp_groupcount, " +
                    "dp.dp_groupnum, " +
                    "dp.dp_relaxtime, " +
                    "dp.dp_moveway," +
                    "pr.pr_sportstrength," +
                    "pr.energy," +
                    "pr.pr_finishgroup," +
                    "pr.finish_num," +
                    "pr.pr_userthoughts " +
                    "FROM bdl_deviceprescription dp,bdl_prescriptionresult pr,bdl_devicesort ds,bdl_traininfo ti " +
                    "WHERE dp.pk_dp_id = pr.fk_dp_id AND dp.fk_ds_id = ds.pk_ds_id AND dp.fk_ti_id = ti.pk_ti_id AND ti.status != 3 AND ti.fk_user_id =@User_Id " +
                    "ORDER BY ti.gmt_create";

                return conn.Query<NewTrainComprehensive>(query, new { User_Id = userId }).ToList();
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
                //const string query = "SELECT gmt_create,pp_high,pp_weight,pp_grip,pp_eyeopenstand,pp_functionprotract,pp_sitandreach,pp_timeupgo,pp_walk5milegeneral,pp_walk5milefast,pp_walk10mile,pp_walk6minute,pp_step2minute,pp_legraise2minute,pp_usermemo,pp_workermemo FROM bdl_physicalpower pp WHERE pp.fk_user_id=@User_Id ORDER BY gmt_create";
                const string query = 
                    "SELECT " +
                    "gmt_create," +
                    "pp_high," +
                    "pp_weight," +
                    "pp_grip," +
                    "pp_eyeopenstand," +
                    "pp_functionprotract," +
                    "pp_sitandreach," +
                    "pp_timeupgo," +
                    "pp_walk5milegeneral," +
                    "pp_walk5milefast," +
                    "pp_walk10mile," +
                    "pp_walk6minute," +
                    "pp_step2minute," +
                    "pp_legraise2minute," +
                    "pp_usermemo," +
                    "pp_workermemo " +
                    "FROM bdl_physicalpower pp " +
                    "WHERE pp.fk_user_id=@User_Id " +
                    "ORDER BY gmt_create";

                return conn.Query<PhysicalPower>(query, new { User_Id = userId }).ToList();
            }
        }
    }
}
