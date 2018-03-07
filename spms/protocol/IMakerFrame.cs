using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.protocol
{
    interface IMakerFrame
    {
        void PackData(ref byte[] source,byte[] cmd,byte[] data);
    }
}
