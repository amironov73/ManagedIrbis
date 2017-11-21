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
    public class Require910Test
        : RuleTest
    {
        [TestMethod]
        public void Require910_Construction_1()
        {
            Require910 check = new Require910();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require910_FieldSpec_1()
        {
            Require910 check = new Require910();
            Assert.AreEqual("910", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require910_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require910 check = new Require910();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
