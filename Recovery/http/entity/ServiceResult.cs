using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http.entity
{
    public class ServiceResult
    {
        //发送url
        public string URL { get; set; }
        //DTO的json串
        public string Data { get; set; }
    }
}
