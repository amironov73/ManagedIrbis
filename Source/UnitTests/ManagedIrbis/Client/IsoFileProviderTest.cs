using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class IsoFileProviderTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "TEST1.ISO"
                );
        }

        [NotNull]
        private IsoFileProvider _GetProvider()
        {
            return new IsoFileProvider(_GetFileName(), IrbisEncoding.Ansi);
        }

        [TestMethod]
        public void IsoFileProvider_Construction_1()
        {
            using (IsoFileProvider provider = _GetProvider())
            {
                Assert.AreEqual(_GetFileName(), provider.FilePath);
                Assert.AreEqual(81, provider.GetMaxMfn());
            }
        }

        [TestMethod]
        public void IsoFileProvider_ReadRecord_1()
        {
            using (IsoFileProvider provider = _GetProvider())
            {
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNotNull(record);
                Assert.AreEqual(1, record.Mfn);
                Assert.AreEqual("TEST1", record.Database);
                Assert.AreEqual(16, record.Fields.Count);
                Assert.AreEqual(@"RU\NLR\bibl\3415", record.FM(1));
                Assert.AreEqual(@"20031126124354.0", record.FM(5));
                //Assert.AreEqual("5-7443-0043-09700", record.FM(10, 'a'));
                //Assert.AreEqual("Вып. 13.", record.FM(200, 'a'));
            }
        }

        [TestMethod]
        public void IsoFileProvider_ReadRecord_2()
        {
            using (IsoFileProvider provider = _GetProvider())
            {
                MarcRecord record = provider.ReadRecord(2);
                Assert.IsNotNull(record);
                Assert.AreEqual(2, record.Mfn);
                Assert.AreEqual("TEST1", record.Database);
                Assert.AreEqual(15, record.Fields.Count);
                Assert.AreEqual(@"RU\NLR\bibl\5996", record.FM(1));
                Assert.AreEqual(@"20031126114737.0", record.FM(5));
                //Assert.AreEqual("5-7443-0043-09700", record.FM(10, 'a'));
                //Assert.AreEqual("Вып. 13.", record.FM(200, 'a'));
            }
        }

        [TestMethod]
        public void IsoFileProvider_SaveLayout_1()
        {
            using (IsoFileProvider provider = _GetProvider())
            {
                provider.SaveLayout();
            }

            using (IsoFileProvider provider = _GetProvider())
            {
                provider.DeleteLayout();
            }
        }

        [TestMethod]
        public void IsoFileProvider_ToString_1()
        {
            using (IsoFileProvider provider = _GetProvider())
            {
                Assert.AreEqual(_GetFileName(), provider.ToString());
            }
        }
    }
}
