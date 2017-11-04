using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Newtonsoft.Json;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class TitleInfoTest
    {
        [TestMethod]
        public void TitleInfo_Constructor_1()
        {
            TitleInfo title = new TitleInfo();
            Assert.IsNull(title.VolumeNumber);
            Assert.IsNull(title.Title);
            Assert.IsNull(title.Specific);
            Assert.IsNull(title.General);
            Assert.IsNull(title.Subtitle);
            Assert.IsNull(title.FirstResponsibility);
            Assert.IsNull(title.OtherResponsibility);
            Assert.IsNull(title.UserData);
        }

        [TestMethod]
        public void TitleInfo_Constructor_2()
        {
            const string expected = "Сказка о рыбаке и рыбке";
            TitleInfo title = new TitleInfo(expected);
            Assert.IsNull(title.VolumeNumber);
            Assert.AreEqual(expected, title.Title);
            Assert.IsNull(title.Specific);
            Assert.IsNull(title.General);
            Assert.IsNull(title.Subtitle);
            Assert.IsNull(title.FirstResponsibility);
            Assert.IsNull(title.OtherResponsibility);
            Assert.IsNull(title.UserData);
        }

        [TestMethod]
        public void TitleInfo_Constructor_3()
        {
            const string expected1 = "Т. 2";
            const string expected2 = "Письма";
            TitleInfo title = new TitleInfo
                (
                    expected1, 
                    expected2
                );
            Assert.AreEqual(expected1, title.VolumeNumber);
            Assert.AreEqual(expected2, title.Title);
            Assert.IsNull(title.Specific);
            Assert.IsNull(title.General);
            Assert.IsNull(title.Subtitle);
            Assert.IsNull(title.FirstResponsibility);
            Assert.IsNull(title.OtherResponsibility);
            Assert.IsNull(title.UserData);
        }

        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add
                (
                    new RecordField
                        (
                            "200", 
                            "^AПикассо сегодня"
                            + "^E[сборник статей]"
                            + "^FА. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев"
                            + "^GРос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
                        )
                );

            return result;
        }

        [TestMethod]
        public void TitleInfo_Parse_1()
        {
            MarcRecord record = _GetRecord();
            TitleInfo[] actual = TitleInfo.Parse(record);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("Пикассо сегодня", actual[0].Title);
            Assert.AreEqual("[сборник статей]", actual[0].Subtitle);
            Assert.AreEqual("А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев", actual[0].FirstResponsibility);
            Assert.AreEqual("Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина", actual[0].OtherResponsibility);
        }

        [TestMethod]
        public void TitleInfo_ToField()
        {
            TitleInfo title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };

            RecordField actual = title.ToField200();
            Assert.AreEqual("Пикассо сегодня", actual.GetFirstSubFieldValue('a'));
            Assert.AreEqual("[сборник статей]", actual.GetFirstSubFieldValue('e'));
            Assert.AreEqual("А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев", actual.GetFirstSubFieldValue('f'));
            Assert.AreEqual("Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина", actual.GetFirstSubFieldValue('g'));
        }

        [TestMethod]
        public void TitleInfo_ToString_1()
        {
            TitleInfo title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };

            const string expected = "Title: Пикассо сегодня, Subtitle: [сборник статей]";
            string actual = title.ToString();
            Assert.AreEqual(expected, actual);
        }

        private void _TestSerialization
            (
                TitleInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            TitleInfo second = bytes
                    .RestoreObjectFromMemory<TitleInfo>();
            Assert.AreEqual(first.VolumeNumber, second.VolumeNumber);
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.Specific, second.Specific);
            Assert.AreEqual(first.General, second.General);
            Assert.AreEqual(first.Subtitle, second.Subtitle);
            Assert.AreEqual(first.FirstResponsibility, second.FirstResponsibility);
            Assert.AreEqual(first.OtherResponsibility, second.OtherResponsibility);
        }

        [TestMethod]
        public void TitleInfo_Serialization_1()
        {
            TitleInfo title = new TitleInfo();
            _TestSerialization(title);

            title = new TitleInfo("Сказка о рыбаке и рыбке");
            _TestSerialization(title);

            title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };
            _TestSerialization(title);
        }

        [TestMethod]
        public void TitleInfo_ToJson_1()
        {
            TitleInfo title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };

            const string expected = "{'title':'Пикассо сегодня','subtitle':'[сборник статей]','first':'А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев','other':'Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина'}";
            string actual = JsonConvert.SerializeObject(title)
                .Replace('"', '\'');
            Assert.AreEqual(expected, actual);
        }
    }
}
