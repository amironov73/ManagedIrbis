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
    public class CommonInfoTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void CommonInfo_ParseRecord_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                MarcRecord record = provider.ReadRecord(2).ThrowIfNull();

                CommonInfo[] array = CommonInfo.ParseRecord(record);
                Assert.IsNotNull(array);
                Assert.AreEqual(1, array.Length);
                CommonInfo info = array[0];
                Assert.AreEqual("Управление банком", info.Title);
                Assert.IsNull(info.Specific);
                Assert.IsNull(info.General);
                Assert.IsNull(info.Subtitle);
                Assert.AreEqual("З. М. Акулова [и др.]", info.Responsibility);
                Assert.AreEqual("Наука", info.Publisher);
                Assert.AreEqual("М.; СПб.", info.City);
                Assert.AreEqual("1990", info.BeginningYear);
                Assert.IsNull(info.EndingYear);
                Assert.IsNull(info.Isbn);
                Assert.IsNull(info.Issn);
                Assert.IsNull(info.Translation);
                Assert.IsNull(info.FirstAuthor);
                Assert.IsNull(info.Collective);
                Assert.IsNull(info.TitleVariant);
                Assert.IsNull(info.SecondLevelNumber);
                Assert.IsNull(info.SecondLevelTitle);
                Assert.IsNull(info.ThirdLevelNumber);
                Assert.IsNull(info.ThirdLevelTitle);
                Assert.IsNull(info.ParallelTitle);
                Assert.AreEqual("Деньги, кредит, финансирование, серия", info.SeriesTitle);
                Assert.IsNull(info.PreviousTitle);
                Assert.IsNotNull(info.Field461);
                Assert.IsNotNull(info.Field46);
            }
        }
    }
}
