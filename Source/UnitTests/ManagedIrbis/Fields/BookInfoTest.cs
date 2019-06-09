using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void BookInfo_CountPages_1()
        {
            Assert.AreEqual(0, BookInfo.CountPages(null));
            Assert.AreEqual(0, BookInfo.CountPages(string.Empty));
            Assert.AreEqual(0, BookInfo.CountPages(" "));
            Assert.AreEqual(0, BookInfo.CountPages("???"));
            Assert.AreEqual(21, BookInfo.CountPages("XXI"));
            Assert.AreEqual(21, BookInfo.CountPages("[XXI]"));
            Assert.AreEqual(35, BookInfo.CountPages("XXI,XIV"));
            Assert.AreEqual(21, BookInfo.CountPages("21"));
            Assert.AreEqual(21, BookInfo.CountPages("[21]"));
            Assert.AreEqual(35, BookInfo.CountPages("XXI,14"));
        }

        [TestMethod]
        public void BookInfo_Pages_1()
        {
            IrbisProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord();
            BookInfo bookInfo = new BookInfo(provider, record);
            Assert.AreEqual(0, bookInfo.Pages);
            string volumeText = "XXI,14";
            record.AddField(new RecordField(215).AddSubField('a', volumeText));
            Assert.AreEqual(35, bookInfo.Pages);
        }

        [TestMethod]
        public void BookInfo_Volume_1()
        {
            IrbisProvider provider = new NullProvider();
            MarcRecord record = new MarcRecord();
            BookInfo bookInfo = new BookInfo(provider, record);
            Assert.IsNull(bookInfo.Volume);
            string volumeText = "XXI,14";
            record.AddField(new RecordField(215).AddSubField('a', volumeText));
            Assert.AreEqual(volumeText, bookInfo.Volume);
        }
    }
}
