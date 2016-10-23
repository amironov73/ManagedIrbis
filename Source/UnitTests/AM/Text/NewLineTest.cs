using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class NewLineTest
    {
        private void _Test
            (
                string expected,
                Func<string,string> func,
                string argument
            )
        {
            string actual = func(argument);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NewLine_RemoveLineBreaks()
        {
            _Test(null, NewLine.RemoveLineBreaks, null);
            _Test("", NewLine.RemoveLineBreaks, "");
            _Test("", NewLine.RemoveLineBreaks, "\n");
            _Test(" ", NewLine.RemoveLineBreaks, " ");
        }

        [TestMethod]
        public void NewLine_UnixToDos()
        {
            _Test(null, NewLine.UnixToDos, null);
            _Test("", NewLine.UnixToDos, "");
            _Test("\r\n", NewLine.UnixToDos, "\n");
            _Test(" ", NewLine.UnixToDos, " ");
        }

        [TestMethod]
        public void NewLine_DosToUnix()
        {
            _Test(null, NewLine.DosToUnix, null);
            _Test("", NewLine.DosToUnix, "");
            _Test("\n", NewLine.DosToUnix, "\r\n");
            _Test(" ", NewLine.DosToUnix, " ");
        }
    }
}
