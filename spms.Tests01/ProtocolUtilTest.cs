// <copyright file="ProtocolUtilTest.cs">Copyright ©  2018</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using spms.util;

namespace spms.util.Tests
{
    /// <summary>此类包含 ProtocolUtil 的参数化单元测试</summary>
    [PexClass(typeof(ProtocolUtil))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ProtocolUtilTest
    {
        /// <summary>测试 BytesToString(Byte[]) 的存根</summary>
        [PexMethod]
        internal string BytesToStringTest(byte[] InBytes)
        {
            string result = ProtocolUtil.BytesToString(InBytes);
            Assert.IsNotNull(result);
            return result;
            // TODO: 将断言添加到 方法 ProtocolUtilTest.BytesToStringTest(Byte[])
        }
    }
}
