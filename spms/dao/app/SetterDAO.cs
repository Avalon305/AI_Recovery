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
    class SetterDAO : BaseDAO<Setter>
    {
        /*获得唯一设置者
         */
        public Setter getSetter()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "select * from bdl_set";
                conn.Open();
                var result = conn.QueryFirst<Setter>(query);
                return result;
            }
        }
        public void InsertOneMacAdress(entity.Setter setter)
        {
            using (var conn = DbUtil.getConn())
            {
                const string sql = "insert into bdl_set(set_unique_id) values(@Set_Unique_Id)";
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
            using (var conn = DbUtil.getConn())//更新Setter
            {
                conn.Execute("update bdl_set set Set_Language=@Set_Language,Set_OrganizationSort=@Set_OrganizationSort,Set_OrganizationName=@Set_OrganizationName,Set_PhotoLocation=@Set_PhotoLocation where Pk_Set_Id=@Pk_Set_Id", setter);
            }
        }


    }
}
