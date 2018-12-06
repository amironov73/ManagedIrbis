using System.IO;

using AM.Text;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstControlRecord64Test
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void MstControlRecord64_Construction_1()
        {
            MstControlRecord64 control = new MstControlRecord64();
            Assert.AreEqual(0, control.CtlMfn);
            Assert.AreEqual(0, control.NextMfn);
            Assert.AreEqual(0L, control.NextPosition);
            Assert.AreEqual(0, control.MftType);
            Assert.AreEqual(0, control.RecCnt);
            Assert.AreEqual(0, control.Reserv1);
            Assert.AreEqual(0, control.Reserv2);
            Assert.AreEqual(0, control.Blocked);
        }

        [TestMethod]
        public void MstControlRecord64_Dump_1()
        {
            string fileName = Path.Combine(TestDataPath, "EMPTY.MST");
            StringWriter writer = new StringWriter();
            using (FileStream stream = File.OpenRead(fileName))
            {
                MstControlRecord64 control = MstControlRecord64.Read(stream);
                control.Dump(writer);
            }

            string expected = "CTLMFN: 0\nNXTMFN: 1\nNXTPOS: 36\nMFTYPE: 0\nRECCNT: 0\nMFCXX1: 0\nMFCXX2: 0\nLOCKED: 0\n";
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MstControlRecord64_Dump_2()
        {
            string fileName = Path.Combine(TestDataPath, "KZD.MST");
            StringWriter writer = new StringWriter();
            using (FileStream stream = File.OpenRead(fileName))
            {
                MstControlRecord64 control = MstControlRecord64.Read(stream);
                control.Dump(writer);
            }

            string expected = "CTLMFN: 0\nNXTMFN: 32\nNXTPOS: 37308\nMFTYPE: 0\nRECCNT: 0\nMFCXX1: 0\nMFCXX2: 0\nLOCKED: 0\n";
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MstControlRecord64_Dump_3()
        {
            string fileName = Path.Combine(Irbis64RootPath, "Datai/IBIS/ibis.mst");
            StringWriter writer = new StringWriter();
            using (FileStream stream = File.OpenRead(fileName))
            {
                MstControlRecord64 control = MstControlRecord64.Read(stream);
                control.Dump(writer);
            }

            string expected = "CTLMFN: 0\nNXTMFN: 333\nNXTPOS: 45843589\nMFTYPE: 0\nRECCNT: 0\nMFCXX1: 14\nMFCXX2: 0\nLOCKED: 0\n";
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MstControlRecord64_Read_1()
        {
            string fileName = Path.Combine(TestDataPath, "EMPTY.MST");
            using (FileStream stream = File.OpenRead(fileName))
            {
                MstControlRecord64 control = MstControlRecord64.Read(stream);
                Assert.AreEqual(0, control.CtlMfn);
                Assert.AreEqual(1, control.NextMfn);
                Assert.AreEqual(36L, control.NextPosition);
                Assert.AreEqual(0, control.MftType);
                Assert.AreEqual(0, control.RecCnt);
                Assert.AreEqual(0, control.Reserv1);
                Assert.AreEqual(0, control.Reserv2);
                Assert.AreEqual(0, control.Blocked);
            }
        }

        [TestMethod]
        public void MstControlRecord64_Read_2()
        {
            string fileName = Path.Combine(TestDataPath, "KZD.MST");
            using (FileStream stream = File.OpenRead(fileName))
            {
                MstControlRecord64 control = MstControlRecord64.Read(stream);
                Assert.AreEqual(0, control.CtlMfn);
                Assert.AreEqual(32, control.NextMfn);
                Assert.AreEqual(37308L, control.NextPosition);
                Assert.AreEqual(0, control.MftType);
                Assert.AreEqual(0, control.RecCnt);
                Assert.AreEqual(0, control.Reserv1);
                Assert.AreEqual(0, control.Reserv2);
                Assert.AreEqual(0, control.Blocked);
            }
        }

        [TestMethod]
        public void MstControlRecord64_Read_3()
        {
            string fileName = Path.Combine(Irbis64RootPath, "Datai/IBIS/ibis.mst");
            using (FileStream stream = File.OpenRead(fileName))
            {
                MstControlRecord64 control = MstControlRecord64.Read(stream);
                Assert.AreEqual(0, control.CtlMfn);
                Assert.AreEqual(333, control.NextMfn);
                Assert.AreEqual(45843589L, control.NextPosition);
                Assert.AreEqual(0, control.MftType);
                Assert.AreEqual(0, control.RecCnt);
                Assert.AreEqual(14, control.Reserv1);
                Assert.AreEqual(0, control.Reserv2);
                Assert.AreEqual(0, control.Blocked);
            }
        }

        [TestMethod]
        public void MstControlRecord64_Write_1()
        {
            MstControlRecord64 control = new MstControlRecord64
            {
                NextMfn = 111,
                NextPosition = 12345678L
            };
            string fileName = Path.GetTempFileName();
            using (FileStream stream = new FileStream(fileName, FileMode.Create,
                FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                control.Write(stream);
            }
            long length = new FileInfo(fileName).Length;
            Assert.AreEqual(MstControlRecord64.RecordSize, length);
        }
    }
}
