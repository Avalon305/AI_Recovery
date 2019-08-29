using spms.bean;
using spms.dao;
using spms.entity;
using spms.entity.newEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static spms.bean.TrainExcelVO;

namespace spms.service
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
