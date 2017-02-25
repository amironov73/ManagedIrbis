using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisTextTest
    {
        private void _TestCleanup
            (
                string source,
                string expected
            )
        {
            string actual = IrbisText.CleanupText(source);

            Assert.AreEqual
                (
                    expected,
                    actual
                );
        }

        [TestMethod]
        public void IrbisText_CleanupText_1()
        {
            _TestCleanup(null, null);
            _TestCleanup(string.Empty, string.Empty);
            _TestCleanup("У попа была собака", "У попа была собака");
            _TestCleanup("1 Hello. 1", "1 Hello. 1");
            _TestCleanup("2 Hello.. 1", "2 Hello. 1");
            _TestCleanup("3 Hello... 3", "3 Hello... 3");
            _TestCleanup("4 Hello.... 2", "4 Hello.. 2");
            _TestCleanup("5 Hello..... 4", "5 Hello.... 4");
            _TestCleanup("6 Hello...... 3", "6 Hello... 3");
            _TestCleanup("7 Hello....... 5", "7 Hello..... 5");
            _TestCleanup("8 Hello........ 4", "8 Hello.... 4");
            _TestCleanup("9 Hello......... 6", "9 Hello...... 6");

            _TestCleanup("Hello, world. - Hello again!", "Hello, world. - Hello again!");
            _TestCleanup("Hello, world. - . - Hello again!", "Hello, world. - Hello again!");
            _TestCleanup("Hello, world.. - .. - Hello again!", "Hello, world. - . - Hello again!");
            _TestCleanup("Hello, world.. - . - Hello again!", "Hello, world. - Hello again!");
            _TestCleanup("Hello, world.-.-Hello again!", "Hello, world.-.-Hello again!");
            _TestCleanup("Hello, world. - . - . - Hello again!", "Hello, world. - Hello again!");
            _TestCleanup("Hello, world. - . - . - . - Hello again!", "Hello, world. - Hello again!");
        }

        private void _TestIrbisToWindows
            (
                string source,
                string expected
           )
        {
            string actual = IrbisText.IrbisToWindows(source);

            Assert.AreEqual
                (
                    expected,
                    actual
                );
        }

        [TestMethod]
        public void IrbisText_IrbisToWindows_1()
        {
            _TestIrbisToWindows("", "");
            _TestIrbisToWindows(" ", " ");
            _TestIrbisToWindows("\x001F\x001E", "\r\n");
            _TestIrbisToWindows("У попа была собака", "У попа была собака");
            _TestIrbisToWindows("У попа была\x001F\x001Eсобака", "У попа была\r\nсобака");
        }

        private void _TestWindowsToIrbis
            (
                string source,
                string expected
           )
        {
            string actual = IrbisText.WindowsToIrbis(source);

            Assert.AreEqual
                (
                    expected,
                    actual
                );
        }

        [TestMethod]
        public void IrbisText_WindowsToIrbis_1()
        {
            _TestWindowsToIrbis("", "");
            _TestWindowsToIrbis(" ", " ");
            _TestWindowsToIrbis("\r\n", "\x001F\x001E");
            _TestWindowsToIrbis("У попа была собака", "У попа была собака");
            _TestWindowsToIrbis("У попа была\r\nсобака", "У попа была\x001F\x001Eсобака");
        }

        private void _TestSplit
            (
                string source,
                int expected
           )
        {
            int actual = IrbisText.SplitIrbisToLines(source).Length;

            Assert.AreEqual
                (
                    expected,
                    actual
                );
        }

        [TestMethod]
        public void IrbisText_SplitIrbisToLines_1()
        {
            _TestSplit("", 0);
            _TestSplit(" ", 1);
            _TestSplit("\x001F", 2);
            _TestSplit("У попа была собака", 1);
            _TestSplit("У попа была\x001Fсобака", 2);
        }
    }
}
