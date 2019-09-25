using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.entity
{
    [Table("bdl_error")]
    class Error
    {
        [Key]
        public int error_id { get; set; }
        public int fk_user_id { get; set; }
        public int device_type { get; set; }
        public int train_mode { get; set; }
        public string error_info { get; set; }
        public string error_time { get; set; }
    }
}
