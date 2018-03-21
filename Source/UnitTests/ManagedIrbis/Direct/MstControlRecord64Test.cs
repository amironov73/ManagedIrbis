using System.IO;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstControlRecord64Test
    {
        [TestMethod]
        public void MstControlRecord64_Construction_1()
        {
            MstControlRecord64 control = new MstControlRecord64();
            Assert.AreEqual(0, control.Reserv1);
            Assert.AreEqual(0, control.NextMfn);
            Assert.AreEqual(0L, control.NextPosition);
            Assert.AreEqual(0, control.Reserv2);
            Assert.AreEqual(0, control.Reserv3);
            Assert.AreEqual(0, control.Reserv4);
            Assert.AreEqual(0, control.Blocked);
        }

        [TestMethod]
        public void MstControlRecord64_Read_1()
        {
            byte[] bytes = { 0, 0, 0, 0, 0, 0, 0, 111, 0, 188, 97,
                78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 1 };
            MemoryStream stream = new MemoryStream(bytes);
            MstControlRecord64 control = MstControlRecord64.Read(stream);
            Assert.AreEqual(0, control.Reserv1);
            Assert.AreEqual(111, control.NextMfn);
            Assert.AreEqual(12345678L, control.NextPosition);
            Assert.AreEqual(0, control.Reserv2);
            Assert.AreEqual(0, control.Reserv3);
            Assert.AreEqual(0, control.Reserv4);
            Assert.AreEqual(1, control.Blocked);
        }

        [TestMethod]
        public void MstControlRecord64_Write_1()
        {
            MstControlRecord64 control = new MstControlRecord64
            {
                NextMfn = 111,
                NextPosition = 12345678L,
                Blocked = 1
            };
            MemoryStream stream = new MemoryStream();
            control.Write(stream);
            byte[] expected = { 0, 0, 0, 0, 0, 0, 0, 111, 0, 188, 97,
                78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 1 };
            byte[] actual = stream.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
