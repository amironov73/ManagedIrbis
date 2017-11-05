using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Readers;

using Moq;

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class VisitInfoTest
    {
        [TestMethod]
        public void VisitInfo_Construction_1()
        {
            VisitInfo visit = new VisitInfo();
            Assert.AreEqual(0, visit.Id);
            Assert.IsNull(visit.Database);
            Assert.IsNull(visit.Index);
            Assert.IsNull(visit.Inventory);
            Assert.IsNull(visit.Barcode);
            Assert.IsNull(visit.Sigla);
            Assert.IsNull(visit.DateGivenString);
            Assert.IsNull(visit.Department);
            Assert.IsNull(visit.DateExpectedString);
            Assert.IsNull(visit.DateProlongString);
            Assert.IsNull(visit.Lost);
            Assert.IsNull(visit.Description);
            Assert.IsNull(visit.Responsible);
            Assert.IsNull(visit.TimeIn);
            Assert.IsNull(visit.TimeOut);
            Assert.IsNull(visit.Prolong);
            Assert.IsTrue(visit.IsVisit);
            Assert.IsFalse(visit.IsReturned);
            Assert.AreEqual(0, visit.ProlongCount);
            Assert.IsNull(visit.Year);
            Assert.IsNull(visit.Price);
            Assert.IsNull(visit.Field);
            Assert.IsNull(visit.Reader);
            Assert.IsNull(visit.UserData);
        }

        [TestMethod]
        public void VisitInfo_DateExpected_1()
        {
            VisitInfo visit = new VisitInfo();
            Assert.AreEqual(DateTime.MinValue, visit.DateExpected);

            visit.DateExpectedString = "20171105";
            Assert.AreEqual(new DateTime(2017, 11, 5), visit.DateExpected);
        }

        [TestMethod]
        public void VisitInfo_DateGiven_1()
        {
            VisitInfo visit = new VisitInfo();
            Assert.AreEqual(DateTime.MinValue, visit.DateGiven);

            visit.DateGivenString = "20171105";
            Assert.AreEqual(new DateTime(2017, 11, 5), visit.DateGiven);
        }

        [TestMethod]
        public void VisitInfo_DateReturned_1()
        {
            VisitInfo visit = new VisitInfo();
            Assert.AreEqual(DateTime.MinValue, visit.DateReturned);

            visit.DateReturnedString = "20171105";
            Assert.AreEqual(new DateTime(2017, 11, 5), visit.DateReturned);
        }

        [TestMethod]
        public void VisitInfo_ProlongCount_1()
        {
            VisitInfo visit = new VisitInfo();
            Assert.AreEqual(0, visit.ProlongCount);

            visit.Prolong = "2";
            Assert.AreEqual(2, visit.ProlongCount);

            visit.ProlongCount = 3;
            Assert.AreEqual("3", visit.Prolong);
        }

        [TestMethod]
        public void VisitInfo_Reader_1()
        {
            VisitInfo visit = new VisitInfo();
            ReaderInfo reader = new ReaderInfo();
            visit.Reader = reader;
            Assert.AreSame(reader, visit.Reader);
        }

        [NotNull]
        private RecordField _GetField()
        {
            return RecordField.Parse(40, "^A84(2=Рус)5/Ц 27-422726^B1539325^CЦветаева М. И. Стихотворения^KФ302^V*^D20170930^1172914^E20171014^F20171028^GIBIS^HE00401004C75DB57^Isamo");
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(906, "84(2=Рус)5"));
            result.Fields.Add(RecordField.Parse(908, "Ц 27"));
            result.Fields.Add(RecordField.Parse(700, "^AЦветаева^BМ. И.^GМарина Ивановна"));
            result.Fields.Add(RecordField.Parse(215, "^A268"));
            result.Fields.Add(RecordField.Parse(10, "^A5-7633-0120-X^D2.00"));
            result.Fields.Add(RecordField.Parse(300, "(Поэтич. родники России). "));
            result.Fields.Add(RecordField.Parse(920, "PAZK"));
            result.Fields.Add(RecordField.Parse(900, "^B05"));
            result.Fields.Add(RecordField.Parse(903, "84(2=Рус)5/Ц 27-000000-422728"));
            result.Fields.Add(RecordField.Parse(101, "rus"));
            result.Fields.Add(RecordField.Parse(9951, "1021780.jpg"));
            result.Fields.Add(RecordField.Parse(9951, "1021781.jpg"));
            result.Fields.Add(RecordField.Parse(907, "^CПК^A20121219^B^1"));
            result.Fields.Add(RecordField.Parse(2012, "4"));
            result.Fields.Add(RecordField.Parse(2013, "^aБО заимствовано^bRETRO^c421180"));
            result.Fields.Add(RecordField.Parse(907, "^C^A20130128^B"));
            result.Fields.Add(RecordField.Parse(907, "^CКР^A20131217^Bmiron"));
            result.Fields.Add(RecordField.Parse(1119, "c791f88d-70d9-47d2-9dee-7a158d903a99"));
            result.Fields.Add(RecordField.Parse(210, "^CПриволжское книжное изд-во^AСаратов^D1990"));
            result.Fields.Add(RecordField.Parse(200, "^AСтихотворения. Проза^FМ. Цветаева ; сост. авт. послесл., с. 245-263, Е. П. Никитина"));
            result.Fields.Add(RecordField.Parse(907, "^CКР^A20140421^Bертаевасб"));
            result.Fields.Add(RecordField.Parse(910, "^B1546154^DФКХ^C20121107^A0^hE00401004C79A941"));
            result.Fields.Add(RecordField.Parse(905, "^D3^F2^S1^21^J1"));

            return result;
        }

        [TestMethod]
        public void VisitInfo_Parse_1()
        {
            RecordField field = _GetField();
            VisitInfo visit = VisitInfo.Parse(field);
            Assert.AreSame(field, visit.Field);
            Assert.AreEqual("84(2=Рус)5/Ц 27-422726", visit.Index);
            Assert.AreEqual("1539325", visit.Inventory);
            Assert.AreEqual("Ф302", visit.Sigla);
            Assert.AreEqual("Цветаева М. И. Стихотворения", visit.Description);
            Assert.AreEqual("*", visit.Department);
            Assert.AreEqual("172914", visit.TimeIn);
            Assert.AreEqual("20170930", visit.DateGivenString);
            Assert.AreEqual(new DateTime(2017, 9, 30), visit.DateGiven);
            Assert.AreEqual("20171014", visit.DateExpectedString);
            Assert.AreEqual(new DateTime(2017, 10, 14), visit.DateExpected);
            Assert.AreEqual("20171028", visit.DateReturnedString);
            Assert.AreEqual(new DateTime(2017, 10, 28), visit.DateReturned);
            Assert.AreEqual("IBIS", visit.Database);
            Assert.AreEqual("E00401004C75DB57", visit.Barcode);
            Assert.AreEqual("samo", visit.Responsible);
            Assert.AreEqual(0, visit.UnknownSubFields.Length);
            Assert.IsFalse(visit.IsVisit);
            Assert.IsTrue(visit.IsReturned);
            Assert.IsNull(visit.Reader);
            Assert.IsNull(visit.UserData);
        }

        [NotNull]
        private VisitInfo _GetVisit()
        {
            VisitInfo result = new VisitInfo
            {
                Index = "84(2=Рус)5/Ц 27-422726",
                Inventory = "1539325",
                Sigla = "Ф302",
                Description = "Цветаева М. И. Стихотворения",
                Department = "*",
                TimeIn = "172914",
                DateGivenString = "20170930",
                DateExpectedString = "20171014",
                DateReturnedString = "20171028",
                Database = "IBIS",
                Barcode = "E00401004C75DB57",
                Responsible = "samo"
            };

            return result;
        }

        [TestMethod]
        public void VisitInfo_ToField_1()
        {
            VisitInfo visit = _GetVisit();
            RecordField field = visit.ToField();
            Assert.AreEqual(visit.Index, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(visit.Inventory, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(visit.Sigla, field.GetFirstSubFieldValue('k'));
            Assert.AreEqual(visit.Description, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(visit.Department, field.GetFirstSubFieldValue('v'));
            Assert.AreEqual(visit.TimeIn, field.GetFirstSubFieldValue('1'));
            Assert.AreEqual(visit.DateGivenString, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(visit.DateExpectedString, field.GetFirstSubFieldValue('e'));
            Assert.AreEqual(visit.DateReturnedString, field.GetFirstSubFieldValue('f'));
            Assert.AreEqual(visit.Database, field.GetFirstSubFieldValue('g'));
            Assert.AreEqual(visit.Barcode, field.GetFirstSubFieldValue('h'));
            Assert.AreEqual(visit.Responsible, field.GetFirstSubFieldValue('i'));
        }

        private void _Compare
            (
                [NotNull] VisitInfo first,
                [NotNull] VisitInfo second
            )
        {
            Assert.AreEqual(first.Id, second.Id);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Inventory, second.Inventory);
            Assert.AreEqual(first.Barcode, second.Barcode);
            Assert.AreEqual(first.Sigla, second.Sigla);
            Assert.AreEqual(first.DateGivenString, second.DateGivenString);
            Assert.AreEqual(first.DateExpectedString, second.DateExpectedString);
            Assert.AreEqual(first.DateReturnedString, second.DateReturnedString);
            Assert.AreEqual(first.DateProlongString, second.DateProlongString);
            Assert.AreEqual(first.Lost, second.Lost);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Responsible, second.Responsible);
            Assert.AreEqual(first.TimeIn, second.TimeIn);
            Assert.AreEqual(first.Prolong, second.Prolong);
            Assert.AreEqual(first.IsVisit, second.IsVisit);
            Assert.AreEqual(first.IsReturned, second.IsReturned);
            Assert.AreEqual(first.Year, second.Year);
            Assert.AreEqual(first.Price, second.Price);
            Assert.IsNull(second.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] VisitInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();

            VisitInfo second = bytes
                .RestoreObjectFromMemory<VisitInfo>();
            _Compare(first, second);
        }

        [TestMethod]
        public void VisitInfo_Serialization_1()
        {
            VisitInfo visitInfo = new VisitInfo();
            _TestSerialization(visitInfo);

            visitInfo.DateGivenString = "20160529";
            visitInfo.Department = "АБ";
            visitInfo.Description = "Книга";
            visitInfo.UserData = "User data";
            _TestSerialization(visitInfo);
        }

        [TestMethod]
        public void VisitInfo_Save_Read_1()
        {
            VisitInfo first = _GetVisit();
            string fileName = Path.GetTempFileName();
            VisitInfo.SaveToFile(fileName, new []{first});
            VisitInfo[] array = VisitInfo.ReadFromFile(fileName);
            Assert.IsNotNull(array);
            VisitInfo second = array[0];
            _Compare(first, second);
        }

        [TestMethod]
        public void VisitInfo_Save_Read_2()
        {
            VisitInfo first = _GetVisit();
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            VisitInfo.SaveToStream(writer, new []{first});
            byte[] bytes = stream.ToArray();
            VisitInfo[] array = bytes.RestoreArrayFromMemory<VisitInfo>();
            VisitInfo second = array[0];
            _Compare(first, second);
        }

        [TestMethod]
        public void VisitInfo_GetBookPrice_1()
        {
            VisitInfo visit = _GetVisit();
            MarcRecord record = _GetRecord();
            string actual = visit.GetBookPrice(record);
            Assert.AreEqual("2.00", actual);
        }

        [TestMethod]
        public void VisitInfo_GetBookPrice_2()
        {
            VisitInfo visit = _GetVisit();
            MarcRecord record = _GetRecord();
            record.RemoveField(910);
            record.Fields.Add(RecordField.Parse(910, "^A0^B1539325^C20121107^DФКХ^E3.00^HE00401004C75DB57"));
            string actual = visit.GetBookPrice(record);
            Assert.AreEqual("3.00", actual);
        }

        [TestMethod]
        public void VisitInfo_GetBookPrice_3()
        {
            VisitInfo visit = _GetVisit();
            visit.Barcode = null;
            MarcRecord record = _GetRecord();
            record.RemoveField(910);
            record.Fields.Add(RecordField.Parse(910, "^A0^B1539325^C20121107^DФКХ^E4.00"));
            string actual = visit.GetBookPrice(record);
            Assert.AreEqual("4.00", actual);
        }

        [TestMethod]
        public void VisitInfo_GetBookYear_1()
        {
            MarcRecord record = _GetRecord();
            string actual = VisitInfo.GetBookYear(record);
            Assert.AreEqual("1990", actual);
        }

        [TestMethod]
        public void VisitInfo_ToXml_1()
        {
            VisitInfo visit = new VisitInfo();
            Assert.AreEqual("<visit />", XmlUtility.SerializeShort(visit));

            visit = _GetVisit();
            Assert.AreEqual("<visit database=\"IBIS\" index=\"84(2=Рус)5/Ц 27-422726\" inventory=\"1539325\" barcode=\"E00401004C75DB57\" sigla=\"Ф302\" dateGiven=\"20170930\" department=\"*\" dateExpected=\"20171014\" dateReturned=\"20171028\" description=\"Цветаева М. И. Стихотворения\" responsible=\"samo\" timeIn=\"172914\" />", XmlUtility.SerializeShort(visit));
        }

        [TestMethod]
        public void VisitInfo_ToJson_1()
        {
            VisitInfo visit = new VisitInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(visit));

            visit = _GetVisit();
            Assert.AreEqual("{'database':'IBIS','index':'84(2=Рус)5/Ц 27-422726','inventory':'1539325','barcode':'E00401004C75DB57','sigla':'Ф302','dateGiven':'20170930','department':'*','dateExpected':'20171014','dateReturned':'20171028','description':'Цветаева М. И. Стихотворения','responsible':'samo','timeIn':'172914'}", JsonUtility.SerializeShort(visit));
        }

        //[TestMethod]
        public void VisitInfo_ToString_1()
        {
            VisitInfo visit = _GetVisit();
            string expected = "Посещение: \t\t\tFalse\nОписание: \t\t\tЦветаева М. И. Стихотворения\nШифр документа: \t\t84(2=Рус)5/Ц 27-422726\nШтрих-код: \t\t\tE00401004C75DB57\nМесто хранения: \t\tФ302\nДата выдачи: \t\t\t30.09.2017\nМесто выдачи: \t\t\t*\nОтветственное лицо: \t\tsamo\nДата предполагаемого возврата: \t14.10.2017\nВозвращена: \t\t\tTrue\nДата возврата: \t\t\t28.10.2017\nСчетчик продлений: \t\t\t(null)\n------------------------------------------------------------\n";
            Assert.AreEqual(expected, visit.ToString().DosToUnix());
        }
    }
}
