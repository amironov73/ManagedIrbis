using System;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl.Infrastructure;

namespace UnitTests.ManagedIrbis.Gbl.Infrastructure
{
    [TestClass]
    public class RepeatSpecificationTest
    {
        [TestMethod]
        public void RepeatSpecification_Constructor_1()
        {
            RepeatSpecification specification = new RepeatSpecification();
            Assert.AreEqual(RepeatKind.All, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_2()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.All);
            Assert.AreEqual(RepeatKind.All, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_3()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.Last);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_4()
        {
            RepeatSpecification specification
                = new RepeatSpecification(3);
            Assert.AreEqual(RepeatKind.Explicit, specification.Kind);
            Assert.AreEqual(3, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_5()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.Last, 4);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RepeatSpecification_Constructor_Exception_1()
        {
            RepeatSpecification specification
                = new RepeatSpecification((RepeatKind)10);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RepeatSpecification_Constructor_Exception_2()
        {
            RepeatSpecification specification
                = new RepeatSpecification(0);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RepeatSpecification_Constructor_Exception_3()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.Last, -1);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_1()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("*");
            Assert.AreEqual(RepeatKind.All, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_2()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("F");
            Assert.AreEqual(RepeatKind.ByFormat, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_3()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("L");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_4()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("1");
            Assert.AreEqual(RepeatKind.Explicit, specification.Kind);
            Assert.AreEqual(1, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_5()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("L-2");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatSpecification_Parse_Exception_1()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RepeatSpecification_Parse_Exception_2()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("A");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RepeatSpecification_Parse_Exception_3()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("L2");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RepeatSpecification_Parse_Exception_4()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("L--2");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RepeatSpecification_Parse_Exception_5()
        {
            RepeatSpecification specification
                = RepeatSpecification.Parse("0");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_ToString_1()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.All);
            Assert.AreEqual("*", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_2()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.ByFormat);
            Assert.AreEqual("F", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_3()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.Last);
            Assert.AreEqual("L", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_4()
        {
            RepeatSpecification specification
                = new RepeatSpecification(RepeatKind.Last, 4);
            Assert.AreEqual("L-4", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_5()
        {
            RepeatSpecification specification
                = new RepeatSpecification(4);
            Assert.AreEqual("4", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_6()
        {
            RepeatSpecification specification = new RepeatSpecification
                {
                    Kind = (RepeatKind)10,
                    Index = 11
                };
            Assert.AreEqual("Kind=10, Index=11", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_Verify_1()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.All
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_2()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.All,
                Index = 5
            };
            Assert.IsFalse(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_3()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.ByFormat
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_4()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.ByFormat,
                Index = 5
            };
            Assert.IsFalse(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_5()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.Last
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_6()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.Last,
                Index = 4
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_7()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.Explicit,
                Index = 4
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_8()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = RepeatKind.Explicit,
                Index = 0
            };
            Assert.IsFalse(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_9()
        {
            RepeatSpecification specification = new RepeatSpecification
            {
                Kind = (RepeatKind)10,
                Index = 0
            };
            Assert.IsFalse(specification.Verify(false));
        }

    }
}
