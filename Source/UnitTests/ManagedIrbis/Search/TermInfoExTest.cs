using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class TermInfoExTest
    {
        private TermInfoEx _GetTerm()
        {
            return new TermInfoEx
            {
                Mfn = 10,
                Text = "K=HELLO$",
                Count = 100,
                Formatted = "Hello world",
                Index = 11,
                Occurrence = 1,
                Tag = 610
            };
        }

        [TestMethod]
        public void TermInfoEx_Construction_1()
        {
            TermInfoEx term = new TermInfoEx();
            Assert.AreEqual(0, term.Count);
            Assert.IsNull(term.Text);
            Assert.AreEqual(0, term.Mfn);
            Assert.AreEqual(0, term.Tag);
            Assert.AreEqual(0, term.Occurrence);
            Assert.AreEqual(0, term.Index);
            Assert.IsNull(term.Formatted);
        }

        private void _TestSerialization
            (
                [NotNull] TermInfoEx first
            )
        {
            byte[] bytes = first.SaveToMemory();

            TermInfoEx second
                = bytes.RestoreObjectFromMemory<TermInfoEx>();

            Assert.AreEqual(first.Count, second.Count);
            Assert.AreEqual(first.Text, second.Text);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.Occurrence, second.Occurrence);
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Formatted, second.Formatted);
        }

        [TestMethod]
        public void TermInfoEx_Serialization_1()
        {
            TermInfoEx term = new TermInfoEx();
            _TestSerialization(term);

            term = _GetTerm();
            _TestSerialization(term);
        }

        [TestMethod]
        public void TermInfoEx_Verify_1()
        {
            TermInfoEx term = new TermInfoEx();
            Assert.IsFalse(term.Verify(false));

            term = _GetTerm();
            Assert.IsTrue(term.Verify(false));
        }

        [TestMethod]
        public void TermInfoEx_ToString_1()
        {
            TermInfoEx term = new TermInfoEx();
            Assert.AreEqual("0#(null)", term.ToString());

            term = _GetTerm();
            Assert.AreEqual("100#K=HELLO$", term.ToString());
        }

        [TestMethod]
        public void TermInfoEx_ToXml_1()
        {
            TermInfoEx term = new TermInfoEx();
            Assert.AreEqual("<termInfo />", XmlUtility.SerializeShort(term));

            term = _GetTerm();
            Assert.AreEqual("<termInfo count=\"100\" text=\"K=HELLO$\" mfn=\"10\" tag=\"610\" occurrence=\"1\" index=\"11\" formatted=\"Hello world\" />", XmlUtility.SerializeShort(term));
        }

        [TestMethod]
        public void TermInfo_ToJson_1()
        {
            TermInfoEx term = new TermInfoEx();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(term));

            term = _GetTerm();
            Assert.AreEqual("{'mfn':10,'tag':610,'occurrence':1,'index':11,'formatted':'Hello world','count':100,'text':'K=HELLO$'}", JsonUtility.SerializeShort(term));
        }
    }
}
