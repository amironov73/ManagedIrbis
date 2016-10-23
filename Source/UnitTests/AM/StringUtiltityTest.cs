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
        public void StringUtility_CompareNoCase1()
        {
            Assert.IsTrue(StringUtility.CompareNoCase('A', 'a'));
            Assert.IsTrue(StringUtility.CompareNoCase('A', 'A'));
            Assert.IsTrue(StringUtility.CompareNoCase(' ', ' '));
            Assert.IsFalse(StringUtility.CompareNoCase(' ', 'A'));
        }

        [TestMethod]
        public void StringUtility_CompareNoCase2()
        {
            Assert.IsTrue(StringUtility.CompareNoCase(string.Empty, string.Empty));
            Assert.IsTrue(StringUtility.CompareNoCase(" ", " "));
            Assert.IsTrue(StringUtility.CompareNoCase("Hello", "HELLO"));
            Assert.IsFalse(StringUtility.CompareNoCase("Hello", "HELLO2"));
        }

        [TestMethod]
        public void StringUtility_CountSubstrings1()
        {
            Assert.AreEqual(0, StringUtility.CountSubstrings(null, null));
            Assert.AreEqual(0, "".CountSubstrings(null));
            Assert.AreEqual(0, "".CountSubstrings(""));
            Assert.AreEqual(0, "aga".CountSubstrings(""));
            Assert.AreEqual(0, "".CountSubstrings("aga"));
        }

        [TestMethod]
        public void StringUtility_CountSubstrings2()
        {
            Assert.AreEqual(2, "aga".CountSubstrings("a"));
            Assert.AreEqual(1, "aga".CountSubstrings("ag"));
            Assert.AreEqual(1, "aga".CountSubstrings("aga"));
            Assert.AreEqual(0, "aga".CountSubstrings("aga2"));
        }

        [TestMethod]
        public void StringUtility_ConsistOf1()
        {
            Assert.IsFalse(string.Empty.ConsistOf('a'));
            Assert.IsTrue("aaa".ConsistOf('a'));
            Assert.IsFalse("aba".ConsistOf('a'));
        }

        [TestMethod]
        public void StringUtility_ConsistOf2()
        {
            Assert.IsFalse(string.Empty.ConsistOf('a', 'b'));
            Assert.IsTrue("abc".ConsistOf('a', 'b', 'c'));
            Assert.IsFalse("abcd".ConsistOf('a', 'b', 'c'));
        }

        [TestMethod]
        public void StringUtility_ConsistOfDigits1()
        {
            Assert.IsFalse
                (
                    StringUtility.ConsistOfDigits
                    (
                        string.Empty,
                        0,
                        5
                    )
                );
            Assert.IsFalse
                (
                    StringUtility.ConsistOfDigits
                    (
                        "     456",
                        0,
                        5
                    )
                );
            Assert.IsTrue
                (
                    StringUtility.ConsistOfDigits
                    (
                        "12345adfg",
                        0,
                        5
                    )
                );
        }

        [TestMethod]
        public void StringUtility_ConsistOfDigits2()
        {
            Assert.IsFalse
                (
                    StringUtility.ConsistOfDigits
                    (
                        string.Empty
                    )
                );
            Assert.IsFalse
                (
                    StringUtility.ConsistOfDigits
                    (
                        "     456"
                    )
                );
            Assert.IsTrue
                (
                    StringUtility.ConsistOfDigits
                    (
                        "123456"
                    )
                );
        }

        [TestMethod]
        public void StringUtility_EmptyToNull()
        {
            Assert.AreEqual("Hello", "Hello".EmptyToNull());
            Assert.AreEqual(null, "".EmptyToNull());
            Assert.AreEqual(null, ((string)null).EmptyToNull());
        }

        private void _TestGetPositions
            (
                string text,
                char c,
                params int[] expected
            )
        {
            int[] actual = text.GetPositions(c);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void StringUtility_GetPositions()
        {
            _TestGetPositions("", 'a');
            _TestGetPositions("a", 'a', 0);
            _TestGetPositions("aga", 'a', 0, 2);
            _TestGetPositions("aga", 'b');
        }

        [TestMethod]
        public void StringUtility_IfEmpty()
        {
            Assert.AreEqual("Hello", "Hello".IfEmpty("Again"));
            Assert.AreEqual("Again", "".IfEmpty("Again"));
            Assert.AreEqual("Again", "".IfEmpty("", "Again"));
        }

        [TestMethod]
        public void StringUtility_IsBlank()
        {
            Assert.IsTrue(StringUtility.IsBlank(null));
            Assert.IsTrue("".IsBlank());
            Assert.IsTrue(" ".IsBlank());
            Assert.IsTrue("  ".IsBlank());
            Assert.IsFalse("1".IsBlank());
        }

        [TestMethod]
        public void StringUtility_IsDecimal()
        {
            Assert.IsFalse(StringUtility.IsDecimal(string.Empty));
            Assert.IsFalse(StringUtility.IsDecimal(" "));
            Assert.IsTrue(StringUtility.IsDecimal("12"));
            Assert.IsTrue(StringUtility.IsDecimal("12.34"));
            Assert.IsTrue(StringUtility.IsDecimal("-12.34"));
            Assert.IsFalse(StringUtility.IsDecimal("12 340"));
            Assert.IsFalse(StringUtility.IsDecimal("12.34E50"));
        }

        [TestMethod]
        public void StringUtility_IsNumeric()
        {
            Assert.IsFalse(StringUtility.IsNumeric(string.Empty));
            Assert.IsFalse(StringUtility.IsNumeric(" "));
            Assert.IsTrue(StringUtility.IsNumeric("12"));
            Assert.IsTrue(StringUtility.IsNumeric("12.34"));
            Assert.IsTrue(StringUtility.IsNumeric("-12.34"));
            Assert.IsFalse(StringUtility.IsNumeric("12 340"));
            Assert.IsTrue(StringUtility.IsNumeric("12.34E50"));
        }

        [TestMethod]
        public void StringUtility_IsValidIdentifier()
        {
            Assert.IsFalse(string.Empty.IsValidIdentifier());
            Assert.IsTrue("a".IsValidIdentifier());
            Assert.IsTrue("a1".IsValidIdentifier());
            Assert.IsTrue("_a1".IsValidIdentifier());
            Assert.IsFalse("1a".IsValidIdentifier());
            Assert.IsFalse("a1$".IsValidIdentifier());
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
        public void StringUtility_Join()
        {
            const string comma = ", ";
            _TestJoin(string.Empty, comma);
            _TestJoin("1", comma, 1);
            _TestJoin(string.Empty, comma, new object[]{null});
            _TestJoin("1, ", comma, 1, null);
            _TestJoin(", 1", comma, null, 1);
            _TestJoin("1, 2, 3", comma, 1, 2, 3);
            _TestJoin("12", null, 1, 2);
        }

        private void _TestSparse
            (
                string text,
                string expected
            )
        {
            string actual = StringUtility.Sparse(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StringUtility_Sparse()
        {
            _TestSparse(null, null);
            _TestSparse(string.Empty, string.Empty);
            _TestSparse("1", "1");
            _TestSparse(" ", string.Empty);
            _TestSparse("Hello,world!", "Hello, world!");
            _TestSparse("Hello,  world!", "Hello, world!");
            _TestSparse("Hello ,world!", "Hello, world!");
        }

        [TestMethod]
        public void StringUtility_SplitString()
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
        public void StringUtility_ToVisibleString()
        {
            Assert.AreEqual("a", "a".ToVisibleString());
            Assert.AreEqual("(null)", StringUtility.ToVisibleString(null));
            Assert.AreEqual("(empty)", String.Empty.ToVisibleString());
        }

        [TestMethod]
        public void StringUtility_Unquote()
        {
            Assert.AreEqual("", "()".Unquote('(', ')'));
            Assert.AreEqual("()1", "()1".Unquote('(', ')'));
            Assert.AreEqual("text", "\"text\"".Unquote('"'));
        }

        [TestMethod]
        public void StringUtility_Wrap()
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
    }
}
