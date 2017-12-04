using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.AM.IO
{
    [TestClass]
    public class DynamicStreamTest
    {
        [TestMethod]
        public void DynamicStream_Construciton_1()
        {
            DynamicStream stream = new DynamicStream();
            Assert.IsNull(stream.FlushHandler);
            Assert.IsNull(stream.SeekHandler);
            Assert.IsNull(stream.SetLengthHandler);
            Assert.IsNull(stream.ReadHandler);
            Assert.IsNull(stream.WriteHandler);
            Assert.IsNull(stream.CanReadHandler);
            Assert.IsNull(stream.CanSeekHandler);
            Assert.IsNull(stream.CanWriteHandler);
            Assert.IsNull(stream.GetLengthHandler);
            Assert.IsNull(stream.GetPositionHandler);
            Assert.IsNull(stream.SetPositionHandler);
            Assert.IsNull(stream.DisposeHandler);
            Assert.IsNull(stream.UserData);
        }

        [TestMethod]
        public void DynamicStream_Flush_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            stream.Flush(); // Nothing happens

            stream.FlushHandler = () => { flag = true; };
            stream.Flush();
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_Seek_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            stream.Seek(123, SeekOrigin.Begin); // Nothing happens

            stream.SeekHandler = (offset, origin) =>
            {
                flag = true;
                return offset;
            };
            Assert.AreEqual(123, stream.Seek(123, SeekOrigin.Begin));
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_SetLength_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            stream.SetLength(123); // Nonthing happens

            stream.SetLengthHandler = length => { flag = true; };
            stream.SetLength(123);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_Read_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            byte[] buffer = new byte[123];
            Assert.AreEqual(0, stream.Read(buffer, 0, buffer.Length)); // Nothing happens

            stream.ReadHandler = (buf, offset, length) =>
            {
                flag = true;
                return length;
            };
            Assert.AreEqual(buffer.Length, stream.Read(buffer, 0, buffer.Length));
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_Write_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            byte[] buffer = new byte[123];
            stream.Write(buffer, 0, buffer.Length); // Nothing happens

            stream.WriteHandler = (buf, offset, length) => { flag = true; };
            stream.Write(buffer, 0, buffer.Length);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_CanRead_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            Assert.IsFalse(stream.CanRead); // Default behavoir

            stream.CanReadHandler = () =>
            {
                flag = true;
                return true;
            };
            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_CanSeek_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            Assert.IsFalse(stream.CanSeek); // Default behavior

            stream.CanSeekHandler = () =>
            {
                flag = true;
                return true;
            };
            Assert.IsTrue(stream.CanSeek);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_CanWrite_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            Assert.IsFalse(stream.CanWrite); // Default behavior

            stream.CanWriteHandler = () =>
            {
                flag = true;
                return true;
            };
            Assert.IsTrue(stream.CanWrite);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_Length_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            Assert.AreEqual(0L, stream.Length); // Default behavior

            stream.GetLengthHandler = () =>
            {
                flag = true;
                return 123;
            };
            Assert.AreEqual(123L, stream.Length);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_GetPosition_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            Assert.AreEqual(0L, stream.Position); // Default behavior

            stream.GetPositionHandler = () =>
            {
                flag = true;
                return 123;
            };
            Assert.AreEqual(123L, stream.Position);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_SetPosition_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            stream.Position = 123; // Nothing happens

            stream.SetPositionHandler = position => { flag = true; };
            stream.Position = 123;
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void DynamicStream_Dispose_1()
        {
            bool flag = false;

            DynamicStream stream = new DynamicStream();
            stream.Dispose(); // Nothing happens

            stream.DisposeHandler = () => { flag = true; };
            stream.Dispose();
            Assert.IsTrue(flag);
        }
    }
}
