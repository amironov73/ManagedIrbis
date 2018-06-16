using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus3Test
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlus3_ConvertToAnsi_1()
        {
            Execute("+3W", "");
            Execute("+3WРџСЂРёРІРµС‚", "Привет");
        }

        [TestMethod]
        public void UniforPlus3_ConvertToUtf_1()
        {
            Execute("+3U", "");
            Execute("+3UПривет", "РџСЂРёРІРµС‚");
        }

        [TestMethod]
        public void UniforPlus3_ReplacePlus_1()
        {
            Execute("+3+", "");
            Execute("+3+Съешь ещё+этих+мягких французских булок", "Съешь ещё%2Bэтих%2Bмягких французских булок");
        }

        [TestMethod]
        public void UniforPlus3_UrlDecode_1()
        {
            Execute("+3D", "");
            Execute("+3D%D0%A2%D0%B8%D0%BB%D0%B8%2D%D1%82%D0%B8%D0%BB%D0%B8%2C%20%D1%82%D1%80%D0%B0%D0%BB%D0%B8%2D%D0%B2%D0%B0%D0%BB%D0%B8", "Тили-тили, трали-вали");
        }

        [TestMethod]
        public void UniforPlus3_UrlEncode_1()
        {
            Execute("+3E", "");
            Execute("+3EТили-тили, трали-вали", "%D0%A2%D0%B8%D0%BB%D0%B8-%D1%82%D0%B8%D0%BB%D0%B8%2C+%D1%82%D1%80%D0%B0%D0%BB%D0%B8-%D0%B2%D0%B0%D0%BB%D0%B8");
        }

        [TestMethod]
        public void UniforPlus3_FieldsToText_1()
        {
            MarcRecord record = null;
            Execute(record, 0, "+3A", "");

            record = new MarcRecord
            {
                Mfn = 1,
                Version = 2,
                Status = RecordStatus.Last
            };
            Execute(record, 0, "+3A", "");

            record.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            Execute(record, 0, "+3A", "692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\n");

            record.Fields.Add(new RecordField(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            Execute(record, 0, "+3A", "692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\n692#^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107\n");

            record.Fields.Add(new RecordField(102, "RU"));
            record.Fields.Add(new RecordField(10, "^a5-7110-0177-9^d300"));
            record.Fields.Add(new RecordField(675, "37"));
            record.Fields.Add(new RecordField(675, "37(470.311)(03)"));
            record.Fields.Add(new RecordField(964, "14"));
            Execute(record, 0, "+3A", "692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\n692#^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107\n102#RU\n10#^a5-7110-0177-9^d300\n675#37\n675#37(470.311)(03)\n964#14\n");
        }

        [TestMethod]
        public void UniforPlus3_HtmlSpecialChars_1()
        {
            Execute("+3H", "");
            Execute("+3HHello", "Hello");
            Execute("+3HПривет", "Привет");
            Execute("+3HПри<ве>т", "При&lt;ве&gt;т");
            Execute("+3HПри<<ве>т", "При&lt;&lt;ве&gt;т");
            Execute("+3HПри\"\"ве&т", "При&quot;&quot;ве&quot;т");
        }

        [TestMethod]
        public void UniforPlus3_GetTermCount_1()
        {
            Execute("+3J", "");
            Execute("+3JK=ATLAS", "");
            Execute("+3JIBIS,K=ATLAS", "1");
            Execute("+3J,K=ATLAS", "1");
            Execute("+3JIBIS,K=ALTAS", "");
            Execute("+3JIBIS,K=ARIE", "2");
            Execute("+3JIBIS,K=AREI", "");
            Execute("+3JIBIS,K=ARI", "2");
            Execute("+3JIBIS,K=", "1");
        }

        [TestMethod]
        public void UniforPlus3_SearchFormat_1()
        {
            Execute("+3S", "");
            Execute("+3S,2,/K=ATLAS + K=ARIE/,v200^a/", "Докембрий, кембрийский, ордовикский и силурийский периоды\n<Die > jahreszeiten\n");
            Execute("+3S,-2,/K=ATLAS + K=ARIE/,v200^a/", "<Die > jahreszeiten\nДокембрий, кембрийский, ордовикский и силурийский периоды\n");
            Execute("+3S,1,/K=ATLAS + K=ARIE/,v200^a/", "Докембрий, кембрийский, ордовикский и силурийский периоды\n");
            Execute("+3S,-1,/K=ATLAS + K=ARIE/,v200^a/", "<Die > jahreszeiten\n");
            Execute("+3S,0,/K=ATLAS + K=ARIE/,v200^a/", "2");
            Execute("+3S,0,/K=ATLAS + K=ARIE/,", "2");
            Execute("+3S,0,/K=ATLAS + K=ARIE/", "2");
            Execute("+3S,,/K=ATLAS + K=ARIE/", "2");

            Execute("+3S,1,/K=ATLAS/,@breif", "");
            Execute("+3S,1,/K=ATLAS/,@brief", "СССР. Атлас литолого-палеографических карт СССРДокембрий, кембрийский, ордовикский и силурийский периоды : карта, картографическое издание / ред.: Б. М. Келлер, Н. Н. Предтеченский, 1968. - 3 л.");
            Execute("+3S,1,/K=ALTAS/,v200^a", "");

            // Обработка ошибок
            Execute("+3S", "");
            Execute("+3SIBIS", "");
            Execute("+3S,", "");
            Execute("+3S,10", "");
            Execute("+3S,,", "");
            Execute("+3S,,/K", "");
            Execute("+3S,10,/K=/", "");
            Execute("+3S,10,/K=/,", "");
            Execute("+3S,10,/K=ATLAS/,v200^", "");
        }

        [TestMethod]
        public void UniforPlus3_Divide_1()
        {
            Execute("+3T", "0");
            Execute("+3T123,11", "11");
            Execute("+3T123,1.1", "111");
            Execute("+3T12.3,1.1", "11");
            Execute("+3T-12.3,1.1", "-11");
            Execute("+3T12.3,-1.1", "-11");
            Execute("+3T-12.3,-1.1", "11");

            // Обработка ошибок
            Execute("+3T12.3", "0");
            Execute("+3T12.3,0", "");
            Execute("+3T-12.3", "0");
            Execute("+3TA,1", "0");
            Execute("+3T1,A", "");
            Execute("+3T1,", "");
        }
    }
}
