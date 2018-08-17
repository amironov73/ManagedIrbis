using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.ImportExport;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.ImportExport
{
    [TestClass]
    public class Iso2709Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private MarcRecord GetRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218"));
            result.Fields.Add(new RecordField(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107"));
            result.Fields.Add(new RecordField(692, "^B2008/2009^CV^AЗИ^D25^E0^F0^S0^G20090830"));
            result.Fields.Add(new RecordField(692, "^B2008/2009^CV^D17^X!COM^K21^E0^M0^G20090830"));
            result.Fields.Add(new RecordField(692, "^B2008/2009^CV^X!NOFOND^D42^K46^E0^N0^S14.00^G20090830"));
            result.Fields.Add(new RecordField(692, "^B2007/2008^CV^AЗИ^D25^S0.00^G20080501"));
            result.Fields.Add(new RecordField(692, "^B2009/2010^CO^AЗИ^D25^E4^F6.25^G20100511"));
            result.Fields.Add(new RecordField(692, "^B2009/2010^CO^D17^X!COM^K21^E0^M0^G20100511"));
            result.Fields.Add(new RecordField(692, "^B2009/2010^CO^X!NOFOND^D42^K46^E4^N11.50^G20100511"));
            result.Fields.Add(new RecordField(692, "^B2007/2008^CV^D17^X!COM^G20080501"));
            result.Fields.Add(new RecordField(692, "^B2007/2008^CV^X!NOFOND^D42^S10.50^G20080501"));
            result.Fields.Add(new RecordField(692, "^B2010/2011^CO^AЗИ^D25^E4^F6.25^G20101208"));
            result.Fields.Add(new RecordField(692, "^B2010/2011^CO^D17^X!COM^K21^E0^M0^G20101208"));
            result.Fields.Add(new RecordField(692, "^B2010/2011^CO^X!NOFOND^D42^K46^E4^N11.50^G20101208"));
            result.Fields.Add(new RecordField(692, "^B2007/2008^CO^D42^E4^N10.50^G20080107"));
            result.Fields.Add(new RecordField(692, "^B2010/2011^CV^AЗИ^D25^E0^F0^S0^G20110408"));
            result.Fields.Add(new RecordField(692, "^B2010/2011^CV^D17^X!COM^K21^E0^M0^G20110408"));
            result.Fields.Add(new RecordField(692, "^B2010/2011^CV^X!NOFOND^D42^K46^E0^N0^S11.50^G20110408"));
            result.Fields.Add(new RecordField(692, "^B2007/2008^CO^D42^E4^Z10.50^G20080107"));
            result.Fields.Add(new RecordField(692, "^B2011/2012^CO^AЗИ^D25^E4^F6.25^G20120524"));
            result.Fields.Add(new RecordField(692, "^B2011/2012^CO^D17^X!COM^K21^E0^M0^G20120524"));
            result.Fields.Add(new RecordField(692, "^B2011/2012^CO^X!NOFOND^D42^K46^E4^N11.50^G20120524"));
            result.Fields.Add(new RecordField(692, "^B2011/2012^CO^X!NOZO^D46^E4^Z11.50^G20120524"));
            result.Fields.Add(new RecordField(692, "^B2008/2009^CO^D17^X!COM^G20081218"));
            result.Fields.Add(new RecordField(692, "^B2012/2013^CO^AЗИ^D25^E4^F1.00^G20130531"));
            result.Fields.Add(new RecordField(692, "^B2012/2013^CO^X!NOFOND^D42^K46^E4^N1.00^G20130531"));
            result.Fields.Add(new RecordField(692, "^B2012/2013^CO^X!NOZO^D46^E4^Z11.50^G20130531"));
            result.Fields.Add(new RecordField(102, "RU"));
            result.Fields.Add(new RecordField(10, "^a5-7110-0177-9^d300"));
            result.Fields.Add(new RecordField(675, "37"));
            result.Fields.Add(new RecordField(675, "37(470.311)(03)"));
            result.Fields.Add(new RecordField(964, "14"));
            result.Fields.Add(new RecordField(999, "0000002"));
            result.Fields.Add(new RecordField(920, "PAZK"));
            result.Fields.Add(new RecordField(210, "^aМ.^cСП \"Вся Москва\"^d1993"));
            result.Fields.Add(new RecordField(215, "^A240^Cил^D12^YДА^Z3"));
            result.Fields.Add(new RecordField(225, "^u2^aВся Москва"));
            result.Fields.Add(new RecordField(101, "rus"));
            result.Fields.Add(new RecordField(908, "К88"));
            result.Fields.Add(new RecordField(903, "37/К88-602720"));
            result.Fields.Add(new RecordField(690, "^L9.92"));
            result.Fields.Add(new RecordField(700, "^AАкулова^BЗ.М.^PНИИ ВК"));
            result.Fields.Add(new RecordField(900, "^B05^Cg^227^3j02"));
            result.Fields.Add(new RecordField(702, "^4340 ред.^AПавловский^BА.С."));
            result.Fields.Add(new RecordField(702, "^4340 ред.^AПанасенко^BВ.А."));
            result.Fields.Add(new RecordField(702, "^4340 ред.^AПанков^BИ."));
            result.Fields.Add(new RecordField(702, "^4340 ред.^AПетрова^BН.Б."));
            result.Fields.Add(new RecordField(907, "^A20020530^BДСМ"));
            result.Fields.Add(new RecordField(907, "^CПРФ^A20060601^BДСМ"));
            result.Fields.Add(new RecordField(907, "^C^A20060601^BДСМ"));
            result.Fields.Add(new RecordField(907, "^C^A20070109^B"));
            result.Fields.Add(new RecordField(907, "^C^A20020530^B"));
            result.Fields.Add(new RecordField(907, "^C^A20030129^B"));
            result.Fields.Add(new RecordField(907, "^C^A20050524^B"));
            result.Fields.Add(new RecordField(907, "^C^A20050525^B"));
            result.Fields.Add(new RecordField(907, "^C^A20051110^B"));
            result.Fields.Add(new RecordField(907, "^C^A20070207^B"));
            result.Fields.Add(new RecordField(907, "^A20071108^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^A20071226^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^A20080107^B^C"));
            result.Fields.Add(new RecordField(907, "^A20081101^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^A20080501^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^A20081218^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^CКТ^A20090108^B"));
            result.Fields.Add(new RecordField(907, "^A20090830^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^A20090909^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^A20100511^BОЛН^C"));
            result.Fields.Add(new RecordField(907, "^CКТ^A20100527^B1"));
            result.Fields.Add(new RecordField(907, "^A20100908^B1^C"));
            result.Fields.Add(new RecordField(20, "^0 ^! ^aRU^b93-1141"));
            result.Fields.Add(new RecordField(907, "^A20110328^B1^C"));
            result.Fields.Add(new RecordField(907, "^A20101208^B1^C"));
            result.Fields.Add(new RecordField(907, "^A20110408^B1^C"));
            result.Fields.Add(new RecordField(907, "^A20110908^B1^C"));
            result.Fields.Add(new RecordField(951, "^AПример PDF-файла.PDF^TПример внешнего объекта в виде PDF-файла - с постраничным просмотром^N14"));
            result.Fields.Add(new RecordField(907, "^CКТ^A20120522^B1"));
            result.Fields.Add(new RecordField(907, "^A20120524^B1^C"));
            result.Fields.Add(new RecordField(907, "^CДК^A20130516^B1"));
            result.Fields.Add(new RecordField(907, "^CКТ^A20130531^B1"));
            result.Fields.Add(new RecordField(693, "^B2012/2013^CV^AЗИ^D25^E0^F1^S0"));
            result.Fields.Add(new RecordField(693, "^B2012/2013^CV^X!NOFOND^D42^K46^E0^N1.00^S1.00"));
            result.Fields.Add(new RecordField(692, "^B2008/2009^CO^X!NOFOND^D42^E3^N14.00^G20081218"));
            result.Fields.Add(new RecordField(692, "^CV^AЗИ^D25^E0^F1^S0^G20140703"));
            result.Fields.Add(new RecordField(692, "^CV^X!NOFOND^D42^K46^E0^N1.00^S1.00^G20140703"));
            result.Fields.Add(new RecordField(691, "^! 3^IАКТ^DАктинометрия^SОПД^BФЭиОЭП^KУМО^AЗИ^Vспц^Oд/о^C310700^F3^WАКТ/3^GОсн^0ЗИ310700спцд/о-S3"));
            result.Fields.Add(new RecordField(907, "^A20140703^B1^C"));
            result.Fields.Add(new RecordField(701, "^AБабич^BА.М.^U2"));
            result.Fields.Add(new RecordField(200, "^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]"));
            result.Fields.Add(new RecordField(907, "^CКТ^A20141001^B1"));
            result.Fields.Add(new RecordField(910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            result.Fields.Add(new RecordField(910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            result.Fields.Add(new RecordField(910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            result.Fields.Add(new RecordField(910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            result.Fields.Add(new RecordField(910, "^A0^B559^C19990924^DЧЗ^H107256G^=2^U2004/7"));
            result.Fields.Add(new RecordField(910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
            result.Fields.Add(new RecordField(910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));
            result.Fields.Add(new RecordField(905, "^F2^21"));
            result.Fields.Add(new RecordField(941, "^A0^B32^H107206G^DБИНТ^U2004/7^C19930907"));
            result.Fields.Add(new RecordField(941, "^A0^B33^H107216G^DБИНТ^U2004/7^C19930907"));
            result.Fields.Add(new RecordField(941, "^AU^BЗИ-1^DЖГ^S20140604^125^TЗИ^!КДИ^C20071226^01"));
            result.Fields.Add(new RecordField(941, "^AU^BЗИ-1^DЖГ^S20140604^125^TЗИ^!КДИ^C20071226^01"));
            result.Mfn = 1;

            return result;
        }

        [NotNull]
        private static MarcRecord GetRecord2()
        {
            MarcRecord result = new MarcRecord()
                .AddField(new RecordField(1, "RU\\NLR\\bibl\\3415"))
                .AddField(new RecordField(5, "20031126124354.0"))
                .AddField(new RecordField(10,
                    new SubField('a', "5-7443-0043-0"),
                    new SubField('9', "700")))
                .AddField(new RecordField(21,
                    new SubField('a', "RU"),
                    new SubField('9', "78"),
                    new SubField('b', "98-1576")))
                .AddField(new RecordField(21,
                    new SubField('a', "RU"),
                    new SubField('b', "2001-1566п"),
                    new SubField('9', "57п")))
                .AddField(new RecordField(100,
                    new SubField('a', "19980716d1997    u  y0rusy0189    ca")))
                .AddField(new RecordField(101,
                    new SubField('a', "rus")))
                .AddField(new RecordField(102,
                    new SubField('a', "RU")))
                .AddField(new RecordField(105,
                    new SubField('a', "ac  |||||||||")))
                .AddField(new RecordField(200,
                    new SubField('a', "Вып. 13.")))
                .AddField(new RecordField(210,
                    new SubField('d', "1997")))
                .AddField(new RecordField(215,
                    new SubField('a', "80 с."),
                    new SubField('c', "ил., портр.")))
                .AddField(new RecordField(461,
                    new SubField('1', "001RU\\NLR\\bibl\\5996"),
                    new SubField('1', "2001 "),
                    new SubField('a', "Задачи и этюды"),
                    new SubField('v', "Вып. 13")))
                .AddField(new RecordField(801,
                    new SubField('a', "RU"),
                    new SubField('b', "NLR"),
                    new SubField('c', "19980716"),
                    new SubField('g', "PSBO")))
                .AddField(new RecordField(801,
                    new SubField('a', "RU"),
                    new SubField('b', "NLR"),
                    new SubField('c', "19980716")))
                .AddField(new RecordField(899,
                    new SubField('a', "NLR"),
                    new SubField('j', "97-4/119")));

            return result;
        }

        [TestMethod]
        public void Iso2709_ReadRecord_1()
        {
            string fileName = Path.Combine(TestDataPath, "TEST1.ISO");

            using (Stream stream = File.OpenRead(fileName))
            {
                MarcRecord record = Iso2709.ReadRecord(stream, IrbisEncoding.Ansi);
                Assert.IsNotNull(record);
                Assert.AreEqual(16, record.Fields.Count);
                Assert.AreEqual("Вып. 13.", record.FM(200, 'a'));

                record = Iso2709.ReadRecord(stream, IrbisEncoding.Ansi);
                Assert.IsNotNull(record);
                Assert.AreEqual(15, record.Fields.Count);
                Assert.AreEqual("Задачи и этюды", record.FM(200, 'a'));

                int count = 0;
                while (true)
                {
                    record = Iso2709.ReadRecord(stream, IrbisEncoding.Ansi);
                    if (ReferenceEquals(record, null))
                    {
                        break;
                    }

                    count++;
                }

                Assert.AreEqual(79, count);
            }
        }

        [TestMethod]
        public void Iso2709_WriteRecord_1()
        {
            string fileName = Path.GetTempFileName();

            using (Stream stream = File.OpenWrite(fileName))
            {
                MarcRecord record = GetRecord2();
                Iso2709.WriteRecord(record, stream, IrbisEncoding.Ansi);
            }

            FileInfo info = new FileInfo(fileName);
            Assert.AreEqual(562L, info.Length);
        }

        [TestMethod]
        public void Iso2709_WriteRecord_2()
        {
            string fileName = Path.GetTempFileName();

            using (Stream stream = File.OpenWrite(fileName))
            {
                MarcRecord record = GetRecord();
                Iso2709.WriteRecord(record, stream, IrbisEncoding.Utf8);
            }

            FileInfo info = new FileInfo(fileName);
            Assert.AreEqual(4977L, info.Length);
        }

        [TestMethod]
        public void Iso2709_WriteRecord_3()
        {
            string fileName = Path.GetTempFileName();

            using (Stream stream = File.OpenWrite(fileName))
            {
                MarcRecord record = new MarcRecord();
                Iso2709.WriteRecord(record, stream, IrbisEncoding.Ansi);
            }

            FileInfo info = new FileInfo(fileName);
            Assert.AreEqual(26L, info.Length);
        }
    }
}
