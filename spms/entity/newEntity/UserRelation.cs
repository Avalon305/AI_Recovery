using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.entity.newEntity
{
    /// <summary>
    /// 3D扫描身体数据表
    /// </summary>
    [Serializable]
    [Table("bdl_user_relation")]
    class UserRelation
    {
        // 主键 自增
        [Key]
        public int Id { get; set; }
        // 关联的用户ID
        public int Dk_user_id { get; set; }
        // 用户绑定手环id
        public string Bind_id { get; set; }
        // 肌力测试值
        public string muscle_test_val { get; set; }
        // 创建时间
        public DateTime? Gmt_create { get; set; }
        // 修改时间
        public DateTime? Gmt_modified { get; set; }
    }
}
