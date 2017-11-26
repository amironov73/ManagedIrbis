using System;

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

// ReSharper disable ObjectCreationAsStatement

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class FstProcessorTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private FstFile _GetFile()
        {
            FstFile result = new FstFile
            {
                FileName = "FST file"
            };

            result.Lines.Add(new FstLine
            {
                LineNumber = 1,
                Tag = 201,
                Method = FstIndexMethod.Method0,
                Format = "(v200 /)"
            });

            return result;
        }

        [TestMethod]
        [Description("Состояние объекта сразу после создания")]
        public void FstProcessor_Construction_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "dumb.fst"
                );
                FstProcessor processor = new FstProcessor
                (
                    provider,
                    specification
                );
                Assert.AreSame(provider, processor.Provider);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void FstProcessor_Construction_1a()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "nosuchfile.fst"
                );
                new FstProcessor
                (
                    provider,
                    specification
                );
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void FstProcessor_Construction_1b()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "empty.fst"
                );
                new FstProcessor
                (
                    provider,
                    specification
                );
            }
        }

        [TestMethod]
        [Description("Состояние объекта сразу после создания")]
        public void FstProcessor_Construction_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FstFile file = _GetFile();
                FstProcessor processor = new FstProcessor
                (
                    provider,
                    file
                );
                Assert.AreSame(file, processor.File);
                Assert.AreSame(provider, processor.Provider);
            }
        }

        [TestMethod]
        [Description("Извлечение терминов")]
        public void FstProcessor_ExtractTerms_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FstFile file = _GetFile();
                FstProcessor processor = new FstProcessor(provider, file);
                MarcRecord record = new MarcRecord();
                FstTerm[] terms = processor.ExtractTerms(record);
                Assert.AreEqual(0, terms.Length);
            }
        }

        [TestMethod]
        [Description("Извлечение терминов")]
        public void FstProcessor_ExtractTerms_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FstFile file = _GetFile();
                FstProcessor processor = new FstProcessor(provider, file);
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNotNull(record);
                FstTerm[] terms = processor.ExtractTerms(record);
                Assert.AreEqual(1, terms.Length);
            }
        }

        [TestMethod]
        [Description("Трансформация записи")]
        public void FstProcessor_TransformRecord_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FstFile file = _GetFile();
                FstProcessor processor = new FstProcessor(provider, file);
                MarcRecord source = new MarcRecord();
                MarcRecord target = processor.TransformRecord(source, file);
                Assert.AreEqual(0, target.Fields.Count);
            }
        }

        [TestMethod]
        [Description("Трансформация записи")]
        public void FstProcessor_TransformRecord_1a()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FstFile file = _GetFile();
                FstProcessor processor = new FstProcessor(provider, file);
                MarcRecord source = provider.ReadRecord(1);
                Assert.IsNotNull(source);
                MarcRecord target = processor.TransformRecord(source, file);
                Assert.AreEqual(1, target.Fields.Count);
            }
        }

        [TestMethod]
        [Description("Трансформация записи")]
        public void FstProcessor_TransformRecord_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FstFile file = _GetFile();
                string format = "mpl,'201',/,(v200 /),'\a'";
                FstProcessor processor = new FstProcessor(provider, file);
                MarcRecord source = new MarcRecord();
                MarcRecord target = processor.TransformRecord(source, format);
                Assert.AreEqual(0, target.Fields.Count);
            }
        }

        [TestMethod]
        [Description("Трансформация записи")]
        public void FstProcessor_TransformRecord_2a()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FstFile file = _GetFile();
                string format = "mpl,'201',/,(v200 /),'\a'";
                FstProcessor processor = new FstProcessor(provider, file);
                MarcRecord source = provider.ReadRecord(1);
                Assert.IsNotNull(source);
                MarcRecord target = processor.TransformRecord(source, format);
                Assert.AreEqual(1, target.Fields.Count);
            }
        }
    }
}
