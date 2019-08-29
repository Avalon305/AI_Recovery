using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuitrackScanProgress.constant
{
    class ProtocolConstant
    {
        
        /// <summary>
        /// 加密狗通讯加密密钥
        /// </summary>
        public static readonly byte[] USB_DOG_PASSWORD = { 0x4D, 0x61, 0x6E, 0x49, 0x6E, 0x42, 0x6C, 0x61, 0x63, 0x6B, 0x2D, 0x7A, 0x68, 0x75, 0x78, 0x67 };
        /// <summary>
        /// 加密狗鉴权密钥
        /// </summary>
        public static readonly byte[] USB_DOG_AUTH_PASSWORD = { 0x41, 0x6C, 0x69, 0x65, 0x6E, 0x74, 0x65, 0x6B, 0x45, 0x78, 0x70, 0x6C, 0x6F, 0x72, 0x65, 0x72 };
        /// <summary>
        /// 加密狗的内容
        /// </summary>
        public static byte[] USB_DOG_CONTENT;
        /// <summary>
        /// 图片每包数据多少字节
        /// </summary>
        public static readonly int PIC_PACK_SIZE = 512;
        /// <summary>
        /// USB鉴权成功
        /// </summary>
        public static int USB_SUCCESS=0;



    }
    
    class TCPConstant
    {
         
    }
    /// <summary>
    /// 通用应答
    /// </summary>
    public enum CommResponse : byte
    {
        Success = 0,
        Failed = 1,
        MistakeMsg = 2,
        UnSupport = 3
    };
    public enum DeviceType : byte
    {
        /// <summary>
        /// 胸部推举机
        /// </summary>
        X01 = 0x01,
        /// <summary>
        /// 腿部内外弯机
        /// </summary>
        X02 = 0x02,
        /// <summary>
        /// 腿部伸展弯曲机
        /// </summary>
        X03 = 0x03,
        /// <summary>
        /// 身体伸展弯曲机
        /// </summary>
        X04 = 0x04,
        /// <summary>
        /// 坐姿划船机
        /// </summary>
        X05 = 0x05,
        /// <summary>
        /// 腿部腿蹬机
        /// </summary>
        X06 = 0x06 
    }
    public enum MsgId : int
    {
        /// <summary>
        /// 通用应答
        /// </summary>
        X0001= 0x0001,
        /// <summary>
        /// 通用应答
        /// </summary>
        X8001 = 0x8001,
        /// <summary>
        /// 终端心跳
        /// </summary>
        X0002 = 0x0002,
        /// <summary>
        /// 开始训练
        /// </summary>
        X0008 = 0x0008,
        /// <summary>
        /// 开始训练应答
        /// </summary>
        X8008 = 0x8008,
        /// <summary>
        /// 训练结果上报（胸部推举机）
        /// </summary>
        X0009 = 0x0009,
        /// <summary>
        /// 请求使用者信息
        /// </summary>
        X000A = 0x000A,
        /// <summary>
        /// 响应使用者信息
        /// </summary>
        X800A = 0x800A,
        /// <summary>
        /// 响应照片总包数
        /// </summary>
        X8006 = 0x8006,
        /// <summary>
        /// 请求照片数据
        /// </summary>
        X0007 = 0x0007,
        /// <summary>
        /// 响应照片数据
        /// </summary>
        X8007 = 0x8007,








    }
}
