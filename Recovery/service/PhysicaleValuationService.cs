using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using Recovery.constant;
using Recovery.dao;
using Recovery.entity;
using Recovery.view.dto;

namespace Recovery.service
{
    class PhysicaleValuationService
    {
        UploadManagementDAO uploadManagementDAO = new UploadManagementDAO();
        public List<PhysicalPower> GetByUserId(User user)
        {
            List<PhysicalPower> symptomInfos = new PhysicalPowerDAO().GetByUserId(user.Pk_User_Id);
            return symptomInfos;
        }

        /// <summary>
        /// 插入体力评价报告
        /// </summary>
        /// <param name="physicalPower"></param>
        public int AddPhysicalPower(PhysicalPower physicalPower)
        {
           

            int row = new PhysicalPowerDAO().AddPhysicalPower(physicalPower);
            //插入至上传表
            UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
            uploadManagementDao.Insert(new UploadManagement(new PhysicalPowerDAO().getIdByGmtCreate(physicalPower.Gmt_Create), "bdl_physicalpower", 0));
            if (row == 1)
            {
                int pk_pp_id = new PhysicalPowerDAO().getIdByGmtCreate(physicalPower.Gmt_Create);
                Console.WriteLine(pk_pp_id);
            }
            
            return row;
        }
    }
}