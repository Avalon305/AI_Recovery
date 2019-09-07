using AI_Sports.AISports.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.service
{
    class MusclePieChartService
    {
        MusclePieChartDAO musclePieChartDAO = new MusclePieChartDAO();
        /// <summary>
        /// 根据用户id，肌肉部位查询处方结果运动个数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="muscle"></param>
        /// <returns></returns>
        public int? selectNumByUserAndMuscle(long userId,string muscle)
        {
            return musclePieChartDAO.selectNumByUserAndMuscle(userId,muscle);
        }
        /// <summary>
        /// 根据用户Id和设备ID查询消耗能量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public int? selectEnergyByUserAndMuscle(long userId,int deviceId)
        {
            return musclePieChartDAO.selectEnergyByUserAndDevice(userId,deviceId);
        }
    }
}
