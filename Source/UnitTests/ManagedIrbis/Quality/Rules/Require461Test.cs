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
    public class Require461Test
        : RuleTest
    {
        [TestMethod]
        public void Require416_Construction_1()
        {
            Require461 check = new Require461();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require461_FieldSpec_1()
        {
            Require461 check = new Require461();
            Assert.AreEqual("461", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require461_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require461 check = new Require461();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
