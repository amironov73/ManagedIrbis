using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;
using static ManagedIrbis.Search.Query;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class QueryTest
    {
        [TestMethod]
        public void Query_Complex_1()
        {
            Assert.AreEqual
                (
                    "(K=бетон + K=железо)",
                    Query.Equals("K=", "бетон", "железо")
                );
            Assert.AreEqual
                (
                    "((T=сказки + T=рассказы) * A=Пушкин)",
                    Query.Equals("T=", "сказки", "рассказы")
                        .And(Query.Equals("A=", "Пушкин"))
                );
        }

        [TestMethod]
        public void Query_All_1()
        {
            Assert.AreEqual("I=$", All());
        }

        [TestMethod]
        public void Query_And_1()
        {
            Assert.AreEqual("(A=B * B=C)", And("A=B", "B=C"));
            Assert.AreEqual("(\"A=(B)\" * B=C)", And("A=(B)", "B=C"));
        }

        [TestMethod]
        public void Query_And_2()
        {
            Assert.AreEqual("A=B", And("A=B"));
            Assert.AreEqual("\"A=(B)\"", And("A=(B)"));
            Assert.AreEqual
                (
                    "(A=B * \"A=(C)\" * A=D)",
                    And("A=B", "A=(C)", "A=D")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_And_3()
        {
            And();
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_And_4()
        {
            And(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_And_5()
        {
            And(string.Empty, string.Empty, string.Empty);
        }

        [TestMethod]
        public void Query_Concat_1()
        {
            Assert.AreEqual("A=B", Concat("A", "=", "B"));
            Assert.AreEqual("\"A=(B)\"", Concat("A", "=", "(B)"));
        }

        [TestMethod]
        public void Query_Equals_1()
        {
            Assert.AreEqual("A=B", Query.Equals("A=", "B"));
            Assert.AreEqual("\"A=(B)\"", Query.Equals("A=", "(B)"));
        }

        [TestMethod]
        public void Query_Equals_2()
        {
            Assert.AreEqual("(A=B + A=C$)", Query.Equals("A=", "B", "C$"));
            Assert.AreEqual("(\"A=(B)\" + A=C$)", Query.Equals("A=", "(B)", "C$"));
        }

        [TestMethod]
        public void Query_Equals_3()
        {
            Assert.AreEqual
                (
                    "(A=B + A=C + A=D$)",
                    Query.Equals("A=", "B", "C", "D$")
                );
            Assert.AreEqual
                (
                    "(A=B + \"A=(C)\" + A=D$)",
                    Query.Equals("A=", "B", "(C)", "D$")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_Equals_4()
        {
            Query.Equals("A=", null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_Equals_5()
        {
            string[] values = { string.Empty };
            Query.Equals("A=", values);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_Equals_6()
        {
            Query.Equals("A=", string.Empty, string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_Equals_7()
        {
            Query.Equals("A=");
        }

        [TestMethod]
        public void Query_Equals_8()
        {
            string[] values = { "B" };
            Assert.AreEqual("A=B", Query.Equals("A=", values));
        }

        [TestMethod]
        public void Query_NeedWrap_1()
        {
            Assert.IsTrue(NeedWrap(null));
            Assert.IsTrue(NeedWrap(string.Empty));
            Assert.IsTrue(NeedWrap(" "));
            Assert.IsFalse(NeedWrap("Hello"));
            Assert.IsFalse(NeedWrap("\"Hello, world\""));
        }

        [TestMethod]
        public void Query_Not_1()
        {
            Assert.AreEqual("(A=B ^ B=C)", "A=B".Not("B=C"));
            Assert.AreEqual("(\"A=(B)\" ^ B=C)", "A=(B)".Not("B=C"));
        }

        [TestMethod]
        public void Query_Or_1()
        {
            Assert.AreEqual("(A=B + B=C)", Or("A=B", "B=C"));
            Assert.AreEqual("(\"A=(B)\" + B=C)", Or("A=(B)", "B=C"));
        }

        [TestMethod]
        public void Query_Or_2()
        {
            Assert.AreEqual("A=B", Or("A=B"));
            Assert.AreEqual("\"A=(B)\"", Or("A=(B)"));
            Assert.AreEqual
                (
                    "(A=B + \"A=(C)\" + A=D)",
                    Or("A=B", "A=(C)", "A=D")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_Or_3()
        {
            Or();
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_Or_4()
        {
            Or(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_Or_5()
        {
            Or(string.Empty, string.Empty, string.Empty);
        }

        [TestMethod]
        public void Query_SameField_1()
        {
            Assert.AreEqual("(A=B (G) B=C)", "A=B".SameField("B=C"));
            Assert.AreEqual("(\"A=(B)\" (G) B=C)", "A=(B)".SameField("B=C"));
        }

        [TestMethod]
        public void Query_SameField_2()
        {
            Assert.AreEqual("A=B", SameField("A=B"));
            Assert.AreEqual("\"A=(B)\"", SameField("A=(B)"));
            Assert.AreEqual
                (
                    "(A=B (G) \"A=(C)\" (G) A=D)",
                    SameField("A=B", "A=(C)", "A=D")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_SameField_3()
        {
            SameField();
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_SameField_4()
        {
            SameField(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_SameField_5()
        {
            SameField(string.Empty, string.Empty, string.Empty);
        }

        [TestMethod]
        public void Query_SameRepeat_1()
        {
            Assert.AreEqual("(A=B (F) B=C)", "A=B".SameRepeat("B=C"));
            Assert.AreEqual("(\"A=(B)\" (F) B=C)", "A=(B)".SameRepeat("B=C"));
        }

        [TestMethod]
        public void Query_SameRepeat_2()
        {
            Assert.AreEqual("A=B", SameRepeat("A=B"));
            Assert.AreEqual("\"A=(B)\"", SameRepeat("A=(B)"));
            Assert.AreEqual
                (
                    "(A=B (F) \"A=(C)\" (F) A=D)",
                    SameRepeat("A=B", "A=(C)", "A=D")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_SameRepeat_3()
        {
            SameRepeat();
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_SameRepeat_4()
        {
            SameRepeat(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(SearchSyntaxException))]
        public void Query_SameRepeat_5()
        {
            SameRepeat(string.Empty, string.Empty, string.Empty);
        }

        [TestMethod]
        public void Query_WrapIfNeeded_1()
        {
            Assert.AreEqual("\"\"", WrapIfNeeded(null));
            Assert.AreEqual("\"\"", WrapIfNeeded(string.Empty));
            Assert.AreEqual("\" \"", WrapIfNeeded(" "));
            Assert.AreEqual("Hello", WrapIfNeeded("Hello"));
            Assert.AreEqual("\"Hello, world\"", WrapIfNeeded("Hello, world"));
            Assert.AreEqual("\"Hello, world\"", WrapIfNeeded("\"Hello, world\""));
        }
    }
}