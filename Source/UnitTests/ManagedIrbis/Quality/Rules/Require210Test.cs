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
    public class Require210Test
        : RuleTest
    {
        [TestMethod]
        public void Require210_Construction_1()
        {
            Require210 check = new Require210();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require210_FieldSpec_1()
        {
            Require210 check = new Require210();
            Assert.AreEqual("210", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require210_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require210 check = new Require210();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
