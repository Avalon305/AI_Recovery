using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Recovery.entity;
using Recovery.util;

namespace Recovery.dao.app
{
    public class OnlineDeviceDAO : BaseDAO<OnlineDevice>
    {
        /// <summary>
        /// 根据设备ID获取
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public OnlineDevice GetByClientId(string clientId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * from bdl_onlinedevice where od_clientid = @ClientID";
                return conn.QueryFirstOrDefault<OnlineDevice>(query, new { ClientID = clientId });
            }
        }

        /// <summary>
        /// 更新在线时间
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="dateTime"></param>
        public void UpdateOnlineTime(object pk,DateTime dateTime)
        {
            //这里不用加上传表，使用该函数处时加了
            using (var conn = DbUtil.getConn())
            {
                const string query = "update bdl_onlinedevice set od_gmt_modified =  @OnlineTime where pk_od_id = @Id";
                  conn.Execute(query, new { OnlineTime = dateTime,Id=pk });
            }
        }

    }
}
