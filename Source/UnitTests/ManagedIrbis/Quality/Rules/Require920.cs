using System;

using AM;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Quality;
using ManagedIrbis.Quality.Rules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Quality.Rules
{
    [TestClass]
    public class Require920Test
        : RuleTest
    {
        [TestMethod]
        public void Require920_Construction_1()
        {
            Require920 check = new Require920();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require920_FieldSpec_1()
        {
            Require920 check = new Require920();
            Assert.AreEqual("920", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require920_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require920 check = new Require920();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
