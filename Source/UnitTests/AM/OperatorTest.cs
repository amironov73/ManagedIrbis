using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class OperatorTest
    {
        [TestMethod]
        public void Operator_New()
        {
            Func<object> func = Operator<object>.New;
            object theObject = func();
            Assert.IsNotNull(theObject);
        }
    }
}
