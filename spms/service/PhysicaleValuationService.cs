using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using spms.constant;
using spms.dao;
using spms.entity;
using spms.view.dto;

namespace spms.service
{
    class PhysicaleValuationService
    {
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
            return new PhysicalPowerDAO().AddPhysicalPower(physicalPower);
        }
    }
}