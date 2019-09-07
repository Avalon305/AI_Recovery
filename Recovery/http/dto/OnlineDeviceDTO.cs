using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http.dto
{
    /// <summary>
    /// 用于对OnlineDevice的对象上传到云平台数据的DTO，本实体成员都是与OnlineDevice实体类成员相对应
    /// </summary>
    public class OnlineDeviceDTO
    {
        //clientid
        public string clientId;
        /// 主键
        public string pkOdId;
        /// 终端ID
        public string odClientid;
        /// 终端类型-英文
        public string odClientnameEn;
        /// 终端类型-中文
        public string odClientnameCh;
        /// 最后在线时间
        public string odGmtModified;
        public OnlineDeviceDTO (OnlineDevice onlineDevice,string mac)
        {
            this.clientId = mac;
            this.pkOdId = onlineDevice.pk_od_id.ToString();
            this.odClientid = onlineDevice.od_clientid;
            this.odClientnameEn = onlineDevice.od_clientname_en;
            this.odClientnameCh = onlineDevice.od_clientname_ch;
            this.odGmtModified = onlineDevice.od_gmt_modified.ToString().Replace("/", "-");
        }
    }
}
