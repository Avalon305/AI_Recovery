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
         * 添加症状信息和子表
         */
        public void AddSymptomnInfo(SymptomInfo symptomInfo, SymptomInfoChild preSymptomInfoChild, SymptomInfoChild sufSymptomInfoChild)
        {
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            {
                //插入症状信息返回主键
                int id = (int)new SymptomInfoDao().Insert(symptomInfo);
                preSymptomInfoChild.Fk_SI_Id = id;
                sufSymptomInfoChild.Fk_SI_Id = id;

                //插入两个症状子表
                int preChildId  = (int) new SymptomInfoChildDao().Insert(preSymptomInfoChild);
                int sufChildId = (int) new SymptomInfoChildDao().Insert(sufSymptomInfoChild);

                //插入至上传表
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                uploadManagementDao.Insert(new UploadManagement(id, "bdl_symptominfo"));
                uploadManagementDao.Insert(new UploadManagement(preChildId, "bdl_symptominfochild"));
                uploadManagementDao.Insert(new UploadManagement(sufChildId, "bdl_symptominfochild"));

                ts.Complete();
            }
        }
    }
}
