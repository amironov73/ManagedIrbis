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
        public void CharSet_Construction()
        {
            CharSet charSet = new CharSet("abc");

            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_Add()
        {
            CharSet charSet = new CharSet();

            charSet.Add('a').Add('b').Add('c');
            
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_AddRange()
        {
            CharSet charSet = new CharSet();

            charSet.AddRange('a', 'z');

            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('C'));
        }

        [TestMethod]
        public void CharSet_Union()
        {
            CharSet left = new CharSet("abc");
            CharSet right = new CharSet("def");
            CharSet union = left + right;

            Assert.AreEqual("abcdef", union.ToString());
        }

        [TestMethod]
        public void CharSet_Intersection()
        {
            CharSet left = new CharSet("abcdef");
            CharSet right = new CharSet("defghi");
            CharSet intersection = left*right;

            Assert.AreEqual("def", intersection.ToString());
        }

        [TestMethod]
        public void CharSet_Exclusion()
        {
            CharSet left = new CharSet("abcdef");
            CharSet right = new CharSet("fed");
            CharSet exclusion = left - right;

            Assert.AreEqual("abc", exclusion.ToString());
        }

        [TestMethod]
        public void CharSet_Remove()
        {
            CharSet charSet = new CharSet("abcdef");
            charSet.Remove('c').Remove('d');

            Assert.AreEqual("abef", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Duplicates()
        {
            CharSet charSet = new CharSet("abcabc");

            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Enumeration()
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
        public void CharSet_JsonSerialization()
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
        public void CharSet_CheckText()
        {
            CharSet charSet = new CharSet("abc");

            Assert.IsTrue(charSet.CheckText("aabcc"));
            Assert.IsFalse(charSet.CheckText("abdcc"));
        }

        [TestMethod]
        public void CharSet_ToArray()
        {
            CharSet charSet = new CharSet("abc");
            char[] array = charSet.ToArray();

            Assert.AreEqual(3, array.Length);
            Assert.IsTrue(array.Contains('a'));
            Assert.IsTrue(array.Contains('b'));
            Assert.IsTrue(array.Contains('c'));
        }

        [TestMethod]
        public void CharSet_Equality()
        {
            CharSet first = new CharSet("abc");
            CharSet second = new CharSet("def");
            CharSet third = new CharSet("abc");

            Assert.IsFalse(first.Equals(second));
            Assert.IsTrue(first.Equals(third));
            Assert.IsFalse(second.Equals(third));
        }

        private void _TestSerialization
            (
                CharSet first
            )
        {
            byte[] bytes = first.SaveToMemory();

            CharSet second = bytes
                .RestoreObjectFromMemory<CharSet>();

            Assert.IsTrue
                (
                    first.Equals(second)
                );
        }

        [TestMethod]
        public void CharSet_Serialization()
        {
            CharSet charSet = new CharSet();
            _TestSerialization(charSet);

            charSet.AddRange('a', 'z');
            _TestSerialization(charSet);
        }
    }
}
