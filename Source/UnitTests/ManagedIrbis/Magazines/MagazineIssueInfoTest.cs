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
    public class MagazineIssueInfoTest
    {
        [NotNull]
        private MarcRecord _GetIssueWithoutArticles()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(920, "NJP"));
            result.Fields.Add(RecordField.Parse(907, "^CКТ^A20171027^BМануйловаТС"));
            result.Fields.Add(RecordField.Parse(933, "Ш620"));
            result.Fields.Add(RecordField.Parse(903, "Ш620/1992/12"));
            result.Fields.Add(RecordField.Parse(934, "1992"));
            result.Fields.Add(RecordField.Parse(936, "12"));
            result.Fields.Add(RecordField.Parse(999, "0000000"));
            result.Fields.Add(RecordField.Parse(463, "^wШ620/1992/Подшивка № 10028 июль-дек. (7-12)"));
            result.Fields.Add(RecordField.Parse(907, "^CРЖ^A20171027^Bmiron"));
            result.Fields.Add(RecordField.Parse(910, "^Ap^b1^c?^dФП^pШ620/1992/Подшивка № 10028 июль-дек. (7-12)^iП10028"));
            result.Fields.Add(RecordField.Parse(905, "^D1^F2^S1^21"));

            return result;
        }

        [NotNull]
        private MarcRecord _GetIssueWithArticles()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(920, "NJ"));
            result.Fields.Add(RecordField.Parse(907, "^CРЖ^A20170913^BБудаговскаянв"));
            result.Fields.Add(RecordField.Parse(933, "О174142"));
            result.Fields.Add(RecordField.Parse(903, "О174142/2017/102"));
            result.Fields.Add(RecordField.Parse(934, "2017"));
            result.Fields.Add(RecordField.Parse(936, "102"));
            result.Fields.Add(RecordField.Parse(931, "^C13-19 сентября^!20170919"));
            result.Fields.Add(RecordField.Parse(907, "^CРЖ^A20170914^Bпанюшкинатн"));
            result.Fields.Add(RecordField.Parse(907, "^CРЖ^A20170915^BНаумочкинаММ"));
            result.Fields.Add(RecordField.Parse(951, "^Aregional_2017_102.pdf^TПолный текст^N28^H01^120170915^216636304"));
            result.Fields.Add(RecordField.Parse(6629, "EL"));
            result.Fields.Add(RecordField.Parse(907, "^CFT^A20170915^Bkudelya"));
            result.Fields.Add(RecordField.Parse(907, "^CПК^A20171004^BКурчинскаяЛН"));
            result.Fields.Add(RecordField.Parse(907, "^CПК^A20171006^BКурчинскаяЛН"));
            result.Fields.Add(RecordField.Parse(922, "^FОрлова Е.^?Елена^CМиссионерский стан на ангинской земле^41, 16^0a-фот. цв.^GЕ. Орлова"));
            result.Fields.Add(RecordField.Parse(922, "^FЮдин Ю.^?Юрий^CСергей Левченко: \"Форсайт Байкальского региона\" должен стать ежегодным^42^0a-фот.^GЮ. Юдин"));
            result.Fields.Add(RecordField.Parse(922, "^FМихайлов Ю.^?Юрий^CЗдоровье всего дороже^42^0a-фот.^GЮ. Михайлов"));
            result.Fields.Add(RecordField.Parse(922, "^FИлошваи А.^?Артем^CВеликий сын нашей страны^43^0a-фот.^GА. Илошваи"));
            result.Fields.Add(RecordField.Parse(922, "^FБагаев Ю.^?Юрий^CВыбор сделан^44^0a-фот.^GЮ. Багаев"));
            result.Fields.Add(RecordField.Parse(922, "^FЮдин Ю.^?Юрий^CИнновации для развития Приангарья^44^GЮ. Юдин"));
            result.Fields.Add(RecordField.Parse(922, "^FВиговская А.^?Анна^CГубернаторская оценка^45^0a-фот.^GА. Виговская"));
            result.Fields.Add(RecordField.Parse(922, "^FАндреева О.^?Ольга^CЕвродрова - альтернативное топливо^46^0a-фот.^GО. Андреева"));
            result.Fields.Add(RecordField.Parse(922, "^FШагунова Л.^?Людмила^CСделано у нас^47^0a-фот.^GЛ. Шагунова"));
            result.Fields.Add(RecordField.Parse(922, "^FМамонтова Ю.^?Юлия^CСледствие вели прокуроры^48^0a-фот.^GЮ. Мамонтова"));
            result.Fields.Add(RecordField.Parse(922, "^FШагунова Л.^?Людмила^CСуглан для северян^410^0a-фот.^GЛ. Шагунова"));
            result.Fields.Add(RecordField.Parse(922, "^FОрлова Е.^?Елена^CЗвезды вновь на Байкале^412^0a-фот.^GЕ. Орлова"));
            result.Fields.Add(RecordField.Parse(922, "^FВиговская А.^?Анна^C\"Омулевая бочка\" в подарок к юбилею^412^0a-фот.^GА. Виговская"));
            result.Fields.Add(RecordField.Parse(922, "^FЛаврентьев Ю. В.^?Юрий Васильевич^2Белых Е.^,Екатерина^S570 беседовала^CНаши цели благодарны, наши помыслы чисты!^413^0a-фот.^GЮ. В. Лаврентьев ; беседовала Е. Белых"));
            result.Fields.Add(RecordField.Parse(922, "^FШагунова Л.^?Людмила^CКитай становится ближе^413^0a-фот.^GЛ. Шагунова"));
            result.Fields.Add(RecordField.Parse(922, "^FИванишина Н.^?Наталья^CМастер-универсал^414^0a-фот.^GН. Иванишина"));
            result.Fields.Add(RecordField.Parse(922, "^FЮдин Ю.^?Юрий^CАфинское золото Федора Балтуева^415^0a-фот.^GЮ. Юдин"));
            result.Fields.Add(RecordField.Parse(922, "^FЮдин Ю.^?Юрий^CПамятник водолазам ВОВ открылся в Слюдянке^416^0a-фот. цв.^GЮ. Юдин"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B1^FМОЭ^C20170913^DФ403^E"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B2^FМОЭ^C20170913^DФ304^E"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B3^FМОЭ^C20170913^DФ304^E"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B4^FМОЭ^DФ104^U^C20170914"));
            result.Fields.Add(RecordField.Parse(910, "^A0^B5^DЭБ^G2017^H^E^C20170915"));
            result.Fields.Add(RecordField.Parse(905, "^D1^J1^S1^21"));
            result.Fields.Add(RecordField.Parse(999, "1"));

            return result;
        }

        [TestMethod]
        public void MagazineIssueInfo_Construction_1()
        {
            MagazineIssueInfo issue = new MagazineIssueInfo();
            Assert.IsNull(issue.Articles);
            Assert.IsNull(issue.Description);
            Assert.IsNull(issue.DocumentCode);
            Assert.IsNull(issue.Exemplars);
            Assert.AreEqual(0, issue.LoanCount);
            Assert.AreEqual(0, issue.Mfn);
            Assert.IsNull(issue.MagazineCode);
            Assert.IsNull(issue.Number);
            Assert.IsNull(issue.NumberForSorting);
            Assert.IsNull(issue.Supplement);
            Assert.IsNull(issue.Volume);
            Assert.IsNull(issue.UserData);
        }

        [TestMethod]
        public void MagazienIssueInfo_Parse_1()
        {
            MarcRecord record = _GetIssueWithoutArticles();
            MagazineIssueInfo issue = MagazineIssueInfo.Parse(record);
            Assert.IsNotNull(issue.Articles);
            Assert.AreEqual(0, issue.Articles.Length);
            Assert.IsNotNull(issue.Exemplars);
            Assert.AreEqual(1, issue.Exemplars.Length);
        }

        [TestMethod]
        public void MagazienIssueInfo_Parse_2()
        {
            MarcRecord record = _GetIssueWithArticles();
            MagazineIssueInfo issue = MagazineIssueInfo.Parse(record);
            Assert.IsNotNull(issue.Articles);
            Assert.AreEqual(18, issue.Articles.Length);
            Assert.IsNotNull(issue.Exemplars);
            Assert.AreEqual(5, issue.Exemplars.Length);
        }

        private void _TestSerialization_1
            (
                [NotNull] MagazineIssueInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MagazineIssueInfo second = bytes.RestoreObjectFromMemory<MagazineIssueInfo>();
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.DocumentCode, second.DocumentCode);
            Assert.AreEqual(first.MagazineCode, second.MagazineCode);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Volume, second.Volume);
            Assert.AreEqual(first.Year, second.Year);
            if (!ReferenceEquals(first.Articles, null))
            {
                Assert.IsNotNull(second.Articles);
                Assert.AreEqual(first.Articles.Length, second.Articles.Length);
            }
            if (!ReferenceEquals(first.Exemplars, null))
            {
                Assert.IsNotNull(second.Exemplars);
                Assert.AreEqual(first.Exemplars.Length, second.Exemplars.Length);
            }
        }

        [TestMethod]
        public void MagazineIssueInfo_Serialization_1()
        {
            MagazineIssueInfo issue = new MagazineIssueInfo();
            _TestSerialization_1(issue);

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            _TestSerialization_1(issue);

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            _TestSerialization_1(issue);
        }

        [TestMethod]
        public void MagazineIssueInfo_ToXml_1()
        {
            MagazineIssueInfo issue = new MagazineIssueInfo();
            Assert.AreEqual("<issue />", XmlUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            Assert.AreEqual("<issue document-code=\"Ш620/1992/12\" magazine-code=\"Ш620\" year=\"1992\" number=\"12\" worksheet=\"NJP\"><exemplar status=\"p\" number=\"1\" date=\"?\" place=\"ФП\" binding-index=\"Ш620/1992/Подшивка № 10028 июль-дек. (7-12)\" binding-number=\"П10028\" mfn=\"0\" marked=\"false\" /></issue>", XmlUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            Assert.AreEqual("<issue document-code=\"О174142/2017/102\" magazine-code=\"О174142\" year=\"2017\" number=\"102\" supplement=\"13-19 сентября\" worksheet=\"NJ\"><article><author familyName=\"Орлова\" initials=\"Е.\" fullName=\"Елена\" cantBeInverted=\"false\" /><title title=\"Миссионерский стан на ангинской земле\" first=\"Е. Орлова\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Сергей Левченко: &quot;Форсайт Байкальского региона&quot; должен стать ежегодным\" first=\"Ю. Юдин\" /></article><article><author familyName=\"Михайлов\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Здоровье всего дороже\" first=\"Ю. Михайлов\" /></article><article><author familyName=\"Илошваи\" initials=\"А.\" fullName=\"Артем\" cantBeInverted=\"false\" /><title title=\"Великий сын нашей страны\" first=\"А. Илошваи\" /></article><article><author familyName=\"Багаев\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Выбор сделан\" first=\"Ю. Багаев\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Инновации для развития Приангарья\" first=\"Ю. Юдин\" /></article><article><author familyName=\"Виговская\" initials=\"А.\" fullName=\"Анна\" cantBeInverted=\"false\" /><title title=\"Губернаторская оценка\" first=\"А. Виговская\" /></article><article><author familyName=\"Андреева\" initials=\"О.\" fullName=\"Ольга\" cantBeInverted=\"false\" /><title title=\"Евродрова - альтернативное топливо\" first=\"О. Андреева\" /></article><article><author familyName=\"Шагунова\" initials=\"Л.\" fullName=\"Людмила\" cantBeInverted=\"false\" /><title title=\"Сделано у нас\" first=\"Л. Шагунова\" /></article><article><author familyName=\"Мамонтова\" initials=\"Ю.\" fullName=\"Юлия\" cantBeInverted=\"false\" /><title title=\"Следствие вели прокуроры\" first=\"Ю. Мамонтова\" /></article><article><author familyName=\"Шагунова\" initials=\"Л.\" fullName=\"Людмила\" cantBeInverted=\"false\" /><title title=\"Суглан для северян\" first=\"Л. Шагунова\" /></article><article><author familyName=\"Орлова\" initials=\"Е.\" fullName=\"Елена\" cantBeInverted=\"false\" /><title title=\"Звезды вновь на Байкале\" first=\"Е. Орлова\" /></article><article><author familyName=\"Виговская\" initials=\"А.\" fullName=\"Анна\" cantBeInverted=\"false\" /><title title=\"&quot;Омулевая бочка&quot; в подарок к юбилею\" first=\"А. Виговская\" /></article><article><author familyName=\"Лаврентьев\" initials=\"Ю. В.\" fullName=\"Юрий Васильевич\" cantBeInverted=\"false\" /><author familyName=\"Белых\" initials=\"Е.\" fullName=\"Екатерина\" cantBeInverted=\"false\" /><title title=\"Наши цели благодарны, наши помыслы чисты!\" first=\"Ю. В. Лаврентьев ; беседовала Е. Белых\" /></article><article><author familyName=\"Шагунова\" initials=\"Л.\" fullName=\"Людмила\" cantBeInverted=\"false\" /><title title=\"Китай становится ближе\" first=\"Л. Шагунова\" /></article><article><author familyName=\"Иванишина\" initials=\"Н.\" fullName=\"Наталья\" cantBeInverted=\"false\" /><title title=\"Мастер-универсал\" first=\"Н. Иванишина\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Афинское золото Федора Балтуева\" first=\"Ю. Юдин\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Памятник водолазам ВОВ открылся в Слюдянке\" first=\"Ю. Юдин\" /></article><exemplar status=\"0\" number=\"1\" date=\"20170913\" place=\"Ф403\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"2\" date=\"20170913\" place=\"Ф304\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"3\" date=\"20170913\" place=\"Ф304\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"4\" date=\"20170914\" place=\"Ф104\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"5\" date=\"20170915\" place=\"ЭБ\" mfn=\"0\" marked=\"false\"><other-subfields code=\"G\" value=\"2017\" /></exemplar><loanCount>1</loanCount></issue>", XmlUtility.SerializeShort(issue));
        }

        [TestMethod]
        public void MagazineIssueInfo_ToJson_1()
        {
            MagazineIssueInfo issue = new MagazineIssueInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            Assert.AreEqual("{'document-code':'Ш620/1992/12','magazine-code':'Ш620','year':'1992','number':'12','worksheet':'NJP','exemplars':[{'status':'p','number':'1','date':'?','place':'ФП','binding-index':'Ш620/1992/Подшивка № 10028 июль-дек. (7-12)','binding-number':'П10028','mfn':0,'marked':false}]}", JsonUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            Assert.AreEqual("{'document-code':'О174142/2017/102','magazine-code':'О174142','year':'2017','number':'102','supplement':'13-19 сентября','worksheet':'NJ','articles':[{'authors':[{'familyName':'Орлова','initials':'Е.','fullName':'Елена','cantBeInverted':false}],'title':{'title':'Миссионерский стан на ангинской земле','first':'Е. Орлова'}},{'authors':[{'familyName':'Юдин','initials':'Ю.','fullName':'Юрий','cantBeInverted':false}],'title':{'title':'Сергей Левченко: \"Форсайт Байкальского региона\" должен стать ежегодным','first':'Ю. Юдин'}},{'authors':[{'familyName':'Михайлов','initials':'Ю.','fullName':'Юрий','cantBeInverted':false}],'title':{'title':'Здоровье всего дороже','first':'Ю. Михайлов'}},{'authors':[{'familyName':'Илошваи','initials':'А.','fullName':'Артем','cantBeInverted':false}],'title':{'title':'Великий сын нашей страны','first':'А. Илошваи'}},{'authors':[{'familyName':'Багаев','initials':'Ю.','fullName':'Юрий','cantBeInverted':false}],'title':{'title':'Выбор сделан','first':'Ю. Багаев'}},{'authors':[{'familyName':'Юдин','initials':'Ю.','fullName':'Юрий','cantBeInverted':false}],'title':{'title':'Инновации для развития Приангарья','first':'Ю. Юдин'}},{'authors':[{'familyName':'Виговская','initials':'А.','fullName':'Анна','cantBeInverted':false}],'title':{'title':'Губернаторская оценка','first':'А. Виговская'}},{'authors':[{'familyName':'Андреева','initials':'О.','fullName':'Ольга','cantBeInverted':false}],'title':{'title':'Евродрова - альтернативное топливо','first':'О. Андреева'}},{'authors':[{'familyName':'Шагунова','initials':'Л.','fullName':'Людмила','cantBeInverted':false}],'title':{'title':'Сделано у нас','first':'Л. Шагунова'}},{'authors':[{'familyName':'Мамонтова','initials':'Ю.','fullName':'Юлия','cantBeInverted':false}],'title':{'title':'Следствие вели прокуроры','first':'Ю. Мамонтова'}},{'authors':[{'familyName':'Шагунова','initials':'Л.','fullName':'Людмила','cantBeInverted':false}],'title':{'title':'Суглан для северян','first':'Л. Шагунова'}},{'authors':[{'familyName':'Орлова','initials':'Е.','fullName':'Елена','cantBeInverted':false}],'title':{'title':'Звезды вновь на Байкале','first':'Е. Орлова'}},{'authors':[{'familyName':'Виговская','initials':'А.','fullName':'Анна','cantBeInverted':false}],'title':{'title':'\"Омулевая бочка\" в подарок к юбилею','first':'А. Виговская'}},{'authors':[{'familyName':'Лаврентьев','initials':'Ю. В.','fullName':'Юрий Васильевич','cantBeInverted':false},{'familyName':'Белых','initials':'Е.','fullName':'Екатерина','cantBeInverted':false}],'title':{'title':'Наши цели благодарны, наши помыслы чисты!','first':'Ю. В. Лаврентьев ; беседовала Е. Белых'}},{'authors':[{'familyName':'Шагунова','initials':'Л.','fullName':'Людмила','cantBeInverted':false}],'title':{'title':'Китай становится ближе','first':'Л. Шагунова'}},{'authors':[{'familyName':'Иванишина','initials':'Н.','fullName':'Наталья','cantBeInverted':false}],'title':{'title':'Мастер-универсал','first':'Н. Иванишина'}},{'authors':[{'familyName':'Юдин','initials':'Ю.','fullName':'Юрий','cantBeInverted':false}],'title':{'title':'Афинское золото Федора Балтуева','first':'Ю. Юдин'}},{'authors':[{'familyName':'Юдин','initials':'Ю.','fullName':'Юрий','cantBeInverted':false}],'title':{'title':'Памятник водолазам ВОВ открылся в Слюдянке','first':'Ю. Юдин'}}],'exemplars':[{'status':'0','number':'1','date':'20170913','place':'Ф403','channel':'МОЭ','mfn':0,'marked':false},{'status':'0','number':'2','date':'20170913','place':'Ф304','channel':'МОЭ','mfn':0,'marked':false},{'status':'0','number':'3','date':'20170913','place':'Ф304','channel':'МОЭ','mfn':0,'marked':false},{'status':'0','number':'4','date':'20170914','place':'Ф104','channel':'МОЭ','mfn':0,'marked':false},{'status':'0','number':'5','date':'20170915','place':'ЭБ','other-subfields':[{'code':'G','value':'2017'}],'mfn':0,'marked':false}],'loanCount':1}", JsonUtility.SerializeShort(issue));
        }

        [TestMethod]
        public void MagazineIssueInfo_Verify_1()
        {
            MagazineIssueInfo issue = new MagazineIssueInfo();
            Assert.IsFalse(issue.Verify(false));

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            Assert.IsTrue(issue.Verify(false));

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            Assert.IsTrue(issue.Verify(false));
        }
    }
}
