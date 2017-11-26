using ManagedIrbis.Fst;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class FstTermTest
    {
        [TestMethod]
        [Description("Состояние объекта сразу после создания")]
        public void FstTerm_Construction_1()
        {
            FstTerm term = new FstTerm();
            Assert.AreEqual(0, term.Mfn);
            Assert.IsNull(term.Text);
            Assert.AreEqual(0, term.Tag);
            Assert.AreEqual(0, term.Occurrence);
            Assert.AreEqual(0, term.Offset);
        }

        [TestMethod]
        [Description("Присвоение значений свойствам")]
        public void FstTerm_Properties_1()
        {
            FstTerm term = new FstTerm();
            term.Mfn = 123;
            Assert.AreEqual(123, term.Mfn);
            term.Text = "Text";
            Assert.AreEqual("Text", term.Text);
            term.Tag = 234;
            Assert.AreEqual(234, term.Tag);
            term.Occurrence = 345;
            Assert.AreEqual(345, term.Occurrence);
            term.Offset = 456;
            Assert.AreEqual(456, term.Offset);
        }
    }
}
