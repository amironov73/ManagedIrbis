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
    public class PftNTest
    {
        private void _Execute
            (
                [CanBeNull] MarcRecord record,
                [NotNull] PftNode node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
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

        [NotNull]
        private PftN _GetNode
            (
                int tag,
                char code
            )
        {
            return new PftN(tag, code)
            {
                LeftHand =
                {
                    new PftConditionalLiteral("absent", false)
                }
            };
        }

        [TestMethod]
        public void PftN_Construction_1()
        {
            PftN node = new PftN();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('\0', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('\0', node.Command);
        }

        [TestMethod]
        public void PftN_Construction_2()
        {
            string text = "n200^a";
            FieldSpecification specification = new FieldSpecification(text);
            PftToken token = new PftToken(PftTokenKind.V, 1, 1, text)
            {
                UserData = specification
            };
            PftN node = new PftN(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('a', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftN_Construction_3()
        {
            string text = "n200^a";
            PftN node = new PftN(text);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('a', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual(text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftN_Construction_3a()
        {
            string text = "n200^";
            new PftN(text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftN_Construction_3b()
        {
            string text = "q200";
            new PftN(text);
        }

        [TestMethod]
        public void PftN_Construction_4()
        {
            PftN node = new PftN(200, 'a');
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('a', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual("n200^a", node.Text);
        }

        [TestMethod]
        public void PftN_Construction_5()
        {
            PftN node = new PftN(200);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual("200", node.Tag);
            Assert.IsNull(node.TagSpecification);
            Assert.AreEqual('\0', node.SubField);
            Assert.IsNotNull(node.LeftHand);
            Assert.AreEqual(0, node.LeftHand.Count);
            Assert.IsNotNull(node.RightHand);
            Assert.AreEqual(0, node.RightHand.Count);
            Assert.AreEqual('n', node.Command);
            Assert.AreEqual("n200", node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftN_CompareNode_1()
        {
            PftN left = new PftN(200, 'a');
            PftN right = new PftN(200, 'b');
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftN_Compile_1()
        {
            PftN node = new PftN(200, 'a');
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftN_Clone_1()
        {
            PftN left = new PftN(200, 'a');
            PftN right = (PftN)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftN_Clone_2()
        {
            PftN left = _GetNode(200, 'a');
            PftN right = (PftN)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftN_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftN node = _GetNode(200, 'a');
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftN node = _GetNode(201, 'a');
            _Execute(record, node, "absent");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftN_Execute_11()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(200, 'a')
            {
                LeftHand = { new PftN(300) }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftN node = new PftN(200, 'a');
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup()
            {
                Children =
                {
                    _GetNode(200, 'a'),
                    new PftSlash()
                }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_5()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup()
            {
                Children =
                {
                    _GetNode(300, '\0'),
                    new PftSlash()
                }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_Execute_6()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup()
            {
                Children =
                {
                    new PftN(300)
                    {
                        LeftHand =
                        {
                            new PftRepeatableLiteral("absent", true)
                        }
                    },
                    new PftSlash()
                }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftN_ToString_1()
        {
            PftN node = new PftN(200);
            Assert.AreEqual("n200", node.ToString());
        }
    }
}
