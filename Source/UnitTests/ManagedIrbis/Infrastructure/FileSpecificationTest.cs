using System;
using AM.Runtime;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Infrastructure;

namespace UnitTests.ManagedIrbis.Infrastructure
{
    [TestClass]
    public class FileSpecificationTest
    {
        [TestMethod]
        public void TestFileSpecification_Construction()
        {
            FileSpecification specification = new FileSpecification();
            Assert.AreEqual(false, specification.BinaryFile);
            Assert.AreEqual(IrbisPath.System, specification.Path);
            Assert.AreEqual(null, specification.Database);
            Assert.AreEqual(null, specification.FileName);
            Assert.AreEqual(null, specification.Contents);

            specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "brief.pft"
                );
            Assert.AreEqual(false, specification.BinaryFile);
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual(null, specification.Database);
            Assert.AreEqual("brief.pft", specification.FileName);
            Assert.AreEqual(null, specification.Contents);

            specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "brief.pft"
                );
            Assert.AreEqual(false, specification.BinaryFile);
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual("IBIS", specification.Database);
            Assert.AreEqual("brief.pft", specification.FileName);
            Assert.AreEqual(null, specification.Contents);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestFileSpecification_Construction_Exception()
        {
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    null
                );
        }

        [TestMethod]
        public void TestFileSpecification_ToString()
        {
            FileSpecification specification = new FileSpecification();
            Assert.AreEqual
                (
                    "0..",
                    specification.ToString()
                );

            specification.Path = IrbisPath.MasterFile;
            specification.FileName = "brief.pft";
            Assert.AreEqual
                (
                    "2..brief.pft",
                    specification.ToString()
                );

            specification.Database = "IBIS";
            Assert.AreEqual
                (
                    "2.IBIS.brief.pft",
                    specification.ToString()
                );

            specification.BinaryFile = true;
            Assert.AreEqual
                (
                    "2.IBIS.@brief.pft",
                    specification.ToString()
                );

            specification.BinaryFile = false;
            specification.Contents = "Hello";
            Assert.AreEqual
                (
                    "2.IBIS.&brief.pft&Hello",
                    specification.ToString()
                );
        }

        [TestMethod]
        public void TestFileSpecification_Equals()
        {
            FileSpecification first = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "brief.pft"
                );
            FileSpecification second = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "BRIEF.pft"
                );
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(first.Equals((object)second));

            second.Database = "RDR";
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(first.Equals((object)second));

            Assert.IsFalse(first.Equals((object)null));
            Assert.IsTrue(first.Equals((object)first));

            first = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "brief.pft"
                );
            second = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "BRIEF.pft"
                );
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(first.Equals((object)second));
        }

        private void _TestSerialization
            (
                FileSpecification first
            )
        {
            byte[] bytes = first.SaveToMemory();

            FileSpecification second =
                bytes.RestoreObjectFromMemory<FileSpecification>();

            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.BinaryFile, second.BinaryFile);
            Assert.AreEqual(first.Contents, second.Contents);
            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Path, second.Path);
        }

        [TestMethod]
        public void TestFileSpecification_Serialization()
        {
            FileSpecification specification = new FileSpecification();
            _TestSerialization(specification);

            specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "brief.pft"
                );
            _TestSerialization(specification);

            specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "brief.pft"
                );
            _TestSerialization(specification);
        }

        [TestMethod]
        public void TestFileSpecification_Verify()
        {
            FileSpecification specification = new FileSpecification();
            Assert.IsFalse(specification.Verify(false));

            specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "brief.pft"
                );
            Assert.IsFalse(specification.Verify(false));

            specification = new FileSpecification
                (
                    IrbisPath.System,
                    "RC.MNU"
                );
            Assert.IsTrue(specification.Verify(false));

            specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "brief.pft"
                );
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void TestFileSpecification_GetHashCode()
        {
            FileSpecification first = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "brief.pft"
                );
            FileSpecification second = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "BRIEF.pft"
                );

            Assert.AreNotEqual
                (
                    first.GetHashCode(),
                    second.GetHashCode()
                );
        }
    }
}
