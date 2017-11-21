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
    public class Check10Test
        : RuleTest
    {
        [TestMethod]
        public void Check10_Construction_1()
        {
            Check10 check = new Check10();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check10_FieldSpec_1()
        {
            Check10 check = new Check10();
            Assert.AreEqual("10", check.FieldSpec);
        }

        [TestMethod]
        public void Check10_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Check10 check = new Check10();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
