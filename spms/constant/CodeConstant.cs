using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.constant
{
    /// <summary>
    /// 枚举
    /// </summary>
    public enum DataCodeTypeEnum
    {
        /// <summary>
        /// 父级编码列表 0
        /// </summary>
        DList,
        /// <summary>
        /// 评价，有没有问题 1
        /// </summary>
        Evaluate, 
        /// <summary>
        /// 疾病编码：与数据库信息严格对应 2
        /// </summary>
        Disease,
        /// <summary>
        /// 分组编码：与数据库信息严格对应 3
        /// </summary>
        Group,
        /// <summary>
        /// 残障编码：与数据库信息严格对应 4
        /// </summary>
        Diagiosis



    }
    public class TypeEnumHelper {
        /// <summary>
        /// 仅仅针对三个自定义字段的string方法
        /// </summary>
        /// <param name="typeEnum"></param>
        /// <returns></returns>
        public static string getByEnum(DataCodeTypeEnum typeEnum) {
            string result = "Disease";
            if (typeEnum == DataCodeTypeEnum.Diagiosis)
            {
                result = "Diagiosis";
            }
            if(typeEnum == DataCodeTypeEnum.Group)
            {
                result = "Group";
            }
            return result;
        } 
    }
}
