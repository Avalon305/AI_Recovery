using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity.newEntity
{
    [Serializable]
    [Table("bdl_nuitrack")]
    class NuitrackEntity
    {
        //主键自增id
        [ExplicitKey]
        public int Id { get; set; } = 0;
        //用户id
        public int Fk_user_id { get; set; }
        //状态 0删除 1存在
        public int Status { get; set; }
    }
}
