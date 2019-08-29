using spms.dao.NewDao;
using spms.entity.newEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.service.NewService
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
