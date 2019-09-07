using Recovery.service;
using Recovery.view.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.view.EchartsClass
{
    public class ResultLine
    {
        private string hLPHeartRateList;
        private string rOWHeartRateList;
        private string tFHeartRateList;
        private string lEHeartRateList;
        private string hAPHeartRateList;
        private string cPHeartRateList;
        private string newAHeartRateList;
        private string newBHeartRateList;
        private string newCHeartRateList;
        private string newDHeartRateList;

        public ResultLine(string HeartRateList, int type)
        {
            if(type == 0)
            {
                this.hLPHeartRateList = HeartRateList;
            }
            else if (type == 1)
            {
                this.rOWHeartRateList = HeartRateList;
            }
            else if (type == 2)
            {
                this.tFHeartRateList = HeartRateList;
            }
            else if (type == 3)
            {
                this.lEHeartRateList = HeartRateList;
            }
            else if (type == 4)
            {
                this.hAPHeartRateList = HeartRateList;
            }
            else if (type == 5)
            {
                this.cPHeartRateList = HeartRateList;
            }
            else if (type == 6)
            {
                this.newAHeartRateList = HeartRateList;
            }
            else if (type == 7)
            {
                this.newBHeartRateList = HeartRateList;
            }
            else if (type == 8)
            {
                this.newCHeartRateList = HeartRateList;
            }
            else if (type == 9)
            {
                this.newDHeartRateList = HeartRateList;
            }
        }

        public ResultLine() { }

        /// <summary>
        /// html获取心率
        /// </summary>
        /// <param name="HLPHeartRates"></param>
        /// <returns></returns>
        public string getHLPHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string getROWHeartRateList()
        {
            return rOWHeartRateList;
        }
        public string getTFHeartRateList()
        {
            return tFHeartRateList;
        }
        public string getLEHeartRateList()
        {
            return lEHeartRateList;
        }
        public string getHAHeartRateList()
        {
            return hAPHeartRateList;
        }
        public string getCPHeartRateList()
        {
            return cPHeartRateList;
        }
        public string getNewAHeartRateList()
        {
            return newAHeartRateList;
        }
        public string getNewBHeartRateList()
        {
            return newBHeartRateList;
        }
        public string getNewCHeartRateList()
        {
            return newCHeartRateList;
        }
        public string getNewDHeartRateList()
        {
            return newDHeartRateList;
        }
    }
}
