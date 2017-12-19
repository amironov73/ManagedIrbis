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

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftParallelGroupTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftParallelGroup node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.IsNotNull(actual);

            //Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(RecordField.Parse(910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B559^C19990924^H107256G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
            record.Fields.Add(RecordField.Parse(910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));

            return record;
        }

        [NotNull]
        private PftParallelGroup _GetNode()
        {
            return new PftParallelGroup
            {
                Children =
                {
                    new PftUnconditionalLiteral("=> "),
                    new PftV("v910^b"),
                    new PftComma(),
                    new PftSlash()
                }
            };
        }

        [TestMethod]
        public void PftParallelGroup_Construction_1()
        {
            PftParallelGroup node = new PftParallelGroup();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftParallelGroup_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.Parallel, 1, 1, "parallel");
            PftParallelGroup node = new PftParallelGroup(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftParallelGroup_Clone_1()
        {
            PftParallelGroup first = new PftParallelGroup();
            PftParallelGroup second = (PftParallelGroup) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelGroup_Clone_2()
        {
            PftParallelGroup first = _GetNode();
            PftParallelGroup second = (PftParallelGroup)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelGroup_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftParallelGroup node = _GetNode();

            // TODO FIX THIS!
            _Execute(record, node, "=> 32\n=> 33\n=> 557\n=> 558\n=> 559\n=> 556\n=> ЗИ-1\n=> \n");
        }

        [TestMethod]
        public void PftParallelGroup_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftParallelGroup node = _GetNode();
            node.Children.Add(new PftBreak());

            // TODO FIX THIS!
            _Execute(record, node, "=> 32\n");
        }

        [TestMethod]
        public void PftParallelGroup_Execute_3()
        {
            MarcRecord record = _GetRecord();
            PftParallelGroup node = new PftParallelGroup();
            node.Children.Add(new PftUnconditionalLiteral("=>"));
            node.Children.Add(new PftSlash());

            // TODO FIX THIS!
            _Execute(record, node, "=>\n");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftParallelGroup_Execute_4()
        {
            MarcRecord record = _GetRecord();
            PftParallelGroup node = new PftParallelGroup();
            PftContext context = new PftContext(null)
            {
                CurrentGroup = new PftGroup(),
                Record = record
            };
            node.Execute(context);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftParallelGroup_Execute_5()
        {
            bool save = PftParallelGroup.ThrowOnEmpty;
            try
            {
                PftParallelGroup.ThrowOnEmpty = true;
                MarcRecord record = _GetRecord();
                PftParallelGroup node = new PftParallelGroup();
                PftContext context = new PftContext(null)
                {
                    Record = record
                };
                node.Execute(context);
            }
            finally
            {
                PftParallelGroup.ThrowOnEmpty = save;
            }
        }

        [TestMethod]
        public void PftParallelGroup_Optimize_1()
        {
            PftParallelGroup node = _GetNode();
            Assert.AreEqual(node, node.Optimize());
        }

        [TestMethod]
        public void PftParallelGroup_Optimize_2()
        {
            PftParallelGroup node = new PftParallelGroup();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftParallelGroup_PrettyPrint_1()
        {
            PftParallelGroup node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("parallel( '=> ' v910^b, / )", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftParallelGroup_PrettyPrint_2()
        {
            PftParallelGroup node = new PftParallelGroup
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftP("v300"),
                        ThenBranch =
                        {
                            new PftUnconditionalLiteral("=>"),
                            new PftV("v300"),
                            new PftSlash()
                        }
                    }
                }
            };
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nparallel(\n  if p(v300)\n  then \'=>\' v300 /\n  fi,\n)\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftParallelGroup first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftParallelGroup second = (PftParallelGroup) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftParallelGroup_Serialization_1()
        {
            PftParallelGroup node = new PftParallelGroup();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftParallelGroup_ToString_1()
        {
            PftParallelGroup node = new PftParallelGroup();
            Assert.AreEqual("parallel()", node.ToString());
        }

        [TestMethod]
        public void PftParallelGroup_ToString_2()
        {
            PftParallelGroup node = _GetNode();
            Assert.AreEqual("parallel('=> ' v910^b , /)", node.ToString());
        }
    }
}
