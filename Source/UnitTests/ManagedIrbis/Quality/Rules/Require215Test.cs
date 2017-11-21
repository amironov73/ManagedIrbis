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
    public class Require215Test
        : RuleTest
    {
        [TestMethod]
        public void Require215_Construction_1()
        {
            Require215 check = new Require215();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require215_FieldSpec_1()
        {
            Require215 check = new Require215();
            Assert.AreEqual("215", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require215_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require215 check = new Require215();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
