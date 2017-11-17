using System.Collections.Generic;
using System.IO;
using System.Text;

using AM.IO;
using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable RedundantCast

namespace UnitTests.AM.IO
{
    [TestClass]
    public class ByteNavigatorTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private byte[] _GetData()
        {
            // 9 bytes
            return new byte[] {3, 14, 15, 9, 26, 5, 35, 89, 79};
        }

        [NotNull]
        private ByteNavigator _GetNavigator()
        {
            return new ByteNavigator(_GetData());
        }

        [TestMethod]
        public void ByteNavigator_Construction_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(9, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreEqual(EncodingUtility.DefaultEncoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_Construction_2()
        {
            ByteNavigator navigator = new ByteNavigator(_GetData(), 8);
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(8, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreEqual(EncodingUtility.DefaultEncoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_Construction_2a()
        {
            ByteNavigator navigator = new ByteNavigator(_GetData(), 10);
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(9, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreEqual(EncodingUtility.DefaultEncoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_Construction_3()
        {
            Encoding encoding = Encoding.ASCII;
            ByteNavigator navigator = new ByteNavigator(_GetData(), 7, encoding);
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(7, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreSame(encoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_Construction_3a()
        {
            Encoding encoding = Encoding.ASCII;
            ByteNavigator navigator = new ByteNavigator(_GetData(), 10, encoding);
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(9, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreSame(encoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_Construction_4()
        {
            Encoding encoding = Encoding.ASCII;
            ByteNavigator navigator = new ByteNavigator(_GetData(), 1, 7, encoding);
            Assert.AreEqual(1, navigator.Position);
            Assert.AreEqual(7, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreSame(encoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_Construction_4a()
        {
            Encoding encoding = Encoding.ASCII;
            ByteNavigator navigator = new ByteNavigator(_GetData(), 1, 10, encoding);
            Assert.AreEqual(1, navigator.Position);
            Assert.AreEqual(9, navigator.Length);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreSame(encoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_Clone_1()
        {
            ByteNavigator first = _GetNavigator();
            ByteNavigator second = first.Clone();
            Assert.AreEqual(first.Position, second.Position);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreSame(first.Encoding, second.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_FromFile_1()
        {
            string fileName = Path.Combine(TestDataPath, "EMPTY.MST");
            ByteNavigator navigator = ByteNavigator.FromFile(fileName);
            Assert.IsFalse(navigator.IsEOF);
            Assert.AreEqual(0, navigator.Position);
            Assert.AreEqual(36, navigator.Length);
            Assert.AreEqual(EncodingUtility.DefaultEncoding, navigator.Encoding);
        }

        [TestMethod]
        public void ByteNavigator_GetRemainingData_1()
        {
            ByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(2);
            byte[] data = navigator.GetRemainingData();
            Assert.IsNotNull(data);
            Assert.AreEqual(7, data.Length);
            Assert.AreEqual((byte)15, data[0]);
        }

        [TestMethod]
        public void ByteNavigator_GetRemainingData_2()
        {
            ByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(9);
            byte[] data = navigator.GetRemainingData();
            Assert.IsNull(data);
        }

        [TestMethod]
        public void ByteNavigator_IsControl_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsTrue(navigator.IsControl());
        }

        [TestMethod]
        public void ByteNavigator_IsDigit_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsDigit());
        }

        [TestMethod]
        public void ByteNavigator_IsEof_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsEOF);
            navigator.MoveAbsolute(navigator.Length);
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void ByteNavigator_IsLetter_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsLetter());
        }

        [TestMethod]
        public void ByteNavigator_IsLetterOrDigit_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsLetterOrDigit());
        }

        [TestMethod]
        public void ByteNavigator_IsNumber_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsNumber());
        }

        [TestMethod]
        public void ByteNavigator_IsPunctuation_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsPunctuation());
        }

        [TestMethod]
        public void ByteNavigator_IsSeparator_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsSeparator());
        }

        [TestMethod]
        public void ByteNavigator_IsSurrogate_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsSurrogate());
        }

        [TestMethod]
        public void ByteNavigator_IsSymbol_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsSymbol());
        }

        [TestMethod]
        public void ByteNavigator_IsWhiteSpace_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.IsFalse(navigator.IsWhiteSpace());
        }

        [TestMethod]
        public void ByteNavigator_MoveAbsolute_1()
        {
            ByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(1);
            Assert.AreEqual(1, navigator.Position);

            navigator.MoveAbsolute(9);
            Assert.AreEqual(9, navigator.Position);

            navigator.MoveAbsolute(-9);
            Assert.AreEqual(0, navigator.Position);

            navigator.MoveAbsolute(99);
            Assert.AreEqual(9, navigator.Position);
        }

        [TestMethod]
        public void ByteNavigator_MoveRelative_1()
        {
            ByteNavigator navigator = _GetNavigator();
            navigator.MoveRelative(1);
            Assert.AreEqual(1, navigator.Position);

            navigator.MoveRelative(8);
            Assert.AreEqual(9, navigator.Position);

            navigator.MoveRelative(1);
            Assert.AreEqual(9, navigator.Position);

            navigator.MoveRelative(-9);
            Assert.AreEqual(0, navigator.Position);

            navigator.MoveRelative(-1);
            Assert.AreEqual(0, navigator.Position);

            navigator.MoveRelative(99);
            Assert.AreEqual(9, navigator.Position);
        }

        [TestMethod]
        public void ByteNavigator_PeekByte_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.AreEqual((byte)3, navigator.PeekByte());
            Assert.AreEqual((byte)3, navigator.PeekByte());

            navigator.MoveRelative(1);
            Assert.AreEqual((byte)14, navigator.PeekByte());
            Assert.AreEqual((byte)14, navigator.PeekByte());

            navigator.MoveAbsolute(99);
            Assert.AreEqual(ByteNavigator.EOF, navigator.PeekByte());
            Assert.AreEqual(ByteNavigator.EOF, navigator.PeekByte());
        }

        [TestMethod]
        public void ByteNavigator_PeekChar_1()
        {
            byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
            ByteNavigator navigator = new ByteNavigator(data);
            Assert.AreEqual('H', navigator.PeekChar());
            Assert.AreEqual('H', navigator.PeekChar());
            navigator.MoveAbsolute(99);
            Assert.AreEqual('\0', navigator.PeekChar());
            Assert.AreEqual('\0', navigator.PeekChar());
        }

        [TestMethod]
        public void ByteNavigator_ReadByte_1()
        {
            ByteNavigator navigator = _GetNavigator();
            Assert.AreEqual(3, navigator.ReadByte());
            Assert.AreEqual(14, navigator.ReadByte());
            navigator.MoveAbsolute(99);
            Assert.AreEqual(-1, navigator.ReadByte());
            Assert.AreEqual(-1, navigator.ReadByte());
        }

        [TestMethod]
        public void ByteNavigator_ReadChar_1()
        {
            byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
            ByteNavigator navigator = new ByteNavigator(data);
            Assert.AreEqual('H', navigator.ReadChar());
            Assert.AreEqual('e', navigator.ReadChar());
            navigator.MoveAbsolute(99);
            Assert.AreEqual('\0', navigator.ReadChar());
            Assert.AreEqual('\0', navigator.ReadChar());
        }

        [TestMethod]
        public void ByteNavigator_ReadLine_1()
        {
            // CR + LF
            byte[] data = {72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100};
            ByteNavigator navigator = new ByteNavigator(data);
            Assert.AreEqual("Hello", navigator.ReadLine());
            Assert.AreEqual("World", navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
        }

        [TestMethod]
        public void ByteNavigator_ReadLine_2()
        {
            // LF only
            byte[] data = {72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100};
            ByteNavigator navigator = new ByteNavigator(data);
            Assert.AreEqual("Hello", navigator.ReadLine());
            Assert.AreEqual("World", navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
        }

        [TestMethod]
        public void ByteNavigator_SkipLine_1()
        {
            // CR + LF
            byte[] data = { 72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100 };
            ByteNavigator navigator = new ByteNavigator(data);
            navigator.ReadChar();
            navigator.SkipLine();
            Assert.AreEqual("World", navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
        }

        [TestMethod]
        public void ByteNavigator_SkipLine_2()
        {
            // LF only
            byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
            ByteNavigator navigator = new ByteNavigator(data);
            navigator.ReadChar();
            navigator.SkipLine();
            Assert.AreEqual("World", navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
            Assert.IsTrue(navigator.IsEOF);
            Assert.IsNull(navigator.ReadLine());
        }

        [TestMethod]
        public void ByteNavigator_SkipLine_3()
        {
            ByteNavigator navigator = _GetNavigator();
            navigator.MoveAbsolute(99);
            navigator.SkipLine();
            Assert.IsTrue(navigator.IsEOF);
        }
    }
}
