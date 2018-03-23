using spms.constant;
using spms.dao;
using spms.entity;
using spms.http.entity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace spms.http
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
            //需要加入解密逻辑
            string mac = setter.Set_Unique_Id;
//                string mac = System.Text.Encoding.Default.GetString(AesUtil.Decrypt(System.Text.Encoding.Default.GetBytes(setter.Set_Unique_Id), ProtocolConstant.USB_DOG_PASSWORD)); ;
            if (!string.IsNullOrEmpty(setter.Set_Unique_Id))
            {
                AuthDAO authDAO = new AuthDAO();
                var result = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_MANAGER);
                //注册用户设置mac与用户名
                //TODO设置mac地址不能从本地拿，必须实时获取
                sendHeartBeat = new HttpHeartBeat(result.Auth_UserName, mac);

                if (result.User_Status == Auther.USER_STATUS_FREEZE)
                {
                    //是否为冻结状态的心跳
                    sendHeartBeat.heartbeatType = 1;
                    sendHeartBeat.authStatus = 1;
                }
                else if (result.User_Status == Auther.USER_STATUS_FREE)
                {
                    //是否完全离线
                    sendHeartBeat.heartbeatType = 2;
                    sendHeartBeat.authStatus = 3;
                }
                else
                {
                    //默认为正常心跳
                    sendHeartBeat.heartbeatType = 0;
                    sendHeartBeat.authStatus = 0;
                }
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
                //永久离线
                auther.User_Status = Auther.USER_STATUS_FREE;
            }
            else if (httpHeartBeat.authStatus == 4)
            {
                //删除用户,做冻结处理
                auther.User_Status = Auther.USER_STATUS_FREEZE;
            }

            authDAO.UpdateByPrimaryKey(auther);
        }
    }
}