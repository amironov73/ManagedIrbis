using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;
using AM.IO;
using AM.Runtime;
using Newtonsoft.Json;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class CharSetTest
    {
        [TestMethod]
        public void TestCharSetConstruction()
        {
            CharSet charSet = new CharSet("abc");

            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void TestCharSetAdd()
        {
            CharSet charSet = new CharSet();

            charSet.Add('a').Add('b').Add('c');
            
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void TestCharSetAddRange()
        {
            CharSet charSet = new CharSet();

            charSet.AddRange('a', 'z');

            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('C'));
        }

        [TestMethod]
        public void TestCharSetUnion()
        {
            CharSet left = new CharSet("abc");
            CharSet right = new CharSet("def");
            CharSet union = left + right;

            Assert.AreEqual("abcdef", union.ToString());
        }

        [TestMethod]
        public void TestCharSetIntersection()
        {
            CharSet left = new CharSet("abcdef");
            CharSet right = new CharSet("defghi");
            CharSet intersection = left*right;

            Assert.AreEqual("def", intersection.ToString());
        }

        [TestMethod]
        public void TestCharSetExclusion()
        {
            CharSet left = new CharSet("abcdef");
            CharSet right = new CharSet("fed");
            CharSet exclusion = left - right;

            Assert.AreEqual("abc", exclusion.ToString());
        }

        [TestMethod]
        public void TestCharSetRemove()
        {
            CharSet charSet = new CharSet("abcdef");
            charSet.Remove('c').Remove('d');

            Assert.AreEqual("abef", charSet.ToString());
        }

        [TestMethod]
        public void TestCharSetDuplicates()
        {
            CharSet charSet = new CharSet("abcabc");

            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void TestCharSetEnumeration()
        {
            CharSet charSet = new CharSet("abcdef");
            StringBuilder builder = new StringBuilder();

            foreach (char c in charSet)
            {
                builder.Append(c);
            }

            Assert.AreEqual("abcdef", builder.ToString());
        }

        [TestMethod]
        public void TestCharSetJsonSerialization()
        {
            CharSet charSet = new CharSet("abcdef");
            string actual = JsonConvert.SerializeObject
                (
                    charSet,
                    Formatting.Indented,
                    new CharSet.CharSetConverter(typeof (CharSet))
                ).Replace("\r", "").Replace("\n", "");
            const string expected = "{  \"charset\": \"abcdef\"}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCharSetCheckText()
        {
            CharSet charSet = new CharSet("abc");

            Assert.IsTrue(charSet.CheckText("aabcc"));
            Assert.IsFalse(charSet.CheckText("abdcc"));
        }

        [TestMethod]
        public void TestCharSetToArray()
        {
            CharSet charSet = new CharSet("abc");
            char[] array = charSet.ToArray();

            Assert.AreEqual(3, array.Length);
            Assert.IsTrue(array.Contains('a'));
            Assert.IsTrue(array.Contains('b'));
            Assert.IsTrue(array.Contains('c'));
        }
    }
}
