using Recovery.bean;
using Recovery.dao;
using Recovery.entity;
using Recovery.entity.newEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recovery.bean.TrainExcelVO;

namespace Recovery.service
{
    class ExcelService
    {
        public List<NewTrainingAndSymptomBean> ListTrainingAndSymptomByUserId(int userId) {
            ExcelDao excelDao = new ExcelDao();
            return excelDao.ListTrainingAndSymptomByUserId(userId);
        }
        public List<NewDevicePrescriptionExcel> ListTrainingDetailByUserId(int userId)
        {
            ExcelDao excelDao = new ExcelDao();
            return excelDao.ListTrainingDetailByUserId(userId);
        }
        public List<PhysicalPowerExcekVO> ListPhysicalPowerExcekVOByUserId(int userId)
        {
            ExcelDao excelDao = new ExcelDao();
            return excelDao.ListPhysicalPowerExcekVOByUserId(userId);
        }
        public List<PhysicalPower> ListPhysicalPowerExcelVO(int userId)
        {
            ExcelDao excelDao = new ExcelDao();
            return excelDao.ListPhysicalPowerExcelVO(userId);
        }
        public List<NewTrainComprehensive> ListTrainExcekVOByUserId(int userId)
        {
            ExcelDao excelDao = new ExcelDao();
            return excelDao.ListTrainExcekVOByUserId(userId);
        }
    }
}
