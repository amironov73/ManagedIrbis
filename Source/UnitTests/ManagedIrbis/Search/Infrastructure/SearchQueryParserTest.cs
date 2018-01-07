using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;
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
            SearchTokenList tokens = SearchQueryLexer.Tokenize(text);
            
            Assert.AreEqual(expected, tokens.Length);
        }

        [TestMethod]
        public void SearchQueryParser_Tokenize_1()
        {
            _TestTokenize("", 0);
            _TestTokenize("  ", 0);
            _TestTokenize("()", 2);
            _TestTokenize(" (  )  ", 2);
            _TestTokenize("#123",1);
            _TestTokenize("\"K=&CONTRA$\"/()", 4);
            _TestTokenize("\"RI=30439B4B\"", 1);
            _TestTokenize("A=Пушкин$ ^ T=Сказк$", 3);
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

        [TestMethod]
        public void SearchQueryParser_Tokenize_2()
        {
            _TestTokenize("K=1$ + \"HELLO\" * \"WORLD\"", 5);
            _TestTokenize("K=1 + \"САМЫЙ \"ПЕРВЫЙ\" СРЕДИ ПЕРВЫХ\"", 3);
            _TestTokenize("\"I=M123/2017/спецвыпуск \"Мой дом\"\"", 1);
        }

        [TestMethod]
        public void SearchQueryParser_Tokenize_3()
        {
            _TestTokenize("<.>K=HELLO$<.>", 1);
            _TestTokenize("<.>K=HELLO$<.> + <.>T=WORLD$<.>", 3);
        }

        [TestMethod]
        public void SearchQueryParser_Tokenize_4()
        {
            _TestTokenize("K=HELLO$ (G) T=WORLD$", 3);
            _TestTokenize("K=HELLO$ (g) T=WORLD$", 3);
            _TestTokenize("K=HELLO$ (f) T=WORLD$", 3);
            _TestTokenize("K=HELLO$ (F) T=WORLD$", 3);
        }

        [TestMethod]
        public void SearchQueryParser_Tokenize_5()
        {
            _TestTokenize("K=HELLO/", 2);
            _TestTokenize("K=HELLO/ ", 1);
            _TestTokenize("K=HELLO$/(600)", 5);
            _TestTokenize("K=HELLO/WORLD", 1);
            _TestTokenize("K=HELLO$/(610) + T=WORLD$/(611)", 11);
            _TestTokenize("K=HELLO/WORLD + T=WORLD/HELLO", 3);
            _TestTokenize("K=HELLO/ + T=WORLD/", 4);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Tokenize_6()
        {
            _TestTokenize("<K=HELLO", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Tokenize_7()
        {
            _TestTokenize("<.>K=HELLO", 0);
        }

        [TestMethod]
        public void SearchQueryParser_Tokenize_8()
        {
            _TestTokenize("/HELLO", 1);
            _TestTokenize("/HELLO/WORLD", 1);
        }

        private void _TestParse
            (
                string text,
                string expected
            )
        {
            SearchTokenList tokens = SearchQueryLexer.Tokenize(text);
            SearchQueryParser parser = new SearchQueryParser(tokens);
            SearchProgram program = parser.Parse();
            string actual = program.ToString();

            Assert.AreEqual(expected, actual);
        }

        private void _TestParse2
            (
                string text
            )
        {
            SearchTokenList tokens = SearchQueryLexer.Tokenize(text);
            SearchQueryParser parser = new SearchQueryParser(tokens);
            SearchProgram program = parser.Parse();
            string actual = program.ToString();
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void SearchQueryParser_Parse_1()
        {
            _TestParse("", "");
            _TestParse("\"RI=30439B4B\"", "\"RI=30439B4B\"");
            _TestParse("\"K=ЭЛЕКТРООБОРУДОВАНИЕ АВТОМОБИЛЕЙ$\"/()",
                "\"K=ЭЛЕКТРООБОРУДОВАНИЕ АВТОМОБИЛЕЙ$\"");
            _TestParse("(\"K=электрооборудовани$\"/()*\"K=автомоб$\"/())",
                "( \"K=электрооборудовани$\" * \"K=автомоб$\" )");
            _TestParse("\"K=ЛЕПКА ИГРУШЕК ИЗ ПЛАСТИЛИНА, ГЛИНЫ, СОЛЕНОГО ТЕСТА$\"/(1200,12251,12252,12253,1330,1430,1451,1452,1454,1461,1462,1463,1464,1465,14611,14612,1470,1481,1510,1517,1541,1922,19231,19232,19233,1924,19251,19252,19253)",
                "\"K=ЛЕПКА ИГРУШЕК ИЗ ПЛАСТИЛИНА, ГЛИНЫ, СОЛЕНОГО ТЕСТА$\"/(1200,12251,12252,12253,1330,1430,1451,1452,1454,1461,1462,1463,1464,1465,14611,14612,1470,1481,1510,1517,1541,1922,19231,19232,19233,1924,19251,19252,19253)");
            _TestParse("\"K=Вербальные $\"/() . \"K=средства $\"/() . \"K=коммуникаций $\"/()",
                "( \"K=Вербальные $\" . \"K=средства $\" . \"K=коммуникаций $\" )");
            _TestParse("\"K=ОЦЕНКА ПРЕДПРИЯТИЙ$\"/()+\"K=ОЦЕНКА ПРЕДПРИЯТИЯ$\"/()",
                "( \"K=ОЦЕНКА ПРЕДПРИЯТИЙ$\" + \"K=ОЦЕНКА ПРЕДПРИЯТИЯ$\" )");
            _TestParse("(\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\"/())*(\"V=KN$\")*(\"G=2009$\"+\"G=201$\")*(\"K=МАЛЫЙ БИЗНЕС$\"/()+\"K=СРЕДНИЙ БИЗНЕС$\"/())",
                "( \"K=ПРЕДПРИНИМАТЕЛЬСТВ$\" * \"V=KN$\" *  ( \"G=2009$\" + \"G=201$\" )  *  ( \"K=МАЛЫЙ БИЗНЕС$\" + \"K=СРЕДНИЙ БИЗНЕС$\" )  )");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_2()
        {
            _TestParse2("\"RI=30439B4");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_3()
        {
            _TestParse2("\"K=ЭЛЕКТРООБОРУДОВАНИЕ АВТОМОБИЛЕЙ$\"/");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_4()
        {
            _TestParse2("(\"K=электрооборудовани$\"/()*");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_5()
        {
            _TestParse2("(\"K=электрооборудовани$\"/()*\"K=автомоб$\"/()");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_6()
        {
            _TestParse2("(\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\"/())*(\"V=KN$\")*(\"G=2009$\"+\"G=201$\")*(\"K=МАЛЫЙ БИЗНЕС$\"/()+\"K=СРЕДНИЙ БИЗНЕС$\"/()");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_7()
        {
            _TestParse2("(\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\"/(*))*(\"V=KN$\")*(\"G=2009$\"+\"G=201$\")*(\"K=МАЛЫЙ БИЗНЕС$\"/()+\"K=СРЕДНИЙ БИЗНЕС$\"/()");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_8()
        {
            _TestParse2("(\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\"/1))*(\"V=KN$\")*(\"G=2009$\"+\"G=201$\")*(\"K=МАЛЫЙ БИЗНЕС$\"/()+\"K=СРЕДНИЙ БИЗНЕС$\"/()");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_9()
        {
            _TestParse2("*");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParser_Parse_10()
        {
            _TestParse2("(K=(");
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void SearchQueryParse_Parse_11()
        {
            _TestParse2("K=A + ");
        }

        [TestMethod]
        public void SearchQueryParse_Parse_12()
        {
            _TestParse2("K=A + #1");
        }

        private void _TestTerms
            (
                string text,
                int expected
            )
        {
            SearchTokenList tokens = SearchQueryLexer.Tokenize(text);
            SearchQueryParser parser = new SearchQueryParser(tokens);
            SearchProgram program = parser.Parse();
            SearchTerm[] terms = SearchQueryUtility.ExtractTerms(program);

            Assert.AreEqual(expected, terms.Length);
        }

        [TestMethod]
        public void SearchQueryParser_ExtractTerms()
        {
            _TestTerms("", 0);
            _TestTerms("\"RI=30439B4B\"", 1);
            _TestTerms("\"K=ЭЛЕКТРООБОРУДОВАНИЕ АВТОМОБИЛЕЙ$\"/()",1);
            _TestTerms("(\"K=электрооборудовани$\"/()*\"K=автомоб$\"/())",2);
            _TestTerms("\"K=ЛЕПКА ИГРУШЕК ИЗ ПЛАСТИЛИНА, ГЛИНЫ, СОЛЕНОГО ТЕСТА$\"/(1200,12251,12252,12253,1330,1430,1451,1452,1454,1461,1462,1463,1464,1465,14611,14612,1470,1481,1510,1517,1541,1922,19231,19232,19233,1924,19251,19252,19253)",1);
            _TestTerms("\"K=Вербальные $\"/() . \"K=средства $\"/() . \"K=коммуникаций $\"/()",3);
            _TestTerms("\"K=ОЦЕНКА ПРЕДПРИЯТИЙ$\"/()+\"K=ОЦЕНКА ПРЕДПРИЯТИЯ$\"/()",2);
            _TestTerms("(\"K=ПРЕДПРИНИМАТЕЛЬСТВ$\"/())*(\"V=KN$\")*(\"G=2009$\"+\"G=201$\")*(\"K=МАЛЫЙ БИЗНЕС$\"/()+\"K=СРЕДНИЙ БИЗНЕС$\"/())",6);
        }
    }
}
