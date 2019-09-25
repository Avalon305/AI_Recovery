using Recovery.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.http.dto
{
    class ErrorDTO
    {
        //clientid
        public string clientId;
        public int fk_user_id;
        public int device_type;
        public int train_mode;
        public string error_info;
        public string error_time;
        public ErrorDTO(Error error, string mac)
        {
            this.clientId = mac;
            this.fk_user_id = error.fk_user_id;
            this.device_type = error.device_type;
            this.train_mode = error.train_mode;
            this.error_info = error.error_info;
            this.error_time = error.error_time.ToString();
        }
    }
}
