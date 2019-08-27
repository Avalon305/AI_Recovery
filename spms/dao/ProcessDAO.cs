
using Dapper;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Sports.AISports.Dao
{
    //废弃的图表查询DAO
    class ProcessDAO
    {
       


        public List<DateTime> selectCreateTime(string trainingCourseId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT bdl_training_activity_record.gmt_create FROM bdl_training_activity_record LEFT JOIN bdl_training_device_record ON fk_training_activity_record_id = bdl_training_activity_record.id WHERE bdl_training_activity_record.activity_type = 0 AND bdl_training_activity_record.fk_training_course_id = @trainingCourseId GROUP BY course_count";
                var para = new { trainingCourseId };
                return (List<DateTime>)conn.Query<DateTime>(query, para);
            }
        }

        public int? selectCount()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT count(*) FROM (SELECT avg(( (bdl_training_device_record.consequent_force ) + ( bdl_training_device_record.reverse_force ) ) / 2),bdl_training_activity_record.gmt_create FROM bdl_training_activity_record LEFT JOIN bdl_training_device_record ON fk_training_activity_record_id = bdl_training_activity_record.id WHERE bdl_training_activity_record.gmt_create > DATE_ADD(NOW(), INTERVAL - 1440 MINUTE) AND bdl_training_activity_record.activity_type = 0 GROUP BY course_count) AS p";
                return conn.QueryFirstOrDefault<int>(query);

            }
        }
        /// <summary>
        /// 获得每个设备的力度平均值--三代
        /// </summary>
        /// <param name="trainingCourseId"></param>
        /// <returns></returns>
        public List<double> selectAvgValue(long userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT avg( ( (pr.consequent_force) + ( pr.reverse_force)) / 2) AS AvgValue FROM bdl_prescriptionresult as pr JOIN bdl_user_relation as ur ON pr.bind_id = ur.bind_id  WHERE ur.fk_user_id = @userId GROUP BY pr.fk_ds_id ORDER BY pr.fk_ds_id";
                var para = new { userId };
                return (List<double>)conn.Query<double>(query, para);
            }
        }

        public List<DateTime> selectStrengthCreateTime(string trainingCourseId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT bdl_training_activity_record.gmt_create FROM bdl_training_activity_record LEFT JOIN bdl_training_device_record ON fk_training_activity_record_id = bdl_training_activity_record.id LEFT JOIN bdl_datacode ON device_code = code_s_value AND code_type_id = 'DEVICE' WHERE bdl_training_activity_record.activity_type = 1 AND bdl_training_activity_record.fk_training_course_id = @trainingCourseId AND code_ext_value3 = 0 GROUP BY course_count";
                var para = new { trainingCourseId };
                return (List<DateTime>)conn.Query<DateTime>(query, para);
            }
        }

        public List<double> selectavgStrengthValue(string trainingCourseId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT avg( ( (bdl_training_device_record.consequent_force) + ( bdl_training_device_record.reverse_force)) / 2) AS AvgValue FROM bdl_training_activity_record LEFT JOIN bdl_training_device_record ON fk_training_activity_record_id = bdl_training_activity_record.id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE  bdl_training_activity_record.activity_type = 1 AND bdl_training_activity_record.fk_training_course_id = @trainingCourseId AND bdl_datacode.code_ext_value3 = 0 GROUP BY course_count";
                var para = new { trainingCourseId };
                return (List<double>)conn.Query<double>(query, para);
            }
        }

        public List<DateTime> selectAerobicCreateTime(string trainingCourseId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT bdl_training_activity_record.gmt_create FROM bdl_training_activity_record LEFT JOIN bdl_training_device_record ON fk_training_activity_record_id = bdl_training_activity_record.id LEFT JOIN bdl_datacode ON device_code = code_s_value AND code_type_id = 'DEVICE' WHERE bdl_training_activity_record.activity_type = 1 AND bdl_training_activity_record.fk_training_course_id = @trainingCourseId AND code_ext_value3 = 1 GROUP BY course_count";
                var para = new { trainingCourseId };
                return (List<DateTime>)conn.Query<DateTime>(query, para);
            }
        }


        public List<double> selectavgAerobicValue(string trainingCourseId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT avg(power)FROM bdl_training_activity_record LEFT JOIN bdl_training_device_record ON fk_training_activity_record_id = bdl_training_activity_record.id LEFT JOIN bdl_datacode ON device_code = code_s_value WHERE bdl_training_activity_record.activity_type = 1 AND bdl_training_activity_record.fk_training_course_id = @trainingCourseId AND bdl_datacode.code_ext_value3 = 1 GROUP BY course_count";
                var para = new { trainingCourseId };
                return (List<double>)conn.Query<double>(query, para);
            }
        }
    }
}
