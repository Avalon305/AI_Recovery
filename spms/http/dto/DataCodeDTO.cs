using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.http.dto
{
    /// <summary>
    /// 用于对DataCode的对象上传到云平台数据的DTO，本实体成员都是与DataCode实体类成员相对应
    /// </summary>
    public class DataCodeDTO
    {
        public string clientId;
        public string pkCodeId;
        public string codeXh;
        public string codeTypeId;
        public string codeSValue;
        public string codeCValue;
        public string codeEValue;
        public string codeState;

        public DataCodeDTO(DataCode dataCode, string mac)
        {
            this.clientId = mac;
            this.pkCodeId = dataCode.Pk_Code_Id.ToString();
            this.codeXh = dataCode.Code_Xh.ToString();
            this.codeTypeId = dataCode.Code_Type_Id;
            this.codeSValue = dataCode.Code_S_Value;
            this.codeCValue = dataCode.Code_E_Value;
            this.codeState = dataCode.Code_State.ToString();
        }
    }
}
