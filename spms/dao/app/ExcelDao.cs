﻿using spms.bean;
using spms.util;
using Dapper;
using spms.constant;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                const string query = "SELECT pr.pr_time1,pr.pr_time2,pr.pr_cal,pr.pr_index,si.gmt_create,si.si_pre_highpressure,si.si_pre_lowpressure,si.si_suf_highpressure,si.si_suf_lowpressure,si.si_waterinput,si.si_careinfo FROM bdl_symptominfo si,bdl_deviceprescription dp,bdl_prescriptionresult pr WHERE si.fk_ti_id = dp.fk_ti_id AND dp.pk_dp_id = pr.fk_dp_id AND si.fk_user_id=@User_Id";

                return conn.Query<TrainingAndSymptomBean>(query, new { User_Id = userId }).ToList();
            }

        }

        /// <summary>
        /// 根据用户id查询用户训练的详细记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DevicePrescription> ListTrainingDetailByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT dp.* FROM bdl_traininfo ti, bdl_deviceprescription dp WHERE ti.pk_ti_id = dp.fk_ti_id AND ti.fk_user_id=@User_Id";

                return conn.Query<DevicePrescription>(query, new { User_Id = userId }).ToList();
            }
        }

        /// <summary>
        /// 根据用户id查询体力评价记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PhysicalPowerExcekVO> ListPhysicalPowerExcekVOByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT gmt_create,pp_high,pp_weight,pp_grip,pp_eyeopenstand,pp_functionprotract,pp_sitandreach FROM bdl_physicalpower pp WHERE pp.fk_user_id=@User_Id";

                return conn.Query<PhysicalPowerExcekVO>(query, new { User_Id = userId }).ToList();
            }
        }


    }
}