using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftLocalTest
    {
        private void _Execute
            (
                [NotNull] PftLocal node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            context.Variables.SetVariable("x", "OldX");
            context.Variables.SetVariable("y", "OldY");
            context.Variables.SetVariable("z", "OldZ");
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftLocal _GetNode()
        {
            return new PftLocal
            {
                Names = { "x", "y" },
                Children =
                {
                    new PftAssignment
                    {
                        Name = "x",
                        Children =
                        {
                            new PftUnconditionalLiteral("NewX")
                        }
                    },
                    new PftAssignment
                    {
                        Name = "y",
                        Children =
                        {
                            new PftUnconditionalLiteral("NewY")
                        }
                    },
                    new PftVariableReference("x"),
                    new PftSlash(),
                    new PftVariableReference("y"),
                    new PftSlash(),
                    new PftVariableReference("z")
                }
            };
        }

        [TestMethod]
        public void PftLocal_Construction_1()
        {
            PftLocal node = new PftLocal();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNotNull(node.Names);
            Assert.AreEqual(0, node.Names.Count);
        }

        [TestMethod]
        public void PftLocal_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Local, 1, 1, "local");
            PftLocal node = new PftLocal(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNotNull(node.Names);
            Assert.AreEqual(0, node.Names.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftLocal_Clone_1()
        {
            PftLocal first = new PftLocal();
            PftLocal second = (PftLocal)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLocal_Clone_2()
        {
            PftLocal first = _GetNode();
            PftLocal second = (PftLocal)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLocal_Execute_1()
        {
            PftLocal node = new PftLocal();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftLocal_Execute_2()
        {
            PftLocal node = _GetNode();
            _Execute(node, "NewX\nNewY\nOldZ");
        }

        [TestMethod]
        public void PftLocal_GetNodeInfo_1()
        {
            PftLocal node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Local", info.Name);
        }

        [TestMethod]
        public void PftLocal_PrettyPrint_1()
        {
            PftLocal node = new PftLocal();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nlocal \ndo\nend\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftLocal_PrettyPrint_2()
        {
            PftLocal node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual
                (
                    "\nlocal $x, $y\ndo\n  $x='NewX'; $y='NewY';$x / $y / $z\nend\n",
                    printer.ToString().DosToUnix()
                );
        }

        private void _TestSerialization
            (
                [NotNull] PftLocal first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftLocal second = (PftLocal)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLocal_Serialization_1()
        {
            PftLocal node = new PftLocal();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftLocal_ToString_1()
        {
            PftLocal node = new PftLocal();
            Assert.AreEqual("local  do  end", node.ToString());

            node = _GetNode();
            Assert.AreEqual("local $x, $y do $x='NewX'; $y='NewY'; $x / $y / $z end", node.ToString());
        }
    }
}
