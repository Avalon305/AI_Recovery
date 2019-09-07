using AI_Sports.AISports.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.service
{
    class ProcessService
    {
        ProcessDAO processDAO = new ProcessDAO();
        public List<DateTime> selectCreateTime(string trainingCourseId)
        {
            return processDAO.selectCreateTime(trainingCourseId);

        }
        /// <summary>
        /// 三代：查询每台设备平均力度，其他的都用不到
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<double> selectAvgValue(long userId)
        {
            return processDAO.selectAvgValue(userId);
        }

        public int? selectCount()
        {
            return processDAO.selectCount();
        }

        public List<DateTime> selectStrengthCreateTime(string trainingCourseId)
        {
            return processDAO.selectStrengthCreateTime(trainingCourseId);
        }

        public List<double> selectavgStrengthValue(string trainingCourseId)
        {
            return processDAO.selectavgStrengthValue(trainingCourseId);
        }

        public List<DateTime> selectAerobicCreateTime(string trainingCourseId)
        {
            return processDAO.selectAerobicCreateTime(trainingCourseId);
        }

        public List<double> selectavgAerobicValue(string trainingCourseId)
        {
            return processDAO.selectavgAerobicValue(trainingCourseId);
        }
    }
}
