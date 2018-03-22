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
    public class SetterService
    {
        SetterDAO setterDAO = new SetterDAO();

        public void updateTest()
        {
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码

            {

                SetterDAO setterDAO = new SetterDAO();

               

                ts.Complete();

            }
        }

        public Setter getSetter() {
           
            return setterDAO.getSetter();
        }
        /// <summary>
        /// 激活时插入setter
        /// </summary>
        public void InsertSetter(Setter setter) {
            setterDAO.Insert(setter);
        }
    }
}
