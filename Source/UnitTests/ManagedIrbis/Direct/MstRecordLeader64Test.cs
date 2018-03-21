using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM.Text;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstRecordLeader64Test
    {
        [TestMethod]
        public void MstRecordLeader64_Construction_1()
        {
            MstRecordLeader64 leader = new MstRecordLeader64();
            Assert.AreEqual(0, leader.Mfn);
            Assert.AreEqual(0, leader.Length);
            Assert.AreEqual(0L, leader.Previous);
            Assert.AreEqual(0, leader.Base);
            Assert.AreEqual(0, leader.Nvf);
            Assert.AreEqual(0, leader.Status);
            Assert.AreEqual(0, leader.Version);
        }

        [TestMethod]
        public void MstRecordLeader64_Read_1()
        {
            byte[] bytes = { 0, 0, 0, 123, 0, 0, 1, 65, 0, 1, 225,
                185, 0, 0, 0, 0, 0, 0, 0, 234, 0, 0, 0, 23, 0, 0, 0,
                11, 0, 0, 0, 111 };
            MemoryStream stream = new MemoryStream(bytes);
            MstRecordLeader64 leader = MstRecordLeader64.Read(stream);
            Assert.AreEqual(123, leader.Mfn);
            Assert.AreEqual(321, leader.Length);
            Assert.AreEqual(123321L, leader.Previous);
            Assert.AreEqual(234, leader.Base);
            Assert.AreEqual(23, leader.Nvf);
            Assert.AreEqual(111, leader.Status);
            Assert.AreEqual(11, leader.Version);
        }

        [TestMethod]
        public void MstRecordLeader64_Write_1()
        {
            MstRecordLeader64 leader = new MstRecordLeader64
            {
                Mfn = 123,
                Length = 321,
                Previous = 123321,
                Base = 234,
                Nvf = 23,
                Status = 111,
                Version = 11
            };
            MemoryStream stream = new MemoryStream();
            leader.Write(stream);
            byte[] expected = { 0, 0, 0, 123, 0, 0, 1, 65, 0, 1, 225,
                185, 0, 0, 0, 0, 0, 0, 0, 234, 0, 0, 0, 23, 0, 0, 0,
                11, 0, 0, 0, 111 };
            byte[] actual = stream.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MstRecordLeader64_ToString_1()
        {
            MstRecordLeader64 leader = new MstRecordLeader64
            {
                Mfn = 123,
                Length = 321,
                Previous = 123321,
                Base = 234,
                Nvf = 23,
                Status = 111,
                Version = 11
            };
            Assert.AreEqual("Mfn: 123, Length: 321, Previous: 123321, Base: 234, Nvf: 23, Status: 111, Version: 11", leader.ToString().DosToUnix());

        }
    }
}
