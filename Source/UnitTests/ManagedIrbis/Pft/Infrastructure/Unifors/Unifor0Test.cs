using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor0Test
        : CommonUniforTest
    {
        [TestMethod]
        public void Unifor0_FormatAll_1()
        {
            MarcRecord record = null;
            Execute(record, 0, "0", "");

            record = new MarcRecord
            {
                Mfn = 1,
                Version = 2,
                Status = RecordStatus.Last
            };
            Execute(record, 0, "0", "\\par fields: 0 data size: 0 record size: 32");

            record.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            Execute(record, 0, "0", "\\b #692/1:_\\b0 ^b2008/2009^cO^x!NOZO^d42^e3^z14.00^g20081218\\par \\par fields: 1 data size: 45 record size: 89");

            record.Fields.Add(new RecordField(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            Execute(record, 0, "0", "\\b #692/1:_\\b0 ^b2008/2009^cO^x!NOZO^d42^e3^z14.00^g20081218\\par \\b #692/2:_\\b0 ^b2007/2008^cO^a\\u1047?\\u1048?^d25^e4^f6.25^g20080107\\par \\par fields: 2 data size: 88 record size: 144");

            record.Fields.Add(new RecordField(102, "RU"));
            record.Fields.Add(new RecordField(10, "^a5-7110-0177-9^d300"));
            record.Fields.Add(new RecordField(675, "37"));
            record.Fields.Add(new RecordField(675, "37(470.311)(03)"));
            record.Fields.Add(new RecordField(964, "14"));
            Execute(record, 0, "0", "\\b #692/1:_\\b0 ^b2008/2009^cO^x!NOZO^d42^e3^z14.00^g20081218\\par \\b #692/2:_\\b0 ^b2007/2008^cO^a\\u1047?\\u1048?^d25^e4^f6.25^g20080107\\par \\b #102/1:_\\b0 RU\\par \\b #10/1:_\\b0 ^a5-7110-0177-9^d300\\par \\b #675/1:_\\b0 37\\par \\b #675/2:_\\b0 37(470.311)(03)\\par \\b #964/1:_\\b0 14\\par \\par fields: 7 data size: 129 record size: 245");

            record = new MarcRecord
            {
                Mfn = 1,
                Version = 2,
                Status = RecordStatus.Last
            };
            record.Fields.Add(new RecordField(1, "Field1"));
            record.Fields.Add(new RecordField(2));
            record.Fields.Add(new RecordField(3, "Field3"));
            record.Fields.Add(new RecordField(953, "Field953"));
            Execute(record, 0, "0", "\\b #1/1:_\\b0 Field1\\par \\b #3/1:_\\b0 Field3\\par \\b #953/1:_\\b0 [Internal Resource]\\par \\par fields: 3 data size: 20 record size: 88");
        }
    }
}
