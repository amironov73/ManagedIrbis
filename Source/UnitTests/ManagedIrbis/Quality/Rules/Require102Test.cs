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
    public class Require102Test
        : RuleTest
    {
        [TestMethod]
        public void Require102_Construction_1()
        {
            Require102 check = new Require102();
            Assert.IsNotNull(check);
        }

        [TestMethod]
        public void Require102_FieldSpec_1()
        {
            Require102 check = new Require102();
            Assert.AreEqual("102", check.FieldSpec);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void Require102_CheckRecord_1()
        {
            RuleContext context = GetContext();
            Require102 check = new Require102();
            RuleReport report = check.CheckRecord(context);
            Assert.IsNotNull(report);
        }
    }
}
