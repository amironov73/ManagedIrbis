using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class BookInfoTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void BookInfo_Construction_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                MarcRecord record = provider.ReadRecord(1).ThrowIfNull();

                BookInfo bookInfo = new BookInfo(provider, record);
                Assert.AreSame(provider, bookInfo.Provider);
                Assert.AreSame(record, bookInfo.Record);
                Assert.AreEqual(42, bookInfo.Amount);
                Assert.IsNotNull(bookInfo.Authors);
                Assert.AreEqual(6, bookInfo.Authors.Length);
                Assert.IsNotNull(bookInfo.Description);
                Assert.IsTrue(bookInfo.Electronic);
                Assert.IsNotNull(bookInfo.Exemplars);
                Assert.AreEqual(7, bookInfo.Exemplars.Length);
                Assert.IsNotNull(bookInfo.Languages);
                Assert.AreEqual(1, bookInfo.Languages.Length);
                Assert.IsNull(bookInfo.Link);
                Assert.IsNotNull(bookInfo.Publishers);
                Assert.AreEqual(1, bookInfo.Publishers.Length);
                Assert.IsNotNull(bookInfo.Title);
                Assert.IsNotNull(bookInfo.TitleText);
                Assert.AreEqual(2, bookInfo.UsageCount);
                Assert.AreEqual("PAZK", bookInfo.Worksheet);
                Assert.AreEqual(1993, bookInfo.Year);
            }
        }
    }
}
