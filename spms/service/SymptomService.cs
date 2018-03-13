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
    class SymptomService
    {
        /**
         * 添加症状信息，返回主键
         */
        public int AddSymptomnInfo(SymptomInfo symptomInfo)
        {
            int id;
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            {
                SymptomInfoDao dao = new SymptomInfoDao();
                id = (int) dao.Insert(symptomInfo);
                ts.Complete();
            }
            return id;
        }

        /**
         * 添加症状子表信息，返回主键
         */
        public int AddSymptomInfoChild()
        {
            return 0;
        }
    }
}
