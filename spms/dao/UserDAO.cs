using Dapper;
using spms.entity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.dao
{
    class UserDAO : BaseDAO<User>
    {
        /// <summary>
        /// 根据身份证号获取用户
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public User GetByIdCard(string idCard)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_user where user_idcard = @IdCard and Is_Deleted = 0";

                return conn.QueryFirstOrDefault<User>(query, new { IdCard = idCard });

            }
        }
        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="phoneNum">手机号</param>
        /// <returns></returns>
        public User GetByPhone(string phoneNum)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_user where User_Phone = @User_Phone and Is_Deleted = 0";

                return conn.QueryFirstOrDefault<User>(query, new { User_Phone = phoneNum });

            }
        }
        /// <summary>
        /// 获得所有未删除的用户
        /// </summary>
        /// <returns></returns>
        public List<User> GetExistUsers() {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_user where Is_Deleted = 0";

                return (List<User>)conn.Query<User>(query);
            }
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<User> SelectByCondition(User user)
        {
            using (var conn = DbUtil.getConn())
            {
                string query = "select * from bdl_user where Is_Deleted = 0";
                var p = new DynamicParameters();
                //ID  name   pinyin  sex  phone    IDCard   group  jibing    canzhang
                if (user.Pk_User_Id != 0) {
                    query += " and Pk_User_Id = @Pk_User_Id";
                    p.Add("Pk_User_Id",user.Pk_User_Id);
                }
                //name
                if (!string.IsNullOrEmpty(user.User_Name)) {
                    
                    
                    query += " and User_Name = @User_Name";
                    p.Add("User_Name", user.User_Name);
                }
                //pinyin
                if (!string.IsNullOrEmpty(user.User_Namepinyin))
                {


                    query += " and User_Namepinyin = @User_Namepinyin";
                    p.Add("User_Namepinyin", user.User_Namepinyin);

                   
                }
                //sex
                if (user.User_Sex != null && (user.User_Sex == 0|| user.User_Sex == 1))
                {
                    
                    query += " and User_Sex = @User_Sex";
                    p.Add("User_Sex", user.User_Sex);
                }
                //phone
                if (!string.IsNullOrEmpty(user.User_Phone))
                {

                    query += " and User_Phone = @User_Phone";
                    p.Add("User_Phone", user.User_Phone);
 
                }
                //IDCard
                if (!string.IsNullOrEmpty(user.User_IDCard))
                {
                    query += " and User_IDCard like @User_IDCard";
                    p.Add("User_IDCard", "%"+user.User_IDCard+"%");
                    
                }
                //group
                if (!string.IsNullOrEmpty(user.User_GroupName))
                {
                    query += " and User_GroupName = @User_GroupName";
                    p.Add("User_GroupName", user.User_GroupName);
                    
                }
                //jibing
                if (!string.IsNullOrEmpty(user.User_IllnessName))
                {
                    query += " and User_IllnessName = @User_IllnessName";
                    p.Add("User_IllnessName", user.User_IllnessName);
                     
                }
                //canzhang
                if (!string.IsNullOrEmpty(user.User_PhysicalDisabilities))
                {
                     
                    query += " and User_PhysicalDisabilities = @User_PhysicalDisabilities";
                    p.Add("User_PhysicalDisabilities", user.User_PhysicalDisabilities);
                }

                return (List<User>)conn.Query<User>(query,p);
            }
        }
    }
}
