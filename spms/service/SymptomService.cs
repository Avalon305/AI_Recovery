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
         * 添加症状信息
         */
        public void AddSymptomnInfo(SymptomInfo symptomInfo)
        {
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            {
                //插入症状信息返回主键
                int id = (int)new SymptomInfoDao().Insert(symptomInfo);
                
                //插入至上传表
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                uploadManagementDao.Insert(new UploadManagement(id, "bdl_symptominfo"));

                ts.Complete();
            }
        }
        /// <summary>
        /// 添加症状信息和子表
        /// </summary>
        public void UpdateSymptomnInfo(SymptomInfo symptomInfo)
        {
            using (TransactionScope ts = new TransactionScope())//使整个代码块成为事务性代码
            {
                //插入症状信息返回主键
                new SymptomInfoDao().UpdateByPrimaryKey(symptomInfo);
                
                //插入至上传表
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                uploadManagementDao.Insert(new UploadManagement(symptomInfo.Pk_SI_Id, "bdl_symptominfo"));

                ts.Complete();
            }
        }

        public List<SymptomInfo> GetByUserId(User user)
        {
            List <SymptomInfo> symptomInfos =  new SymptomInfoDao().GetByUserId(user.Pk_User_Id);
            return symptomInfos;
        }
    }
}
