using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusAtTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusAt_FormatJson_1()
        {
            MarcRecord record = null;
            Execute(record, 0, "+@", "");

            record = new MarcRecord
            {
                Mfn = 1,
                Version = 2,
                Status = RecordStatus.Last
            };
            Execute(record, 0, "+@", "0\n1#0\n0#2\n{}");

            record.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            Execute(record, 0, "+@", "0\n1#0\n0#2\n{\n  \"692\": {\n    \"0\": {\n      \"B\": \"2008/2009\",\n      \"C\": \"O\",\n      \"X\": \"!NOZO\",\n      \"D\": \"42\",\n      \"E\": \"3\",\n      \"Z\": \"14.00\",\n      \"G\": \"20081218\"\n    }\n  }\n}");

            record.Fields.Add(new RecordField(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            Execute(record, 0, "+@", "0\n1#0\n0#2\n{\n  \"692\": {\n    \"0\": {\n      \"B\": \"2008/2009\",\n      \"C\": \"O\",\n      \"X\": \"!NOZO\",\n      \"D\": \"42\",\n      \"E\": \"3\",\n      \"Z\": \"14.00\",\n      \"G\": \"20081218\"\n    },\n    \"1\": {\n      \"B\": \"2007/2008\",\n      \"C\": \"O\",\n      \"A\": \"ЗИ\",\n      \"D\": \"25\",\n      \"E\": \"4\",\n      \"F\": \"6.25\",\n      \"G\": \"20080107\"\n    }\n  }\n}");

            record.Fields.Add(new RecordField(102, "RU"));
            record.Fields.Add(new RecordField(10, "^a5-7110-0177-9^d300"));
            record.Fields.Add(new RecordField(675, "37"));
            record.Fields.Add(new RecordField(675, "37(470.311)(03)"));
            record.Fields.Add(new RecordField(964, "14"));
            Execute(record, 0, "+@", "0\n1#0\n0#2\n{\n  \"10\": {\n    \"0\": {\n      \"a\": \"5-7110-0177-9\",\n      \"d\": \"300\"\n    }\n  },\n  \"102\": {\n    \"0\": {\n      \"*\": \"RU\"\n    }\n  },\n  \"675\": {\n    \"0\": {\n      \"*\": \"37\"\n    },\n    \"1\": {\n      \"*\": \"37(470.311)(03)\"\n    }\n  },\n  \"692\": {\n    \"0\": {\n      \"B\": \"2008/2009\",\n      \"C\": \"O\",\n      \"X\": \"!NOZO\",\n      \"D\": \"42\",\n      \"E\": \"3\",\n      \"Z\": \"14.00\",\n      \"G\": \"20081218\"\n    },\n    \"1\": {\n      \"B\": \"2007/2008\",\n      \"C\": \"O\",\n      \"A\": \"ЗИ\",\n      \"D\": \"25\",\n      \"E\": \"4\",\n      \"F\": \"6.25\",\n      \"G\": \"20080107\"\n    }\n  },\n  \"964\": {\n    \"0\": {\n      \"*\": \"14\"\n    }\n  }\n}");

        }
    }
}
