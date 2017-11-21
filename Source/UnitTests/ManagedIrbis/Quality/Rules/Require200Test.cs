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
    public class Require200Test
        : RuleTest
    {
        [TestMethod]
        public void Require200_Construction_1()
        {
            Require200 check = new Require200();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require200_FieldSpec_1()
        {
            Require200 check = new Require200();
            Assert.AreEqual("200", check.FieldSpec);
        }

        [TestMethod]
        //[ExpectedException(typeof(VerificationException))]
        public void Require200_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require200 check = new Require200();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
