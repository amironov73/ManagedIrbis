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
    public class PlainTextProviderTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "records.txt"
                );
        }

        [NotNull]
        private PlainTextProvider _GetProvider()
        {
            return new PlainTextProvider(_GetFileName(), IrbisEncoding.Utf8);
        }

        [NotNull]
        private string _BrokenFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "broken_records.txt"
                );
        }

        [NotNull]
        private PlainTextProvider _BrokenProvider()
        {
            return new PlainTextProvider(_BrokenFileName(), IrbisEncoding.Utf8);
        }

        [TestMethod]
        public void PlainTextProvider_Construction_1()
        {
            using (PlainTextProvider provider = _GetProvider())
            {
                Assert.AreEqual(_GetFileName(), provider.FilePath);
                Assert.AreEqual(3, provider.GetMaxMfn());
            }
        }

        [TestMethod]
        public void PlainTextProvider_ReadRecord_1()
        {
            using (PlainTextProvider provider = _GetProvider())
            {
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNotNull(record);
                Assert.AreEqual(1, record.Mfn);
                Assert.AreEqual("records", record.Database);
                Assert.AreEqual(20, record.Fields.Count);
                Assert.AreEqual("rus", record.FM(101));
                Assert.AreEqual("80", record.FM(10, 'd'));
                Assert.AreEqual("1759089", record.FM(910, 'b'));
            }
        }

        [TestMethod]
        public void PlainTextProvider_ReadRecord_2()
        {
            using (PlainTextProvider provider = _GetProvider())
            {
                MarcRecord record = provider.ReadRecord(2);
                Assert.IsNotNull(record);
                Assert.AreEqual(2, record.Mfn);
                Assert.AreEqual("records", record.Database);
                Assert.AreEqual(23, record.Fields.Count);
                Assert.AreEqual("ASP", record.FM(920));
                Assert.AreEqual("Передовые охотники", record.FM(200, 'a'));
                Assert.AreEqual("2", record.FM(905, '2'));
            }
        }

        [TestMethod]
        public void PlainTextProvider_ReadRecord_3()
        {
            using (PlainTextProvider provider = _GetProvider())
            {
                MarcRecord record = provider.ReadRecord(100500);
                Assert.IsNull(record);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PlainTextProvider_ReadRecord_4()
        {
            using (PlainTextProvider provider = _BrokenProvider())
            {
                provider.ReadRecord(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PlainTextProvider_ReadRecord_5()
        {
            using (PlainTextProvider provider = _BrokenProvider())
            {
                provider.ReadRecord(2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PlainTextProvider_ReadRecord_6()
        {
            using (PlainTextProvider provider = _BrokenProvider())
            {
                provider.ReadRecord(3);
            }
        }

        [TestMethod]
        public void PlainTextProvider_SaveLayout_1()
        {
            using (PlainTextProvider provider = _GetProvider())
            {
                provider.SaveLayout();
            }

            using (PlainTextProvider provider = _GetProvider())
            {
                provider.DeleteLayout();
            }
        }

        //[TestMethod]
        //public void PlainTextProvider_ToString_1()
        //{
        //    using (PlainTextProvider provider = _GetProvider())
        //    {
        //        Assert.AreEqual(_GetFileName(), provider.ToString());
        //    }
        //}
    }
}
