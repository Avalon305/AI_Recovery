// <copyright file="MakerTCPFrameTest.cs">Copyright ©  2018</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using spms.protocol;

namespace spms.protocol.Tests
{
    /// <summary>此类包含 MakerTCPFrame 的参数化单元测试</summary>
    [PexClass(typeof(MakerTCPFrame))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class MakerTCPFrameTest
    {
        /// <summary>测试 Make0001Frame() 的存根</summary>
        [PexMethod]
        internal byte[] Make0001FrameTest([PexAssumeUnderTest]MakerTCPFrame target)
        {
            byte[] result = target.Make0001Frame();
            PexAssert.IsTrue(false);
            return result;
            // TODO: 将断言添加到 方法 MakerTCPFrameTest.Make0001FrameTest(MakerTCPFrame)
        }
    }
}
