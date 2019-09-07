using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http.dto
{
    /// <summary>
    /// 用于对DeviceSort的对象上传到云平台数据的DTO，本实体成员都是与DeviceSort实体类成员相对应
    /// </summary>
    public class DeviceSortDTO
    {
        //clientid
        public string clientId;
        //上传数据主键
        public string pkDSId;
        //数据创建时间
        public string gmtCreate;
        //数据更新时间
        public string gmtModified;
        //设备名称
        public string dSName;
        //所属的设备系列ID
        public string fkDSetId;
        //设备状态
        public string dSStatus;
        public DeviceSortDTO (DeviceSort deviceSort,string mac)
        {
            this.clientId = mac;
            this.pkDSId = deviceSort.Pk_DS_Id.ToString();
            this.gmtCreate = deviceSort.Gmt_Create.ToString().Replace("/", "-");
            this.gmtModified = deviceSort.Gmt_Modified.ToString().Replace("/", "-");
            this.dSName = deviceSort.DS_name;
            this.fkDSetId = deviceSort.Fk_DSet_Id.ToString();
            this.dSStatus = deviceSort.DS_Status.ToString();
        }
    }
}
