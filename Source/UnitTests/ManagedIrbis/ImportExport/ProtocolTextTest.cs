using System;
using System.Text;
using AM;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.ImportExport
{
    [TestClass]
    public class ProtocolTextTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord
            {
                Mfn = 123,
                Status = RecordStatus.Last,
                Version = 12
            };
            result.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));;
            result.Fields.Add(new RecordField(102, "RU"));
            result.Fields.Add(new RecordField(920, "PAZK"));
            result.Fields.Add(new RecordField(200, "^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]"));

            return result;
        }

        [TestMethod]
        public void ProtocolText_EncodeSubField_1()
        {
            SubField subField = new SubField('a', "Заглавие");
            StringBuilder builder = new StringBuilder();
            ProtocolText.EncodeSubField(builder, subField);
            Assert.AreEqual("^aЗаглавие", builder.ToString());

            subField = new SubField('a');
            builder.Clear();
            ProtocolText.EncodeSubField(builder, subField);
            Assert.AreEqual("^a", builder.ToString());
        }

        [TestMethod]
        public void ProtocolText_EncodeField_1()
        {
            RecordField field = new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218");
            StringBuilder builder = new StringBuilder();
            ProtocolText.EncodeField(builder, field);
            Assert.AreEqual("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\x001F\x001E", builder.ToString());

            field = new RecordField(920, "PAZK");
            builder.Clear();
            ProtocolText.EncodeField(builder, field);
            Assert.AreEqual("920#PAZK\x001F\x001E", builder.ToString());

            field = new RecordField(300);
            builder.Clear();
            ProtocolText.EncodeField(builder, field);
            Assert.AreEqual("300#\x001F\x001E", builder.ToString());
        }

        [TestMethod]
        public void ProtocolText_EncodeRecord_1()
        {
            MarcRecord record = new MarcRecord
            {
                Mfn = 123,
                Status = RecordStatus.Last,
                Version = 12
            };
            record.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            record.Fields.Add(new RecordField(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            string expected = "123#32\u001f\u001e0#12\u001f\u001e692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\u001f\u001e692#^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107\u001f\u001e";
            string actual = ProtocolText.EncodeRecord(record);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProtocolText_ParseLine_1()
        {
            RecordField field = ProtocolText.ParseLine("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218");
            Assert.AreEqual(692, field.Tag);
            Assert.IsNull(field.Value);
            Assert.AreEqual(7, field.SubFields.Count);
            Assert.AreEqual('b', field.SubFields[0].Code);
            Assert.AreEqual("2008/2009", field.SubFields[0].Value);
            Assert.AreEqual('c', field.SubFields[1].Code);
            Assert.AreEqual("O", field.SubFields[1].Value);
            Assert.AreEqual('x', field.SubFields[2].Code);
            Assert.AreEqual("!NOZO", field.SubFields[2].Value);
            Assert.AreEqual('d', field.SubFields[3].Code);
            Assert.AreEqual("42", field.SubFields[3].Value);
            Assert.AreEqual('e', field.SubFields[4].Code);
            Assert.AreEqual("3", field.SubFields[4].Value);
            Assert.AreEqual('z', field.SubFields[5].Code);
            Assert.AreEqual("14.00", field.SubFields[5].Value);
            Assert.AreEqual('g', field.SubFields[6].Code);
            Assert.AreEqual("20081218", field.SubFields[6].Value);
        }

        [TestMethod]
        public void ProtocolText_ParseMfnStatusVersion_1()
        {
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseMfnStatusVersion("123#32", "0#12", record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(123, record1.Mfn);
            Assert.AreEqual(RecordStatus.Last, record1.Status);
            Assert.AreEqual(12, record1.Version);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForReadRecord_1()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendUtf("123#32").NewLine()
                .AppendUtf("0#12").NewLine()
                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").NewLine()
                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForReadRecord(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(123, record1.Mfn);
            Assert.AreEqual(RecordStatus.Last, record1.Status);
            Assert.AreEqual(12, record1.Version);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsNull(record1.Fields[0].Value);
            Assert.AreEqual(7, record1.Fields[0].SubFields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsNull(record1.Fields[1].Value);
            Assert.AreEqual(3, record1.Fields[1].SubFields.Count);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForReadRecord_2()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendUtf("123#32").NewLine()
                .AppendUtf("0#12").NewLine()
                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").NewLine()
                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine()
                .AppendUtf("#").NewLine()
                .AppendUtf("0").NewLine()
                .AppendUtf(IrbisText.WindowsToIrbis("Описание")).NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForReadRecord(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(123, record1.Mfn);
            Assert.AreEqual(RecordStatus.Last, record1.Status);
            Assert.AreEqual(12, record1.Version);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsNull(record1.Fields[0].Value);
            Assert.AreEqual(7, record1.Fields[0].SubFields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsNull(record1.Fields[1].Value);
            Assert.AreEqual(3, record1.Fields[1].SubFields.Count);
            Assert.AreEqual("Описание", record1.Description);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForWriteRecord_1()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendUtf("123#32").NewLine()
                .AppendUtf("0#12").Delimiter()
                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").Delimiter()
                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForWriteRecord(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(123, record1.Mfn);
            Assert.AreEqual(RecordStatus.Last, record1.Status);
            Assert.AreEqual(12, record1.Version);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsNull(record1.Fields[0].Value);
            Assert.AreEqual(7, record1.Fields[0].SubFields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsNull(record1.Fields[1].Value);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForWriteRecord_2()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .NewLine()
                .AppendUtf("0#12").Delimiter()
                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").Delimiter()
                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForWriteRecord(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(0, record1.Mfn);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForWriteRecord_3()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendUtf("123#32").NewLine().NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForWriteRecord(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(0, record1.Mfn);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForWriteRecords_1()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendUtf("123#32").Delimiter()
                .AppendUtf("0#12").Delimiter()
                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").Delimiter()
                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForWriteRecords(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsNull(record1.Fields[0].Value);
            Assert.AreEqual(7, record1.Fields[0].SubFields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsNull(record1.Fields[1].Value);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForAllFormat_1()
        {
            ResponseBuilder builder = new ResponseBuilder();
            builder
                .AppendUtf("0").AppendUtf("\x1F")
                .AppendUtf("123#32").AppendUtf("\x1F")
                .AppendUtf("0#12").AppendUtf("\x1F")
                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").AppendUtf("\x1F")
                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsNull(record1.Fields[0].Value);
            Assert.AreEqual(7, record1.Fields[0].SubFields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsNull(record1.Fields[1].Value);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForAllFormat_2()
        {
            ResponseBuilder builder = new ResponseBuilder();
            byte[] request = EmptyArray<byte>.Value;
            byte[] answer = builder.Encode();
            IIrbisConnection connection = new IrbisConnection();
            ServerResponse response = new ServerResponse(connection, answer, request, true);
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
            Assert.IsNull(record2);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForAllFormat_3()
        {
            string response = "0\x001F123#32\x001F0#12\x001F692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\x001F200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]\x001F";
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsNull(record1.Fields[0].Value);
            Assert.AreEqual(7, record1.Fields[0].SubFields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsNull(record1.Fields[1].Value);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForAllFormat_4()
        {
            string response = null;
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
            Assert.IsNull(record2);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForGblFormat_1()
        {
            string response = "0\x001E692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\x001E200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]\x001E";
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForGblFormat(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsNull(record1.Fields[0].Value);
            Assert.AreEqual(7, record1.Fields[0].SubFields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsNull(record1.Fields[1].Value);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForGblFormat_2()
        {
            string response = null;
            MarcRecord record1 = new MarcRecord();
            MarcRecord record2 = ProtocolText.ParseResponseForGblFormat(response, record1);
            Assert.IsNull(record2);
        }

        [TestMethod]
        public void ProtocolText_ToProtocolText_1()
        {
            MarcRecord record = _GetRecord();
            string expected = "123#32\u001f\u001e"
                + "0#12\u001f\u001e"
                + "692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\u001f\u001e"
                + "102#RU\u001f\u001e"
                + "920#PAZK\u001f\u001e"
                + "200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]\u001f\u001e";
            string actual = record.ToProtocolText();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProtocolText_ToProtocolText_2()
        {
            MarcRecord record = null;
            string expected = null;
            string actual = record.ToProtocolText();
            Assert.AreEqual(expected, actual);
        }
    }
}
