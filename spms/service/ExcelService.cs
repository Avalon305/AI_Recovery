﻿using spms.bean;
using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.service
{
    class ExcelService
    {
        public List<TrainingAndSymptomBean> ListTrainingAndSymptomByUserId(int userId) {
            ExcelDao excelDao = new ExcelDao();
            return excelDao.ListTrainingAndSymptomByUserId(userId);
        }
        public List<DevicePrescription> ListTrainingDetailByUserId(int userId)
        {
            ExcelDao excelDao = new ExcelDao();
            return excelDao.ListTrainingDetailByUserId(userId);
        }
    }
}
