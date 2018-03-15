using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.dao.app
{
    /// <summary>
    /// 分组管理DAO
    /// </summary>
    public class AssistDAO : BaseDAO<Assist>
    {
        /// <summary>
        /// 获得分组list<string>
        /// </summary>
        /// <returns></returns>
        public List<string> GetGroupStr() {
            List<string> lists = new List<string>();
            var result = this.ListAll();
            foreach (var i in result) {
                lists.Add(i.Gr_Name);
            }
            return lists;
        }
    }
    /// <summary>
    /// 疾病名称DAO
    /// </summary>
    public class DiseaseDAO : BaseDAO<Disease>
    {
        /// <summary>
        /// 获得分组list<string>
        /// </summary>
        /// <returns></returns>
        public List<string> GetDiseaseStr()
        {
            List<string> lists = new List<string>();
            var result = this.ListAll();
            foreach (var i in result)
            {
                lists.Add(i.Ds_Name);
            }
            return lists;
        }
    }
    /// <summary>
    /// 残障名称DAO
    /// </summary>
    public class DiagnosisDAO : BaseDAO<Diagnosis>
    {
        /// <summary>
        /// 获得分组list<string>
        /// </summary>
        /// <returns></returns>
        public List<string> GetDiagnosisStr()
        {
            List<string> lists = new List<string>();
            var result = this.ListAll();
            foreach (var i in result)
            {
                lists.Add(i.Dn_Name);
            }
            return lists;
        }
    }
}
