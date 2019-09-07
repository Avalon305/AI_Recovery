using Recovery.dao.NewDao;
using Recovery.entity.newEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.service.NewService
{
    class NewExcelService
    {
        public List<NewTrainingAndSymptomBean> ListTrainingAndSymptomByUserId(int userId)
        {
            NewExcelDao excelDao = new NewExcelDao();
            return excelDao.ListTrainingAndSymptomByUserId(userId);
        }
    }
}
