using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class TypeUtilityTest
    {
        [TestMethod]
        public void TypeUtility_GetGenericType()
        {
            Type expectedType = typeof(List<int>);
            Type actualType = TypeUtility.GetGenericType
                (
                    "System.Collections.Generic.List",
                    "System.Int32"
                );
            Assert.AreEqual(expectedType, actualType);
        }

        [TestMethod]
        public void TypeUtility_GetType()
        {
            Type expecteType = typeof(List<int>);
            List<int> variable = new List<int>();
            Type actualType = TypeUtility.GetType(variable);
            Assert.AreEqual(expecteType, actualType);
        }
    }
}
