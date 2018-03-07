using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.protocol
{
    interface IParserFrame
    {
        void Parser(ref object result, byte[] source);
    }
}
