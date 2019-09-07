using Recovery.constant;
using Recovery.dao;
using Recovery.entity;
using Recovery.http.entity;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Recovery.http
{
    /// <summary>
    /// 心跳指挥官
    /// </summary>
    public class HeartBeatOffice
    {
        //得到当前状态的心跳
        public HttpHeartBeat GetHeartBeatByCurrent()
        {
            HttpHeartBeat sendHeartBeat = null;

            //不属于未注册状态
            SetterDAO setterDAO = new SetterDAO();
            Setter setter = setterDAO.getSetter();
            if (setterDAO.ListAll().Count!=1)
            {
                //设置表没有唯一标识，直接返回
                return null;
            }

            //需要加入解密逻辑
            string mac = "";
            
            //获得当前主机的mac地址
            mac = SystemInfo.GetMacAddress().Replace(":", "-");
            AuthDAO authDAO = new AuthDAO();
            var result = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_MANAGER);

            //注册用户设置mac与用户名
            //TODO设置mac地址不能从本地拿，必须实时获取

            
            sendHeartBeat = new HttpHeartBeat(result.Auth_UserName, mac);

            if (result.User_Status == Auther.USER_STATUS_FREEZE)
            {
                //是否为冻结状态的心跳,这里不能从数据库取，否则，云通知本地锁死，本地改状态后，会再次通知云锁死本机，陷入死循环
                sendHeartBeat.heartbeatType = 1;
                sendHeartBeat.authStatus = 0;
            }
            else if (result.User_Status == Auther.USER_STATUS_FREE)

            {
                //是否完全离线
                sendHeartBeat.heartbeatType = 2;
                sendHeartBeat.authStatus = 0;
            }
            else
            {
                //默认为正常心跳
                sendHeartBeat.heartbeatType = 0;
                sendHeartBeat.authStatus = 0;
            }


            return sendHeartBeat;
        }

        /// <summary>
        /// 根据web传来的心跳信息，做操作
        /// </summary>
        /// <param name="httpHeartBeat"></param>
        public void SolveHeartbeat(HttpHeartBeat httpHeartBeat)
        {
            AuthDAO authDAO = new AuthDAO();
            Auther auther = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_MANAGER);

            if (httpHeartBeat.authStatus == 0)
            {
                //0权限操作位  不作处理，属于正常心跳
                //解锁
                auther.User_Status = Auther.USER_STATUS_GENERAL;
            }
            else if (httpHeartBeat.authStatus == 1)
            {
                //锁定
                auther.User_Status = Auther.USER_STATUS_FREEZE;
            }
            else if (httpHeartBeat.authStatus == 2)
            {
                //解锁
                auther.User_Status = Auther.USER_STATUS_GENERAL;
            }
            else if (httpHeartBeat.authStatus == 3)
            {
                //永久离线至9999年
                auther.User_Status = Auther.USER_STATUS_FREE;
                auther.Auth_OfflineTime = Auther.Auth_OFFLINETIMEFREE;
            }
            else if (httpHeartBeat.authStatus == 4)
            {
                //删除用户,做冻结处理
                auther.User_Status = Auther.USER_STATUS_FREEZE;
            }

            authDAO.UpdateByPrimaryKey(auther);
            //插入至上传表
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            uploadManagementDao.Insert(new UploadManagement(auther.Pk_Auth_Id, "bdl_auth", 1));
        }
    }
}