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
    public class IsbnInfoTest
    {
        private IsbnInfo _GetIsbn()
        {
            return new IsbnInfo
            {
                Isbn = "5-200-00723-2",
                PriceString = "3.40"
            };
        }

        private RecordField _GetField()
        {
            return new RecordField(IsbnInfo.Tag)
                .AddSubField('a', "5-200-00723-2")
                .AddSubField('d', "3.40");
        }

        [TestMethod]
        public void IsbnInfo_Construction_1()
        {
            IsbnInfo isbn = new IsbnInfo();
            Assert.IsNull(isbn.Isbn);
            Assert.IsNull(isbn.Refinement);
            Assert.IsNull(isbn.Erroneous);
            Assert.IsNull(isbn.PriceString);
            Assert.IsNull(isbn.Currency);
            Assert.IsNull(isbn.UnknownSubFields);
            Assert.IsNull(isbn.Field);
            Assert.IsNull(isbn.UserData);
            Assert.AreEqual(0.0m, isbn.Price);
        }

        private void _TestSerialization
            (
                [NotNull] IsbnInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IsbnInfo second = bytes.RestoreObjectFromMemory<IsbnInfo>();
            Assert.AreEqual(first.Isbn, second.Isbn);
            Assert.AreEqual(first.Refinement, second.Refinement);
            Assert.AreEqual(first.Erroneous, second.Erroneous);
            Assert.AreEqual(first.PriceString, second.PriceString);
            Assert.AreEqual(first.Price, second.Price);
            Assert.AreEqual(first.Currency, second.Currency);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void IsbnInfo_Serialization_1()
        {
            IsbnInfo isbn = new IsbnInfo();
            _TestSerialization(isbn);

            isbn.Field = new RecordField();
            isbn.UserData = "User data";
            _TestSerialization(isbn);

            isbn = _GetIsbn();
            _TestSerialization(isbn);
        }

        [TestMethod]
        public void IsbnInfo_ParseField_1()
        {
            RecordField field = _GetField();
            IsbnInfo isbn = IsbnInfo.ParseField(field);
            Assert.AreSame(field, isbn.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), isbn.Isbn);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), isbn.Refinement);
            Assert.AreEqual(field.GetFirstSubFieldValue('z'), isbn.Erroneous);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), isbn.PriceString);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), isbn.Currency);
            Assert.IsNotNull(isbn.UnknownSubFields);
            Assert.AreEqual(0, isbn.UnknownSubFields.Length);
        }

        [TestMethod]
        public void IsbnInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            record.Fields.Add(field);
            IsbnInfo[] isbn = IsbnInfo.ParseRecord(record);
            Assert.AreEqual(1, isbn.Length);
            Assert.AreSame(field, isbn[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), isbn[0].Isbn);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), isbn[0].Refinement);
            Assert.AreEqual(field.GetFirstSubFieldValue('z'), isbn[0].Erroneous);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), isbn[0].PriceString);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), isbn[0].Currency);
            Assert.IsNotNull(isbn[0].UnknownSubFields);
            Assert.AreEqual(0, isbn[0].UnknownSubFields.Length);
        }

        [TestMethod]
        public void IsbnInfo_ToField_1()
        {
            IsbnInfo isbn = _GetIsbn();
            RecordField field = isbn.ToField();
            Assert.AreEqual(2, field.SubFields.Count);
            Assert.AreEqual(isbn.Isbn, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(isbn.Refinement, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(isbn.Erroneous, field.GetFirstSubFieldValue('z'));
            Assert.AreEqual(isbn.PriceString, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(isbn.Currency, field.GetFirstSubFieldValue('c'));
        }

        [TestMethod]
        public void IsbnInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(IsbnInfo.Tag)
                .AddSubField('a', "???");
            IsbnInfo isbn = _GetIsbn();
            isbn.ApplyToField(field);
            Assert.AreEqual(2, field.SubFields.Count);
            Assert.AreEqual(isbn.Isbn, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(isbn.Refinement, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(isbn.Erroneous, field.GetFirstSubFieldValue('z'));
            Assert.AreEqual(isbn.PriceString, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(isbn.Currency, field.GetFirstSubFieldValue('c'));
        }

        [TestMethod]
        public void Isbn_Verify_1()
        {
            IsbnInfo isbn = new IsbnInfo();
            Assert.IsFalse(isbn.Verify(false));

            isbn = _GetIsbn();
            Assert.IsTrue(isbn.Verify(false));
        }

        [TestMethod]
        public void IsbnInfo_ToXml_1()
        {
            IsbnInfo isbn = new IsbnInfo();
            Assert.AreEqual("<isbn />", XmlUtility.SerializeShort(isbn));

            isbn = _GetIsbn();
            Assert.AreEqual("<isbn><isbn>5-200-00723-2</isbn><price>3.40</price></isbn>", XmlUtility.SerializeShort(isbn));
        }

        [TestMethod]
        public void IsbnInfo_ToJson_1()
        {
            IsbnInfo isbn = new IsbnInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(isbn));

            isbn = _GetIsbn();
            Assert.AreEqual("{'isbn':'5-200-00723-2','price':'3.40'}", JsonUtility.SerializeShort(isbn));
        }

        [TestMethod]
        public void IsbnInfo_Price_1()
        {
            IsbnInfo isbn = new IsbnInfo();
            Assert.AreEqual(0.0m, isbn.Price);

            isbn.Price = 1.5m;
            Assert.AreEqual("1.50", isbn.PriceString);
        }

        [TestMethod]
        public void IsbnInfo_ToString_1()
        {
            IsbnInfo isbn = new IsbnInfo();
            Assert.AreEqual
                (
                    "(null)",
                    isbn.ToString().DosToUnix()
                );

            isbn = _GetIsbn();
            Assert.AreEqual
                (
                    "5-200-00723-2 : 3.40",
                    isbn.ToString().DosToUnix()
                );

            isbn = _GetIsbn();
            isbn.Isbn = null;
            Assert.AreEqual
            (
                "3.40",
                isbn.ToString().DosToUnix()
            );

            isbn = _GetIsbn();
            isbn.PriceString = null;
            Assert.AreEqual
            (
                "5-200-00723-2",
                isbn.ToString().DosToUnix()
            );
        }
    }
}
