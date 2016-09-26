using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search.Infrastructure;

namespace UnitTests.ManagedIrbis.Search.Infrastructure
{
    [TestClass]
    public class SearchQueryParserTest
    {
        private void _TestTokenize
            (
                string text,
                int expected
            )
        {
            SearchQueryParser parser = new SearchQueryParser();
            QAstTokenList tokens = parser.Tokenize(text);
            Assert.AreEqual(expected, tokens.Length);
        }

        [TestMethod]
        public void SearchQueryParser_Tokenize()
        {
            _TestTokenize("", 0);
            _TestTokenize("()", 2);
            _TestTokenize("\"K=&CONTRA$\"/()", 4);
            _TestTokenize("\"RI=30439B4B\"", 1);
            _TestTokenize("\"K=ЭЛЕКТРООБОРУДОВАНИЕ АВТОМОБИЛЕЙ$\"/()", 4);
            _TestTokenize("(\"K=электрооборудовани$\"/()*\"K=автомоб$\"/())", 11);
            _TestTokenize("\"T=МАТЕМАТИЧЕСКИЙ АНАЛИЗ В ЗАДАЧАХ И УПРАЖНЕНИЯХ$\" + \"T=МАТЕМАТИЧЕСКИЙ ПРАКТИКУМ ДЛЯ ВУЗОВ$\"", 3);
            _TestTokenize("(\"K=аргановский@\"/()*\"K=Анатол$\"/())*(\"V=08$\")", 15);
            _TestTokenize("\"K=ЛЕПКА ИГРУШЕК ИЗ ПЛАСТИЛИНА, ГЛИНЫ, СОЛЕНОГО ТЕСТА$\"/(1200,12251,12252,12253,1330,1430,1451,1452,1454,1461,1462,1463,1464,1465,14611,14612,1470,1481,1510,1517,1541,1922,19231,19232,19233,1924,19251,19252,19253)", 61);
            _TestTokenize("\"K=ОЦЕНКА ПРЕДПРИЯТИЙ$\"/()+\"K=ОЦЕНКА ПРЕДПРИЯТИЯ$\"/()", 9);
            _TestTokenize("\"K=Вербальные $\"/() . \"K=средства $\"/() . \"K=коммуникаций $\"/()", 14);
            _TestTokenize("(\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\"/())*(\"V=KN$\")*(\"G=2009$\"+\"G=201$\")*(\"K=МАЛЫЙ БИЗНЕС$\"/()+\"K=СРЕДНИЙ БИЗНЕС$\"/())", 28);
            _TestTokenize("\"TJ=ТЕХНИКА - МОЛОДЕЖИ\"",1);
            _TestTokenize("TJ=АВРОРА",1);
            _TestTokenize("K=ЭЛЕКТРО$(F)K=БЕТОН$",3);
        }

        private void _TestParse
            (
                string text,
                string expected
            )
        {
            SearchQueryParser parser = new SearchQueryParser();
            QAstTokenList tokens = parser.Tokenize(text);
            QAstRoot root = parser.Parse(tokens);
            string actual = root.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SearchQueryParser_Parse()
        {
            _TestParse("", "");
            _TestParse("\"RI=30439B4B\"", "\"RI=30439B4B\"");
            _TestParse("\"K=ЭЛЕКТРООБОРУДОВАНИЕ АВТОМОБИЛЕЙ$\"/()",
                "\"K=ЭЛЕКТРООБОРУДОВАНИЕ АВТОМОБИЛЕЙ$\"");
            _TestParse("(\"K=электрооборудовани$\"/()*\"K=автомоб$\"/())",
                "\"K=электрооборудовани$\" * \"K=автомоб$\"");
            _TestParse("\"K=ЛЕПКА ИГРУШЕК ИЗ ПЛАСТИЛИНА, ГЛИНЫ, СОЛЕНОГО ТЕСТА$\"/(1200,12251,12252,12253,1330,1430,1451,1452,1454,1461,1462,1463,1464,1465,14611,14612,1470,1481,1510,1517,1541,1922,19231,19232,19233,1924,19251,19252,19253)",
                "\"K=ЛЕПКА ИГРУШЕК ИЗ ПЛАСТИЛИНА, ГЛИНЫ, СОЛЕНОГО ТЕСТА$\"/(1200,12251,12252,12253,1330,1430,1451,1452,1454,1461,1462,1463,1464,1465,14611,14612,1470,1481,1510,1517,1541,1922,19231,19232,19233,1924,19251,19252,19253)");
            _TestParse("\"K=Вербальные $\"/() . \"K=средства $\"/() . \"K=коммуникаций $\"/()",
                "\"K=Вербальные $\" . \"K=средства $\" . \"K=коммуникаций $\"");
            _TestParse("\"K=ОЦЕНКА ПРЕДПРИЯТИЙ$\"/()+\"K=ОЦЕНКА ПРЕДПРИЯТИЯ$\"/()",
                "\"K=ОЦЕНКА ПРЕДПРИЯТИЙ$\" + \"K=ОЦЕНКА ПРЕДПРИЯТИЯ$\"");
            //_TestParse("(\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\"/())*(\"V=KN$\")*(\"G=2009$\"+\"G=201$\")*(\"K=МАЛЫЙ БИЗНЕС$\"/()+\"K=СРЕДНИЙ БИЗНЕС$\"/())",
            //    "\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\" * \"V=KN$\" * ( \"G=2009$\" + \"G=201$\" ) * ( \"K=МАЛЫЙ БИЗНЕС$\" + \"K=СРЕДНИЙ БИЗНЕС$\" )");
        }
    }
}
