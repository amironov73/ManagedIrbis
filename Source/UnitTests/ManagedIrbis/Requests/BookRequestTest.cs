using System;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Readers;
using ManagedIrbis.Requests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Requests
{
    [TestClass]
    public class BookRequestTest
    {
        private BookRequest _GetRequest()
        {
            return new BookRequest
            {
                Mfn = 1,
                BookDescription = "Библиографическое описание",
                BookCode = "ABC123",
                RequestDate = "20170901",
                FulfillmentDate = "20170902",
                ReservationDate = "20170903",
                ReaderID = "123456",
                ReaderDescription = "Наш читатель",
                Database = "IBIS",
                Department = "ЧЗ"
            };
        }

        [TestMethod]
        public void BookRequest_Construction_1()
        {
            BookRequest request = new BookRequest();
            Assert.AreEqual(0, request.Mfn);
            Assert.IsNull(request.BookDescription);
            Assert.IsNull(request.BookCode);
            Assert.IsNull(request.RequestDate);
            Assert.IsNull(request.FulfillmentDate);
            Assert.IsNull(request.ReservationDate);
            Assert.IsNull(request.ReaderID);
            Assert.IsNull(request.ReaderDescription);
            Assert.IsNull(request.Database);
            Assert.IsNull(request.RejectInfo);
            Assert.IsNull(request.Department);
            Assert.IsNull(request.ResponsiblePerson);
            Assert.IsFalse(request.CanBeDeleted);
            Assert.IsNull(request.BookRecord);
            Assert.IsNull(request.Reader);
            Assert.IsNull(request.FreeNumbers);
            Assert.IsNull(request.MyNumbers);
            Assert.IsNull(request.RequestRecord);
        }

        private void _TestSerialization
            (
                [NotNull] BookRequest first
            )
        {
            byte[] bytes = first.SaveToMemory();

            BookRequest second = bytes
                .RestoreObjectFromMemory<BookRequest>();

            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.BookDescription, second.BookDescription);
            Assert.AreEqual(first.BookCode, second.BookCode);
            Assert.AreEqual(first.RequestDate, second.RequestDate);
            Assert.AreEqual(first.FulfillmentDate, second.FulfillmentDate);
            Assert.AreEqual(first.ReservationDate, second.ReservationDate);
            Assert.AreEqual(first.ReaderID, second.ReaderID);
            Assert.AreEqual(first.ReaderDescription, second.ReaderDescription);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.RejectInfo, second.RejectInfo);
            Assert.AreEqual(first.Department, second.Department);
            Assert.AreEqual(first.ResponsiblePerson, second.ResponsiblePerson);
            Assert.AreEqual(first.CanBeDeleted, second.CanBeDeleted);
            Assert.IsNull(second.BookRecord);
            Assert.IsNull(second.Reader);
            Assert.IsNull(second.FreeNumbers);
            Assert.IsNull(second.MyNumbers);
            Assert.IsNull(second.RequestRecord);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void BookRequest_Serialization_1()
        {
            BookRequest request = new BookRequest();
            _TestSerialization(request);

            request.BookRecord = new MarcRecord();
            request.Reader = new ReaderInfo();
            request.FreeNumbers = new string[0];
            request.MyNumbers = new string[0];
            request.RequestRecord = new MarcRecord();
            request.UserData = "User data";
            _TestSerialization(request);

            request = _GetRequest();
            _TestSerialization(request);
        }

        [TestMethod]
        public void BookRequest_ToXml_1()
        {
            BookRequest request = new BookRequest();
            Assert.AreEqual("<request mfn=\"0\" />", XmlUtility.SerializeShort(request));

            request = _GetRequest();
            Assert.AreEqual("<request mfn=\"1\" bookDescription=\"Библиографическое описание\" bookCode=\"ABC123\" requestDate=\"20170901\" fulfillmentDate=\"20170902\" reservationDate=\"20170903\" readerID=\"123456\" readerDescription=\"Наш читатель\" database=\"IBIS\" department=\"ЧЗ\" />", XmlUtility.SerializeShort(request));
        }

        [TestMethod]
        public void BookRequest_ToJson_1()
        {
            BookRequest request = new BookRequest();
            Assert.AreEqual("{'mfn':0}", JsonUtility.SerializeShort(request));

            request = _GetRequest();
            Assert.AreEqual("{'mfn':1,'bookDescription':'Библиографическое описание','bookCode':'ABC123','requestDate':'20170901','fulfillmentDate':'20170902','reservationDate':'20170903','readerID':'123456','readerDescription':'Наш читатель','database':'IBIS','department':'ЧЗ'}", JsonUtility.SerializeShort(request));
        }

        [TestMethod]
        public void BookRequest_ToString_1()
        {
            BookRequest request = new BookRequest();
            Assert.AreEqual
                (
                    "Reader: (null)\nBook: (null)\nDepartment: (null)\n",
                    request.ToString().DosToUnix()
                );

            request = _GetRequest();
            Assert.AreEqual
                (
                    "Reader: Наш читатель\nBook: Библиографическое описание\nDepartment: ЧЗ\n",
                    request.ToString().DosToUnix()
                );

            request.FreeNumbers = new []{"1", "2", "3"};
            request.MyNumbers = new []{"1", "2"};
            Assert.AreEqual
                (
                    "Reader: Наш читатель\nBook: Библиографическое описание\nFree exemplars: 1, 2, 3\nMy exemplars: 1, 2\nDepartment: ЧЗ\n",
                    request.ToString().DosToUnix()
                );
        }

        [TestMethod]
        public void BookRequest_Parse_1()
        {
            MarcRecord record = new MarcRecord
            {
                Mfn = 1
            };
            record.Fields.Add(new RecordField(201, "Библиографическое описание"));
            record.Fields.Add(new RecordField(903, "ABC123"));
            record.Fields.Add(new RecordField(40, "20170901"));
            record.Fields.Add(new RecordField(41, "20170902"));
            record.Fields.Add(new RecordField(43, "20170903"));
            record.Fields.Add(new RecordField(30, "12345"));
            record.Fields.Add(new RecordField(31, "Наш читатель"));
            record.Fields.Add(new RecordField(1, "IBIS"));
            record.Fields.Add(new RecordField(102, "ЧЗ"));
            record.Fields.Add(new RecordField(50, "Ответственный"));

            BookRequest request = BookRequest.Parse(record);
            Assert.AreEqual(1, request.Mfn);
            Assert.AreEqual("Библиографическое описание", request.BookDescription);
            Assert.AreEqual("ABC123", request.BookCode);
            Assert.AreEqual("20170901", request.RequestDate);
            Assert.AreEqual("20170902", request.FulfillmentDate);
            Assert.AreEqual("20170903", request.ReservationDate);
            Assert.AreEqual("12345", request.ReaderID);
            Assert.AreEqual("Наш читатель", request.ReaderDescription);
            Assert.AreEqual("IBIS", request.Database);
            Assert.AreEqual("ЧЗ",request.Department);
            Assert.AreEqual("Ответственный", request.ResponsiblePerson);
            Assert.AreSame(record, request.RequestRecord);
        }

        [TestMethod]
        public void BookRequest_Parse_2()
        {
            MarcRecord record = new MarcRecord
            {
                Mfn = 1
            };
            record.Fields.Add(new RecordField(201, "Библиографическое описание"));
            record.Fields.Add(new RecordField(903, "ABC123"));
            record.Fields.Add(new RecordField(40, "20170901"));
            record.Fields.Add(new RecordField(41, "20170902"));
            record.Fields.Add(new RecordField(43, "20170903"));
            record.Fields.Add(new RecordField(44, "^a01^b20170817"));
            record.Fields.Add(new RecordField(30, "12345"));
            record.Fields.Add(new RecordField(31, "Наш читатель"));
            record.Fields.Add(new RecordField(1, "IBIS"));
            record.Fields.Add(new RecordField(102, "ЧЗ"));
            record.Fields.Add(new RecordField(50, "Ответственный"));

            BookRequest request = BookRequest.Parse(record);
            Assert.AreEqual(1, request.Mfn);
            Assert.AreEqual("Библиографическое описание", request.BookDescription);
            Assert.AreEqual("ABC123", request.BookCode);
            Assert.AreEqual("20170901", request.RequestDate);
            Assert.AreEqual("20170902", request.FulfillmentDate);
            Assert.AreEqual("20170903", request.ReservationDate);
            Assert.AreEqual("12345", request.ReaderID);
            Assert.AreEqual("Наш читатель", request.ReaderDescription);
            Assert.AreEqual("IBIS", request.Database);
            Assert.AreEqual("ЧЗ", request.Department);
            Assert.AreEqual("Ответственный", request.ResponsiblePerson);
            Assert.AreEqual("^a01^b20170817", request.RejectInfo);
            Assert.AreSame(record, request.RequestRecord);
        }

        [TestMethod]
        public void BookRequest_Encode_1()
        {
            BookRequest request = new BookRequest
            {
                Mfn = 1,
                BookDescription = "Библиографическое описание",
                BookCode = "ABC123",
                RequestDate = "20170901",
                FulfillmentDate = "20170902",
                ReservationDate = "20170903",
                ReaderID = "12345",
                ReaderDescription = "Наш читатель",
                Database = "IBIS",
                Department = "ЧЗ",
                ResponsiblePerson = "Ответственный"
            };

            MarcRecord record = request.Encode();
            Assert.AreEqual("Библиографическое описание", record.FM(201));
            Assert.AreEqual("ABC123", record.FM(903));
            Assert.AreEqual("20170901", record.FM(40));
            Assert.AreEqual("20170902", record.FM(41));
            Assert.AreEqual("20170903", record.FM(43));
            Assert.AreEqual("12345", record.FM(30));
            Assert.AreEqual("Наш читатель", record.FM(31));
            Assert.AreEqual("IBIS", record.FM(1));
            Assert.AreEqual("ЧЗ", record.FM(102));
            Assert.AreEqual("Ответственный", record.FM(50));
        }

        [TestMethod]
        public void BookRequest_Encode_2()
        {
            BookRequest request = new BookRequest
            {
                Mfn = 1,
                BookDescription = "Библиографическое описание",
                BookCode = "ABC123",
                RequestDate = "20170901",
                FulfillmentDate = "20170902",
                ReservationDate = "20170903",
                ReaderID = "12345",
                ReaderDescription = "Наш читатель",
                Database = "IBIS",
                Department = "ЧЗ",
                ResponsiblePerson = "Ответственный",
                RejectInfo = "^a01^b20170817"
            };

            MarcRecord record = request.Encode();
            Assert.AreEqual("Библиографическое описание", record.FM(201));
            Assert.AreEqual("ABC123", record.FM(903));
            Assert.AreEqual("20170901", record.FM(40));
            Assert.AreEqual("20170902", record.FM(41));
            Assert.AreEqual("20170903", record.FM(43));
            Assert.AreEqual("12345", record.FM(30));
            Assert.AreEqual("Наш читатель", record.FM(31));
            Assert.AreEqual("IBIS", record.FM(1));
            Assert.AreEqual("ЧЗ", record.FM(102));
            Assert.AreEqual("Ответственный", record.FM(50));
            Assert.AreEqual("^a01^b20170817", record.Fields.GetFirstField(44).ToText());
        }
    }
}
