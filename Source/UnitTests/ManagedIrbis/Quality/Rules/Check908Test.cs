using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Quality;
using ManagedIrbis.Quality.Rules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Quality.Rules
{
    [TestClass]
    public class Check908Test
        : RuleTest
    {
        [TestMethod]
        public void Check908_Construction_1()
        {
            Check908 check = new Check908();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check908_FieldSpec_1()
        {
            Check908 check = new Check908();
            Assert.AreEqual("908", check.FieldSpec);
        }

        [TestMethod]
        public void Check908_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Check908 check = new Check908();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
