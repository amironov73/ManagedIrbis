using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.CommandLine;

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class CommandLineUtilityTest
    {
        [TestMethod]
        public void CommandLineUtility_SplitText()
        {
            string[] items = CommandLineUtility.SplitText(string.Empty);
            Assert.AreEqual(0, items.Length);

            items = CommandLineUtility.SplitText("1");
            Assert.AreEqual(1, items.Length);

            items = CommandLineUtility.SplitText(" ");
            Assert.AreEqual(0, items.Length);

            items = CommandLineUtility.SplitText("1 2 3");
            Assert.AreEqual(3, items.Length);

            items = CommandLineUtility.SplitText(" 1    2 333");
            Assert.AreEqual(3, items.Length);

            items = CommandLineUtility.SplitText("1 \"2 3\" 4");
            Assert.AreEqual(3, items.Length);

            items = CommandLineUtility.SplitText("1 \"\" 2");
            Assert.AreEqual(3, items.Length);

            items = CommandLineUtility.SplitText("\"\"");
            Assert.AreEqual(1, items.Length);

            items = CommandLineUtility.SplitText(" \"\" ");
            Assert.AreEqual(1, items.Length);
        }

        [TestMethod]
        public void CommandLineUtility_WrapArgumentIfNeeded()
        {
            Assert.AreEqual
                (
                    null,
                    CommandLineUtility.WrapArgumentIfNeeded(null)
                );
            Assert.AreEqual
                (
                    string.Empty,
                    CommandLineUtility.WrapArgumentIfNeeded(string.Empty)
                );
            Assert.AreEqual
                (
                    "1",
                    CommandLineUtility.WrapArgumentIfNeeded("1")
                );
            Assert.AreEqual
                (
                    "\"1 1\"",
                    CommandLineUtility.WrapArgumentIfNeeded("1 1")
                );
        }
    }
}
