using ManagedIrbis.Pft;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftTextSeparatorTest
    {
        [TestMethod]
        public void PftTextSeparator_Construction_1()
        {
            PftTextSeparator separator = new PftTextSeparator();
            Assert.AreEqual("<%", separator.Open);
            Assert.AreEqual("%>", separator.Close);
        }

        [TestMethod]
        public void PftTextSeparator_Construction_2()
        {
            const string open = "<<", close = ">>";
            PftTextSeparator separator = new PftTextSeparator(open, close);
            Assert.AreEqual(open, separator.Open);
            Assert.AreEqual(close, separator.Close);
        }

        [TestMethod]
        public void PftTextSeparator_SeparateText_1()
        {
            PftTextSeparator separator = new PftTextSeparator();
            Assert.IsFalse(separator.SeparateText(""));
            Assert.AreEqual("", separator.Accumulator);
        }

        [TestMethod]
        public void PftTextSeparator_SeparateText_2()
        {
            PftTextSeparator separator = new PftTextSeparator();
            Assert.IsFalse(separator.SeparateText("<html> <%'Hello'%>"));
            Assert.AreEqual("<<<<html> >>>'Hello'", separator.Accumulator);
        }

        [TestMethod]
        public void PftTextSeparator_SeparateText_3()
        {
            PftTextSeparator separator = new PftTextSeparator();
            Assert.IsTrue(separator.SeparateText("<html> <%'Hello'"));
            Assert.AreEqual("<<<<html> >>>'Hello'", separator.Accumulator);
        }
    }
}
