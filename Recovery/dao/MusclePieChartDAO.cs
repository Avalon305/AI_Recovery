using Recovery.util;
using Dapper;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recovery.entity;
using Recovery.entity.newEntity;
using System.Collections.Generic;

namespace AI_Sports.AISports.Dao
{
    class MusclePieChartDAO
    {

        /// <summary>
        /// 根据用户id，肌肉部位查询处方结果运动个数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="muscle"></param>
        /// <returns></returns>
        public int? selectNumByUserAndMuscle(long userId,string muscle)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT SUM(finishnum) FROM bdl_prescriptionresult as pr JOIN bdl_user_relation as ur ON pr.bind_id = ur.bind_id JOIN bdl_devicesort as ds ON pr.fk_ds_id = ds.pk_ds_id WHERE  ur.fk_user_id = @userId AND ds.muscle = @muscle";
                return conn.QueryFirstOrDefault<int?>(query, new { userId, muscle });
            }
        }
        /// <summary>
        /// 根据用户和设备查询消耗的总能量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public int? selectEnergyByUserAndDevice(long userId,int deviceId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT SUM(energy) FROM bdl_prescriptionresult as pr JOIN bdl_user_relation as ur ON pr.bind_id = ur.bind_id  WHERE ur.fk_user_id = @userId AND pr.fk_ds_id = @deviceId";
                var para = new { userId,deviceId };
                return conn.QueryFirstOrDefault<int?>(query, para);
            }
        }
        /// <summary>
        /// 折线图查询处方结果 根据用户ID，设备类型，次数查询几条
        /// </summary>
        /// <param name="devicePrescriptionPkDpId"></param>
        /// <returns></returns>
        public List<PrescriptionResultTwo> ListHeartRate(long userId,int deviceId,int num)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select pr.heart_rate_list,pr.gmt_create from bdl_prescriptionresult as pr JOIN bdl_user_relation as ur ON pr.bind_id = ur.bind_id  WHERE ur.fk_user_id = @userId AND pr.fk_ds_id = @deviceId ORDER BY pr.gmt_create DESC LIMIT 0,@num";
                var para = new { userId, deviceId, num };
                return conn.Query<PrescriptionResultTwo>(query, para).ToList();
            }
        }
        /// <summary>
        /// 查询用户使用感想折线 限制最近20条
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public List<PrescriptionResultTwo> ListUserThoughts(long userId, int deviceId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select " +
                    "pr.pr_userthoughts," +
                    "pr.gmt_create " +
                    "from bdl_prescriptionresult as pr " +
                    "JOIN bdl_user_relation as ur " +
                    "ON pr.bind_id = ur.bind_id " +
                    "WHERE ur.fk_user_id = @userId AND pr.fk_ds_id = @deviceId " +
                    "ORDER BY pr.gmt_create DESC LIMIT 20";
                var para = new { userId, deviceId };
                return conn.Query<PrescriptionResultTwo>(query, para).ToList();
            }
        }


        //public int? selectLegTraining(string trainingCourseId)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string query = "SELECT sum(count) FROM bdl_training_course LEFT JOIN bdl_training_activity_record ON bdl_training_course.id = fk_training_course_id LEFT JOIN bdl_training_device_record ON bdl_training_activity_record.id = fk_training_activity_record_id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_course.id = @trainingCourseId AND code_ext_value2 = 0 AND code_type_id = 'DEVICE' AND code_ext_value = '腿部'";
        //        var para = new { trainingCourseId };
        //        return conn.QueryFirstOrDefault<int?>(query, para);
        //    }
        //}

        //public int? selectArmTraining(string trainingCourseId)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string query = "SELECT sum(count) FROM bdl_training_course LEFT JOIN bdl_training_activity_record ON bdl_training_course.id = fk_training_course_id LEFT JOIN bdl_training_device_record ON bdl_training_activity_record.id = fk_training_activity_record_id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_course.id = @trainingCourseId AND code_ext_value2 = 0 AND code_type_id = 'DEVICE' AND code_ext_value = '手臂'";
        //        var para = new { trainingCourseId };
        //        return conn.QueryFirstOrDefault<int?>(query, para);
        //    }
        //}

        //public int? selectTrunkTraining(string trainingCourseId)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string query = "SELECT sum(count) FROM bdl_training_course LEFT JOIN bdl_training_activity_record ON bdl_training_course.id = fk_training_course_id LEFT JOIN bdl_training_device_record ON bdl_training_activity_record.id = fk_training_activity_record_id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_course.id = @trainingCourseId AND code_ext_value2 = 0 AND code_type_id = 'DEVICE' AND code_ext_value = '躯干'";
        //        var para = new { trainingCourseId };
        //        return conn.QueryFirstOrDefault<int?>(query, para);
        //    }
        //}

        //public int? selectchestEnduranceTraining(string trainingCourseId)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string query = "SELECT sum(count) FROM bdl_training_course LEFT JOIN bdl_training_activity_record ON bdl_training_course.id = fk_training_course_id LEFT JOIN bdl_training_device_record ON bdl_training_activity_record.id = fk_training_activity_record_id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_course.id = @trainingCourseId AND code_ext_value2 = 1 AND code_type_id = 'DEVICE' AND code_ext_value = '胸'";
        //        var para = new { trainingCourseId };
        //        return conn.QueryFirstOrDefault<int?>(query, para);
        //    }
        //}

        //public int? selectLegEnduranceTraining(string trainingCourseId)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string query = "SELECT sum(count) FROM bdl_training_course LEFT JOIN bdl_training_activity_record ON bdl_training_course.id = fk_training_course_id LEFT JOIN bdl_training_device_record ON bdl_training_activity_record.id = fk_training_activity_record_id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_course.id = @trainingCourseId AND code_ext_value2 = 1 AND code_type_id = 'DEVICE' AND code_ext_value = '腿部'";
        //        var para = new { trainingCourseId };
        //        return conn.QueryFirstOrDefault<int?>(query, para);
        //    }
        //}

        ///// <summary>
        ///// 耐力循环背部 bYcqz
        ///// </summary>
        ///// <param name="trainingCourseId"></param>
        ///// <returns></returns>
        //public int? selectEnduranceBackTraining(string trainingCourseId)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string query = "SELECT sum(count) FROM bdl_training_course LEFT JOIN bdl_training_activity_record ON bdl_training_course.id = fk_training_course_id LEFT JOIN bdl_training_device_record ON bdl_training_activity_record.id = fk_training_activity_record_id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_course.id = @trainingCourseId AND code_ext_value2 = 1 AND code_type_id = 'DEVICE' AND code_ext_value = '背部'";
        //        var para = new { trainingCourseId };
        //        return conn.QueryFirstOrDefault<int?>(query, para);
        //    }
        //}

        //public int? selectTrunkEnduranceTraining(string trainingCourseId)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string query = "SELECT sum(count) FROM bdl_training_course LEFT JOIN bdl_training_activity_record ON bdl_training_course.id = fk_training_course_id LEFT JOIN bdl_training_device_record ON bdl_training_activity_record.id = fk_training_activity_record_id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_course.id = @trainingCourseId AND code_ext_value2 = 1 AND code_type_id = 'DEVICE' AND code_ext_value = '躯干'";
        //        var para = new { trainingCourseId };
        //        return conn.QueryFirstOrDefault<int?>(query, para);
        //    }
        //}


    }
}
