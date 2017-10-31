using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus0Test
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlus0_FormatAll_1()
        {
            MarcRecord record = null;
            Execute(record, 0, "+0", "");

            record = new MarcRecord
            {
                Mfn = 1,
                Version = 2,
                Status = RecordStatus.Last
            };
            Execute(record, 0, "+0", "0\n1#0\n0#2\n");

            record.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            Execute(record, 0, "+0", "0\n1#0\n0#2\n692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\n");

            record.Fields.Add(new RecordField(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            Execute(record, 0, "+0", "0\n1#0\n0#2\n692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\n692#^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107\n");

            record.Fields.Add(new RecordField(102, "RU"));
            record.Fields.Add(new RecordField(10, "^a5-7110-0177-9^d300"));
            record.Fields.Add(new RecordField(675, "37"));
            record.Fields.Add(new RecordField(675, "37(470.311)(03)"));
            record.Fields.Add(new RecordField(964, "14"));
            Execute(record, 0, "+0", "0\n1#0\n0#2\n692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\n692#^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107\n102#RU\n10#^a5-7110-0177-9^d300\n675#37\n675#37(470.311)(03)\n964#14\n");
        }
    }
}
