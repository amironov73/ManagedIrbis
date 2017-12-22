using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftMinusTest
    {
        private void _Execute
            (
                [NotNull] PftMinus node,
                 double expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            double actual = node.Value;
            Assert.AreEqual(expected, actual, 1E-6);
        }

        [NotNull]
        private PftMinus _GetNode()
        {
            return new PftMinus
            {
                Children =
                {
                    new PftNumericExpression
                    {
                        LeftOperand = new PftNumericLiteral(1),
                        Operation = "+",
                        RightOperand = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(2),
                            Operation = "*",
                            RightOperand = new PftNumericLiteral(3)
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void PftMinus_Construction_1()
        {
            PftMinus node = new PftMinus();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftMinus_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Minus, 1, 1, "-");
            PftMinus node = new PftMinus(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0.0, node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftMinus_Compile_1()
        {
            PftMinus node = _GetNode();
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
            PftMinus node = new PftMinus();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftMinus_Execute_1()
        {
            PftMinus node = new PftMinus();
            _Execute(node, 0);
        }

        [TestMethod]
        public void PftMinus_Execute_2()
        {
            PftMinus node = _GetNode();
            _Execute(node, -7);
        }

        [TestMethod]
        public void PftMinus_PrettyPrint_1()
        {
            PftMinus node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("-(1 + 2 * 3)", printer.ToString());
        }

        [TestMethod]
        public void PftMinus_ToString_1()
        {
            PftMinus node = new PftMinus();
            Assert.AreEqual("-()", node.ToString());
        }

        [TestMethod]
        public void PftMinus_ToString_2()
        {
            PftMinus node = _GetNode();
            Assert.AreEqual("-(1+2*3)", node.ToString());
        }
    }
}
