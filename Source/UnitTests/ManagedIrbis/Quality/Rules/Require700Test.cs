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
    public class Require700Test
        : RuleTest
    {
        [TestMethod]
        public void Require700_Construction_1()
        {
            Require700 check = new Require700();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require700_FieldSpec_1()
        {
            Require700 check = new Require700();
            Assert.AreEqual("70[01]", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require700_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require700 check = new Require700();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
