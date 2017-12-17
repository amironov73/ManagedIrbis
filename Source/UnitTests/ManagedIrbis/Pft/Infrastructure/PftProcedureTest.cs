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
    public class PftProcedureTest
    {
        private void _Execute
            (
                [NotNull] PftProcedure procedure,
                [NotNull] string argument,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            procedure.Execute(context, argument);
            string actual = context.Output.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftProcedure _GetNode()
        {
            return new PftProcedure
            {
                Name = "name",
                Body =
                {
                    new PftUnconditionalLiteral("Arg: "),
                    new PftVariableReference("arg")
                }
            };
        }

        [TestMethod]
        public void PftProcedure_Construction_1()
        {
            PftProcedure procedure = new PftProcedure();
            Assert.IsNull(procedure.Name);
            Assert.IsNotNull(procedure.Body);
        }

        [TestMethod]
        public void PftProcedure_Execute_1()
        {
            PftProcedure procedure = new PftProcedure();
            _Execute(procedure, "", "");
        }

        [TestMethod]
        public void PftProcedure_Execute_2()
        {
            PftProcedure procedure = _GetNode();
            _Execute(procedure, "hello", "Arg: hello");
        }

        private void _TestSerialization
            (
                [NotNull] PftProcedure first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            first.Serialize(writer);
            byte[] bytes = stream.ToArray();

            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftProcedure second = new PftProcedure();
            second.Deserialize(reader);

            Assert.AreEqual(first.Name, second.Name);
            PftSerializationUtility.CompareLists(first.Body, second.Body);
        }

        [TestMethod]
        public void PftProcedure_Serialization_1()
        {
            PftProcedure procedure = new PftProcedure();
            _TestSerialization(procedure);

            procedure = _GetNode();
            _TestSerialization(procedure);
        }

        private void _TestClone
            (
                [NotNull] PftProcedure first
            )
        {
            PftProcedure second = (PftProcedure) first.Clone();
            Assert.AreEqual(first.Name, second.Name);
            PftSerializationUtility.CompareLists(first.Body, second.Body);
        }

        [TestMethod]
        public void PftProcedure_Clone_1()
        {
            PftProcedure procedure = new PftProcedure();
            _TestClone(procedure);

            procedure = _GetNode();
            _TestClone(procedure);
        }

        [TestMethod]
        public void PftProcedure_ToString_1()
        {
            PftProcedure procedure = new PftProcedure();
            Assert.AreEqual("(null)", procedure.ToString());

            procedure = _GetNode();
            Assert.AreEqual("name", procedure.ToString());
        }
    }
}
