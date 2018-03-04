using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using spms.dao;
using spms.entity;

namespace spms.service
{
    class SetterService
    {

        public void updateTest()
        {
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码

            {

                SetterDAO setterDAO = new SetterDAO();

               

                ts.Complete();

            }
        }

        public Setter getSetter() {
            SetterDAO setterDAO = new SetterDAO();
            return setterDAO.getSetter();
        }
    }
}
