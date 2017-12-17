using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Collections;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Walking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftProcedureManagerTest
    {
        private void _Execute
            (
                [NotNull] PftProcedureManager manager,
                [NotNull] string name,
                [NotNull] string argument,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            manager.Execute(context, name, argument);
            string actual = context.Output.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftProcedureManager _GetManager()
        {
            PftProcedureManager result = new PftProcedureManager();
            PftProcedure procedure = new PftProcedure
            {
                Name = "name",
                Body =
                {
                    new PftUnconditionalLiteral("Arg: "),
                    new PftVariableReference("arg")
                }
            };
            result.Registry.Add("name", procedure);

            return result;
        }

        [TestMethod]
        public void PftProcedureManager_Construction_1()
        {
            PftProcedureManager manager = new PftProcedureManager();
            Assert.IsNotNull(manager.Registry);
            Assert.AreEqual(0, manager.Registry.Count);
        }

        [TestMethod]
        public void PftProcedureManager_Execute_1()
        {
            PftProcedureManager manager = _GetManager();
            _Execute(manager, "name", "hello", "Arg: hello");
        }

        [TestMethod]
        public void PftProcedureManager_Execute_2()
        {
            PftProcedureManager manager = _GetManager();
            _Execute(manager, "name1", "hello", "");
        }

        private void _TestSerialization
            (
                [NotNull] PftProcedureManager first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            first.Serialize(writer);
            byte[] bytes = stream.ToArray();

            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftProcedureManager second = new PftProcedureManager();
            second.Deserialize(reader);

            Assert.AreEqual(first.Registry.Count, second.Registry.Count);
            Dictionary<string, PftProcedure>.KeyCollection keys = first.Registry.Keys;
            foreach (string key in keys)
            {
                PftProcedure left = first.Registry[key];
                PftProcedure right = second.Registry[key];
                Assert.AreEqual(left.Name, right.Name);
                PftSerializationUtility.CompareLists(left.Body, right.Body);
            }
        }

        [TestMethod]
        public void PftProcedureManager_Serialization_1()
        {
            PftProcedureManager manager = new PftProcedureManager();
            _TestSerialization(manager);

            manager = _GetManager();
            _TestSerialization(manager);
        }

        [TestMethod]
        public void PftProcedureManager_FindProcedure_1()
        {
            PftProcedureManager manager = _GetManager();
            PftProcedure procedure = manager.FindProcedure("name");
            Assert.IsNotNull(procedure);
            Assert.AreEqual("name", procedure.Name);

            procedure = manager.FindProcedure("noSuchProcedure");
            Assert.IsNull(procedure);
        }

        [TestMethod]
        public void PftProcedureManager_HaveProcedure_1()
        {
            PftProcedureManager manager = _GetManager();
            Assert.IsTrue(manager.HaveProcedure("name"));
            Assert.IsFalse(manager.HaveProcedure("noSuchProcedure"));
        }
    }
}
