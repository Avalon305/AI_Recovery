﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.entity
{
    //http心跳对象
    class HttpHeartBeat
    {
        //用户唯一标识
        public string username { get; set; }
        //设备唯一标识
        public string clientId { get; set; }
        //权 限 操 作 状 态
        public int authStatus { get; set; }
        //心跳类型（int）
        public int heartbeatType { get; set; }
        public HttpHeartBeat(string UserID,string DeviceID) {
            this.username = UserID;
            this.clientId = DeviceID;
        }
        //ping/pang测试构造函数
        public HttpHeartBeat(string ping)
        {
            this.username = ping;
            this.clientId = ping;
            //权限位正常
            this.authStatus = 0;
            //心跳测试
            this.heartbeatType = 3;
            
        }
    }
}