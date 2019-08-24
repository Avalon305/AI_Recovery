using spms.service;
using spms.view.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.view.EchartsClass
{
    public class ResultLine
    {
        //private NewTrainDTO trainDto;
        private string[] hLPHeartRateList;
        private string[] pOWHeartRateList;
        private string[] tFHeartRateList;
        private string[] lEHeartRateList;
        private string[] hAPHeartRateList;
        private string[] cPPHeartRateList;
        private string[] newAPHeartRateList;
        private string[] newBPHeartRateList;
        private string[] newCPHeartRateList;
        private string[] newDPHeartRateList;

        public ResultLine() { }

        public string[] HLPHeartRateList { get => hLPHeartRateList; set => hLPHeartRateList = value; }
        public string[] POWHeartRateList { get => pOWHeartRateList; set => pOWHeartRateList = value; }
        public string[] TFHeartRateList { get => tFHeartRateList; set => tFHeartRateList = value; }
        public string[] LEHeartRateList { get => lEHeartRateList; set => lEHeartRateList = value; }
        public string[] HAPHeartRateList { get => hAPHeartRateList; set => hAPHeartRateList = value; }
        public string[] CPPHeartRateList { get => cPPHeartRateList; set => cPPHeartRateList = value; }
        public string[] NewAPHeartRateList { get => newAPHeartRateList; set => newAPHeartRateList = value; }
        public string[] NewBPHeartRateList { get => newBPHeartRateList; set => newBPHeartRateList = value; }
        public string[] NewCPHeartRateList { get => newCPHeartRateList; set => newCPHeartRateList = value; }
        public string[] NewDPHeartRateList { get => newDPHeartRateList; set => newDPHeartRateList = value; }

        /// <summary>
        /// html获取心率
        /// </summary>
        /// <param name="HLPHeartRates"></param>
        /// <returns></returns>
        public string[] getHLPHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getROWHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getTFHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getLEHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getHAHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getCPHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getNewAHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getNewBHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getNewCHeartRateList()
        {
            return hLPHeartRateList;
        }
        public string[] getNewDHeartRateList()
        {
            return hLPHeartRateList;
        }
    }
}
