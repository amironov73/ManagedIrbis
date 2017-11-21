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
    public class Require900Test
        : RuleTest
    {
        [TestMethod]
        public void Require900_Construction_1()
        {
            Require900 check = new Require900();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require900_FieldSpec_1()
        {
            Require900 check = new Require900();
            Assert.AreEqual("900", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require900_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require900 check = new Require900();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
