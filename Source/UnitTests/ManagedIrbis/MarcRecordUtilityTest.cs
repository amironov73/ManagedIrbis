using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Moq;

// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable AssignNullToNotNullAttribute

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class MarcRecordUtilityTest
    {
        [NotNull]
        [ItemNotNull]
        private string[] _GetLines()
        {
            string[] result =
            {
                "1#0\x1F"
                + "0#4\x1F"
                + "101#rus\x1F"
                + "102#RU\x1F"
                + "331#О положении дел в Ангарской нефтехимической компании после перехода ее в собственность компании ЮКОС.\x1F"
                + "621#65.304.13\x1F"
                + "700#^34^AЛаврентьев^BО.^GОлег\x1F"
                + "903#65.304.13-772296\x1F"
                + "919#^Arus^N0102\x1F"
                + "999#0000000\x1F"
                + "463#^CСМ Номер один^J2000^S4^V25 дек.\x1F"
                + "900#^Ta^B12\x1F"
                + "920#ASP\x1F"
                + "629#^BМестное изд. с краеведческим материалом^C80\x1F"
                + "690#^L05.05.03\x1F"
                + "661#^A2000^B2000\x1F"
                + "200#^AБудущее Ангарской нефтехимической компании поставлено на карту^FО. Лаврентьев\x1F"
                + "606#^3IRPL-0000014802-332461^A\"Ангарская нефтехимическая компания\", нефтеперерабатывающее предприятие (Ангарск, город; Иркутская область)^BМенеджмент\x1F"
                + "905#521962^D1^J1^21",

                "25645#0\x1F"
                + "0#3\x1F"
                + "10#^A5-94227-084-8^D77.78\x1F"
                + "60#12\x1F"
                + "101#rus\x1F"
                + "102#RU\x1F"
                + "205#^A3-е изд., перераб. и доп.\x1F"
                + "210#^D2001^CЮрайт-М^AМосква\x1F"
                + "215#^A352\x1F"
                + "606#^AПрокурорский надзор\x1F"
                + "621#67.629.1\x1F"
                + "702#^U2^4Ред.^AВинокуров^BЮ.Е.^GЮрий Евгеньевич\x1F"
                + "900#^Ta^B05^C44\x1F"
                + "908#П 80\x1F"
                + "920#PAZK\x1F"
                + "964#10.71.35\x1F"
                + "903#67.629.1/П80-137874\x1F"
                + "200#^AПрокурорский надзор^EУчебник^FПод ред.  Ю.Е. Винокурова\x1F"
                + "910#^A0^B1641483^DФ202^U2002/73^Y73^C20020902^hE00401004DD691BD^!Ф202^s20160530\x1F"
                + "905#^d1^J1^S1^21\x1F"
                + "906#67.629.1\x1F"
                + "999#6"

            };

            return result;
        }

        [NotNull]
        private RecordField _GetField()
        {
            RecordField result = new RecordField(701)
                .AddSubField('a', "Сидоров")
                .AddSubField('b', "С. С.");

            return result;
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void MarcRecordUtility_AddField_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            MarcRecord result = record.AddField(field);
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreSame(field, record.Fields[0]);
            Assert.AreSame(record, field.Record);
        }

        [TestMethod]
        public void MarcRecordUtility_AddField_2()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            MarcRecord result = record.AddField(field.Tag, field);
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreSame(field, record.Fields[0]);
            Assert.AreSame(record, field.Record);
        }

        [TestMethod]
        public void MarcRecordUtility_AddField_3()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.AddField(701, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(701, record.Fields[0].Tag);
            Assert.AreEqual(2, result.Fields[0].SubFields.Count);
            Assert.AreEqual("Сидоров", record.Fields[0].GetFirstSubFieldValue('a'));
        }

        [TestMethod]
        public void MarcRecordUtility_AddField_4()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.AddField(701, string.Empty);
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(701, record.Fields[0].Tag);
            Assert.AreEqual(0, result.Fields[0].SubFields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_AddField_5()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.AddField(701, 555);
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(701, record.Fields[0].Tag);
            Assert.AreEqual(0, result.Fields[0].SubFields.Count);
            Assert.AreEqual(555.ToString(), result.Fields[0].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MarcRecordUtility_AddField_6()
        {
            MarcRecord record = new MarcRecord();
            object value = null;
            record.AddField(701, value);
        }

        [TestMethod]
        public void MarcRecordUtility_AddNonEmptyField_1()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.AddNonEmptyField(701, _GetField());
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_AddNonEmptyField_2()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.AddNonEmptyField(701, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_AddNonEmptyField_3()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.AddNonEmptyField(701, string.Empty);
            Assert.AreSame(record, result);
            Assert.AreEqual(0, record.Fields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_AddNonEmptyField_4()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.AddNonEmptyField(701, null);
            Assert.AreSame(record, result);
            Assert.AreEqual(0, record.Fields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_BeginUpdate_1()
        {
            MarcRecord record = new MarcRecord();
            record.BeginUpdate();
            RecordField field = new RecordField(100);
            record.Fields.Add(field);
            Assert.AreEqual(0, field.Repeat);
            field = new RecordField(200);
            record.Fields.Add(field);
            Assert.AreEqual(0, field.Repeat);
            field = new RecordField(300);
            record.Fields.Add(field);
            Assert.AreEqual(0, field.Repeat);
            record.EndUpdate();
            field = new RecordField(400);
            record.Fields.Add(field);
            Assert.AreEqual(1, field.Repeat);
        }

        [TestMethod]
        public void MarcRecordUtility_BeginUpdate_2()
        {
            MarcRecord record = new MarcRecord();
            record.BeginUpdate(10);
            RecordField field = new RecordField(100);
            record.Fields.Add(field);
            Assert.AreEqual(0, field.Repeat);
            field = new RecordField(200);
            record.Fields.Add(field);
            Assert.AreEqual(0, field.Repeat);
            field = new RecordField(300);
            record.Fields.Add(field);
            Assert.AreEqual(0, field.Repeat);
            record.EndUpdate();
            field = new RecordField(400);
            record.Fields.Add(field);
            Assert.AreEqual(1, field.Repeat);
        }

        [TestMethod]
        public void MarcRecordUtility_HaveField_1()
        {
            MarcRecord record = new MarcRecord();
            Assert.IsFalse(record.HaveField(700));
            record.AddField(200, "^aЗаглавие");
            record.AddField(700, "^aСидоров^bС. С.");
            Assert.IsTrue(record.HaveField(700));
        }

        [TestMethod]
        public void MarcRecordUtility_HaveField_2()
        {
            MarcRecord record = new MarcRecord();
            Assert.IsFalse(record.HaveField(700, 701));
            record.AddField(200, "^aЗаглавие");
            record.AddField(700, "^aСидоров^bС. С.");
            Assert.IsTrue(record.HaveField(700, 701));
            record.AddField(701, "^aКозлов^bК. К.");
            Assert.IsTrue(record.HaveField(700, 701));
        }

        [TestMethod]
        public void MarcRecordUtility_HaveNotField_1()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(200, "^aЗаглавие");
            Assert.IsTrue(record.HaveNotField(700));
            record.AddField(700, "^aСидоров^bС. С.");
            Assert.IsFalse(record.HaveNotField(700));
        }

        [TestMethod]
        public void MarcRecordUtility_HaveNotField_2()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(200, "^aЗаглавие");
            Assert.IsTrue(record.HaveNotField(700, 701));
            record.AddField(700, "^aСидоров^bС. С.");
            Assert.IsFalse(record.HaveNotField(700, 701));
            record.AddField(701, "^aКозлов^bК. К.");
            Assert.IsFalse(record.HaveNotField(700, 701));
        }

        [TestMethod]
        public void MarcRecordUtility_ParseAllFormat_1()
        {
            string host = "host";
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Host).Returns(host);
            IIrbisConnection connection = mock.Object;
            AbstractEngine executive = new StandardEngine(connection, null);
            mock.SetupGet(c => c.Executive).Returns(executive);
            string database = "IBIS";
            string[] lines = _GetLines();
            ResponseBuilder builder = new ResponseBuilder();
            foreach (string line in lines)
            {
                builder.AppendUtf(line);
                builder.NewLine();
            }
            byte[] rawAnswer = builder.Encode();
            byte[][] rawRequest = { new byte[0], new byte[0] };
            ServerResponse response = new ServerResponse
                (
                    connection,
                    rawAnswer,
                    rawRequest,
                    true
                );
            MarcRecord[] records = MarcRecordUtility.ParseAllFormat
                (
                    database,
                    response
                );
            Assert.AreEqual(lines.Length, records.Length);
            for (int i = 0; i < records.Length; i++)
            {
                MarcRecord record = records[i];
                Assert.AreEqual(host, record.HostName);
                Assert.AreEqual(database, record.Database);
            }
        }

        [TestMethod]
        public void MarcRecordUtility_ParseAllFormat_2()
        {
            string host = "host";
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            mock.SetupGet(c => c.Host).Returns(host);
            IIrbisConnection connection = mock.Object;

            string database = "IBIS";
            string[] lines = _GetLines();
            MarcRecord[] records = MarcRecordUtility.ParseAllFormat
                (
                    database,
                    connection,
                    lines
                );
            Assert.AreEqual(lines.Length, records.Length);
            for (int i = 0; i < records.Length; i++)
            {
                MarcRecord record = records[i];
                Assert.AreEqual(host, record.HostName);
                Assert.AreEqual(database, record.Database);
            }
        }

        [TestMethod]
        public void MarcRecordUtiltity_RemoveField_1()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.RemoveField(700);
            Assert.AreSame(record, result);

            record.AddField(700, "^aСидоров^bС. С.");
            result = record.RemoveField(700);
            Assert.AreSame(record, result);

            record.AddField(200, "^aЗаглавие");
            record.AddField(700, "^aИванов^bИ. И.");
            record.AddField(700, "^aПетров^bП. П.");
            record.AddField(700, "^aСидоров^bС. С.");
            result = record.RemoveField(700);
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_ReplaceField_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField[] replacement =
            {
                new RecordField(702, "^aИванов^bИ. И."),
                new RecordField(702, "^aПетров^bП. П."),
                new RecordField(702, "^aСидоров^bС. С."),
            };
            MarcRecord result = record.ReplaceField(700, replacement);
            Assert.AreSame(record, result);
            Assert.AreEqual(3, record.Fields.Count);
            Assert.IsFalse(record.HaveField(700));
            Assert.IsTrue(record.HaveField(702));
        }

        [TestMethod]
        public void MarcRecordUtility_ReplaceField_2()
        {
            MarcRecord record = new MarcRecord();
            RecordField[] replacement =
            {
                new RecordField(702, "^aИванов^bИ. И."),
                new RecordField(702, "^aПетров^bП. П."),
                new RecordField(702, "^aСидоров^bС. С."),
            };
            record.AddField(700, "^aСидоров^bС. С.");
            MarcRecord result = record.RemoveField(700);
            Assert.AreSame(record, result);
            result = record.ReplaceField(700, replacement);
            Assert.AreSame(record, result);
            Assert.AreEqual(3, record.Fields.Count);
            Assert.IsFalse(record.HaveField(700));
            Assert.IsTrue(record.HaveField(702));
        }

        [TestMethod]
        public void MarcRecordUtility_ReplaceField_3()
        {
            MarcRecord record = new MarcRecord();
            RecordField[] replacement =
            {
                new RecordField(702, "^aИванов^bИ. И."),
                new RecordField(702, "^aПетров^bП. П."),
                new RecordField(702, "^aСидоров^bС. С."),
            };
            record.AddField(200, "^aЗаглавие");
            record.AddField(700, "^aИванов^bИ. И.");
            record.AddField(700, "^aПетров^bП. П.");
            record.AddField(700, "^aСидоров^bС. С.");
            MarcRecord result = record.ReplaceField(700, replacement);
            Assert.AreSame(record, result);
            Assert.AreEqual(4, record.Fields.Count);
            Assert.IsFalse(record.HaveField(700));
            Assert.IsTrue(record.HaveField(702));
        }

        [TestMethod]
        public void MarcRecordUtility_SetField_1()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.SetField(700, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Сидоров", record.Fields[0].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetField_2()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            MarcRecord result = record.SetField(700, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Сидоров", record.Fields[0].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetField_3()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            record.AddField(700, "^aПетров^bП. П.");
            MarcRecord result = record.SetField(700, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(2, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Сидоров", record.Fields[0].SubFields[0].Value);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Петров", record.Fields[1].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetField_4()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.SetField(700, 1, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(0, record.Fields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_SetField_5()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            MarcRecord result = record.SetField(700, 1, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Иванов", record.Fields[0].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetField_6()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            record.AddField(700, "^aПетров^bП. П.");
            MarcRecord result = record.SetField(700, 1, "^aСидоров^bС. С.");
            Assert.AreSame(record, result);
            Assert.AreEqual(2, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Иванов", record.Fields[0].SubFields[0].Value);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Сидоров", record.Fields[1].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_1()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.SetSubField(700, 'a', "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(1, record.Fields[0].SubFields.Count);
            Assert.AreEqual('a', record.Fields[0].SubFields[0].Code);
            Assert.AreEqual("Сидоров", record.Fields[0].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_2()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            MarcRecord result = record.SetSubField(700, 'a', "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual('a', record.Fields[0].SubFields[0].Code);
            Assert.AreEqual("Сидоров", record.Fields[0].SubFields[0].Value);
            Assert.AreEqual('b', record.Fields[0].SubFields[1].Code);
            Assert.AreEqual("И. И.", record.Fields[0].SubFields[1].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_3()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            record.AddField(700, "^aПетров^bП. П.");
            MarcRecord result = record.SetSubField(700, 'a', "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(2, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual('a', record.Fields[0].SubFields[0].Code);
            Assert.AreEqual("Сидоров", record.Fields[0].SubFields[0].Value);
            Assert.AreEqual('b', record.Fields[0].SubFields[1].Code);
            Assert.AreEqual("И. И.", record.Fields[0].SubFields[1].Value);
            Assert.AreEqual(2, record.Fields[1].SubFields.Count);
            Assert.AreEqual("Петров", record.Fields[1].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_4()
        {
            MarcRecord record = new MarcRecord();
            MarcRecord result = record.SetSubField(700, 1, 'a', 1, "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(0, record.Fields.Count);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_5()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            MarcRecord result = record.SetSubField(700, 1, 'a', 1, "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Иванов", record.Fields[0].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_6()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.");
            MarcRecord result = record.SetSubField(700, 1, 'a', 1, "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(2, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Иванов", record.Fields[0].SubFields[0].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_7()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.^aСнова Иванов^bСнова И. И.");
            MarcRecord result = record.SetSubField(700, 1, 'a', 1, "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(4, record.Fields[0].SubFields.Count);
            Assert.AreEqual("Иванов", record.Fields[0].SubFields[0].Value);
            Assert.AreEqual("И. И.", record.Fields[0].SubFields[1].Value);
            Assert.AreEqual("Снова Иванов", record.Fields[0].SubFields[2].Value);
            Assert.AreEqual("Снова И. И.", record.Fields[0].SubFields[3].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_SetSubField_8()
        {
            MarcRecord record = new MarcRecord();
            record.AddField(700, "^aИванов^bИ. И.^aСнова Иванов^bСнова И. И.");
            record.AddField(700, "^aПетров^bП. П.^aСнова Петров^bСнова П. П.");
            MarcRecord result = record.SetSubField(700, 1, 'a', 1, "Сидоров");
            Assert.AreSame(record, result);
            Assert.AreEqual(2, record.Fields.Count);
            Assert.AreEqual(4, record.Fields[1].SubFields.Count);
            Assert.AreEqual("Петров", record.Fields[1].SubFields[0].Value);
            Assert.AreEqual("П. П.", record.Fields[1].SubFields[1].Value);
            Assert.AreEqual("Сидоров", record.Fields[1].SubFields[2].Value);
            Assert.AreEqual("Снова П. П.", record.Fields[1].SubFields[3].Value);
        }

        [TestMethod]
        public void MarcRecordUtility_ToJson_1()
        {
            MarcRecord record = new MarcRecord();
            Assert.AreEqual("{\"fields\":[]}", MarcRecordUtility.ToJson(record).DosToUnix());

            record = _GetRecord();
            Assert.AreEqual("{\"fields\":[{\"tag\":700,\"subfields\":[{\"code\":\"a\",\"value\":\"Иванов\"},{\"code\":\"b\",\"value\":\"И. И.\"}]},{\"tag\":701,\"subfields\":[{\"code\":\"a\",\"value\":\"Петров\"},{\"code\":\"b\",\"value\":\"П. П.\"}]},{\"tag\":200,\"subfields\":[{\"code\":\"a\",\"value\":\"Заглавие\"},{\"code\":\"e\",\"value\":\"подзаголовочное\"},{\"code\":\"f\",\"value\":\"И. И. Иванов, П. П. Петров\"}]},{\"tag\":300,\"value\":\"Первое примечание\"},{\"tag\":300,\"value\":\"Второе примечание\"},{\"tag\":300,\"value\":\"Третье примечание\"}]}", MarcRecordUtility.ToJson(record).DosToUnix());
        }

        //[TestMethod]
        //public void MarcRecordUtility_ToYaml_1()
        //{
        //    MarcRecord record = new MarcRecord();
        //    Assert.AreEqual("Fields: []\n", MarcRecordUtility.ToYaml(record).DosToUnix());

        //    record = _GetRecord();
        //    Assert.AreEqual("", MarcRecordUtility.ToYaml(record).DosToUnix());
        //}

        [TestMethod]
        public void MarcRecordUtility_ToSourceCode_1()
        {
            MarcRecord record = new MarcRecord();
            Assert.AreEqual
                (
                    "new MarcRecord()",
                    MarcRecordUtility.ToSourceCode(record)
                );
        }

        [TestMethod]
        public void MarcRecordUtility_ToSourceCode_2()
        {
            MarcRecord record = new MarcRecord()
                .AddField(100, "Field100");
            Assert.AreEqual
                (
                    "new MarcRecord()\n"
                    + ".AddField(new RecordField(100, \"Field100\"))",
                    MarcRecordUtility.ToSourceCode(record).DosToUnix()
                );
        }

        [TestMethod]
        public void MarcRecordUtility_ToSourceCode_3()
        {
            MarcRecord record = new MarcRecord()
                .AddField(100, "Field100")
                .AddField(200, new SubField('a', "SubA"));
            Assert.AreEqual
                (
                    "new MarcRecord()\n"
                    + ".AddField(new RecordField(100, \"Field100\"))\n"
                    + ".AddField(new RecordField(200,\n"
                    + "new SubField('a', \"SubA\")))",
                    MarcRecordUtility.ToSourceCode(record).DosToUnix()
                );
        }
    }
}
