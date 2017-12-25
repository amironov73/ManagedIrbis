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
    public class PftConditionalLiteralTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftField node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            context.Globals.Add(100, "First");
            context.Globals.Append(100, "Second");
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private void _ExecuteUpper
            (
                [NotNull] MarcRecord record,
                [NotNull] PftField node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record,
                UpperMode = true
            };
            context.Globals.Add(100, "First");
            context.Globals.Append(100, "Second");
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private PftField _GetVNode
            (
                int tag,
                char code,
                bool suffix
            )
        {
            PftV result = new PftV(tag, code);

            if (suffix)
            {
                result.RightHand.Add
                    (
                        new PftConditionalLiteral(" <<", true)
                    );
            }
            else
            {
                result.LeftHand.Add
                    (
                        new PftConditionalLiteral(">> ", false)
                    );
            }

            return result;
        }

        [NotNull]
        private PftField _GetVNode2
            (
                int tag,
                char code,
                bool suffix
            )
        {
            PftV result = new PftV(tag, code);

            if (suffix)
            {
                result.RightHand.Add
                    (
                        new PftConditionalLiteral(" суффикс", true)
                    );
            }
            else
            {
                result.LeftHand.Add
                    (
                        new PftConditionalLiteral("префикс ", false)
                    );
            }

            return result;
        }

        [NotNull]
        private PftField _GetGNode
            (
                int tag,
                char code,
                bool suffix
            )
        {
            PftG result = new PftG(tag, code);

            if (suffix)
            {
                result.RightHand.Add
                    (
                        new PftConditionalLiteral(" <<", true)
                    );
            }
            else
            {
                result.LeftHand.Add
                    (
                        new PftConditionalLiteral(">> ", false)
                    );
            }

            return result;
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField(700);
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField(701);
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField(200);
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void PftConditionalLiteral_Construction_1()
        {
            PftConditionalLiteral node = new PftConditionalLiteral();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.IsSuffix);
            Assert.IsNull(node.Text);
        }

        [TestMethod]
        public void PftConditionalLiteral_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.ConditionalLiteral, 1, 1, "\"\"");
            PftConditionalLiteral node = new PftConditionalLiteral(token, false);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.IsSuffix);
            Assert.IsNotNull(node.Text);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionalLiteral_Construction_2a()
        {
            PftToken token = new PftToken(PftTokenKind.ConditionalLiteral, 1, 1, null);
            new PftConditionalLiteral(token, false);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftConditionalLiteral_CompareNode_1()
        {
            PftConditionalLiteral left = new PftConditionalLiteral("Hello", false);
            PftConditionalLiteral right = new PftConditionalLiteral("Hello", true);
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftConditionalLiteral_Compile_1()
        {
            PftNode node = _GetVNode(200, 'a', true);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftConditionalLiteral_Compile_2()
        {
            PftNode node = _GetGNode(100, '\0', true);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionalLiteral_Compile_3()
        {
            PftNode node = new PftConditionalLiteral("text", false);
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode(200, 'a', false);
            _Execute(record, node, ">> Заглавие");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode(200, 'a', true);
            _Execute(record, node, "Заглавие <<");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode(201, 'a', false);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode(201, 'a', true);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_5()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetGNode(100, '\0', false);
            _Execute(record, node, ">> FirstSecond");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_6()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetGNode(100, '\0', true);
            _Execute(record, node, "FirstSecond <<");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_7()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetGNode(101, '\0', false);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_8()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetGNode(101, '\0', true);
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_9()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode(300, '\0', false);
            _Execute(record, node, ">> Первое примечаниеВторое примечаниеТретье примечание");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_10()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode(300, '\0', true);
            _Execute(record, node, "Первое примечаниеВторое примечаниеТретье примечание <<");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_11()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode2(200, 'a', false);
            _ExecuteUpper(record, node, "ПРЕФИКС ЗАГЛАВИЕ");
        }

        [TestMethod]
        public void PftConditionalLiteral_Execute_12()
        {
            MarcRecord record = _GetRecord();
            PftField node = _GetVNode2(200, 'a', true);
            _ExecuteUpper(record, node, "ЗАГЛАВИЕ СУФФИКС");
        }

        [TestMethod]
        public void PftConditionalLiteral_Optimize_1()
        {
            PftConditionalLiteral node = new PftConditionalLiteral();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftConditionalLiteral_Optimize_2()
        {
            PftConditionalLiteral node = new PftConditionalLiteral(string.Empty, true);
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftConditionalLiteral_Optimize_3()
        {
            PftConditionalLiteral node = new PftConditionalLiteral("Hello", true);
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_1()
        {
            PftField node = _GetVNode(200, 'a', true);
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("v200^a\" <<\"", printer.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_2()
        {
            PftField node = _GetVNode(200, 'a', false);
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\">> \"v200^a", printer.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_3()
        {
            PftField node = _GetGNode(100, '\0', true);
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("g100\" <<\"", printer.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_PrettyPrint_4()
        {
            PftField node = _GetGNode(100, '\0', false);
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\">> \"g100", printer.ToString());
        }

        private void _TestSerialization
            (
                [NotNull] PftNode first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftNode second = PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionalLiteral_Serialization_1()
        {
            PftNode node = new PftRepeatableLiteral();
            _TestSerialization(node);

            node = new PftRepeatableLiteral("Hello", true, true);
            _TestSerialization(node);

            node = _GetVNode(200, 'a', true);
            _TestSerialization(node);

            node = _GetVNode2(200, 'a', true);
            _TestSerialization(node);

            node = _GetGNode(100, '\0', true);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionalLiteral_ToString_1()
        {
            PftConditionalLiteral node = new PftConditionalLiteral();
            Assert.AreEqual("\"\"", node.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_ToString_2()
        {
            PftField node = _GetVNode(200, 'a', true);
            Assert.AreEqual("v200^a\" <<\"", node.ToString());

            node = _GetVNode(200, 'a', false);
            Assert.AreEqual("\">> \"v200^a", node.ToString());
        }

        [TestMethod]
        public void PftConditionalLiteral_ToString_3()
        {
            PftField node = _GetGNode(100, '\0', true);
            Assert.AreEqual("g100\" <<\"", node.ToString());

            node = _GetGNode(100, '\0', false);
            Assert.AreEqual("\">> \"g100", node.ToString());
        }
    }
}
