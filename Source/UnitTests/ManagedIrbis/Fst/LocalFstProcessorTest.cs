using System;
using System.IO;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fst;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class LocalFstProcessorTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void LocalFstProcessor_Construction_1()
        {
            string rootPath = Irbis64RootPath;
            using (LocalFstProcessor processor
                = new LocalFstProcessor(rootPath, "IBIS"))
            {
                Assert.IsNotNull(processor.Provider);
            }
        }

        [TestMethod]
        public void LocalFstProcessor_ReadFile_1()
        {
            string rootPath = Irbis64RootPath;
            string fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\dumb.fst"
                );
            using (LocalFstProcessor processor
                = new LocalFstProcessor(rootPath, "IBIS"))
            {
                FstFile fst = processor.ReadFile(fileName);
                Assert.IsNotNull(fst);
                Assert.AreEqual("dumb.fst", fst.FileName);
            }
        }

        [TestMethod]
        public void LocalFstProcessor_ReadFile_2()
        {
            string rootPath = Irbis64RootPath;
            string fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\empty.fst"
                );
            using (LocalFstProcessor processor
                = new LocalFstProcessor(rootPath, "IBIS"))
            {
                FstFile fst = processor.ReadFile(fileName);
                Assert.IsNull(fst);
            }
        }

        [TestMethod]
        public void LocalFstProcessor_ReadFile_2a()
        {
            string rootPath = Irbis64RootPath;
            string fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\empty2.fst"
                );
            using (LocalFstProcessor processor
                = new LocalFstProcessor(rootPath, "IBIS"))
            {
                FstFile fst = processor.ReadFile(fileName);
                Assert.IsNull(fst);
            }
        }

        [TestMethod]
        public void LocalFstProcessor_TransformRecord_1()
        {
            string rootPath = Irbis64RootPath;
            string fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\dumb.fst"
                );
            using (LocalFstProcessor processor
                = new LocalFstProcessor(rootPath, "IBIS"))
            {
                FstFile fst = processor.ReadFile(fileName);
                Assert.IsNotNull(fst);
                MarcRecord source = new MarcRecord();
                MarcRecord target = processor.TransformRecord(source, fst);
                Assert.AreEqual(0, target.Fields.Count);
            }
        }

        [TestMethod]
        public void LocalFstProcessor_TransformRecord_2()
        {
            string rootPath = Irbis64RootPath;
            string fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\dumb.fst"
                );
            using (LocalFstProcessor processor
                = new LocalFstProcessor(rootPath, "IBIS"))
            {
                FstFile fst = processor.ReadFile(fileName);
                Assert.IsNotNull(fst);
                MarcRecord source = processor.Provider.ReadRecord(1);
                Assert.IsNotNull(source);
                MarcRecord target = processor.TransformRecord(source, fst);
                Assert.AreEqual(1, target.Fields.Count);
            }
        }
    }
}
