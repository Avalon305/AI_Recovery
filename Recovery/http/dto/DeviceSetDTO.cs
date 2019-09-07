using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http.dto
{
    /// <summary>
    /// 用于对DeviceSet的对象上传到云平台数据的DTO，本实体成员都是与DeviceSet实体类成员相对应
    /// </summary>
    public class DeviceSetDTO
    {
        //clientid
        public string clientId;
        //上传数据主键
        public string bdlDsetId;
        //数据创建时间
        public string Gmt_Create;
        //数据更新时间
        public string Gmt_Modified;
        //系列名称
        public string DSet_Name;

        public DeviceSetDTO (DeviceSet deviceSet,string mac)
        {
            this.clientId = mac;
            this.bdlDsetId = deviceSet.Bdl_Dset_Id.ToString();
            this.Gmt_Create = deviceSet.Gmt_Create.ToString().Replace("/", "-");
            this.Gmt_Modified = deviceSet.Gmt_Modified.ToString().Replace("/", "-");
            this.DSet_Name = deviceSet.DSet_Name;
        }

    }
}
