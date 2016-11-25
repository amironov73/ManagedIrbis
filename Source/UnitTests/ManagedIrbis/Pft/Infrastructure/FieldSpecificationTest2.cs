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
        public void FieldSpecification_ParseUnifor1()
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
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor1a()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("g200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor1b_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("d200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor1c_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("n200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor2()
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
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200", specification.RawText);
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor3()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("200^a"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor4()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("va^a"));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor5_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^"));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor5a_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^я"));
        }

        [TestMethod]
        public void FieldSpecification_ParseUnifor6()
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
            Assert.AreEqual("200", specification.Tag);
            Assert.AreEqual("v200^a#10", specification.RawText);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldSpecification_ParseUnifor6a_Exception()
        {
            FieldSpecification specification = new FieldSpecification();
            Assert.AreEqual(false, specification.ParseUnifor("v200^a#"));
        }
    }
}
