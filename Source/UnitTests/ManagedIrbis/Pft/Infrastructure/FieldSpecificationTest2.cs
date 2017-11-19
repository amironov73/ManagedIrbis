using System.IO;
using JetBrains.Annotations;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class FieldSpecificationTest2
    {
        [TestMethod]
        public void FieldSpecification_ParseUnifor_1()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(200, specification.Tag);
            Assert.AreEqual("v200^a", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_1a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("g200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_1b()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("d200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_1c()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("n200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_2()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('\0', specification.SubField);
            Assert.AreEqual(200, specification.Tag);
            Assert.AreEqual("v200", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_3()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_4()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("va^a"));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_5()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^"));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_5a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^я"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_6()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a#10"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.Literal, specification.FieldRepeat.Kind);
            Assert.AreEqual(10, specification.FieldRepeat.Literal);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(200, specification.Tag);
            Assert.AreEqual("v200^a#10", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_6a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a#-1"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.Literal, specification.FieldRepeat.Kind);
            Assert.AreEqual(-1, specification.FieldRepeat.Literal);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(200, specification.Tag);
            Assert.AreEqual("v200^a#-1", specification.RawText);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_6b()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^a#-"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_7()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a*2#10"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.Literal, specification.FieldRepeat.Kind);
            Assert.AreEqual(10, specification.FieldRepeat.Literal);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(2, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(200, specification.Tag);
            Assert.AreEqual("v200^a*2#10", specification.RawText);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_7a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a*#10"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_8()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a.2#10"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.Literal, specification.FieldRepeat.Kind);
            Assert.AreEqual(10, specification.FieldRepeat.Literal);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(2, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(200, specification.Tag);
            Assert.AreEqual("v200^a.2#10", specification.RawText);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_8a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a.#10"));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_8b()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a.2*2#10"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor_9()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a*2.2#10"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.Literal, specification.FieldRepeat.Kind);
            Assert.AreEqual(10, specification.FieldRepeat.Literal);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(2, specification.Offset);
            Assert.AreEqual(2, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(200, specification.Tag);
            Assert.AreEqual("v200^a*2.2#10", specification.RawText);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_9a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^a#"));
        }

        [TestMethod]
        public void FieldSpecification_ToString_1()
        {
            FieldSpecification specification = new FieldSpecification
            {
                Command = 'v',
                Tag = 200,
                SubField = 'a'
            };
            string actual = specification.ToString();
            Assert.AreEqual("v200^a", actual);
        }

        [TestMethod]
        public void FieldSpecification_ToString_2()
        {
            FieldSpecification specification = new FieldSpecification
            {
                Command = 'v',
                Tag = 200,
                SubField = 'a',
                Embedded = "461"
            };
            string actual = specification.ToString();
            Assert.AreEqual("v200@461^a", actual);
        }

        [TestMethod]
        public void FieldSpecification_ToString_3()
        {
            FieldSpecification specification = new FieldSpecification
            {
                Command = 'v',
                Tag = 200,
                SubField = 'a',
                Offset = 10
            };
            string actual = specification.ToString();
            Assert.AreEqual("v200^a*10", actual);
        }

        [TestMethod]
        public void FieldSpecification_ToString_4()
        {
            FieldSpecification specification = new FieldSpecification
            {
                Command = 'v',
                Tag = 200,
                SubField = 'a',
                Length = 5
            };
            string actual = specification.ToString();
            Assert.AreEqual("v200^a.5", actual);
        }

        [TestMethod]
        public void FieldSpecification_ToString_5()
        {
            FieldSpecification specification = new FieldSpecification
            {
                Command = 'v',
                Tag = 200,
                SubField = 'a',
                Offset = 10,
                Length = 5
            };
            string actual = specification.ToString();
            Assert.AreEqual("v200^a*10.5", actual);
        }

        [TestMethod]
        public void FieldSpecification_ToString_6()
        {
            FieldSpecification specification = new FieldSpecification
            {
                Command = 'v',
                Tag = 200,
                SubField = 'a',
                ParagraphIndent = 10
            };
            string actual = specification.ToString();
            Assert.AreEqual("v200^a(10)", actual);
        }

        [TestMethod]
        public void FieldSpecification_Parse_TagSpecification_1()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v[200]^a"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(0, specification.Tag);
            Assert.AreEqual("200", specification.TagSpecification);
            Assert.AreEqual("v[200]^a", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse_TagSpecification_2()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v[$x + 2]^a"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual(0, specification.Tag);
            Assert.AreEqual("$x + 2", specification.TagSpecification);
            Assert.AreEqual("v[$x + 2]^a", specification.RawText);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_Parse_TagSpecification_3()
        {
            FieldSpecification specification = new FieldSpecification();
            specification.Parse("v[200");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_Parse_TagSpecification_4()
        {
            FieldSpecification specification = new FieldSpecification();
            specification.Parse("v[]");
        }

        [TestMethod]
        public void FieldSpecification_FirstLine_1()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(0, specification.FirstLine);
            specification.FirstLine = 10;
            Assert.AreEqual(10, specification.FirstLine);
        }

        [TestMethod]
        public void FieldSpecification_SubfieldSpecification_1()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsNull(specification.SubFieldSpecification);
            specification.SubFieldSpecification = "a";
            Assert.AreEqual("a", specification.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldSpecification_SubfieldSpecification_2()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^[a]"));
            Assert.AreEqual("a", specification.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldSpecification_SubfieldSpecification_3()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^[a]"));
            Assert.AreEqual("a", specification.SubFieldSpecification);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_SubfieldSpecification_3a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^[]"));
            Assert.AreEqual("a", specification.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldSpecification_SubfieldSpecification_4()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^["));
            Assert.AreEqual('[', specification.SubField);
        }

        [TestMethod]
        public void FieldSpecification_SubfieldSpecification_4a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^[*10.5"));
            Assert.AreEqual('[', specification.SubField);
            Assert.AreEqual(10, specification.Offset);
            Assert.AreEqual(5, specification.Length);
        }

        [TestMethod]
        public void FieldSpecification_SubfieldRepeat_1()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^a[123]"));
            Assert.AreEqual(IndexKind.Literal, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(123, specification.SubFieldRepeat.Literal);
        }

        [TestMethod]
        public void FieldSpecification_SubfieldRepeat_2()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^a[-]"));
            Assert.AreEqual(IndexKind.AllRepeats, specification.SubFieldRepeat.Kind);

            specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^a[.]"));
            Assert.AreEqual(IndexKind.CurrentRepeat, specification.SubFieldRepeat.Kind);

            specification = new FieldSpecification();
            Assert.IsTrue(specification.Parse("v200^a[$x]"));
            Assert.AreEqual(IndexKind.Expression, specification.SubFieldRepeat.Kind);
            Assert.AreEqual("$x", specification.SubFieldRepeat.Expression);
        }

        [TestMethod]
        public void FieldSpecification_Clone_1()
        {
            FieldSpecification first = new FieldSpecification();
            Assert.IsTrue(first.Parse("v200[$x]^a[$y]*10.5"));
            FieldSpecification second = (FieldSpecification) first.Clone();
            Assert.AreEqual(first.Command, second.Command);
            Assert.AreEqual(first.Embedded, second.Embedded);
            Assert.AreEqual(first.FirstLine, second.FirstLine);
            Assert.AreEqual(first.ParagraphIndent, second.ParagraphIndent);
            Assert.AreEqual(first.Offset, second.Offset);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreEqual(first.SubField, second.SubField);
            Assert.AreEqual(first.SubFieldSpecification, second.SubFieldSpecification);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.TagSpecification, second.TagSpecification);
            Assert.AreEqual(first.RawText, second.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Compare_1()
        {
            FieldSpecification left = new FieldSpecification();
            Assert.IsTrue(left.Parse("v200[$x]^a[$y]*10.5"));
            FieldSpecification right = new FieldSpecification();
            Assert.IsTrue(right.Parse("v200[$x]^a[$y]*10.5"));
            Assert.IsTrue(FieldSpecification.Compare(left, right));

            right = new FieldSpecification();
            Assert.IsTrue(right.Parse("n200[$x]^a[$y]*10.5"));
            Assert.IsFalse(FieldSpecification.Compare(left, right));

            right = new FieldSpecification();
            Assert.IsTrue(right.Parse("v201[$x]^a[$y]*10.5"));
            Assert.IsFalse(FieldSpecification.Compare(left, right));
        }

        private void _TestSerialization
            (
                [NotNull] FieldSpecification first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            first.Serialize(writer);
            byte[] memory = stream.ToArray();

            FieldSpecification second = new FieldSpecification();
            stream = new MemoryStream(memory);
            BinaryReader reader = new BinaryReader(stream);
            second.Deserialize(reader);

            Assert.AreEqual(first.Command, second.Command);
            Assert.AreEqual(first.Embedded, second.Embedded);
            Assert.AreEqual(first.FirstLine, second.FirstLine);
            Assert.AreEqual(first.ParagraphIndent, second.ParagraphIndent);
            Assert.AreEqual(first.Offset, second.Offset);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreEqual(first.SubField, second.SubField);
            Assert.AreEqual(first.SubFieldSpecification, second.SubFieldSpecification);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.TagSpecification, second.TagSpecification);
            Assert.AreEqual(first.RawText, second.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Serialization_1()
        {
            FieldSpecification specification = new FieldSpecification();
            _TestSerialization(specification);

            specification.Parse("v200[$x]^a[$y]*10.5");
            _TestSerialization(specification);
        }
    }
}
