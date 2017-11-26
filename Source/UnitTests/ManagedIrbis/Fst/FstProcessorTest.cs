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

// ReSharper disable HeapView.ObjectAllocation.Evident

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
    }
}
