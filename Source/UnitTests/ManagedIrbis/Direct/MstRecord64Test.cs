using System;
using System.IO;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstRecord64Test
    {
        [TestMethod]
        public void MstRecord64_Construction_1()
        {
            MstRecord64 record = new MstRecord64();
            Assert.IsNotNull(record.Dictionary);
            Assert.IsNotNull(record.Leader);
            Assert.AreEqual(0L, record.Offset);
            Assert.IsFalse(record.Deleted);
        }
    }
}
