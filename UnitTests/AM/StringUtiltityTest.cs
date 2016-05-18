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
        public void TestStringUtility()
        {
            Assert.AreEqual("a", "a".ToVisibleString());
            Assert.AreEqual("(null)", StringUtility.ToVisibleString(null));
            Assert.AreEqual("(empty)", String.Empty.ToVisibleString());
        }

        [TestMethod]
        public void TestIsValidIdentifier()
        {
            Assert.IsFalse(string.Empty.IsValidIdentifier());
            Assert.IsTrue("a".IsValidIdentifier());
            Assert.IsTrue("a1".IsValidIdentifier());
            Assert.IsTrue("_a1".IsValidIdentifier());
            Assert.IsFalse("1a".IsValidIdentifier());
        }

        [TestMethod]
        public void TestSplitString()
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
        public void TestUnquote()
        {
            Assert.AreEqual("", "()".Unquote('(', ')'));
            Assert.AreEqual("()1", "()1".Unquote('(', ')'));
            Assert.AreEqual("text", "\"text\"".Unquote('"'));
        }
    }
}
