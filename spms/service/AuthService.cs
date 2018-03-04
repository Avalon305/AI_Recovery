using spms.dao;
using spms.entity;
using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
