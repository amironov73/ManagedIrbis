using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnsafeAM.Collections;
using UnsafeAM.Runtime;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable EqualExpressionComparison

namespace UnitTests.UnsafeAM.Collections
{
    [TestClass]
    public class CharSetTest
    {
        [TestMethod]
        public void CharSet_Construction_1()
        {
            CharSet charSet = new CharSet();

            Assert.AreEqual(0, charSet.Count);
            Assert.IsFalse(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_Construction_2()
        {
            CharSet charSet = new CharSet("abc");

            Assert.AreEqual(3, charSet.Count);
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_Construction_3()
        {
            BitArray bitArray = new BitArray(CharSet.DefaultCapacity)
            {
                [97] = true,
                [98] = true,
                [99] = true
            };
            CharSet charSet = new CharSet(bitArray);
            Assert.AreEqual(3, charSet.Count);
            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Construction_4()
        {
            List<char> list = new List<char> { 'a', 'b', 'c' };
            CharSet charSet = new CharSet(list);
            Assert.AreEqual(3, charSet.Count);
            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Construction_5()
        {
            CharSet charSet = new CharSet('a', 'b', 'c');
            Assert.AreEqual(3, charSet.Count);
            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Add_1()
        {
            CharSet charSet = new CharSet();
            charSet.Add('a').Add('b').Add('c');
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        public void CharSet_Add_2()
        {
            CharSet charSet = new CharSet();
            charSet.Add("\\n");
            Assert.IsFalse(charSet.Contains('a'));
            Assert.IsFalse(charSet.Contains('\n'));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CharSet_Add_2a()
        {
            CharSet charSet = new CharSet();
            charSet.Add("a\\");
        }

        [TestMethod]
        public void CharSet_Add_3()
        {
            CharSet charSet = new CharSet();
            charSet.Add("a-c");
            Assert.IsTrue(charSet.Contains('a'));
            Assert.IsTrue(charSet.Contains('b'));
            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('d'));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CharSet_Add_3a()
        {
            CharSet charSet = new CharSet();
            charSet.Add("a-");
        }

        [TestMethod]
        public void CharSet_AddRange_1()
        {
            CharSet charSet = new CharSet();

            charSet.AddRange('a', 'z');

            Assert.IsTrue(charSet.Contains('c'));
            Assert.IsFalse(charSet.Contains('C'));
        }

        [TestMethod]
        public void CharSet_Remove_1()
        {
            CharSet charSet = new CharSet("abcdef");
            charSet.Remove('c').Remove('d');

            Assert.AreEqual("abef", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Remove_2()
        {
            CharSet charSet = new CharSet("abcdef");
            charSet.Remove('c', 'd');

            Assert.AreEqual("abef", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Duplicates_1()
        {
            CharSet charSet = new CharSet("abcabc");

            Assert.AreEqual("abc", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Enumeration_1()
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
        public void CharSet_Enumeration_2()
        {
            CharSet charSet = new CharSet("abc");
            IEnumerator enumerator = ((IEnumerable) charSet).GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('a', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('b', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('c', enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());

            enumerator.Reset();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('a', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('b', enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual('c', enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void CharSet_JsonSerialization_1()
        {
            CharSet charSet = new CharSet("abcdef");
            string actual = JsonConvert.SerializeObject
                (
                    charSet,
                    Formatting.Indented,
                    new CharSet.CharSetConverter(typeof(CharSet))
                ).Replace("\r", "").Replace("\n", "");
            const string expected = "{  \"charset\": \"abcdef\"}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CharSet_CheckText_1()
        {
            CharSet charSet = new CharSet("abc");

            Assert.IsTrue(charSet.CheckText("aabcc"));
            Assert.IsFalse(charSet.CheckText("abdcc"));
            Assert.IsTrue(charSet.CheckText(string.Empty));
            Assert.IsTrue(charSet.CheckText(null));
        }

        [TestMethod]
        public void CharSet_ToArray_1()
        {
            CharSet charSet = new CharSet("abc");
            char[] array = charSet.ToArray();

            Assert.AreEqual(3, array.Length);
            Assert.IsTrue(array.Contains('a'));
            Assert.IsTrue(array.Contains('b'));
            Assert.IsTrue(array.Contains('c'));
        }

        [TestMethod]
        public void CharSet_Equality_1()
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
                [NotNull] CharSet first
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
        public void CharSet_Serialization_1()
        {
            CharSet charSet = new CharSet();
            _TestSerialization(charSet);

            charSet.AddRange('a', 'z');
            _TestSerialization(charSet);
        }

        [TestMethod]
        public void CharSet_Item_1()
        {
            CharSet charSet = new CharSet("abc");
            Assert.IsTrue(charSet['a']);
            Assert.IsTrue(charSet['b']);
            Assert.IsTrue(charSet['c']);
            Assert.IsFalse(charSet['d']);
        }

        [TestMethod]
        public void CharSet_Item_2()
        {
            CharSet charSet = new CharSet("abc")
            {
                ['a'] = false,
                ['d'] = true
            };
            Assert.IsFalse(charSet['a']);
            Assert.IsTrue(charSet['b']);
            Assert.IsTrue(charSet['c']);
            Assert.IsTrue(charSet['d']);
        }

        [TestMethod]
        public void CharSet_Equals_1()
        {
            CharSet first = new CharSet("abc");
            object second = new CharSet("abc");
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(first.Equals(first));
            Assert.IsTrue(second.Equals(first));
            Assert.IsTrue(second.Equals(second));

            second = new CharSet("bcd");
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = null;
            Assert.IsFalse(first.Equals(second));

            second = 123;
            Assert.IsFalse(first.Equals(second));

            second = "abc";
            Assert.IsTrue(first.Equals(second));

            second = "bcd";
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void CharSet_GetHashCode_1()
        {
            CharSet charSet = new CharSet();
            Assert.AreEqual(0, charSet.GetHashCode());

            charSet.Add('a');
            Assert.AreEqual(98, charSet.GetHashCode());

            charSet.Add('b');
            Assert.AreEqual(1765, charSet.GetHashCode());

            charSet.Add('c');
            Assert.AreEqual(30105, charSet.GetHashCode());
        }

        [TestMethod]
        public void CharSet_Addition_1()
        {
            CharSet left = new CharSet("abc");
            CharSet right = new CharSet("bcd");
            CharSet result = left + right;
            Assert.AreEqual("abcd", result.ToString());
        }

        [TestMethod]
        public void CharSet_Addition_2()
        {
            CharSet left = new CharSet("abc");
            CharSet result = left + "bcd";
            Assert.AreEqual("abcd", result.ToString());
        }

        [TestMethod]
        public void CharSet_Addition_3()
        {
            CharSet left = new CharSet("abc");
            CharSet result = left + 'd';
            Assert.AreEqual("abcd", result.ToString());
        }

        [TestMethod]
        public void CharSet_Multiply_1()
        {
            CharSet left = new CharSet("abc");
            CharSet right = new CharSet("bcd");
            CharSet result = left * right;
            Assert.AreEqual("bc", result.ToString());
        }

        [TestMethod]
        public void CharSet_Multiply_2()
        {
            CharSet left = new CharSet("abc");
            CharSet result = left * "bcd";
            Assert.AreEqual("bc", result.ToString());
        }

        [TestMethod]
        public void CharSet_Subtraction_1()
        {
            CharSet left = new CharSet("abc");
            CharSet right = new CharSet("bcd");
            CharSet result = left - right;
            Assert.AreEqual("a", result.ToString());
        }

        [TestMethod]
        public void CharSet_Subtraction_2()
        {
            CharSet left = new CharSet("abc");
            CharSet result = left - "bcd";
            Assert.AreEqual("a", result.ToString());
        }

        [TestMethod]
        public void CharSet_Subtraction_3()
        {
            CharSet left = new CharSet("abc");
            CharSet result = left - 'c';
            Assert.AreEqual("ab", result.ToString());
        }

        [TestMethod]
        public void CharSet_RemoveRange_1()
        {
            CharSet charSet = new CharSet("abcdefg");
            charSet.RemoveRange('c', 'e');
            Assert.AreEqual("abfg", charSet.ToString());
        }

        [TestMethod]
        public void CharSet_Xor_1()
        {
            CharSet left = new CharSet("abc");
            CharSet right = new CharSet("cde");
            CharSet result = left.Xor(right);
            Assert.AreEqual("abde", result.ToString());
        }
    }
}

