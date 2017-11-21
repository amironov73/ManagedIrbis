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
    public class Require60Test
        : RuleTest
    {
        [TestMethod]
        public void Require60_Construction_1()
        {
            Require60 check = new Require60();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require60_FieldSpec_1()
        {
            Require60 check = new Require60();
            Assert.AreEqual("60", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require60_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require60 check = new Require60();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
