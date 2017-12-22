using System.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
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
    public class PftNumericExpressionTest
    {
        private void _Execute
            (
                [NotNull] PftNumericExpression node,
                double expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            double actual = node.Value;
            Assert.AreEqual(expected, actual, 1E-6);
        }

        [NotNull]
        private PftNumericExpression _GetNode()
        {
            return new PftNumericExpression
            {
                LeftOperand = new PftNumericExpression
                {
                    LeftOperand = new PftNumericLiteral(2),
                    Operation = "*",
                    RightOperand = new PftNumericLiteral(3.14)
                },
                Operation = "+",
                RightOperand = new PftAbs
                {
                    Children =
                    {
                        new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(100),
                            Operation = "-",
                            RightOperand = new PftPow
                            {
                                X = new PftNumericLiteral(2.78),
                                Y = new PftNumericLiteral(5)
                            }
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void PftNumericExpression_Construction_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0.0, node.Value);
            Assert.IsNull(node.LeftOperand);
            Assert.IsNull(node.Operation);
            Assert.IsNull(node.RightOperand);
        }

        [TestMethod]
        public void PftNumericExpression_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Number, 1, 1, "1");
            PftNumericExpression node = new PftNumericExpression(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0.0, node.Value);
            Assert.IsNull(node.LeftOperand);
            Assert.IsNull(node.Operation);
            Assert.IsNull(node.RightOperand);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftNumericExpression_CompareNode_1()
        {
            PftNumericExpression left = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "+",
                RightOperand = new PftNumericLiteral(2)
            };
            PftNumericExpression right = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "-",
                RightOperand = new PftNumericLiteral(2)
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftNumericExpression_Compile_1()
        {
            PftNumericExpression node = _GetNode();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftNumericExpression_Compile_2()
        {
            PftNumericExpression node = new PftNumericExpression();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftNumericExpression_Clone_1()
        {
            PftNumericExpression left = _GetNode();
            PftNumericExpression right = (PftNumericExpression)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftNumericExpression_Execute_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            _Execute(node, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftNumericExpression_Execute_1a()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1)
            };
            _Execute(node, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftNumericExpression_Execute_1b()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "+"
            };
            _Execute(node, 0);
        }

        [TestMethod]
        public void PftNumericExpression_Execute_2()
        {
            PftNumericExpression node = _GetNode();
            _Execute(node, 72.3243030367999);
        }

        [TestMethod]
        public void PftNumericExpression_Execute_3()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "+",
                RightOperand = new PftNumericLiteral(2)
            };
            _Execute(node, 3);
        }

        [TestMethod]
        public void PftNumericExpression_Execute_4()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "-",
                RightOperand = new PftNumericLiteral(2)
            };
            _Execute(node, -1);
        }

        [TestMethod]
        public void PftNumericExpression_Execute_5()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "*",
                RightOperand = new PftNumericLiteral(2)
            };
            _Execute(node, 2);
        }

        [TestMethod]
        public void PftNumericExpression_Execute_6()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "/",
                RightOperand = new PftNumericLiteral(2)
            };
            _Execute(node, 0.5);
        }

        [TestMethod]
        public void PftNumericExpression_Execute_7()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "%",
                RightOperand = new PftNumericLiteral(2)
            };
            _Execute(node, 1);
        }

        [TestMethod]
        public void PftNumericExpression_Execute_8()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "div",
                RightOperand = new PftNumericLiteral(2)
            };
            _Execute(node, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftNumericExpression_Execute_9()
        {
            PftNumericExpression node = new PftNumericExpression
            {
                LeftOperand = new PftNumericLiteral(1),
                Operation = "qqq",
                RightOperand = new PftNumericLiteral(2)
            };
            _Execute(node, 0);
        }

        [TestMethod]
        public void PftNumericExpression_GetNodeInfo_1()
        {
            PftNumericExpression node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("NumericExpression", info.Name);
        }

        [TestMethod]
        public void PftNumericExpression_Optimize_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftNumericExpression_Optimize_2()
        {
            PftNumericExpression node = _GetNode();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftNumericExpression_PrettyPrint_1()
        {
            PftNumericExpression node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("2 * 3.14 + abs(100 - pow(2.78, 5))", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftNumericExpression first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftNumericExpression second = (PftNumericExpression) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftNumericLiteral_Serialization_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }
        
        [TestMethod]
        public void PftNumericExpression_ToString_1()
        {
            PftNumericExpression node = new PftNumericExpression();
            Assert.AreEqual("", node.ToString());
        }

        [TestMethod]
        public void PftNumericExpression_ToString_2()
        {
            PftNumericExpression node = _GetNode();
            Assert.AreEqual("2*3.14+abs(100-pow(2.78,5))", node.ToString());
        }
    }
}
