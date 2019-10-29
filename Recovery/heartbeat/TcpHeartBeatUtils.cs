using Dapper;
using Recovery.dao;
using Recovery.entity;
using Recovery.http.entity;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.heartbeat
{

    public class TcpHeartBeatUtils
    {
        public static void WriteLogFile(string content)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = System.IO.Path.Combine(path
            , "ZLogs\\");

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string fileFullName = System.IO.Path.Combine(path
            , string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMdd-HHmm")));


            using (StreamWriter output = System.IO.File.AppendText(fileFullName))
            {
                output.WriteLine(content);
                output.Close();
            }

        }

        /// <summary>
        /// 获取当前心跳
        /// </summary>
        /// <returns></returns>
        public static HeartbeatRequest GetHeartBeatByCurrent()
        {
            HeartbeatRequest sendHeartBeat = new HeartbeatRequest();
            //不属于未注册状态
            SetterDAO setterDAO = new SetterDAO();
            Setter setter = setterDAO.getSetter();
            if (setterDAO.ListAll().Count != 1)
            {
                //设置表没有唯一标识，直接返回
                return null;
            }
            Console.WriteLine("直接返回");
            AuthDAO authDAO = new AuthDAO();
            var result = authDAO.GetByAuthLevel(Auther.AUTH_LEVEL_MANAGER);
            if (result == null) {
                return null;
            }
            Console.WriteLine("直接返回1");
            //机器码mac地址从本地拿
            sendHeartBeat.UniqueId = setter.Set_Unique_Id;
            //setting表装换成json，赋值
            sendHeartBeat.SettingJSON = JsonTools.Obj2JSONStrNew<Setter>(setter);
            //项目名 医疗康复赋值
            sendHeartBeat.ProductName = "医疗康复";
            //使用期限
            sendHeartBeat.UseDeadTime = result.Auth_OfflineTime.ToString().Replace("/", "-");
			//地理位置
			sendHeartBeat.Address = setter.Set_Organizationaddress;
            //冻结
            if (result.User_Status == Auther.USER_STATUS_FREEZE)
            {
                //是否为冻结状态的心跳,这里不能从数据库取，否则，云通知本地锁死，本地改状态后，会再次通知云锁死本机，陷入死循环
                //状态 正常0和锁定1
                sendHeartBeat.Status = 1.ToString();
                Console.WriteLine("直接返回2");
            }
            //正常
            else if(result.User_Status == Auther.USER_STATUS_GENERAL)
            {
                //状态 正常0和锁定1
                //默认为正常心跳
                sendHeartBeat.Status = 0.ToString();
                Console.WriteLine("直接返回3");
            }
            return sendHeartBeat;
        }

        //增加使用时间
        //根据用户名增加使用时间
        public static void AddUseTime(HeartbeatResponse heartbeatResponse)
        {

            //将接受到的字符串(yyyy-mm-dd)转换成Datetime格式
            DateTime Offlinetime = Convert.ToDateTime(heartbeatResponse.Attachment);

            //如果返回的时间等于9999/12/31 就把状态设为永久离线 
            if (Offlinetime.ToShortDateString().ToString() == DateTime.MaxValue.ToString("yyyy/MM/dd"))
            {
                using (var conn = DbUtil.getConn())
                {
                    Console.WriteLine("________________________________________________________设为永久离线");
                    const string query = "UPDATE bdl_auth SET user_status = @User_Status,auth_offlinetime = @Auth_Offlinetime WHERE auth_level = 1";

                    conn.Execute(query, new { Auth_Offlinetime = Offlinetime , User_Status = Auther.USER_STATUS_FREE });
                }
            }
            //如果不是9999/12/31就只更改时间，不改变状态
            else
            {
                using (var conn = DbUtil.getConn())
                {
                    Console.WriteLine("________________________________________________________增加使用时间" + Offlinetime);
                    const string query = "UPDATE bdl_auth SET auth_offlinetime = @Auth_Offlinetime WHERE auth_level = 1";
                    conn.Execute(query, new { Auth_Offlinetime = Offlinetime });
                }
            }
        }
        //上锁
        //根据用户名改变状态设为冻结状态，时间未改动。
        public static void LockUse(HeartbeatResponse heartbeatResponse)
        {
            //将接受到的字符串(yyyy-mm-dd)转换成Datetime格式
            //DateTime Offlinetime = Convert.ToDateTime(heartbeatResponse.Attachment);
            //状态 正常0和锁定1
            //int status = 1;
            using (var conn = DbUtil.getConn())
            {
                const string query = "UPDATE bdl_auth SET user_status = @User_Status WHERE auth_level = 1";
                conn.Execute(query, new {  User_Status = Auther.USER_STATUS_FREEZE });
            }
        }
        //解锁
        //根据用户名改变状态设为正常状态
        public static void UnLockUse(HeartbeatResponse heartbeatResponse)
        {
            //将接受到的字符串(yyyy-mm-dd)转换成Datetime格式
            //DateTime Offlinetime = Convert.ToDateTime(heartbeatResponse.Attachment);
            //状态 正常0和锁定1
            //int status = 0;
            using (var conn = DbUtil.getConn())
            {
                const string query = "UPDATE bdl_auth SET user_status = @User_Status WHERE auth_level = 1";

                conn.Execute(query, new {User_Status = Auther.USER_STATUS_GENERAL });
            }
        }



    }
}