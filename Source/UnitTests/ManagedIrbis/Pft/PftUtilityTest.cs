using System;
using System.Text;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftUtilityTest
    {
        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void PftUtility_GetFieldCount_1()
        {
            MarcRecord record = _GetRecord();
            PftContext context = new PftContext(null)
            {
                Record = record
            };

            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 701));
            Assert.AreEqual(0, PftUtility.GetFieldCount(context, 710));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700, 710));
            Assert.AreEqual(1, PftUtility.GetFieldCount(context, 700, 701));
            Assert.AreEqual(3, PftUtility.GetFieldCount(context, 300));
            Assert.AreEqual(3, PftUtility.GetFieldCount(context, 300, 700));
        }

        [TestMethod]
        public void PftUtility_NodesToText_1()
        {
            PftNode[] nodes =
            {
                new PftUnconditionalLiteral("unconditional"),
                new PftConditionalLiteral("conditional", false),
                new PftRepeatableLiteral("repeatable")
            };
            string expected = "'unconditional' \"conditional\" |repeatable|";
            StringBuilder builder = new StringBuilder();
            PftUtility.NodesToText(builder, nodes);
            string actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_NodesToText_()
        {
            PftNode[] nodes =
            {
                new PftGroup
                {
                    Children =
                    {
                        new PftUnconditionalLiteral("unconditional"),
                        new PftConditionalLiteral("conditional", false)
                    }
                },
                new PftRepeatableLiteral("repeatable")
            };
            string expected = "('unconditional' \"conditional\") |repeatable|";
            StringBuilder builder = new StringBuilder();
            PftUtility.NodesToText(builder, nodes);
            string actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_NodesToText_3()
        {
            PftNode[] nodes = new PftNode[0];
            string expected = "";
            StringBuilder builder = new StringBuilder();
            PftUtility.NodesToText(builder, nodes);
            string actual = builder.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_1()
        {
            string expected = null;
            string actual = PftUtility.PrepareText(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_2()
        {
            string expected = string.Empty;
            string actual = PftUtility.PrepareText(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_3()
        {
            string expected = "There is single-line text";
            string actual = PftUtility.PrepareText(expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_4()
        {
            string text = "There is multi-line\ntext";
            string expected = "There is multi-linetext";
            string actual = PftUtility.PrepareText(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_5()
        {
            string text = "There is\rmulti-line\ntext";
            string expected = "There ismulti-linetext";
            string actual = PftUtility.PrepareText(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_PrepareText_6()
        {
            string text = "\r\n";
            string expected = string.Empty;
            string actual = PftUtility.PrepareText(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_1()
        {
            PftNode[] nodes = { new PftNode() };
            Assert.IsTrue(PftUtility.RequiresConnection(nodes));
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_2()
        {
            Mock<PftNode> mock = new Mock<PftNode>();
            PftNode node = mock.Object;
            mock.SetupGet(n => n.RequiresConnection).Returns(false);
            mock.SetupGet(n => n.Children).Returns(new PftNodeCollection(node));
            PftNode[] nodes = { node };
            Assert.IsFalse(PftUtility.RequiresConnection(nodes));
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_3()
        {
            PftNode node = new PftNode();
            Assert.IsTrue(PftUtility.RequiresConnection(node));
        }

        [TestMethod]
        public void PftUtility_RequiresConnection_4()
        {
            Mock<PftNode> mock = new Mock<PftNode>();
            PftNode node = mock.Object;
            mock.SetupGet(n => n.RequiresConnection).Returns(false);
            mock.SetupGet(n => n.Children).Returns(new PftNodeCollection(node));
            Assert.IsFalse(PftUtility.RequiresConnection(node));
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_1()
        {
            PftContext context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            IndexSpecification index = new IndexSpecification()
            {
                Kind = IndexKind.Literal,
                Literal = 2
            };
            int value = 123;
            int[] result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(value, result[1]);
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_2()
        {
            PftContext context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            IndexSpecification index = new IndexSpecification()
            {
                Kind = IndexKind.Expression,
                Expression = "1+1"
            };
            int value = 123;
            int[] result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(value, result[1]);
        }

        [TestMethod]
        public void PftUtility_SetArrayItem_3()
        {
            PftContext context = new PftContext(null);
            int[] array = { 1, 2, 3, 4 };
            IndexSpecification index = new IndexSpecification()
            {
                Kind = IndexKind.None
            };
            int value = 123;
            int[] result = PftUtility.SetArrayItem(context, array, index, value);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(value, result[0]);
        }
    }
}
