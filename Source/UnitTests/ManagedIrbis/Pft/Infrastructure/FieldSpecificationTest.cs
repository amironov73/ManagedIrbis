using System;
using ManagedIrbis.Pft;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Pft.Infrastructure;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class FieldSpecificationTest
    {
        [TestMethod]
        public void FieldSpecification_Construction()
        {
            FieldSpecification specification = new FieldSpecification();

            Assert.AreEqual('\0', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('\0', specification.SubField);
            Assert.AreEqual(null, specification.Tag);
            Assert.AreEqual(null, specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse1()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse2()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('\0', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse3()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.Parse("200^a"));
        }

        [TestMethod]
        public void FieldSpecification_Parse4()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v461@200"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual("200", specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('\0', specification.SubField);
            Assert.AreEqual("461", specification.Tag);
            Assert.AreEqual("v461@200", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse5()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200[2]^a[3]"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.Literal, specification.FieldRepeat.Kind);
            Assert.AreEqual(2, specification.FieldRepeat.Literal);
            Assert.AreEqual(IndexKind.Literal, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(3, specification.SubFieldRepeat.Literal);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200[2]^a[3]", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse6()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a[2]"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.Literal, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(2, specification.SubFieldRepeat.Literal);
            Assert.AreEqual(0, specification.FieldRepeat.Literal);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a[2]", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse7()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a[*]"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.LastRepeat, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a[*]", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse8()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a[+]"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.NewRepeat, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a[+]", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse9()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a*5"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(5, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a*5", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse10()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a.5"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(5, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a.5", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse11()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a*5.5"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(5, specification.Offset);
            Assert.AreEqual(5, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a*5.5", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse12()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v200^a(10)"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(10, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a(10)", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_Parse14()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.Parse("v461@200[2]^a[3]*4.5(6)"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual("200", specification.Embedded);
            Assert.AreEqual(6, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.Literal, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.Literal, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(2, specification.FieldRepeat.Literal);
            Assert.AreEqual(3, specification.SubFieldRepeat.Literal);
            Assert.AreEqual(4, specification.Offset);
            Assert.AreEqual(5, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("461", specification.Tag);
            Assert.AreEqual("v461@200[2]^a[3]*4.5(6)", specification.RawText);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_Parse_Exception1()
        {
            FieldSpecification specification = new FieldSpecification();
            specification.Parse("v200^");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_Parse_Exception2()
        {
            FieldSpecification specification = new FieldSpecification();
            specification.Parse("v200^a[]");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_Parse_Exception5()
        {
            FieldSpecification specification = new FieldSpecification();
            specification.Parse("v200^a*");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_Parse_Exception6()
        {
            FieldSpecification specification = new FieldSpecification();
            specification.Parse("v200^a.5*5");
        }

        [TestMethod]
        public void FieldSpecification_ParseShort1()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseShort("v200^a"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('a', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_ParseShort2()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(true, specification.ParseShort("v200"));
            Assert.AreEqual('v', specification.Command);
            Assert.AreEqual(null, specification.Embedded);
            Assert.AreEqual(0, specification.ParagraphIndent);
            Assert.AreEqual(IndexKind.None, specification.FieldRepeat.Kind);
            Assert.AreEqual(IndexKind.None, specification.SubFieldRepeat.Kind);
            Assert.AreEqual(0, specification.Offset);
            Assert.AreEqual(0, specification.Length);
            Assert.AreEqual('\0', specification.SubField);
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_ParseShort3()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseShort("200^a"));
        }
    }
}
