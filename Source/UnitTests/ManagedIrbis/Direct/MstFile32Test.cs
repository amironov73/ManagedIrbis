using System;
using System.IO;

using AM;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstFile32Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private MstRecord32 _GetRecord()
        {
            MarcRecord record = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            record.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            record.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            record.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            record.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            record.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            record.Fields.Add(field);

            MstRecord32 result = MstRecord32.EncodeRecord(record);

            return result;
        }

        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis32RootPath,
                    "ibis.mst"
                );
        }

        [NotNull]
        private string _CreateDatabase()
        {
            Random random = new Random();
            string directory = Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToInvariantString()
                );
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, "database");
            DirectUtility.CreateDatabase32(path);
            string result = path + ".mst";

            return result;
        }

        [TestMethod]
        public void MstFile32_ReadRecord_1()
        {
            string fileName = _GetFileName();
            using (MstFile32 file = new MstFile32(fileName))
            {
                Assert.AreSame(fileName, file.FileName);
                Assert.AreEqual(270, file.ControlRecord.NextMfn);
                MstRecord32 record = file.ReadRecord(3780548L);
                Assert.AreEqual(88, record.Dictionary.Count);
                string expected = "Tag: 691, Position: 2405, Length: 97, Text: ^! 3^IАКТ^DАктинометрия^SОПД^BФЭиОЭП^KУМО^AЗИ^Vспц^Oд/о^C310700^F3^WАКТ/3^GОсн^0ЗИ310700спцд/о-S3";
                Assert.AreEqual(expected, record.Dictionary[87].ToString());
            }
        }

        [TestMethod]
        public void MstFile32_ReadRecord2_1()
        {
            string fileName = _GetFileName();
            using (MstFile32 file = new MstFile32(fileName))
            {
                Assert.AreSame(fileName, file.FileName);
                Assert.AreEqual(270, file.ControlRecord.NextMfn);
                MstRecord32 record = file.ReadRecord2(3780548L);
                Assert.AreEqual(88, record.Dictionary.Count);
                string expected = "Tag: 691, Position: 2405, Length: 97, Text: ^! 3^IАКТ^DАктинометрия^SОПД^BФЭиОЭП^KУМО^AЗИ^Vспц^Oд/о^C310700^F3^WАКТ/3^GОсн^0ЗИ310700спцд/о-S3";
                Assert.AreEqual(expected, record.Dictionary[87].ToString());
            }
        }

    }
}
