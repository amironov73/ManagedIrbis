using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class ChunkedBufferTest
    {
        [TestMethod]
        public void ChunkedBuffer_Construction_1()
        {
            int chunkSize = 4096;
            ChunkedBuffer buffer = new ChunkedBuffer(chunkSize);
            Assert.AreEqual(chunkSize, buffer.ChunkSize);
            Assert.AreEqual(0, buffer.Length);
            Assert.IsTrue(buffer.Eof);
        }

        [TestMethod]
        public void ChunkedBuffer_Construction_2()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            Assert.AreEqual(ChunkedBuffer.DefaultChunkSize, buffer.ChunkSize);
            Assert.AreEqual(0, buffer.Length);
            Assert.IsTrue(buffer.Eof);
        }

        [TestMethod]
        public void ChunkedBuffer_CopyFrom_1()
        {
            MemoryStream memory = new MemoryStream();
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    memory.WriteByte((byte) j);
                }
            }

            memory.Position = 0;
            ChunkedBuffer buffer = new ChunkedBuffer();
            buffer.CopyFrom(memory, 1024);
            Assert.AreEqual(20 * 256, buffer.Length);
            byte[] original = memory.ToArray();
            byte[] copy = buffer.ToBigArray();
            CollectionAssert.AreEqual(original, copy);
        }

        [TestMethod]
        public void ChunkedBuffer_Read_1()
        {
            MemoryStream memory = new MemoryStream();
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    memory.WriteByte((byte) j);
                }
            }

            memory.Position = 0;
            ChunkedBuffer buffer = new ChunkedBuffer();
            buffer.CopyFrom(memory, 1024);
            buffer.Rewind();
            for (int i = 0; i < 20; i++)
            {
                Assert.IsFalse(buffer.Eof);
                byte[] bytes = new byte[256];
                Assert.AreEqual(256, buffer.Read(bytes));
                for (int j = 0; j < 256; j++)
                {
                    Assert.AreEqual(j, bytes[j]);
                }
            }
            Assert.IsTrue(buffer.Eof);
        }

        [TestMethod]
        public void ChunkedBuffer_Read_2()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            byte[] bytes = new byte[3];
            Assert.AreEqual(0, buffer.Read(bytes, 0, -1));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 0));
            Assert.AreEqual(3, buffer.Read(bytes, 0, 3));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 3));
        }

        [TestMethod]
        public void ChunkedBuffer_Read_3()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            byte[] bytes = new byte[3];
            Assert.AreEqual(0, buffer.Read(bytes, 0, -1));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 0));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 3));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 3));
        }

        [TestMethod]
        public void ChunkedBuffer_Peek_1()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.ReadByte());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_Peek_2()
        {
            MemoryStream memory = new MemoryStream();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    memory.WriteByte((byte) j);
                }
            }

            memory.Position = 0;
            ChunkedBuffer buffer = new ChunkedBuffer();
            buffer.CopyFrom(memory, 1024);
            buffer.Rewind();
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.ReadByte());
            int size = ChunkedBuffer.DefaultChunkSize - 1;
            byte[] bytes = new byte[size];
            Assert.AreEqual(size, buffer.Read(bytes));
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.ReadByte());
            Assert.AreEqual(size, buffer.Read(bytes));
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_Peek_3()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            buffer.WriteByte(6);
            buffer.Rewind();
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.ReadByte());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.ReadByte());
            Assert.AreEqual(4, buffer.Peek());
            Assert.AreEqual(4, buffer.Peek());
            Assert.AreEqual(4, buffer.ReadByte());
            Assert.AreEqual(5, buffer.Peek());
            Assert.AreEqual(5, buffer.Peek());
            Assert.AreEqual(5, buffer.ReadByte());
            Assert.AreEqual(6, buffer.Peek());
            Assert.AreEqual(6, buffer.Peek());
            Assert.AreEqual(6, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_Peek_4()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            buffer.WriteByte(6);
            buffer.Rewind();
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(2, buffer.ReadByte());
            Assert.AreEqual(3, buffer.ReadByte());
            Assert.AreEqual(4, buffer.ReadByte());
            Assert.AreEqual(5, buffer.ReadByte());
            Assert.AreEqual(6, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_ReadByte_1()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_ReadByte_2()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            buffer.WriteByte(1);
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_ToArrays_1()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            Assert.AreEqual(0, buffer.Length);
            byte[][] arrays = buffer.ToArrays(0);
            Assert.AreEqual(0, arrays.Length);
            arrays = buffer.ToArrays(1);
            Assert.AreEqual(1, arrays.Length);
            Assert.AreEqual(0, arrays[0].Length);
            arrays = buffer.ToArrays(2);
            Assert.AreEqual(2, arrays.Length);
            Assert.AreEqual(0, arrays[0].Length);
            Assert.AreEqual(0, arrays[1].Length);
        }

        [TestMethod]
        public void ChunkedBuffer_ToArrays_2()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            Assert.AreEqual(5, buffer.Length);
            byte[][] arrays = buffer.ToArrays(0);
            Assert.AreEqual(3, arrays.Length);
            Assert.AreEqual(2, arrays[0].Length);
            Assert.AreEqual(1, arrays[0][0]);
            Assert.AreEqual(2, arrays[0][1]);
            Assert.AreEqual(2, arrays[1].Length);
            Assert.AreEqual(3, arrays[1][0]);
            Assert.AreEqual(4, arrays[1][1]);
            Assert.AreEqual(1, arrays[2].Length);
            Assert.AreEqual(5, arrays[2][0]);
            arrays = buffer.ToArrays(1);
            Assert.AreEqual(4, arrays.Length);
            Assert.AreEqual(0, arrays[0].Length);
            Assert.AreEqual(2, arrays[1].Length);
            Assert.AreEqual(2, arrays[2].Length);
            Assert.AreEqual(1, arrays[3].Length);
        }

        [TestMethod]
        public void ChunkedBuffer_ToBigArray_1()
        {
            ChunkedBuffer buffer = new ChunkedBuffer();
            Assert.AreEqual(0, buffer.Length);
            byte[] array = buffer.ToBigArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void ChunkedBuffer_ToBigArray_2()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            Assert.AreEqual(5, buffer.Length);
            byte[] array = buffer.ToBigArray();
            Assert.AreEqual(5, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
            Assert.AreEqual(5, array[4]);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_1()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            byte[] array = new byte[0];
            buffer.Write(array);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write(array, 0, -1);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write(array, 0, 0);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_2()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            byte[] array = { 1, 2, 3, 4, 5 };
            buffer.Write(array);
            Assert.AreEqual(5, buffer.Length);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_3()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write("Hello", Encoding.ASCII);
            Assert.AreEqual(5, buffer.Length);
            buffer.Rewind();
            byte[] array = buffer.ToBigArray();
            Assert.AreEqual(5, array.Length);
            Assert.AreEqual(72, array[0]);
            Assert.AreEqual(101, array[1]);
            Assert.AreEqual(108, array[2]);
            Assert.AreEqual(108, array[3]);
            Assert.AreEqual(111, array[4]);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_4()
        {
            ChunkedBuffer buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write(string.Empty, Encoding.ASCII);
            Assert.AreEqual(0, buffer.Length);
            buffer.Rewind();
            byte[] array = buffer.ToBigArray();
            Assert.AreEqual(0, array.Length);
        }
    }
}
