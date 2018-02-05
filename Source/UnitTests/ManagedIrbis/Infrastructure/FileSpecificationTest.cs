using System;

using AM.Runtime;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Infrastructure;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Infrastructure
{
    [TestClass]
    public class FileSpecificationTest
    {
        [TestMethod]
        public void FileSpecification_Constructor_1()
        {
            FileSpecification specification = new FileSpecification();
            Assert.AreEqual(false, specification.BinaryFile);
            Assert.AreEqual(IrbisPath.System, specification.Path);
            Assert.AreEqual(null, specification.Database);
            Assert.AreEqual(null, specification.FileName);
            Assert.AreEqual(null, specification.Content);

            specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "brief.pft"
                );
            Assert.AreEqual(false, specification.BinaryFile);
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual(null, specification.Database);
            Assert.AreEqual("brief.pft", specification.FileName);
            Assert.AreEqual(null, specification.Content);

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
            Assert.AreEqual(null, specification.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FileSpecification_Constructor_2()
        {
            new FileSpecification
                (
                    IrbisPath.MasterFile,
                    null
                );
        }

        [TestMethod]
        public void FileSpecification_ToString_1()
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
            specification.Content = "Hello";
            Assert.AreEqual
                (
                    "2.IBIS.&brief.pft&Hello",
                    specification.ToString()
                );
        }

        [TestMethod]
        public void FileSpecification_Equals_1()
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
            Assert.AreEqual(first.Content, second.Content);
            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.Path, second.Path);
        }

        [TestMethod]
        public void FileSpecification_Serialization_1()
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
        public void FileSpecification_Verify_1()
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
        public void FileSpecification_GetHashCode_1()
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

        [TestMethod]
        public void FileSpecification_Parse_1()
        {
            FileSpecification specification = FileSpecification.Parse("2.IBIS.brief.pft");
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual("IBIS", specification.Database);
            Assert.AreEqual("brief.pft", specification.FileName);
            Assert.IsNull(specification.Content);
            Assert.IsFalse(specification.BinaryFile);
        }

        [TestMethod]
        public void FileSpecification_Parse_2()
        {
            FileSpecification specification = FileSpecification.Parse("0..iri.mnu");
            Assert.AreEqual(IrbisPath.System, specification.Path);
            Assert.IsNull(specification.Database);
            Assert.AreEqual("iri.mnu", specification.FileName);
            Assert.IsNull(specification.Content);
            Assert.IsFalse(specification.BinaryFile);
        }

        [TestMethod]
        public void FileSpecification_Parse_3()
        {
            FileSpecification specification = FileSpecification.Parse("2.IBIS.@doclad99.doc");
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual("IBIS", specification.Database);
            Assert.AreEqual("doclad99.doc", specification.FileName);
            Assert.IsNull(specification.Content);
            Assert.IsTrue(specification.BinaryFile);
        }

        [TestMethod]
        public void FileSpecification_Parse_4()
        {
            FileSpecification specification = FileSpecification.Parse("2.IBIS.brief.pft&Hello");
            Assert.AreEqual(IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual("IBIS", specification.Database);
            Assert.AreEqual("brief.pft", specification.FileName);
            Assert.AreEqual("Hello", specification.Content);
            Assert.IsFalse(specification.BinaryFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FileSpecification_Parse5()
        {
            FileSpecification.Parse("Hello");
        }
    }
}
