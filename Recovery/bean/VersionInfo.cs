using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.bean
{
    class VersionInfo
    {
        public string currentVersion { get; set; }
        public string lastVersion { get; set; }
        public string md5 { get; set; }
        public string downloadUrl { get; set; }
        public string logUrl { get; set; }
        public Boolean update { get; set; }
        public int language { get; set; }


        public string GetProcessString()
        {
         
            return currentVersion + " " + lastVersion + " " + logUrl + " " + downloadUrl 
                +" " + AppDomain.CurrentDomain.BaseDirectory + " "+ md5 + " " + language;
        }
    }
}
