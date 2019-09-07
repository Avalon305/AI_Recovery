using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace Recovery.entity
{
    /// <summary>
    /// 在线设备实体
    /// </summary>
    [Table("bdl_onlinedevice")]
    public class OnlineDevice
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int pk_od_id { get; set; }
        /// <summary>
        /// 终端ID
        /// </summary>
        public string od_clientid { get; set; }
        /// <summary>
        /// 终端类型-英文
        /// </summary>
        public string od_clientname_en { get; set; }
        /// <summary>
        /// 终端类型-中文
        /// </summary>
        public string od_clientname_ch { get; set; }
        /// <summary>
        /// 最后在线时间
        /// </summary>
        public DateTime od_gmt_modified { get; set; }
    }
}
