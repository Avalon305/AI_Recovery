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
    public class SetterDAO : BaseDAO<Setter>
    {
 
        /*获得唯一设置者
         */
        public Setter getSetter()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_set";
                conn.Open();
                var result = conn.QueryFirstOrDefault<Setter>(query);
                return result;
            }
        }
        public void InsertOneMacAdress(entity.Setter setter)
        {
            //插入至上传表
            UploadManagementDAO uploadManagementDao1 = new UploadManagementDAO();
            uploadManagementDao1.Insert(new UploadManagement(setter.Pk_Set_Id, "bdl_set", 0));
            using (var conn = DbUtil.getConn())
            {
                const string sql = "insert into bdl_set(set_unique_id, set_language,set_photolocation, set_version,back_up) values(@Set_Unique_Id,@Set_Language, @Set_PhotoLocation, @Set_Version,@Back_Up)";
                conn.Execute(sql, setter);
            }
        }
        public void UpdateOneSet(entity.Setter setter)
        {
            using (var conn = DbUtil.getConn())
            {
                const string sql = "update bdl_set set Set_Unique_Id=@Set_Unique_Id where Pk_Set_Id = @Pk_Set_Id";
                conn.Execute(sql, setter);
            }
        }
        public void UpdateSetter(entity.Setter setter)
        {
            //插入至上传表
            UploadManagementDAO uploadManagementDao1 = new UploadManagementDAO();
            uploadManagementDao1.Insert(new UploadManagement(setter.Pk_Set_Id, "bdl_set", 1));
            using (var conn = DbUtil.getConn())//更新Setter
            {
                conn.Execute("update bdl_set set Set_Language=@Set_Language,Set_OrganizationSort=@Set_OrganizationSort,Set_OrganizationName=@Set_OrganizationName,Set_PhotoLocation=@Set_PhotoLocation,Set_OrganizationPhone=@Set_OrganizationPhone,Back_Up=@Back_Up where Pk_Set_Id=@Pk_Set_Id", setter);
            }
        }

        /// <summary>
        /// 获取mysql的安装路径
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select @@basedir as basePath from dual";
                conn.Open();
                var result = conn.QueryFirstOrDefault<string>(query);
                return result.ToString();
            }
        }
    }
}
