using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity
{
    [Table("bdl_auth")]
    public class Auth
    {
        [Key]
        public int Pk_Auth_Id { get; set; }
        public string Auth_UserName { get; set; }
        public string Auth_UserPass { get; set; }
        public byte Auth_Level { get; set; }

        public DateTime? Gmt_Create { get; set; }

        public DateTime? Gmt_Modified { get; set; }
        public byte? User_Status { get; set; }
        public DateTime? Auth_OfflineTime { get; set; }
    }
}
