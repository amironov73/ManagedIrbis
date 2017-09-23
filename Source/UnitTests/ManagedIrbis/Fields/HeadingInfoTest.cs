using System;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class HeadingInfoTest
    {
        private HeadingInfo _GetHeading()
        {
            return new HeadingInfo
            {
                Title = "Русская литература",
                Subtitle1 = "Проза",
                GeographicalSubtitle1 = "Санкт-Петербург",
                ChronologicalSubtitle = "19 в.",
                Aspect = "Сборники"
            };
        }

        private RecordField _GetField()
        {
            RecordField result = new RecordField(HeadingInfo.Tag)
                .AddSubField('a', "Русская литература")
                .AddSubField('a', "Проза")
                .AddSubField('a', "Санкт-Петербург")
                .AddSubField('a', "19 в.")
                .AddSubField('a', "Сборники");

            return result;
        }

        [TestMethod]
        public void HeadingInfo_Construction_1()
        {
            HeadingInfo heading = new HeadingInfo();
            Assert.IsNull(heading.Title);
            Assert.IsNull(heading.Subtitle1);
            Assert.IsNull(heading.Subtitle2);
            Assert.IsNull(heading.Subtitle3);
            Assert.IsNull(heading.GeographicalSubtitle1);
            Assert.IsNull(heading.GeographicalSubtitle2);
            Assert.IsNull(heading.GeographicalSubtitle3);
            Assert.IsNull(heading.ChronologicalSubtitle);
            Assert.IsNull(heading.UnknownSubFields);
            Assert.IsNull(heading.Aspect);
            Assert.IsNull(heading.Field);
            Assert.IsNull(heading.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] HeadingInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            HeadingInfo second = bytes.RestoreObjectFromMemory<HeadingInfo>();
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.Subtitle1, second.Subtitle1);
            Assert.AreEqual(first.Subtitle2, second.Subtitle2);
            Assert.AreEqual(first.Subtitle3, second.Subtitle3);
            Assert.AreEqual(first.GeographicalSubtitle1, second.GeographicalSubtitle1);
            Assert.AreEqual(first.GeographicalSubtitle2, second.GeographicalSubtitle2);
            Assert.AreEqual(first.GeographicalSubtitle3, second.GeographicalSubtitle3);
            Assert.AreEqual(first.ChronologicalSubtitle, second.ChronologicalSubtitle);
            Assert.AreEqual(first.Aspect, second.Aspect);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void HeadingInfo_Serialization_1()
        {
            HeadingInfo heading = new HeadingInfo();
            _TestSerialization(heading);

            heading.UserData = "User data";
            _TestSerialization(heading);

            heading = _GetHeading();
            _TestSerialization(heading);
        }

        [TestMethod]
        public void HeadingInfo_ParseField_1()
        {
            RecordField field = _GetField();
            HeadingInfo heading = HeadingInfo.ParseField(field);
            Assert.AreSame(field, heading.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), heading.Title);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), heading.Subtitle1);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), heading.Subtitle2);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), heading.Subtitle3);
            Assert.AreEqual(field.GetFirstSubFieldValue('g'), heading.GeographicalSubtitle1);
            Assert.AreEqual(field.GetFirstSubFieldValue('e'), heading.GeographicalSubtitle2);
            Assert.AreEqual(field.GetFirstSubFieldValue('o'), heading.GeographicalSubtitle3);
            Assert.AreEqual(field.GetFirstSubFieldValue('h'), heading.ChronologicalSubtitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('9'), heading.Aspect);
            Assert.IsNotNull(heading.UnknownSubFields);
            Assert.AreEqual(0, heading.UnknownSubFields.Length);
            Assert.IsNull(heading.UserData);
        }

        [TestMethod]
        public void HeadingInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            record.Fields.Add(field);
            HeadingInfo[] heading = HeadingInfo.ParseRecord(record);
            Assert.AreSame(field, heading[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), heading[0].Title);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), heading[0].Subtitle1);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), heading[0].Subtitle2);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), heading[0].Subtitle3);
            Assert.AreEqual(field.GetFirstSubFieldValue('g'), heading[0].GeographicalSubtitle1);
            Assert.AreEqual(field.GetFirstSubFieldValue('e'), heading[0].GeographicalSubtitle2);
            Assert.AreEqual(field.GetFirstSubFieldValue('o'), heading[0].GeographicalSubtitle3);
            Assert.AreEqual(field.GetFirstSubFieldValue('h'), heading[0].ChronologicalSubtitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('9'), heading[0].Aspect);
            Assert.IsNotNull(heading[0].UnknownSubFields);
            Assert.AreEqual(0, heading[0].UnknownSubFields.Length);
            Assert.IsNull(heading[0].UserData);
        }

        [TestMethod]
        public void HeadingInfo_ToField_1()
        {
            HeadingInfo heading = _GetHeading();
            RecordField field = heading.ToField();
            Assert.AreEqual(HeadingInfo.Tag, field.Tag);
            Assert.AreEqual(5, field.SubFields.Count);
            Assert.AreEqual(heading.Title, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(heading.Subtitle1, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(heading.Subtitle2, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(heading.Subtitle3, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(heading.GeographicalSubtitle1, field.GetFirstSubFieldValue('g'));
            Assert.AreEqual(heading.GeographicalSubtitle2, field.GetFirstSubFieldValue('e'));
            Assert.AreEqual(heading.GeographicalSubtitle3, field.GetFirstSubFieldValue('o'));
            Assert.AreEqual(heading.ChronologicalSubtitle, field.GetFirstSubFieldValue('h'));
            Assert.AreEqual(heading.Aspect, field.GetFirstSubFieldValue('9'));
        }

        [TestMethod]
        public void HeadingInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(HeadingInfo.Tag)
                .AddSubField('a', "???")
                .AddSubField('b', "???");
            HeadingInfo heading = _GetHeading();
            heading.ApplyToField(field);
            Assert.AreEqual(5, field.SubFields.Count);
            Assert.AreEqual(heading.Title, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(heading.Subtitle1, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(heading.Subtitle2, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(heading.Subtitle3, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(heading.GeographicalSubtitle1, field.GetFirstSubFieldValue('g'));
            Assert.AreEqual(heading.GeographicalSubtitle2, field.GetFirstSubFieldValue('e'));
            Assert.AreEqual(heading.GeographicalSubtitle3, field.GetFirstSubFieldValue('o'));
            Assert.AreEqual(heading.ChronologicalSubtitle, field.GetFirstSubFieldValue('h'));
            Assert.AreEqual(heading.Aspect, field.GetFirstSubFieldValue('9'));
        }

        [TestMethod]
        public void HeadingInfo_Verify_1()
        {
            HeadingInfo heading = new HeadingInfo();
            Assert.IsFalse(heading.Verify(false));

            heading = _GetHeading();
            Assert.IsTrue(heading.Verify(false));
        }

        [TestMethod]
        public void HeadingInfo_ToXml_1()
        {
            HeadingInfo heading = new HeadingInfo();
            Assert.AreEqual("<heading />", XmlUtility.SerializeShort(heading));

            heading = _GetHeading();
            Assert.AreEqual("<heading><title>Русская литература</title><subtitle1>Проза</subtitle1><geoSubtitle1>Санкт-Петербург</geoSubtitle1><chronoSubtitle>19 в.</chronoSubtitle><aspect>Сборники</aspect></heading>", XmlUtility.SerializeShort(heading));
        }

        [TestMethod]
        public void HeadingInfo_ToJson_1()
        {
            HeadingInfo heading = new HeadingInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(heading));

            heading = _GetHeading();
            Assert.AreEqual("{'title':'Русская литература','subtitle1':'Проза','geoSubtitle1':'Санкт-Петербург','chronoSubtitle':'19 в.','aspect':'Сборники'}", JsonUtility.SerializeShort(heading));
        }

        [TestMethod]
        public void HeadingInfo_ToString_1()
        {
            HeadingInfo heading = new HeadingInfo();
            Assert.AreEqual
                (
                    "(null)",
                    heading.ToString().DosToUnix()
                );

            heading = _GetHeading();
            Assert.AreEqual
                (
                    "Русская литература -- Проза -- Санкт-Петербург -- 19 в. -- Сборники",
                    heading.ToString().DosToUnix()
                );
        }
    }
}
