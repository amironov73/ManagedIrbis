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
        public void IrbisText_CleanupText()
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
    }
}