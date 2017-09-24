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
    public class PersonaliaInfoTest
    {
        [NotNull]
        private PersonaliaInfo _GetPersonalia()
        {
            return new PersonaliaInfo
            {
                DataKind = "600",
                Text = "о нем",
                Name = "Булгаков М. А.",
                Extension = "Михаил Афанасьевич",
                CantBeInverted = false,
                Appendix = "русский писатель, драматург, театральный режиссер и актер",
                Dates = "1891-1940"
            };
        }

        [NotNull]
        public RecordField _GetField()
        {
            return new RecordField(PersonaliaInfo.Tag)
                .AddSubField(')', "600")
                .AddSubField('b', "о нем")
                .AddSubField('a', "Булгаков М. А.")
                .AddSubField('g', "Михаил Афанасьевич")
                .AddSubField('c', "русский писатель, драматург, театральный режиссер и актер")
                .AddSubField('f', "1891-1940")
                .AddSubField('k', "Взаимоотношения с современниками");
        }

        [TestMethod]
        public void PersonaliaInfo_Construction_1()
        {
            PersonaliaInfo personalia = new PersonaliaInfo();
            Assert.IsNull(personalia.DataKind);
            Assert.IsNull(personalia.Text);
            Assert.IsNull(personalia.Name);
            Assert.IsNull(personalia.Extension);
            Assert.IsFalse(personalia.CantBeInverted);
            Assert.IsNull(personalia.Postfix);
            Assert.IsNull(personalia.Appendix);
            Assert.IsNull(personalia.Number);
            Assert.IsNull(personalia.Dates);
            Assert.IsNull(personalia.Variant);
            Assert.IsNull(personalia.WorkPlace);
            Assert.IsNull(personalia.UnknownSubFields);
            Assert.IsNull(personalia.Field);
            Assert.IsNull(personalia.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] PersonaliaInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            PersonaliaInfo second = bytes.RestoreObjectFromMemory<PersonaliaInfo>();
            Assert.AreEqual(first.DataKind, second.DataKind);
            Assert.AreEqual(first.Text, second.Text);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Extension, second.Extension);
            Assert.AreEqual(first.CantBeInverted, second.CantBeInverted);
            Assert.AreEqual(first.Postfix, second.Postfix);
            Assert.AreEqual(first.Appendix, second.Appendix);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Dates, second.Dates);
            Assert.AreEqual(first.Variant, second.Variant);
            Assert.AreEqual(first.WorkPlace, second.WorkPlace);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void PersonaliaInfo_Serialization_1()
        {
            PersonaliaInfo personalia = new PersonaliaInfo();
            _TestSerialization(personalia);

            personalia.Field = new RecordField();
            personalia.UserData = "User data";
            _TestSerialization(personalia);

            personalia = _GetPersonalia();
            _TestSerialization(personalia);
        }

        [TestMethod]
        public void PersonaliaInfo_ParseField_1()
        {
            RecordField field = _GetField();
            PersonaliaInfo personalia = PersonaliaInfo.ParseField(field);
            Assert.AreEqual(field.GetFirstSubFieldValue(')'), personalia.DataKind);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), personalia.Text);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), personalia.Name);
            Assert.AreEqual(field.GetFirstSubFieldValue('g'), personalia.Extension);
            Assert.IsFalse(personalia.CantBeInverted);
            Assert.AreEqual(field.GetFirstSubFieldValue('1'), personalia.Postfix);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), personalia.Appendix);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), personalia.Number);
            Assert.AreEqual(field.GetFirstSubFieldValue('f'), personalia.Dates);
            Assert.AreEqual(field.GetFirstSubFieldValue('r'), personalia.Variant);
            Assert.AreEqual(field.GetFirstSubFieldValue('p'), personalia.WorkPlace);
            Assert.IsNotNull(personalia.UnknownSubFields);
            Assert.AreEqual(1, personalia.UnknownSubFields.Length);
            Assert.AreEqual('k', personalia.UnknownSubFields[0].Code);
        }

        [TestMethod]
        public void PersonaliaInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            record.Fields.Add(field);
            PersonaliaInfo[] personalia = PersonaliaInfo.ParseRecord(record);
            Assert.AreEqual(field.GetFirstSubFieldValue(')'), personalia[0].DataKind);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), personalia[0].Text);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), personalia[0].Name);
            Assert.AreEqual(field.GetFirstSubFieldValue('g'), personalia[0].Extension);
            Assert.IsFalse(personalia[0].CantBeInverted);
            Assert.AreEqual(field.GetFirstSubFieldValue('1'), personalia[0].Postfix);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), personalia[0].Appendix);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), personalia[0].Number);
            Assert.AreEqual(field.GetFirstSubFieldValue('f'), personalia[0].Dates);
            Assert.AreEqual(field.GetFirstSubFieldValue('r'), personalia[0].Variant);
            Assert.AreEqual(field.GetFirstSubFieldValue('p'), personalia[0].WorkPlace);
            Assert.IsNotNull(personalia[0].UnknownSubFields);
            Assert.AreEqual(1, personalia[0].UnknownSubFields.Length);
            Assert.AreEqual('k', personalia[0].UnknownSubFields[0].Code);
        }

        [TestMethod]
        public void PersonaliaInfo_ToField_1()
        {
            PersonaliaInfo personalia = _GetPersonalia();
            RecordField field = personalia.ToField();
            Assert.AreEqual(PersonaliaInfo.Tag, field.Tag);
            Assert.AreEqual(6, field.SubFields.Count);
            Assert.AreEqual(personalia.DataKind, field.GetFirstSubFieldValue(')'));
            Assert.AreEqual(personalia.Text, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(personalia.Name, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(personalia.Extension, field.GetFirstSubFieldValue('g'));
            Assert.IsFalse(field.HaveSubField('9'));
            Assert.AreEqual(personalia.Postfix, field.GetFirstSubFieldValue('1'));
            Assert.AreEqual(personalia.Appendix, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(personalia.Number, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(personalia.Dates, field.GetFirstSubFieldValue('f'));
            Assert.AreEqual(personalia.Variant, field.GetFirstSubFieldValue('r'));
            Assert.AreEqual(personalia.WorkPlace, field.GetFirstSubFieldValue('p'));
        }

        [TestMethod]
        public void PersonaliaInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(PersonaliaInfo.Tag)
                .AddSubField('a', "???")
                .AddSubField('b', "???");
            PersonaliaInfo personalia = _GetPersonalia();
            personalia.ApplyToField(field);
            Assert.AreEqual(PersonaliaInfo.Tag, field.Tag);
            Assert.AreEqual(6, field.SubFields.Count);
            Assert.AreEqual(personalia.DataKind, field.GetFirstSubFieldValue(')'));
            Assert.AreEqual(personalia.Text, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(personalia.Name, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(personalia.Extension, field.GetFirstSubFieldValue('g'));
            Assert.IsFalse(field.HaveSubField('9'));
            Assert.AreEqual(personalia.Postfix, field.GetFirstSubFieldValue('1'));
            Assert.AreEqual(personalia.Appendix, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(personalia.Number, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(personalia.Dates, field.GetFirstSubFieldValue('f'));
            Assert.AreEqual(personalia.Variant, field.GetFirstSubFieldValue('r'));
            Assert.AreEqual(personalia.WorkPlace, field.GetFirstSubFieldValue('p'));
        }

        [TestMethod]
        public void PersonaliaInfo_Verify_1()
        {
            PersonaliaInfo personalia = new PersonaliaInfo();
            Assert.IsFalse(personalia.Verify(false));

            personalia = _GetPersonalia();
            Assert.IsTrue(personalia.Verify(false));
        }

        [TestMethod]
        public void PersonaliaInfo_ToXml_1()
        {
            PersonaliaInfo personalia = new PersonaliaInfo();
            Assert.AreEqual("<personalia />", XmlUtility.SerializeShort(personalia));

            personalia = _GetPersonalia();
            Assert.AreEqual("<personalia appendix=\"русский писатель, драматург, театральный режиссер и актер\" dates=\"1891-1940\"><dataKind>600</dataKind><text>о нем</text><name>Булгаков М. А.</name><extension>Михаил Афанасьевич</extension></personalia>", XmlUtility.SerializeShort(personalia));
        }

        [TestMethod]
        public void PersonaliaInfo_ToJson_1()
        {
            PersonaliaInfo personalia = new PersonaliaInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(personalia));

            personalia = _GetPersonalia();
            Assert.AreEqual("{'dataKind':'600','text':'о нем','name':'Булгаков М. А.','extension':'Михаил Афанасьевич','appendix':'русский писатель, драматург, театральный режиссер и актер','dates':'1891-1940'}", JsonUtility.SerializeShort(personalia));
        }

        [TestMethod]
        public void PersonaliaInfo_ToString_1()
        {
            PersonaliaInfo personalia = new PersonaliaInfo();
            Assert.AreEqual("(null)", personalia.ToString());

            personalia = _GetPersonalia();
            Assert.AreEqual("Булгаков М. А.", personalia.ToString());
        }
    }
}
