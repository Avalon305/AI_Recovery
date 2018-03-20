using spms.dao;
using spms.entity;
using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spms.util;

namespace spms.service
{

    class AuthService
    {

        public void updateTest()
        {
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码

            {

                AuthDAO dao = new AuthDAO();
                Auther a = dao.Load(2);
                a.Auth_UserName = "mod";
                dao.UpdateByPrimaryKey(a);
                Auther b = new Auther();
                b.Auth_UserName = "new";
                dao.Insert(b);
                int p = 0;
                //int l = 8 / p;

                ts.Complete();

            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public string Login(string username,string password) {
            string loginResult = "success";
            AuthDAO authDAO = new AuthDAO();

            
            Auther autherCN = authDAO.GetByName(username);

             
             

            //密码错误
            if (autherCN == null)
            {
                loginResult = "没有该用户";
                return loginResult;
            }



            Auther auther = authDAO.Login(username, password);
            //没有该用户
            if (autherCN!=null && auther == null) {
                loginResult = "密码错误！";
                return loginResult;
            }
            //普通用户测试是否超时登录
            if (auther.Auth_OfflineTime < DateTime.Now)
            {
                loginResult = "您的使用时间已经用尽，请联系宝德龙管理员";
                return loginResult;
            }
            //超管监测权限监测是否插入U盾
            if (auther.Auth_Level == Auther.AUTH_LEVEL_ADMIN)
            {
                loginResult = "check_U";
                return loginResult;
            }

            //登录mac与激活mac不对应
            SetterDAO setterDAO = new SetterDAO();
            Setter setter = setterDAO.getSetter();
            if (setter.Set_Unique_Id != SystemInfo.GetMacAddress())
            {
                loginResult = "登录异常";
                return loginResult;
            }
            return loginResult;
        }
    }
}
