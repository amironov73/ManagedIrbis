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
    public class PftVTest
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

        [TestMethod]
        public void PftV_Construction_1()
        {
            PftV node = new PftV();
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
        public void PftV_Construction_2()
        {
            string text = "v200^a";
            FieldSpecification specification = new FieldSpecification(text);
            PftToken token = new PftToken(PftTokenKind.V, 1, 1, text)
            {
                UserData = specification
            };
            PftV node = new PftV(token);
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
            Assert.AreEqual('v', node.Command);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftV_Construction_3()
        {
            string text = "v200^a";
            PftV node = new PftV(text);
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
            Assert.AreEqual('v', node.Command);
            Assert.AreEqual(text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftV_Construction_3a()
        {
            string text = "v200^";
            new PftV(text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftV_Construction_3b()
        {
            string text = "q200";
            new PftV(text);
        }

        [TestMethod]
        public void PftV_Construction_4()
        {
            PftV node = new PftV(200, 'a');
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
            Assert.AreEqual('v', node.Command);
            Assert.AreEqual("v200^a", node.Text);
        }

        [TestMethod]
        public void PftV_Construction_5()
        {
            PftV node = new PftV(200);
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
            Assert.AreEqual('v', node.Command);
            Assert.AreEqual("v200", node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftV_CompareNode_1()
        {
            PftV left = new PftV(200, 'a');
            PftV right = new PftV(200, 'b');
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftV_Compile_1()
        {
            PftV node = new PftV(200, 'a');
            NullProvider provider = new NullProvider();
            PftCompiler compiler = new PftCompiler();
            compiler.SetProvider(provider);
            PftProgram program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftV_Clone_1()
        {
            PftV left = new PftV(200, 'a');
            PftV right = (PftV)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftV_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(200, 'a');
            _Execute(record, node, "Заглавие");
        }

        [TestMethod]
        public void PftV_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(200);
            _Execute(record, node, "^aЗаглавие^eподзаголовочное^fИ. И. Иванов, П. П. Петров");
        }

        [TestMethod]
        public void PftV_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(200, '*');
            _Execute(record, node, "Заглавие");
        }

        [TestMethod]
        public void PftV_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(300)
            {
                LeftHand = { new PftRepeatableLiteral("=>", true) }
            };
            _Execute(record, node, "=>Первое примечание=>Второе примечание=>Третье примечание");
        }

        [TestMethod]
        public void PftV_Execute_5()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(300)
            {
                RightHand = { new PftRepeatableLiteral(";") }
            };
            _Execute(record, node, "Первое примечание;Второе примечание;Третье примечание;");
        }

        [TestMethod]
        public void PftV_Execute_6()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(300)
            {
                RightHand = { new PftRepeatableLiteral(";", false, true) }
            };
            _Execute(record, node, "Первое примечание;Второе примечание;Третье примечание");
        }

        [TestMethod]
        public void PftV_Execute_7()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(333);
            _Execute(record, node, string.Empty);
        }

        [TestMethod]
        public void PftV_Execute_8()
        {
            MarcRecord record = _GetRecord();
            PftProgram node = new PftProgram
            {
                Children =
                    {
                        new PftMode("mpu"),
                        new PftV(200, 'a')
                    }
            };
            _Execute(record, node, "ЗАГЛАВИЕ");
        }

        [TestMethod]
        public void PftV_Execute_9()
        {
            MarcRecord record = _GetRecord();
            PftProgram node = new PftProgram
            {
                Children =
                    {
                        new PftMode("mdl"),
                        new PftV(200)
                    }
            };
            _Execute(record, node, "Заглавие, подзаголовочное, И. И. Иванов, П. П. Петров.  ");
        }

        [TestMethod]
        public void PftV_Execute_10()
        {
            MarcRecord record = _GetRecord();
            PftProgram node = new PftProgram
            {
                Children =
                    {
                        new PftMode("mhl"),
                        new PftV(200)
                    }
            };
            _Execute(record, node, "Заглавие, подзаголовочное, И. И. Иванов, П. П. Петров");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftV_Execute_11()
        {
            MarcRecord record = _GetRecord();
            PftV node = new PftV(200, 'a')
            {
                LeftHand = { new PftV(300) }
            };
            _Execute(record, node, "");
        }

        [TestMethod]
        public void PftV_Execute_12()
        {
            PftV node = new PftV(200, 'a');
            _Execute(null, node, "");
        }

        [TestMethod]
        public void PftV_Execute_13()
        {
            MarcRecord record = _GetRecord();
            PftGroup node = new PftGroup
            {
                Children =
                {
                    new PftUnconditionalLiteral("=> "),
                    new PftV(300),
                    new PftHash()
                }
            };
            _Execute(record, node, "=> Первое примечание\n" +
                                   "=> Второе примечание\n" +
                                   "=> Третье примечание\n" +
                                   "=> \n");
        }
    }
}
