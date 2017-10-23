using System;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class FieldSpecificationTest2
    {
        [TestMethod]
        public void FieldSpecification_ParseUnifor_11()
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
        public void FieldSpecification_ParseUnifor_5_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^"));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_5a_Exception()
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
        public void FieldSpecification_PaeseUnifor_6b_Exception()
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
        public void FieldSpecification_ParseUnifor_7a_Exception()
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
        public void FieldSpecification_ParseUnifor_8a_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseUnifor("v200^a.#10"));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor_8b_Exception()
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
        public void FieldSpecification_ParseUnifor_9a_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^a#"));
        }

        [TestMethod]
        public void FieldSpecification_ToString1()
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
        public void FieldSpecification_ToString2()
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
        public void FieldSpecification_ToString3()
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
        public void FieldSpecification_ToString4()
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
        public void FieldSpecification_ToString5()
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
        public void FieldSpecification_ToString6()
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
        public void FieldSpecification_Parse_TagSpecification1()
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
        public void FieldSpecification_Parse_TagSpecification2()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v[$x + 2]^a"));
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

    }
}
