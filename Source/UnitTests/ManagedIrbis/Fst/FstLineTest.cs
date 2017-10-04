using System;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fst;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class FstLineTest
    {
        [NotNull]
        private FstLine _GetLine()
        {
            return new FstLine
            {
                LineNumber = 4,
                Tag = 12252,
                Method = FstIndexMethod.Method8,
                Format = "MHL,'/K=/'(v225^i,|%|d225/)"
            };
        }

        [TestMethod]
        public void FstLine_Construciton_1()
        {
            FstLine line = new FstLine();
            Assert.AreEqual(0, line.LineNumber);
            Assert.AreEqual(0, line.Tag);
            Assert.AreEqual(FstIndexMethod.Method0, line.Method);
            Assert.IsNull(line.Format);
            Assert.IsNull(line.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] FstLine first
            )
        {
            byte[] bytes = first.SaveToMemory();
            FstLine second = bytes.RestoreObjectFromMemory<FstLine>();
            Assert.AreEqual(first.LineNumber, second.LineNumber);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.Method, second.Method);
            Assert.AreEqual(first.Format, second.Format);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void FstLine_Serialization_1()
        {
            FstLine line = new FstLine();
            _TestSerialization(line);

            line.UserData = "User data";
            _TestSerialization(line);

            line = _GetLine();
            _TestSerialization(line);
        }

        [TestMethod]
        public void FstLine_Verify_1()
        {
            FstLine line = new FstLine();
            Assert.IsFalse(line.Verify(false));

            line = _GetLine();
            Assert.IsTrue(line.Verify(false));
        }

        [TestMethod]
        public void FstLine_ToFormat_1()
        {
            FstLine line = _GetLine();
            Assert.AreEqual("mpl,\'12252\',/,MHL,\'/K=/\'(v225^i,|%|d225/),\'\a\'", line.ToFormat());
        }

        [TestMethod]
        public void FstLine_ToXml_1()
        {
            FstLine line = new FstLine();
            Assert.AreEqual("<line><method>Method0</method></line>", XmlUtility.SerializeShort(line));

            line = _GetLine();
            Assert.AreEqual("<line><tag>12252</tag><method>Method8</method><format>MHL,'/K=/'(v225^i,|%|d225/)</format></line>", XmlUtility.SerializeShort(line));
        }

        [TestMethod]
        public void FstLine_ToJson_1()
        {
            FstLine line = new FstLine();
            Assert.AreEqual("{'method':0}", JsonUtility.SerializeShort(line));

            line = _GetLine();
            Assert.AreEqual("{'tag':12252,'method':8,'format':'MHL,\\'/K=/\\'(v225^i,|%|d225/)'}", JsonUtility.SerializeShort(line));
        }

        [TestMethod]
        public void FstLine_ToString_1()
        {
            FstLine line = new FstLine();
            Assert.AreEqual("0 0 (null)", line.ToString());

            line = _GetLine();
            Assert.AreEqual("12252 8 MHL,'/K=/'(v225^i,|%|d225/)", line.ToString());
        }
    }
}
