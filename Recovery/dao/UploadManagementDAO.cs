using Dapper;
using Recovery.entity;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.dao
{
    class UploadManagementDAO : BaseDAO<UploadManagement>
    {
        /// <summary>
        /// 查询30条数据上传
        /// </summary>
        /// <returns></returns>
        public List<UploadManagement> ListLimit30()
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();
                const string query = "select * from bdl_uploadmanagement limit 0,30";

                return (List<UploadManagement>)conn.Query<UploadManagement>(query);


            }

        }
        /// <summary>
        /// 查询是否有权限用户信息
        /// </summary>
        /// <returns></returns>
        public UploadManagement CheckExistAuth()
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();
                const string query = "select * from bdl_uploadmanagement where UM_DataTable = @UM_DataTable";

                 

                return (UploadManagement)conn.QueryFirstOrDefault<UploadManagement>(query, new { UM_DataTable = "bdl_auth" });


            }

        }


    }
}
