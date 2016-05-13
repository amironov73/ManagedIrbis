using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CodeJam;

namespace UnitTests.CodeJam
{
    [TestClass]
    public class CodeTest
    {
        private void _TestNotNull(object parameter)
        {
            Code.NotNull(()=>parameter);
        }

        [TestMethod]
        public void TestNotNull1()
        {
            _TestNotNull(new object());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNotNull2()
        {
            _TestNotNull(null);
        }

    }
}
