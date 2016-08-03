using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class StringUtilityTest
    {
        [TestMethod]
        public void TestStringUtility_ToVisibleString()
        {
            Assert.AreEqual("a", "a".ToVisibleString());
            Assert.AreEqual("(null)", StringUtility.ToVisibleString(null));
            Assert.AreEqual("(empty)", String.Empty.ToVisibleString());
        }

        [TestMethod]
        public void TestStringUtility_IsValidIdentifier()
        {
            Assert.IsFalse(string.Empty.IsValidIdentifier());
            Assert.IsTrue("a".IsValidIdentifier());
            Assert.IsTrue("a1".IsValidIdentifier());
            Assert.IsTrue("_a1".IsValidIdentifier());
            Assert.IsFalse("1a".IsValidIdentifier());
        }

        [TestMethod]
        public void TestStringUtility_SplitString()
        {
            Func<char, bool> func = c => c == '!';
            string[] result = "!!aaa!bbb!!c".SplitString(func).ToArray();
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("aaa", result[0]);
            Assert.AreEqual("bbb", result[1]);
            Assert.AreEqual("c", result[2]);

            result = string.Empty.SplitString(func).ToArray();
            Assert.AreEqual(0, result.Length);

            result = string.Empty.SplitString(func).ToArray();
            Assert.AreEqual(0, result.Length);

            result = "!".SplitString(func).ToArray();
            Assert.AreEqual(0, result.Length);

            result = "a".SplitString(func).ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("a", result[0]);

            result = "!!!".SplitString(func).ToArray();
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void TestStringUtility_Unquote()
        {
            Assert.AreEqual("", "()".Unquote('(', ')'));
            Assert.AreEqual("()1", "()1".Unquote('(', ')'));
            Assert.AreEqual("text", "\"text\"".Unquote('"'));
        }

        [TestMethod]
        public void TestStringUtility_ConsistOf()
        {
            Assert.AreEqual(true, "aaa".ConsistOf('a'));
            Assert.AreEqual(false, "aba".ConsistOf('a'));
            Assert.AreEqual(true, "abc".ConsistOf('a', 'b', 'c'));
            Assert.AreEqual(false, "abcd".ConsistOf('a', 'b', 'c'));
        }

        [TestMethod]
        public void TestStringUtility_Wrap()
        {
            const string nullString = null;

            Assert.AreEqual
                (
                    nullString,
                    nullString.Wrap("(", ")")
                );
            Assert.AreEqual
                (
                    "()",
                    string.Empty.Wrap("(", ")")
                );
            Assert.AreEqual
                (
                    string.Empty,
                    string.Empty.Wrap(nullString, nullString)
                );
            Assert.AreEqual
                (
                    "[ArsMagna]",
                    "ArsMagna".Wrap("[", "]")
                );
        }

        private void _TestJoin
            (
                string expected,
                string separator,
                params object[] objects
            )
        {
            string actual = StringUtility.Join
                (
                    separator,
                    objects
                );

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestStringUtility_Join()
        {
            const string comma = ", ";
            _TestJoin(string.Empty, comma);
            _TestJoin("1", comma, 1);
            _TestJoin(string.Empty, comma, new object[]{null});
            _TestJoin("1, ", comma, 1, null);
            _TestJoin(", 1", comma, null, 1);
            _TestJoin("1, 2, 3", comma, 1, 2, 3);
        }
    }
}
