using System;
using System.IO;
using System.Text;

using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisOptTest
        : Common.CommonUnitTest
    {
        private void _TestCompare
            (
                string left,
                string right,
                bool expected
            )
        {
            Assert.AreEqual
                (
                    expected,
                    IrbisOpt.CompareString
                    (
                        left,
                        right
                    )
                );
        }

        [TestMethod]
        public void IrbisOpt_CompareString_1()
        {
            _TestCompare("PAZK", "pazk", true);
            _TestCompare("PAZK", "PAZ", false);
            _TestCompare("PAZK", "PAZK2", false);
            _TestCompare("PAZ+", "PAZ", true);
            _TestCompare("PAZ+", "PAZK", true);
            _TestCompare("PAZ+", "PAZK2", false);
            _TestCompare("SPEC", "PAZK", false);
            _TestCompare("PA+K", "pazk", true);
            _TestCompare("PA+K", "PARK", true);
            _TestCompare("PA+K", "SPEC", false);
            _TestCompare("PA++", "PAZK", true);
            _TestCompare("+++++", string.Empty, true);
            _TestCompare("+++++", "PAZK", true);
        }

        [TestMethod]
        public void IrbisOpt_CompareString_2()
        {
            _TestCompare("PAZK", "", false);
            _TestCompare("+AZK", "PAZK", true);
            _TestCompare("P+ZK", "P", false);
            _TestCompare("P+++", "P", true);
        }

        private void _TestSerialization
            (
                IrbisOpt first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisOpt second = bytes.RestoreObjectFromMemory<IrbisOpt>();

            Assert.AreEqual(first.WorksheetLength, second.WorksheetLength);
            Assert.AreEqual(first.WorksheetTag, second.WorksheetTag);
            Assert.AreEqual(first.Items.Count, second.Items.Count);
            for (int i = 0; i < first.Items.Count; i++)
            {
                Assert.AreEqual(first.Items[i].Key, second.Items[i].Key);
                Assert.AreEqual(first.Items[i].Value, second.Items[i].Value);
            }
        }

        [TestMethod]
        public void IrbisOpt_LoadFromOptFile_1()
        {
            string filePath = Path.Combine
                (
                    TestDataPath,
                    "WS31.OPT"
                );

            IrbisOpt opt = IrbisOpt.LoadFromOptFile(filePath);
            Assert.IsNotNull(opt);
            Assert.AreEqual(920, opt.WorksheetTag);
            Assert.AreEqual(5, opt.WorksheetLength);
            Assert.AreEqual(14, opt.Items.Count);

            string optimized = opt.SelectWorksheet("UNKN");
            Assert.AreEqual("PAZK42", optimized);

            opt.Validate(true);

            _TestSerialization(opt);

            StringWriter writer = new StringWriter();
            opt.WriteOptFile(writer);
            string actual = writer.ToString().Replace("\r\n", "\n");
            string expected = File.ReadAllText(filePath, Encoding.Default)
                .Replace("\r\n", "\n");
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IrbisOpt_SetWorksheetLength_1()
        {
            IrbisOpt opt = new IrbisOpt();
            opt.SetWorksheetLength(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void IrbisOpt_SelectWorksheet_1()
        {
            IrbisOpt opt = new IrbisOpt();
            opt.SelectWorksheet("NOWS");
        }

        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField(700);
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField(701);
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField(200);
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField(210);
            field.AddSubField('a', "Иркутск");
            field.AddSubField('d', "2016");
            result.Fields.Add(field);

            field = new RecordField(215);
            field.AddSubField('a', "123");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            result.Fields.Add(field);

            field = new RecordField(920, "PAZK");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void IrbisOpt_GetWorksheet_1()
        {
            IrbisOpt opt = new IrbisOpt();
            opt.SetWorksheetTag(920);
            MarcRecord record = _GetRecord();
            const string expected = "PAZK";
            string actual = opt.GetWorksheet(record);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisOpt_LoadFromServer_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "WS31.OPT"
                    );
                IrbisOpt opt = IrbisOpt.LoadFromServer
                    (
                        provider,
                        specification
                    );
                Assert.IsNotNull(opt);
                Assert.AreEqual(14, opt.Items.Count);
            }
        }

        [TestMethod]
        public void IrbisOpt_LoadFromServer_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "NOSUCH.OPT"
                    );
                IrbisOpt opt = IrbisOpt.LoadFromServer
                    (
                        provider,
                        specification
                    );
                Assert.IsNull(opt);
            }
        }

        [TestMethod]
        public void IrbisOpt_WriteOptFile_1()
        {
            string fileName = Path.GetTempFileName();
            IrbisOpt opt = new IrbisOpt();
            opt.SetWorksheetLength(5);
            opt.SetWorksheetTag(920);
            opt.Items.Add(new IrbisOpt.Item { Key = "OGO", Value = "AGA" });
            opt.Items.Add(new IrbisOpt.Item { Key = "UGU", Value = "EGE" });
            opt.WriteOptFile(fileName);
            string actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual("920\n5\nOGO   AGA\nUGU   EGE\n*****\n", actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void IrbisOpt_Validate_1()
        {
            IrbisOpt opt = new IrbisOpt();
            opt.Validate(true);
        }
    }
}
