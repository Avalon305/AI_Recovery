using spms.constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.bean
{
    class TcpFrameBean
    {
        public string TerminalId { get; set; }
        public Int16 SerialNo { get; set; }
        public MsgId MsgId { get; set; }
        public byte[] DataBody { get; set; }
        public Boolean Success { get; set; } = true;
        public string ErrorMsg { get; set; }

        public void AppendErrorMsg(string msg)
        {
            this.Success = false;
            this.ErrorMsg += msg;
        }
    }
}
