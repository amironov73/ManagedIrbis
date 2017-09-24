using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class ImprintInfoTest
    {
        [NotNull]
        private ImprintInfo _GetImprint()
        {
            return new ImprintInfo
            {
                Publisher = "Центрполиграф",
                City1 = "Москва",
                Year = "2012"
            };
        }

        [NotNull]
        private RecordField _GetField()
        {
            return new RecordField(ImprintInfo.Tag)
                .AddSubField('d', "2012")
                .AddSubField('a', "Москва")
                .AddSubField('c', "Центрполиграф");
        }

        [TestMethod]
        public void ImprintInfo_Construction_1()
        {
            ImprintInfo imprint = new ImprintInfo();
            Assert.IsNull(imprint.Publisher);
            Assert.IsNull(imprint.PrintedPublisher);
            Assert.IsNull(imprint.City1);
            Assert.IsNull(imprint.City2);
            Assert.IsNull(imprint.City3);
            Assert.IsNull(imprint.Year);
            Assert.IsNull(imprint.Place);
            Assert.IsNull(imprint.PrintingHouse);
            Assert.IsNull(imprint.UnknownSubFields);
            Assert.IsNull(imprint.Field);
            Assert.IsNull(imprint.UserData);
        }

        [TestMethod]
        public void ImprintInfo_Construction_2()
        {
            ImprintInfo imprint = new ImprintInfo("Центрполиграф", "Москва", "2012");
            Assert.AreEqual("Центрполиграф", imprint.Publisher);
            Assert.IsNull(imprint.PrintedPublisher);
            Assert.AreEqual("Москва", imprint.City1);
            Assert.IsNull(imprint.City2);
            Assert.IsNull(imprint.City3);
            Assert.AreEqual("2012", imprint.Year);
            Assert.IsNull(imprint.Place);
            Assert.IsNull(imprint.PrintingHouse);
            Assert.IsNull(imprint.UnknownSubFields);
            Assert.IsNull(imprint.Field);
            Assert.IsNull(imprint.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] ImprintInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ImprintInfo second = bytes.RestoreObjectFromMemory<ImprintInfo>();
            Assert.AreEqual(first.Publisher, second.Publisher);
            Assert.AreEqual(first.PrintedPublisher, second.PrintedPublisher);
            Assert.AreEqual(first.City1, second.City1);
            Assert.AreEqual(first.City2, second.City2);
            Assert.AreEqual(first.City3, second.City3);
            Assert.AreEqual(first.Year, second.Year);
            Assert.AreEqual(first.Place, second.Place);
            Assert.AreEqual(first.PrintingHouse, second.PrintingHouse);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void ImprintInfo_Serialization_1()
        {
            ImprintInfo imprint = new ImprintInfo();
            _TestSerialization(imprint);

            imprint.Field = new RecordField();
            imprint.UserData = "User data";
            _TestSerialization(imprint);

            imprint = _GetImprint();
            _TestSerialization(imprint);
        }

        [TestMethod]
        public void ImprintInfo_ParseField_1()
        {
            RecordField field = _GetField();
            ImprintInfo imprint = ImprintInfo.ParseField(field);
            Assert.AreSame(field, imprint.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), imprint.Publisher);
            Assert.AreEqual(field.GetFirstSubFieldValue('l'), imprint.PrintedPublisher);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), imprint.City1);
            Assert.AreEqual(field.GetFirstSubFieldValue('x'), imprint.City2);
            Assert.AreEqual(field.GetFirstSubFieldValue('y'), imprint.City3);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), imprint.Year);
            Assert.AreEqual(field.GetFirstSubFieldValue('1'), imprint.Place);
            Assert.AreEqual(field.GetFirstSubFieldValue('t'), imprint.PrintingHouse);
            Assert.IsNotNull(imprint.UnknownSubFields);
            Assert.AreEqual(0, imprint.UnknownSubFields.Length);
        }

        [TestMethod]
        public void ImprintInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            record.Fields.Add(field);
            ImprintInfo[] imprint = ImprintInfo.ParseRecord(record);
            Assert.AreEqual(1, imprint.Length);
            Assert.AreSame(field, imprint[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), imprint[0].Publisher);
            Assert.AreEqual(field.GetFirstSubFieldValue('l'), imprint[0].PrintedPublisher);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), imprint[0].City1);
            Assert.AreEqual(field.GetFirstSubFieldValue('x'), imprint[0].City2);
            Assert.AreEqual(field.GetFirstSubFieldValue('y'), imprint[0].City3);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), imprint[0].Year);
            Assert.AreEqual(field.GetFirstSubFieldValue('1'), imprint[0].Place);
            Assert.AreEqual(field.GetFirstSubFieldValue('t'), imprint[0].PrintingHouse);
            Assert.IsNotNull(imprint[0].UnknownSubFields);
            Assert.AreEqual(0, imprint[0].UnknownSubFields.Length);
        }

        [TestMethod]
        public void ImprintInfo_ToField_1()
        {
            ImprintInfo imprint = _GetImprint();
            RecordField field = imprint.ToField();
            Assert.AreEqual(3, field.SubFields.Count);
            Assert.AreEqual(imprint.Publisher, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(imprint.PrintedPublisher, field.GetFirstSubFieldValue('l'));
            Assert.AreEqual(imprint.City1, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(imprint.City2, field.GetFirstSubFieldValue('x'));
            Assert.AreEqual(imprint.City3, field.GetFirstSubFieldValue('y'));
            Assert.AreEqual(imprint.Year, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(imprint.Place, field.GetFirstSubFieldValue('1'));
            Assert.AreEqual(imprint.PrintingHouse, field.GetFirstSubFieldValue('t'));
        }

        [TestMethod]
        public void ImprintInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(ImprintInfo.Tag)
                .AddSubField('a', "???")
                .AddSubField('c', "???");
            ImprintInfo imprint = _GetImprint();
            imprint.ApplyToField(field);
            Assert.AreEqual(ImprintInfo.Tag, field.Tag);
            Assert.AreEqual(3, field.SubFields.Count);
            Assert.AreEqual(imprint.Publisher, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(imprint.PrintedPublisher, field.GetFirstSubFieldValue('l'));
            Assert.AreEqual(imprint.City1, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(imprint.City2, field.GetFirstSubFieldValue('x'));
            Assert.AreEqual(imprint.City3, field.GetFirstSubFieldValue('y'));
            Assert.AreEqual(imprint.Year, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(imprint.Place, field.GetFirstSubFieldValue('1'));
            Assert.AreEqual(imprint.PrintingHouse, field.GetFirstSubFieldValue('t'));
        }

        [TestMethod]
        public void ImprintInfo_Verify_1()
        {
            ImprintInfo imprint = new ImprintInfo();
            Assert.IsFalse(imprint.Verify(false));

            imprint = _GetImprint();
            Assert.IsTrue(imprint.Verify(false));
        }

        [TestMethod]
        public void ImprintInfo_ToXml_1()
        {
            ImprintInfo imprint = new ImprintInfo();
            Assert.AreEqual("<imprint />", XmlUtility.SerializeShort(imprint));

            imprint = _GetImprint();
            Assert.AreEqual("<imprint publisher=\"Центрполиграф\" city1=\"Москва\" year=\"2012\" />", XmlUtility.SerializeShort(imprint));
        }

        [TestMethod]
        public void ImprintInfo_ToJson_1()
        {
            ImprintInfo imprint = new ImprintInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(imprint));

            imprint = _GetImprint();
            Assert.AreEqual("{'publisher':'Центрполиграф','city1':'Москва','year':'2012'}", JsonUtility.SerializeShort(imprint));
        }

        [TestMethod]
        public void ImprintInfo_ToString_1()
        {
            ImprintInfo imprint = new ImprintInfo();
            Assert.AreEqual("(null): (null), (null)", imprint.ToString());

            imprint = _GetImprint();
            Assert.AreEqual("Москва: Центрполиграф, 2012", imprint.ToString());
        }

    }
}
