using System;
using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftGTest
    {
        private void _Execute
            (
                [NotNull] PftNode node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            context.Globals.Add(100, "Global100");
            context.Globals.Add(101, "^ASubA^BSubB");
            context.Globals.Add(102, "FirstRepeat\nSecondRepeat");
            context.Globals.Add(103, "FirstRepeat\nSecondRepeat\nThirdRepeat");
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftG_Construction_1()
        {
            PftG node = new PftG();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(0, node.Number);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('\0', node.Command);
        }

        [TestMethod]
        public void PftG_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.V, 1, 1, "g100");
            PftG node = new PftG(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(0, node.Number);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('\0', node.Command);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftG_Construction_3()
        {
            PftG node = new PftG("g100");
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(100, node.Number);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('g', node.Command);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftG_Construction_3a()
        {
            new PftG("ga");
        }

        [TestMethod]
        public void PftG_Construction_4()
        {
            PftG node = new PftG(100);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(100, node.Number);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('g', node.Command);
            Assert.AreEqual("100", node.Tag);
            Assert.AreEqual('\0', node.SubField);
        }

        [TestMethod]
        public void PftG_Construction_5()
        {
            PftG node = new PftG(100, 'a');
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(100, node.Number);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('g', node.Command);
            Assert.AreEqual("100", node.Tag);
            Assert.AreEqual('a', node.SubField);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftG_CompareNode_1()
        {
            PftG left = new PftG(100);
            PftG right = new PftG(100) {Number = 101};
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftG_Compile_1()
        {
            PftG node = new PftG(101, 'a');
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftG_Compile_2()
        {
            PftG node = new PftG();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftG_Execute_1()
        {
            PftG node = new PftG();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftG_Execute_2()
        {
            PftG node = new PftG(100);
            _Execute(node, "Global100");
        }

        [TestMethod]
        public void PftG_Execute_3()
        {
            PftG node = new PftG(99);
            _Execute(node, "");
        }

        [TestMethod]
        public void PftG_Execute_4()
        {
            PftG node = new PftG(101);
            _Execute(node, "^aSubA^bSubB");
        }

        [TestMethod]
        public void PftG_Execute_5()
        {
            PftG node = new PftG(101, 'a');
            _Execute(node, "SubA");
        }

        [TestMethod]
        public void PftG_Execute_6()
        {
            PftG node = new PftG(102);
            _Execute(node, "FirstRepeatSecondRepeat");
        }

        [TestMethod]
        public void PftG_Execute_7()
        {
            PftG node = new PftG(103);
            _Execute(node, "FirstRepeatSecondRepeatThirdRepeat");
        }

        [TestMethod]
        public void PftG_Execute_8()
        {
            PftGroup group = new PftGroup
            {
                Children =
                {
                    new PftG(102),
                    new PftHash()
                }
            };
            _Execute(group, "FirstRepeat\nSecondRepeat\n\n");
        }

        [TestMethod]
        public void PftG_Execute_9()
        {
            PftGroup group = new PftGroup
            {
                Children =
                {
                    new PftG(100),
                    new PftHash()
                }
            };
            _Execute(group, "Global100\n\n");
        }

        [TestMethod]
        public void PftG_Execute_10()
        {
            PftGroup group = new PftGroup
            {
                Children =
                {
                    new PftG(99),
                    new PftHash()
                }
            };
            _Execute(group, "\n");
        }

        [TestMethod]
        public void PftG_Execute_11()
        {
            PftG node = new PftG(100);
            node.LeftHand.Add(new PftRepeatableLiteral("=>"));
            _Execute(node, "=>Global100");
        }

        [TestMethod]
        public void PftG_Execute_12()
        {
            PftG node = new PftG(102);
            node.LeftHand.Add
                (
                    new PftRepeatableLiteral("=>")
                    {
                        IsPrefix = true,
                        Plus = false
                    }
                );
            _Execute(node, "=>FirstRepeat=>SecondRepeat");
        }

        [TestMethod]
        public void PftG_Execute_13()
        {
            PftG node = new PftG(102);
            node.LeftHand.Add
                (
                    new PftRepeatableLiteral("=>")
                    {
                        IsPrefix = true,
                        Plus = true
                    }
                );
            _Execute(node, "FirstRepeat=>SecondRepeat");
        }

        [TestMethod]
        public void PftG_Execute_14()
        {
            PftG node = new PftG(102);
            node.RightHand.Add
            (
                new PftRepeatableLiteral("<=")
                {
                    IsPrefix = false,
                    Plus = false
                }
            );
            _Execute(node, "FirstRepeat<=SecondRepeat<=");
        }

        [TestMethod]
        public void PftG_Execute_15()
        {
            PftG node = new PftG(102);
            node.RightHand.Add
            (
                new PftRepeatableLiteral("<=")
                {
                    IsPrefix = false,
                    Plus = true
                }
            );
            _Execute(node, "FirstRepeat<=SecondRepeat");
        }

        [TestMethod]
        public void PftG_Execute_16()
        {
            PftG node = new PftG(99);
            node.LeftHand.Add(new PftRepeatableLiteral("=>"));
            _Execute(node, "");
        }

        [TestMethod]
        public void PftG_Execute_17()
        {
            PftG node = new PftG(100, '*');
            _Execute(node, "Global100");
        }

        [TestMethod]
        public void PftG_Execute_18()
        {
            PftG node = new PftG(101, '*');
            _Execute(node, "SubA");
        }

        [TestMethod]
        public void PftG_PrettyPrint_1()
        {
            PftG node = new PftG(101, 'a');
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("g101^a", printer.ToString());
        }

        [TestMethod]
        public void PftG_PrettyPrint_2()
        {
            PftG node = new PftG(101, 'a')
            {
                LeftHand =
                {
                    new PftConditionalLiteral("=>", false)
                },
                RightHand =
                {
                    new PftRepeatableLiteral(";")
                    {
                        IsPrefix = false,
                        Plus = true
                    }
                }
            };
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\"=>\"g101^a+|;|", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftG first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftG second = (PftG) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftG_Serialization_1()
        {
            PftG node = new PftG();
            _TestSerialization(node);

            node = new PftG(100);
            _TestSerialization(node);

            node = new PftG(101, 'a');
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftG_ToString_1()
        {
            PftG node = new PftG(100);
            Assert.AreEqual("g100", node.ToString());

            node = new PftG(101, 'a');
            Assert.AreEqual("g101^a", node.ToString());
        }
    }
}
