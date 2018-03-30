using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusPlus0Test
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusPlus0_FormatAll_1()
        {
            MarcRecord record = null;
            Execute(record, 0, "++0", "");

            record = new MarcRecord
            {
                Mfn = 1,
                Version = 2,
                Status = RecordStatus.Last
            };
            Execute(record, 0, "++0", "");

            record.Fields.Add(new RecordField(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            record.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            record.Fields.Add(new RecordField(102, "RU"));
            record.Fields.Add(new RecordField(10, "^a5-7110-0177-9^d300"));
            record.Fields.Add(new RecordField(11));
            record.Fields.Add(new RecordField(953, "Field953"));
            record.Fields.Add(new RecordField(675, "37"));
            record.Fields.Add(new RecordField(675, "37(470.311)(03)"));
            record.Fields.Add(new RecordField(964, "14"));
            Execute(record, 0, "++0", " 2007/2008 O ЗИ 25 4 6.25 20080107\n 2008/2009 O !NOZO 42 3 14.00 20081218\nRU\n 5-7110-0177-9 300\n37\n37(470.311)(03)\n14\n");

            Execute(record, 0, "++0,692", "RU\n 5-7110-0177-9 300\n37\n37(470.311)(03)\n14\n");
            Execute(record, 0, "++0,692,675", "RU\n 5-7110-0177-9 300\n14\n");
        }
    }
}
