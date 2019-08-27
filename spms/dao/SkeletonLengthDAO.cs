using Dapper;
using spms.entity.newEntity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.dao
{
    class SkeletonLengthDAO : BaseDAO<SkeletonLengthEntity>
    {
        public int insertSkeletonLengthRecord(SkeletonLengthEntity skeletonLengthEntity)
        {
            using (var conn = DbUtil.getConn())
            {
                const string insert = "INSERT INTO bdl_skeleton_length (`fk_member_id`, `height`, `body_length`, `shoulder_width`, `arm_length_up`, `arm_length_down`, `leg_length_up`, `leg_length_down`) VALUES (@Fk_member_id, @Height, @Body_length, @Shoulder_width, @Arm_length_up, @Arm_length_down, @Leg_length_up, @Leg_length_down)";

                return conn.Execute(insert, skeletonLengthEntity);

            }
        }

        public SkeletonLengthEntity getSkeletonLengthRecord(int Fk_member_id)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_skeleton_length where fk_member_id=@Fk_member_id";

                return conn.QueryFirstOrDefault<SkeletonLengthEntity>(query, new { Fk_member_id });

            }
        }

        public void updateSkeletonLengthRecord(SkeletonLengthEntity skeletonLengthEntity)
        {
            using (var conn = DbUtil.getConn())
            {
                const string update = "update bdl_skeleton_length set height=@Height,body_length=@Body_length,shoulder_width=@Shoulder_width,arm_length_up=@Arm_length_up,arm_length_down=@Arm_length_down, leg_length_up=@Leg_length_up, leg_length_down=@Leg_length_down where fk_member_id=@Fk_member_id";

                conn.Execute(update, skeletonLengthEntity);

            }
        }

        public void updateSkeletonLengthAndWeightRecord(SkeletonLengthEntity skeletonLengthEntity)
        {
            using (var conn = DbUtil.getConn())
            {
                const string update = "update bdl_skeleton_length set weigth=@Weigth,height=@Height,body_length=@Body_length,shoulder_width=@Shoulder_width,arm_length_up=@Arm_length_up,arm_length_down=@Arm_length_down, leg_length_up=@Leg_length_up, leg_length_down=@Leg_length_down where fk_member_id=@Fk_member_id";

                conn.Execute(update, skeletonLengthEntity);

            }
        }

        public SkeletonLengthEntity GetByPk_User_Id(int Pk_User_Id)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_skeleton_length where fk_member_id = @Pk_User_Id";

                return conn.QueryFirstOrDefault<SkeletonLengthEntity>(query, new { Pk_User_Id });

            }
        }
    }

}
