using System;
using System.Linq;
using AM;
using AM.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class NumberTextTest
    {
        [TestMethod]
        public void NumberText_Construction1()
        {
            NumberText number = new NumberText();
            Assert.IsTrue(number.Empty);
        }

        [TestMethod]
        public void NumberText_Construction2()
        {
            NumberText number = new NumberText("hello1");
            Assert.IsFalse(number.Empty);
        }

        [TestMethod]
        public void NumberText_Enumeration1()
        {
            NumberText number = new NumberText();
            string[] array = number.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void NumberText_Enumeration2()
        {
            NumberText number = new NumberText("hello1");
            string[] array = number.ToArray();
            Assert.AreEqual(1, array.Length);
        }

        [TestMethod]
        public void NumberText_Enumeration3()
        {
            NumberText number = new NumberText("hello1goodbye2");
            string[] array = number.ToArray();
            Assert.AreEqual(2, array.Length);
        }

        [TestMethod]
        public void NumberText_Clone()
        {
            NumberText first = "Hello1";
            NumberText second = first.Clone();
            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Comparison1()
        {
            NumberText first = "hello2";
            NumberText second = "hello10";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison2()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison3()
        {
            NumberText first = "20";
            NumberText second = "21";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison4()
        {
            NumberText first = "20";
            NumberText second = "21";
            Assert.IsFalse(second.CompareTo(first) <= 0);
        }

        [TestMethod]
        public void NumberText_Increment1()
        {
            NumberText number = "hello2";
            number.Increment();
            Assert.AreEqual("hello3", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment2()
        {
            NumberText number = "hello2goodbye1";
            number.Increment();
            Assert.AreEqual("hello2goodbye2", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment3()
        {
            NumberText number = "hello2goodbye1";
            number.Increment(0, 1);
            Assert.AreEqual("hello3goodbye1", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment4()
        {
            NumberText number = "hello002goodbye001";
            number.Increment();
            Assert.AreEqual("hello002goodbye002", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment5()
        {
            NumberText number = "hello002goodbye001";
            number.Increment(1, 5);
            Assert.AreEqual("hello002goodbye006", number.ToString());
        }

        [TestMethod]
        public void NumberText_Max1()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            NumberText max = NumberText.Max(first, second);
            Assert.AreEqual("hello010", max.ToString());
        }

        [TestMethod]
        public void NumberText_Min1()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            NumberText min = NumberText.Min(first, second);
            Assert.AreEqual("hello2", min.ToString());
        }

        [TestMethod]
        public void NumberText_EqualityOperator()
        {
            const string text = "hello2";
            NumberText number = text;
            Assert.IsTrue(number == text);
        }

        [TestMethod]
        public void NumberText_TextOnly()
        {
            NumberText number = "Hello1";
            Assert.IsFalse(number.TextOnly);

            number = "Hello";
            Assert.IsTrue(number.TextOnly);

            number = "1";
            Assert.IsFalse(number.TextOnly);
        }

        [TestMethod]
        public void NumberText_ValueOnly()
        {
            NumberText number = "Hello1";
            Assert.IsFalse(number.ValueOnly);

            number = "Hello";
            Assert.IsFalse(number.ValueOnly);

            number = "1";
            Assert.IsTrue(number.ValueOnly);
        }

        [TestMethod]
        public void NumberText_HaveChunk()
        {
            NumberText number = "Hello1";
            Assert.IsTrue(number.HaveChunk(0));
            Assert.IsFalse(number.HaveChunk(1));
            Assert.IsFalse(number.HaveChunk(-1));
        }

        [TestMethod]
        public void NumberText_AppendChunk1()
        {
            NumberText number = "Hello1";
            number.AppendChunk("Goodbye");
            Assert.AreEqual("Hello1Goodbye", number.ToString());
        }

        [TestMethod]
        public void NumberText_AppendChunk2()
        {
            NumberText number = "Hello1";
            number.AppendChunk("Goodbye", 2, 3);
            Assert.AreEqual("Hello1Goodbye002", number.ToString());
        }

        [TestMethod]
        public void NumberText_AppendChunk3()
        {
            NumberText number = "Hello1";
            number.AppendChunk(100);
            Assert.AreEqual("Hello1100", number.ToString());
        }

        [TestMethod]
        public void NumberText_GetDifference()
        {
            NumberText first = "Hello100";
            NumberText second = "Goodbye2";
            int difference = (int) first.GetDifference(second);
            Assert.AreEqual(98, difference);
        }

        [TestMethod]
        public void NumberText_GetLength()
        {
            NumberText number = "Hello100";
            int length = number.GetLength(0);
            Assert.AreEqual(3, length);
        }

        [TestMethod]
        public void NumberText_GetPrefix()
        {
            NumberText number = "Hello2";
            string prefix = number.GetPrefix(0);
            Assert.AreEqual("Hello", prefix);
        }

        [TestMethod]
        public void NumberText_GetValue()
        {
            NumberText number = "Hello001";
            int value = (int) number.GetValue(0);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void NumberText_HavePrefix1()
        {
            NumberText number = "Hello1";
            Assert.IsTrue(number.HavePrefix(0));
            Assert.IsFalse(number.HavePrefix(1));

            number = "Hello";
            Assert.IsTrue(number.HavePrefix(0));
            Assert.IsFalse(number.HavePrefix(1));

            number = "1";
            Assert.IsFalse(number.HavePrefix(0));
            Assert.IsFalse(number.HavePrefix(1));
        }

        [TestMethod]
        public void NumberText_HavePrefix2()
        {
            NumberText number = "1Hello2";
            Assert.IsFalse(number.HavePrefix(0));
            Assert.IsTrue(number.HavePrefix(1));
            Assert.IsFalse(number.HavePrefix(2));

            number = "1Hello";
            Assert.IsFalse(number.HavePrefix(0));
            Assert.IsTrue(number.HavePrefix(1));
            Assert.IsFalse(number.HavePrefix(2));
        }

        [TestMethod]
        public void NumberText_HavePrefix3()
        {
            NumberText number = "1Hello2Goodbye";
            Assert.IsFalse(number.HavePrefix(0));
            Assert.IsTrue(number.HavePrefix(1));
            Assert.IsTrue(number.HavePrefix(2));
            Assert.IsFalse(number.HavePrefix(3));
        }

        [TestMethod]
        public void NumberText_HaveValue1()
        {
            NumberText number = "Hello1";
            Assert.IsTrue(number.HaveValue(0));
            Assert.IsFalse(number.HaveValue(1));

            number = "Hello";
            Assert.IsFalse(number.HaveValue(0));
            Assert.IsFalse(number.HaveValue(1));

            number = "1";
            Assert.IsTrue(number.HaveValue(0));
            Assert.IsFalse(number.HaveValue(1));
        }

        [TestMethod]
        public void NumberText_HaveValue2()
        {
            NumberText number = "1Hello1";
            Assert.IsTrue(number.HaveValue(0));
            Assert.IsTrue(number.HaveValue(1));
            Assert.IsFalse(number.HaveValue(2));

            number = "1Hello";
            Assert.IsTrue(number.HaveValue(0));
            Assert.IsFalse(number.HaveValue(1));
            Assert.IsFalse(number.HaveValue(2));
        }

        [TestMethod]
        public void NumberText_ParseRanges1()
        {
            NumberText[] array = NumberText
                .ParseRanges("h1-h10")
                .ToArray();
            Assert.AreEqual(10, array.Length);
            Assert.AreEqual("h1", array[0].ToString());
            Assert.AreEqual("h2", array[1].ToString());
            Assert.AreEqual("h3", array[2].ToString());
            Assert.AreEqual("h4", array[3].ToString());
            Assert.AreEqual("h5", array[4].ToString());
            Assert.AreEqual("h6", array[5].ToString());
            Assert.AreEqual("h7", array[6].ToString());
            Assert.AreEqual("h8", array[7].ToString());
            Assert.AreEqual("h9", array[8].ToString());
            Assert.AreEqual("h10", array[9].ToString());
        }

        [TestMethod]
        public void NumberText_ParseRanges2()
        {
            NumberText[] array = NumberText
                .ParseRanges(string.Empty)
                .ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void NumberText_ParseRanges3()
        {
            NumberText[] array = NumberText
                .ParseRanges("hello1")
                .ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual("hello1", array[0].ToString());
        }

        [TestMethod]
        public void NumberText_ParseRanges4()
        {
            NumberText[] array = NumberText
                .ParseRanges("hello1;hello2")
                .ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual("hello1", array[0].ToString());
            Assert.AreEqual("hello2", array[1].ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArsMagnaException))]
        public void NumberText_ParesRanges_Exception1()
        {
            NumberText[] array = NumberText
                .ParseRanges("-hello2")
                .ToArray();
        }

        [TestMethod]
        public void NumberText_RemoveChunk1()
        {
            NumberText number = "hello1goodbye2";
            number.RemoveChunk(1);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_RemoveChunk2()
        {
            NumberText number = "hello1";
            number.RemoveChunk(1);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetLength1()
        {
            NumberText number = "hello1goodbye2";
            number.SetLength(0, 3);
            Assert.AreEqual("hello001goodbye2", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetLength2()
        {
            NumberText number = "hello1";
            number.SetLength(1, 3);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetPrefix1()
        {
            NumberText number = "hello1goodbye2";
            number.SetPrefix(1, "sayonara");
            Assert.AreEqual("hello1sayonara2", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetPrefix2()
        {
            NumberText number = "hello1";
            number.SetPrefix(1, "sayonara");
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetValue1()
        {
            NumberText number = "hello1goodbye2";
            number.SetValue(1, 100);
            Assert.AreEqual("hello1goodbye100", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetValue2()
        {
            NumberText number = "hello1";
            number.SetValue(1, 100);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_Equals1()
        {
            NumberText first = "hello1";
            NumberText second = "hello1";
            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Equals2()
        {
            NumberText first = "hello1";
            NumberText second = "hello2";
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Equals3()
        {
            NumberText first = "hello1";
            NumberText second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Equals4()
        {
            NumberText first = "hello1";
            NumberText second = "hello1";
            Assert.IsTrue(first.Equals((object) second));
        }

        [TestMethod]
        public void NumberText_Equals5()
        {
            NumberText first = "hello1";
            NumberText second = "hello2";
            Assert.IsFalse(first.Equals((object) second));
        }

        [TestMethod]
        public void NumberText_Equals6()
        {
            NumberText first = "hello1";
            object second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Parse1()
        {
            NumberText number = new NumberText();
            number.Parse("hello1");
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_Parse2()
        {
            NumberText number = new NumberText();
            number.Parse(null);
            Assert.IsTrue(number.Empty);
        }

        [TestMethod]
        public void NumberText_Verify1()
        {
            NumberText number = "hello1";
            Assert.IsTrue(number.Verify(false));
        }

        [TestMethod]
        public void NumberText_Verify2()
        {
            NumberText number = "hello1";
            number.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ArsMagnaException))]
        public void NumberText_Verify_Exception1()
        {
            NumberText number = new NumberText();
            number.AppendChunk(1);
            number.AppendChunk(2);
            number.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ArsMagnaException))]
        public void NumberText_Verify_Exception2()
        {
            NumberText number = new NumberText();
            number.AppendChunk("hello");
            number.AppendChunk("goodbye");
            number.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ArsMagnaException))]
        public void NumberText_Verify_Exception3()
        {
            NumberText number = "hello";
            number.SetPrefix(0, null);
            number.Verify();
        }

        private void _TestSerialization
        (
            NumberText first
        )
        {
            byte[] bytes = first.SaveToMemory();

            NumberText second = bytes
                .RestoreObjectFromMemory<NumberText>();

            Assert.AreEqual(first, second);
        }

        [TestMethod]
        public void NumberText_HandmadeSerialization()
        {
            NumberText number = new NumberText();
            _TestSerialization(number);

            number = "hello1";
            _TestSerialization(number);

            number = "1hello2";
            _TestSerialization(number);

            number = "hello1hello2";
            _TestSerialization(number);
        }

        [TestMethod]
        public void NumberText_Compare1()
        {
            Assert.IsTrue(NumberText.Compare("hello10", "hello2") > 0);
            Assert.IsTrue(NumberText.Compare("hello1", "hello2") < 0);
        }

        [TestMethod]
        public void NumberText_Compare2()
        {
            Assert.IsTrue(NumberText.Compare("hello10", null) > 0);
            Assert.IsTrue(NumberText.Compare(null, "hello2") < 0);
        }

        [TestMethod]
        public void NumberText_Compare3()
        {
            Assert.IsTrue(NumberText.Compare(null, null) == 0);
        }
    }
}
