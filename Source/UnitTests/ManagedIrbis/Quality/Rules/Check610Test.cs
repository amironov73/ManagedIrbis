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
    public class Check610Test
        : RuleTest
    {
        [TestMethod]
        public void Check610_Construction_1()
        {
            Check610 check = new Check610();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Check610_FieldSpec_1()
        {
            Check610 check = new Check610();
            Assert.AreEqual("610", check.FieldSpec);
        }

        [TestMethod]
        public void Check610_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Check610 check = new Check610();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
