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
    public class PftValTest
    {
        private void _Execute
            (
                [NotNull] PftNode node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVal_Construction_1()
        {
            PftVal node = new PftVal();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftVal_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Val, 1, 1, "val");
            PftVal node = new PftVal(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftVal_Construction_3()
        {
            double value = 123.45;
            PftVal node = new PftVal(value);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(value, node.Value);
        }

        [TestMethod]
        public void PftVal_Compile_1()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftUnconditionalLiteral("123.45"));
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftVal_Compile_2()
        {
            PftVal node = new PftVal();
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftVal_Execute_1()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftUnconditionalLiteral("123.45"));
            PftF format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(5.0),
                Argument3 = new PftNumericLiteral(2.0)
            };
            _Execute(format, "123.45");
        }

        [TestMethod]
        public void PftVal_Execute_2()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftUnconditionalLiteral("123"));
            node.Children.Add(new PftComma());
            node.Children.Add(new PftUnconditionalLiteral(".45"));
            PftF format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(5.0),
                Argument3 = new PftNumericLiteral(2.0)
            };
            _Execute(format, "123.45");
        }

        [TestMethod]
        public void PftVal_Execute_3()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftNumericLiteral(123.45));
            PftF format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(5.0),
                Argument3 = new PftNumericLiteral(2.0)
            };
            _Execute(format, "123.45");
        }

        [TestMethod]
        public void PftVal_PrettyPrint_1()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftUnconditionalLiteral("123.45"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("val('123.45')", printer.ToString());
        }

        [TestMethod]
        public void PftVal_PrettyPrint_2()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftUnconditionalLiteral("123"));
            node.Children.Add(new PftComma());
            node.Children.Add(new PftUnconditionalLiteral(".45"));
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("val('123', '.45')", printer.ToString());
        }

        [TestMethod]
        public void PftVal_ToString_1()
        {
            PftVal node = new PftVal();
            Assert.AreEqual("val()", node.ToString());
        }

        [TestMethod]
        public void PftVal_ToString_2()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftUnconditionalLiteral("123.45"));
            Assert.AreEqual("val('123.45')", node.ToString());
        }

        [TestMethod]
        public void PftVal_ToString_3()
        {
            PftVal node = new PftVal();
            node.Children.Add(new PftUnconditionalLiteral("123"));
            node.Children.Add(new PftComma());
            node.Children.Add(new PftUnconditionalLiteral(".45"));
            Assert.AreEqual("val('123' , '.45')", node.ToString());
        }

        [TestMethod]
        public void PftVal_ToString_4()
        {
            PftVal node = new PftVal(123.45);
            Assert.AreEqual("val(123.45)", node.ToString());
        }
    }
}
