using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Magazines;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Magazines
{
    [TestClass]
    public class MagazineArticleInfoTest
    {
        [NotNull]
        private MarcRecord _GetAspRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(101, "rus"));
            result.Fields.Add(RecordField.Parse(102, "RU"));
            result.Fields.Add(RecordField.Parse(900, "^Ta^B12"));
            result.Fields.Add(RecordField.Parse(920, "ASP"));
            result.Fields.Add(RecordField.Parse(463, "^CУсольская городская газета^J2017^S1, 4^0a-фот.^H№ 42 (19 окт.)^WУ761/2017/42"));
            result.Fields.Add(RecordField.Parse(999, "0000000"));
            result.Fields.Add(RecordField.Parse(700, "^AКоролева^GОльга^BО."));
            result.Fields.Add(RecordField.Parse(200, "^AЧетыре новых руководителя в администрации города^FО. Королева"));
            result.Fields.Add(RecordField.Parse(907, "^CПК^A20171123^BЧерноваМА"));
            result.Fields.Add(RecordField.Parse(629, "^BМестное изд. с краеведческим материалом^C114"));
            result.Fields.Add(RecordField.Parse(690, "^L02.03"));
            result.Fields.Add(RecordField.Parse(661, "^A2017^B2017"));
            result.Fields.Add(RecordField.Parse(621, "67.401.011.2"));
            result.Fields.Add(RecordField.Parse(621, "66.3(2Рос),124"));
            result.Fields.Add(RecordField.Parse(621, "65.9(2Рос-4Ирк)"));
            result.Fields.Add(RecordField.Parse(606, "^3IRPL-011689-739926^AУсолье-Сибирское, город (Иркутская область)^BСоциально-экономическое развитие"));
            result.Fields.Add(RecordField.Parse(606, "^3IRPL-0000020631-790781^AМестное самоуправление^GУсолье-Сибирское, город (Иркутская область)"));
            result.Fields.Add(RecordField.Parse(908, "К 68"));
            result.Fields.Add(RecordField.Parse(903, "67.401.011.2/К 68-765137866"));
            result.Fields.Add(RecordField.Parse(905, "^22^D3"));

            return result;
        }

        [NotNull]
        private MarcRecord _GetIssueRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(920, "NJ"));
            result.Fields.Add(RecordField.Parse(907, "^CРЖ^A20171027^BНаумочкинаММ"));
            result.Fields.Add(RecordField.Parse(933, "У761"));
            result.Fields.Add(RecordField.Parse(903, "У761/2017/42"));
            result.Fields.Add(RecordField.Parse(934, "2017"));
            result.Fields.Add(RecordField.Parse(936, "42"));
            result.Fields.Add(RecordField.Parse(931, "^C19 октября^!20171019"));
            result.Fields.Add(RecordField.Parse(999, "0000000"));
            result.Fields.Add(RecordField.Parse(951, "^Ausol'skaja_gorodskaja_gazeta_2017_042.pdf^TПолный текст^N28^H01^120171030^229601060"));
            result.Fields.Add(RecordField.Parse(6629, "EL"));
            result.Fields.Add(RecordField.Parse(907, "^CFT^A20171030^Bkudelya"));
            result.Fields.Add(RecordField.Parse(907, "^CРФИД^A20171031^BБудаговскаяНВ"));
            result.Fields.Add(RecordField.Parse(907, "^CПК^A20171123^BЧерноваМА"));
            result.Fields.Add(RecordField.Parse(922, "^FКоролева О.^?Ольга^CЧетыре новых руководителя в администрации города^41, 4^0a-фот.^GО. Королева"));
            result.Fields.Add(RecordField.Parse(922, "^FДроздовская Г.^?Галина^CПоэтический марафон есенинских строк^48^0a-фот.^GГ. Дроздовская"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B1^DФ403^FМОЭ^G2017^E^C20171031"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B2^DФ304^FМОЭ^G2017^E^C20171031"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B3^DФ304^FМОЭ^G2017^E^C20171031"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B4^C20171027^DЭБ^E"));
            result.Fields.Add(RecordField.Parse(905, "^S1^D1^J1"));

            return result;
        }

        [NotNull]
        private MarcRecord _GetBookRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(10, "^a5-7971-0106-8^d16"));
            result.Fields.Add(RecordField.Parse(101, "rus"));
            result.Fields.Add(RecordField.Parse(102, "RU"));
            result.Fields.Add(RecordField.Parse(606, "^aРусская советская литература^bПроза"));
            result.Fields.Add(RecordField.Parse(621, "84(2=Рус)7"));
            result.Fields.Add(RecordField.Parse(700, "^aРаспутин^bВ. Г.^gВалентин Григорьевич^cрусский писатель^f1937-2015"));
            result.Fields.Add(RecordField.Parse(900, "^ta^b05^c11a"));
            result.Fields.Add(RecordField.Parse(903, "84(2=Рус)7/Р24-207867"));
            result.Fields.Add(RecordField.Parse(908, "Р24"));
            result.Fields.Add(RecordField.Parse(919, "^arus^n0102"));
            result.Fields.Add(RecordField.Parse(920, "PAZK"));
            result.Fields.Add(RecordField.Parse(964, "17.82"));
            result.Fields.Add(RecordField.Parse(210, "^d2001^aИркутск^tИздание ОГУП \"Иркутская областная типография № 1\""));
            result.Fields.Add(RecordField.Parse(2012, "3"));
            result.Fields.Add(RecordField.Parse(2013, "^aЭкземпляры заимствованы^bRETRO^c300856"));
            result.Fields.Add(RecordField.Parse(9951, "0750404.jpg"));
            result.Fields.Add(RecordField.Parse(9951, "0750405.jpg"));
            result.Fields.Add(RecordField.Parse(1119, "677df383-000c-4ae1-9899-a7c9ab48eb12"));
            result.Fields.Add(RecordField.Parse(215, "^a182, [2]^cил"));
            result.Fields.Add(RecordField.Parse(200, "^aС любовью к Родине^eпублицистика, повесть, рассказы^fВалентин Распутин ; [авт. проекта и отв. за вып. Л. В. Бабин ; ред. С. Вихристюк ; худож. С. Богатов ; фот.: А. Бызов, В. Поляков]"));
            result.Fields.Add(RecordField.Parse(702, "^4570 авт. проекта и отв. за вып.^aБабин^bЛ. В."));
            result.Fields.Add(RecordField.Parse(702, "^4340 ред.^aВихристюк^bС."));
            result.Fields.Add(RecordField.Parse(702, "^4440 худож.^aБогатов^bС."));
            result.Fields.Add(RecordField.Parse(702, "^4600 фот.^aБызов^bА."));
            result.Fields.Add(RecordField.Parse(702, "^4600 фот.^aПоляков^bВ."));
            result.Fields.Add(RecordField.Parse(629, "^bНеместное изд. с краеведческим материалом"));
            result.Fields.Add(RecordField.Parse(690, "^l21.02.01"));
            result.Fields.Add(RecordField.Parse(606, "^3IRPL-006935-660479^aРаспутин Валентин Григорьевич (писатель; 15.03.1937-14.03.2015)^bПроизведения"));
            result.Fields.Add(RecordField.Parse(6629, "RASP1"));
            result.Fields.Add(RecordField.Parse(661, "^a2001^b2001"));
            result.Fields.Add(RecordField.Parse(6628, " 1. 1. 2"));
            result.Fields.Add(RecordField.Parse(330, "^gавт. предисл. Валентин Распутин^fРаспутин В. Г.^?Валентин Григорьевич^q080 авт. предисл.^cНа перевале видно лучше^44-6"));
            result.Fields.Add(RecordField.Parse(330, "^gБорис Говорин ; беседовал Валентин Распутин^fГоворин А.^q070 авт.^2Распутин В. Г.^,Валентин Григорьевич^cМестная власть и местный народ^eинтервью  с губернатором Иркут. обл.^48-35"));
            result.Fields.Add(RecordField.Parse(330, "^gБорис Говорин ; беседовал Валентин Распутин^fГоворин Б. А.^?Борис Александрович^q070 авт.^2Распутин В. Г.^,Валентин Григорьевич^cДеньги могут многое, но не все. В России - не все...^eинтервью  с губернатором Иркут. обл.^436-54"));
            result.Fields.Add(RecordField.Parse(330, "^gавт. предисл. Л. Бабин^fБабин Л.^q080 авт. предисл.^cПожар^eповесть^456-112"));
            result.Fields.Add(RecordField.Parse(330, "^cЖенский разговор^eрассказ^4113-134"));
            result.Fields.Add(RecordField.Parse(330, "^cУроки французского^eрассказ^4135-183"));
            result.Fields.Add(RecordField.Parse(905, "^j1^s1^21^d1^f2"));
            result.Fields.Add(RecordField.Parse(999, "0000000"));
            result.Fields.Add(RecordField.Parse(910, "^a5^b1638120^c20011008^dФ403^hE00401004DD345F4^s20160516^!Ф403"));
            result.Fields.Add(RecordField.Parse(910, "^a0^b1638121^c20011008^dФ602^hE00401004DD0DF5B^8ДА"));

            return result;
        }

        [TestMethod]
        public void MagazineArticleInfo_Construction_1()
        {
            MagazineArticleInfo article = new MagazineArticleInfo();
            Assert.IsNull(article.Authors);
            Assert.IsNull(article.Title);
            Assert.IsNull(article.Sources);
        }

        [TestMethod]
        public void MagazineArticleInfo_ParseAsp_1()
        {
            MarcRecord record = _GetAspRecord();
            MagazineArticleInfo article = MagazineArticleInfo.ParseAsp(record);
            Assert.IsNotNull(article.Authors);
            Assert.AreEqual(1, article.Authors.Length);
            Assert.IsNotNull(article.Sources);
            Assert.AreEqual(1, article.Sources.Length);
            Assert.AreEqual("2017", article.Sources[0].Year);
            Assert.AreEqual("Усольская городская газета", article.Sources[0].Title);
            Assert.IsNotNull(article.Title);
            Assert.AreEqual("Четыре новых руководителя в администрации города", article.Title.Title);
        }

        [TestMethod]
        public void MagazineArticleInfo_ParseIssue_1()
        {
            MarcRecord record = _GetIssueRecord();
            MagazineArticleInfo[] articles = MagazineArticleInfo.ParseIssue(record);
            Assert.AreEqual(2, articles.Length);
        }

        [TestMethod]
        public void MagazineArticleInfo_ParseBook_1()
        {
            MarcRecord record = _GetBookRecord();
            MagazineArticleInfo[] articles = MagazineArticleInfo.ParseBook(record);
            Assert.AreEqual(6, articles.Length);
        }

        private void _TestSerialization
            (
                [NotNull] MagazineArticleInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MagazineArticleInfo second = bytes.RestoreObjectFromMemory<MagazineArticleInfo>();
            if (!ReferenceEquals(first.Authors, null))
            {
                Assert.IsNotNull(second.Authors);
                Assert.AreEqual(first.Authors.Length, second.Authors.Length);
            }
            if (!ReferenceEquals(first.Sources, null))
            {
                Assert.IsNotNull(second.Sources);
                Assert.AreEqual(first.Sources.Length, second.Sources.Length);
            }
            if (!ReferenceEquals(first.Title, null))
            {
                Assert.IsNotNull(second.Title);
                Assert.AreEqual(first.Title.Title, second.Title.Title);
            }
        }

        [TestMethod]
        public void MagazineArticleInfo_Serialization_1()
        {
            MagazineArticleInfo article = new MagazineArticleInfo();
            _TestSerialization(article);

            article = MagazineArticleInfo.ParseAsp(_GetAspRecord());
            _TestSerialization(article);
        }

        [TestMethod]
        public void MagazineArticleInfo_ToXml_1()
        {
            MagazineArticleInfo article = new MagazineArticleInfo();
            Assert.AreEqual("<article />", XmlUtility.SerializeShort(article));

            article = MagazineArticleInfo.ParseAsp(_GetAspRecord());
            Assert.AreEqual("<article><author familyName=\"Королева\" initials=\"О.\" fullName=\"Ольга\" cantBeInverted=\"false\" /><title title=\"Четыре новых руководителя в администрации города\" first=\"О. Королева\" /><source><title>Усольская городская газета</title><year>2017</year><position>1, 4</position><illustrations>a-фот.</illustrations><secondLevelNumber>№ 42 (19 окт.)</secondLevelNumber><index>У761/2017/42</index></source></article>", XmlUtility.SerializeShort(article));
        }

        [TestMethod]
        public void MagazineArticleInfo_ToJson_1()
        {
            MagazineArticleInfo article = new MagazineArticleInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(article));

            article = MagazineArticleInfo.ParseAsp(_GetAspRecord());
            Assert.AreEqual("{'authors':[{'familyName':'Королева','initials':'О.','fullName':'Ольга','cantBeInverted':false}],'title':{'title':'Четыре новых руководителя в администрации города','first':'О. Королева'},'sources':[{'title':'Усольская городская газета','year':'2017','position':'1, 4','illustrations':'a-фот.','secondLevelNumber':'№ 42 (19 окт.)','index':'У761/2017/42'}]}", JsonUtility.SerializeShort(article));
        }

        [TestMethod]
        public void MagazineArticleInfo_Verify_1()
        {
            MagazineArticleInfo article = new MagazineArticleInfo();
            Assert.IsFalse(article.Verify(false));

            article = MagazineArticleInfo.ParseAsp(_GetAspRecord());
            Assert.IsTrue(article.Verify(false));
        }
    }
}
