using System;

using AM;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Quality;
using ManagedIrbis.Quality.Rules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Quality.Rules
{
    [TestClass]
    public class Require101Test
        : RuleTest
    {
        [TestMethod]
        public void Require101_Construction_1()
        {
            Require101 check = new Require101();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require101_FieldSpec_1()
        {
            Require101 check = new Require101();
            Assert.AreEqual("101", check.FieldSpec);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void Require101_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require101 check = new Require101();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
