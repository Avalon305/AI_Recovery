using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using spms.entity;
using spms.util;


namespace spms.dao
{
    public class PhysicalPowerDAO : BaseDAO<PhysicalPower>
    {
        public List<PhysicalPower> GetByUserId(int userId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_physicalpower WHERE fk_user_id = @FK_User_Id";

                return conn.Query<PhysicalPower>(query, new { FK_User_Id = userId }).ToList();
            }
        }

        /// <summary>
        /// 插入体力评价报告
        /// </summary>
        /// <param name="physicalPower"></param>
        public int AddPhysicalPower(PhysicalPower physicalPower)
        {
            using (var conn = DbUtil.getConn())
            {
                const string insert = "INSERT INTO bdl_physicalpower(gmt_create,gmt_modified,fk_user_id,pp_high,pp_weight,pp_grip,pp_eyeopenstand,pp_functionprotract,pp_sitandreach,pp_timeupgo,pp_walk5milegeneral,pp_walk5milefast,pp_walk10mile,pp_walk6minute,pp_step2minute,pp_legraise2minute,pp_usermemo,pp_workermemo) VALUES(@Gmt_Create,@Gmt_Modified,@FK_user_Id,@PP_High,@PP_Weight,@PP_Grip,@PP_EyeOpenStand,@PP_FunctionProtract,@PP_SitandReach,@PP_TimeUpGo,@PP_Walk5MileGeneral,@PP_Walk5MileFast,@PP_Walk10Mile,@PP_Walk6Minute,@PP_Step2Minute,@PP_LegRaise2Minute,@PP_UserMemo,@PP_WorkerMemo);";

                return conn.Execute(insert, physicalPower);
            }
        }

        /// <summary>
        /// 根据时间查询刚才插入的体力评价信息id
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public int getIdByGmtCreate(DateTime? gmtCreate)
        {
            using (var conn = DbUtil.getConn())
            {
                Console.WriteLine(gmtCreate.ToString());
                const string query = "SELECT pk_pp_id FROM bdl_physicalpower WHERE gmt_create = @gmt_create";
                int id = conn.QueryFirstOrDefault<int>(query, new { gmt_create = gmtCreate });
                return conn.QueryFirstOrDefault<int>(query, new { gmt_create = gmtCreate });
            }
        }


    }
}
