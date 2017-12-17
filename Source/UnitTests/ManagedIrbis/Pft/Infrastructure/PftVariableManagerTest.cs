using System;
using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftVariableManagerTest
    {
        [NotNull]
        private PftVariableManager _GetSingleManager()
        {
            PftVariableManager result = new PftVariableManager(null);
            result.SetVariable("firstVar", "firstValue");
            result.SetVariable("secondVar", 3.14);

            return result;
        }

        [NotNull]
        private PftVariableManager _GetChildManager()
        {
            PftVariableManager parent = new PftVariableManager(null);
            parent.SetVariable("thirdVar", "thirdValue");
            parent.SetVariable("fourthVar", 2.78);
            PftVariableManager result = new PftVariableManager(parent);
            result.SetVariable("firstVar", "firstValue");
            result.SetVariable("secondVar", 3.14);

            return result;
        }

        [TestMethod]
        public void PftVariableManager_Construction_1()
        {
            PftVariableManager manager = new PftVariableManager(null);
            Assert.IsNull(manager.Parent);
            Assert.IsNotNull(manager.Registry);
            Assert.AreEqual(0, manager.Registry.Count);
        }

        [TestMethod]
        public void PftVariableManager_DumpVariables_1()
        {
            PftVariableManager manager = new PftVariableManager(null);
            StringWriter writer = new StringWriter();
            manager.DumpVariables(writer);
            string expected = "============================================================\n";
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableManager_DumpVariables_2()
        {
            PftVariableManager manager = _GetSingleManager();
            StringWriter writer = new StringWriter();
            manager.DumpVariables(writer);
            string expected = "firstVar: \"firstValue\"\nsecondVar: 3.14\n" +
                              "============================================================\n";
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableManager_DumpVariables_3()
        {
            PftVariableManager manager = _GetChildManager();
            StringWriter writer = new StringWriter();
            manager.DumpVariables(writer);
            string expected = "firstVar: \"firstValue\"\nsecondVar: 3.14\n" +
                              "============================================================\n" +
                              "fourthVar: 2.78\nthirdVar: \"thirdValue\"\n" +
                              "============================================================\n";
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableManager_GetAllVariables_1()
        {
            PftVariableManager manager = new PftVariableManager(null);
            PftVariable[] variables = manager.GetAllVariables();
            Assert.AreEqual(0, variables.Length);
        }

        [TestMethod]
        public void PftVariableManager_GetAllVariables_2()
        {
            PftVariableManager manager = _GetSingleManager();
            PftVariable[] variables = manager.GetAllVariables();
            Assert.AreEqual(2, variables.Length);
        }

        [TestMethod]
        public void PftVariableManager_GetAllVariables_3()
        {
            PftVariableManager manager = _GetChildManager();
            PftVariable[] variables = manager.GetAllVariables();
            Assert.AreEqual(4, variables.Length);
        }

        [TestMethod]
        public void PftVariableManager_GetExistingVariable_1()
        {
            PftVariableManager manager = new PftVariableManager(null);
            PftVariable variable = manager.GetExistingVariable("firstVar");
            Assert.IsNull(variable);
        }

        [TestMethod]
        public void PftVariableManager_GetExistingVariable_2()
        {
            PftVariableManager manager = _GetSingleManager();
            PftVariable variable = manager.GetExistingVariable("firstVar");
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable.Name);

            variable = manager.GetExistingVariable("noSuchVariable");
            Assert.IsNull(variable);
        }

        [TestMethod]
        public void PftVariableManager_GetExistingVariable_3()
        {
            PftVariableManager manager = _GetChildManager();
            PftVariable variable = manager.GetExistingVariable("firstVar");
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable.Name);

            variable = manager.GetExistingVariable("thirdVar");
            Assert.IsNotNull(variable);
            Assert.AreEqual("thirdVar", variable.Name);

            variable = manager.GetExistingVariable("noSuchVariable");
            Assert.IsNull(variable);
        }

        [TestMethod]
        public void PftVariableManager_OrCreateVariable_1()
        {
            PftVariableManager manager = new PftVariableManager(null);
            PftVariable variable = manager.GetOrCreateVariable("noSuchVariable", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("noSuchVariable", variable.Name);
        }

        [TestMethod]
        public void PftVariableManager_GetOrCreateVariable_2()
        {
            PftVariableManager manager = _GetSingleManager();
            PftVariable variable = manager.GetOrCreateVariable("firstVar", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable.Name);

            variable = manager.GetOrCreateVariable("noSuchVariable", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("noSuchVariable", variable.Name);
        }

        [TestMethod]
        public void PftVariableManager_GetCreateVariable_3()
        {
            PftVariableManager manager = _GetChildManager();
            PftVariable variable = manager.GetOrCreateVariable("firstVar", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable.Name);

            variable = manager.GetOrCreateVariable("thirdVar", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("thirdVar", variable.Name);

            variable = manager.GetOrCreateVariable("noSuchVariable", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("noSuchVariable", variable.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftVariableManager_GetOrCreateVariable_4()
        {
            PftVariableManager manager = _GetSingleManager();
            manager.GetOrCreateVariable("firstVar", true);
        }

        [TestMethod]
        public void PftVariable_SetVariable_1()
        {
            string name = "firstVar";
            string value = "secondValue";
            PftVariableManager manager = new PftVariableManager(null);
            PftVariable variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);
        }

        [TestMethod]
        public void PftVariable_SetVariable_2()
        {
            string name = "firstVar";
            string value = "secondValue";
            PftVariableManager manager = _GetSingleManager();
            PftVariable variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);

            name = "noSuchVar";
            variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);
        }

        [TestMethod]
        public void PftVariable_SetVariable_3()
        {
            string name = "firstVar";
            string value = "secondValue";
            PftVariableManager manager = _GetChildManager();
            PftVariable variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);

            name = "thirdVar";
            variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);

            name = "noSuchVar";
            variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);
        }

        [TestMethod]
        public void PftVariable_SetVariable_4()
        {
            string name = "noSuchVar";
            double value = 123.45;
            PftVariableManager manager = new PftVariableManager(null);
            PftVariable variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsTrue(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.NumericValue);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftVariable_SetVariable_5()
        {
            string name = "firstVar";
            double value = 123.45;
            PftVariableManager manager = _GetSingleManager();
            manager.SetVariable(name, value);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftVariable_SetVariable_6()
        {
            string name = "thirdVar";
            double value = 123.45;
            PftVariableManager manager = _GetChildManager();
            manager.SetVariable(name, value);
        }

        [TestMethod]
        public void PftVariable_SetVariable_7()
        {
            string name = "firstVar";
            PftContext context = new PftContext(null);
            PftVariableManager manager = new PftVariableManager(null);
            IndexSpecification index = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 1
            };
            manager.SetVariable(context, name, index, "line1");
            index.Literal = 2;
            manager.SetVariable(context, name, index, "line2");
            index.Literal = 3;
            manager.SetVariable(context, name, index, "line3");
            PftVariable variable = manager.GetExistingVariable(name);
            Assert.IsNotNull(variable);
            string expected = "line1\nline2\nline3";
            string actual = variable.StringValue.DosToUnix();
            Assert.AreEqual(expected, actual);
        }
    }
}
