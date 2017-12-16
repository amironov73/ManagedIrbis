using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftVariableTest
    {
        [TestMethod]
        public void PftVariable_Construction_1()
        {
            PftVariable variable = new PftVariable();
            Assert.IsFalse(variable.IsNumeric);
            Assert.IsNull(variable.Name);
            Assert.IsNull(variable.StringValue);
            Assert.AreEqual(0.0, variable.NumericValue);
        }

        [TestMethod]
        public void PftVariable_Construction_2()
        {
            string name = "name";
            PftVariable variable = new PftVariable(name, true);
            Assert.IsTrue(variable.IsNumeric);
            Assert.AreSame(name, variable.Name);
            Assert.IsNull(variable.StringValue);
            Assert.AreEqual(0.0, variable.NumericValue);
        }

        [TestMethod]
        public void PftVariable_Construction_3()
        {
            string name = "name", value = "value";
            PftVariable variable = new PftVariable(name, value);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreSame(name, variable.Name);
            Assert.AreSame(value, variable.StringValue);
            Assert.AreEqual(0.0, variable.NumericValue);
        }

        [TestMethod]
        public void PftVariable_Construction_4()
        {
            string name = "name";
            double value = 3.14;
            PftVariable variable = new PftVariable(name, value);
            Assert.IsTrue(variable.IsNumeric);
            Assert.AreSame(name, variable.Name);
            Assert.IsNull(variable.StringValue);
            Assert.AreEqual(value, variable.NumericValue);
        }

        [TestMethod]
        public void PftVariable_ToString_1()
        {
            PftVariable variable = new PftVariable();
            Assert.AreEqual("(null): (null)", variable.ToString());
        }

        [TestMethod]
        public void PftVariable_ToString_2()
        {
            string name = "name";
            PftVariable variable = new PftVariable(name, true);
            Assert.AreEqual("name: 0", variable.ToString());
        }

        [TestMethod]
        public void PftVariable_ToString_3()
        {
            string name = "name", value = "value";
            PftVariable variable = new PftVariable(name, value);
            Assert.AreEqual("name: \"value\"", variable.ToString());
        }

        [TestMethod]
        public void PftVariable_ToString_3a()
        {
            string name = "name";
            PftVariable variable = new PftVariable(name, string.Empty);
            Assert.AreEqual("name: \"\"", variable.ToString());
        }

        [TestMethod]
        public void PftVariable_ToString_4()
        {
            string name = "name";
            double value = 3.14;
            PftVariable variable = new PftVariable(name, value);
            Assert.AreEqual("name: 3.14", variable.ToString());
        }
    }
}
