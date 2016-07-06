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
            Code.NotNull(parameter, "parameter");
        }

        [TestMethod]
        public void TestCodeNotNull()
        {
            _TestNotNull(new object());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCodeNotNullFail()
        {
            _TestNotNull(null);
        }

    }
}
