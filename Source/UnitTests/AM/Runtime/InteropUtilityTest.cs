using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

namespace UnitTests.AM.Runtime
{
    [TestClass]
    public class InteropUtilityTest
    {
        [StructLayout(LayoutKind.Explicit)]
        struct DummyStruct
        {
            [FieldOffset(0)]
            public int FirstField;

            [FieldOffset(4)]
            public short SecondField;

            [FieldOffset(6)]
            public int ThirdField;
        }

        [TestMethod]
        public void TestInteropUtilityByteArrayToStructure()
        {
            byte[] bytes =  {1, 0, 0, 0, 2, 0, 3, 0, 0, 0};
            DummyStruct dummy = InteropUtility
                    .ByteArrayToStructure<DummyStruct>
                    (
                        bytes
                    );
            Assert.AreEqual(1,dummy.FirstField);
            Assert.AreEqual(2,dummy.SecondField);
            Assert.AreEqual(3,dummy.ThirdField);
        }
    }
}
