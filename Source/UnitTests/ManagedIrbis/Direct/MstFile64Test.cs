using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstFile64Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private MstRecord64 _GetRecord()
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

            MstRecord64 result = MstRecord64.EncodeRecord(record);

            return result;
        }

        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS/ibis.mst"
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
            DirectUtility.CreateDatabase64(path);
            string result = path + ".mst";

            return result;
        }

        [TestMethod]
        public void MstFile64_ReadRecord_1()
        {
            string fileName = _GetFileName();
            using (MstFile64 file = new MstFile64(fileName, DirectAccessMode.ReadOnly))
            {
                Assert.AreSame(fileName, file.FileName);
                Assert.AreEqual(333, file.ControlRecord.NextMfn);
                MstRecord64 record = file.ReadRecord(22951100L);
                Assert.AreEqual(100, record.Dictionary.Count);
                string expected = "Tag: 200, Position: 2652, Length: 173, Text: ^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
                Assert.AreEqual(expected, record.Dictionary[87].ToString());
            }
        }

        [TestMethod]
        public void MstFile64_LockDatabase_1()
        {
            string fileName = _CreateDatabase();
            using (MstFile64 file = new MstFile64(fileName, DirectAccessMode.Exclusive))
            {
                file.LockDatabase(true);
                Assert.IsTrue(file.ReadDatabaseLockedFlag());
                file.LockDatabase(false);
                Assert.IsFalse(file.ReadDatabaseLockedFlag());
            }
        }

        [TestMethod]
        public void MstFile64_ReopenFile_1()
        {
            string fileName = _CreateDatabase();
            using (MstFile64 file = new MstFile64(fileName, DirectAccessMode.ReadOnly))
            {
                Assert.AreEqual(DirectAccessMode.ReadOnly, file.Mode);
                file.ReopenFile(DirectAccessMode.Exclusive);
                Assert.AreEqual(DirectAccessMode.Exclusive, file.Mode);
            }
        }

        [TestMethod]
        public void MstFile64_WriteRecord_1()
        {
            string fileName = _CreateDatabase();
            using (MstFile64 file = new MstFile64(fileName, DirectAccessMode.Exclusive))
            {
                MstRecord64 record = _GetRecord();
                MstRecordLeader64 leader1 = record.Leader;
                leader1.Mfn = 1;
                leader1.Version = 1;
                record.Leader = leader1;
                record.Prepare();
                long offset = file.WriteRecord(record);
                file.UpdateLeader(leader1, offset);
                Assert.AreNotEqual(0L, offset);
                file.UpdateControlRecord(true);
                MstRecordLeader64 leader2 = file.ReadLeader(offset);
                Assert.AreEqual(leader1.Mfn, leader2.Mfn);
                Assert.AreEqual(leader1.Length, leader2.Length);
            }
        }
    }
}
