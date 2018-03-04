using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.util
{
    /// <summary>
    /// 通用应答，与平台端交互时用,序列化成json
    /// </summary>
    public class GeneralResponse
    {
        //是否成功
        public Boolean? Success { get; set; } = true;
        //响应状态码 20X成功，40X客户端错误，50X服务器端错误
        public int? Status { get; set; } = 200;
        //错误编码，自定
        public int? ErrorCode { get; set; } = null;
        //提示信息
        public string Message { get; set; }
        //数据体
        public object Data { get; set; }


    }
}
