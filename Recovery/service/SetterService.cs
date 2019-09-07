using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Recovery.dao;
using Recovery.entity;

namespace Recovery.service
{
    public class SetterService
    {
        SetterDAO setterDAO = new SetterDAO();
        /// <summary>
        /// 方便查询
        /// </summary>
        /// <returns></returns>
        public SetterDAO GetSetterDAO()
        {
            return setterDAO;
        }

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
            //插入至上传表
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            uploadManagementDao.Insert(new UploadManagement(setter.Pk_Set_Id, "bdl_set", 0));
        }

        public string getPath()
        {
            return setterDAO.GetPath();
        }
    }
}
