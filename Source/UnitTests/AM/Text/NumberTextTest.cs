using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Runtime;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.AM.Text
{
    [TestClass]
    public class NumberTextTest
    {
        [TestMethod]
        public void NumberText_Construction_1()
        {
            NumberText number = new NumberText();
            Assert.IsTrue(number.Empty);
        }

        [TestMethod]
        public void NumberText_Construction_2()
        {
            NumberText number = new NumberText("hello1");
            Assert.IsFalse(number.Empty);
        }

        [TestMethod]
        public void NumberText_Enumeration_1()
        {
            NumberText number = new NumberText();
            string[] array = number.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void NumberText_Enumeration_2()
        {
            NumberText number = new NumberText("hello1");
            string[] array = number.ToArray();
            Assert.AreEqual(1, array.Length);
        }

        [TestMethod]
        public void NumberText_Enumeration_3()
        {
            NumberText number = new NumberText("hello1goodbye2");
            string[] array = number.ToArray();
            Assert.AreEqual(2, array.Length);
        }

        [TestMethod]
        public void NumberText_Clone_1()
        {
            NumberText first = "Hello1";
            NumberText second = first.Clone();
            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Comparison_1()
        {
            NumberText first = "hello2";
            NumberText second = "hello10";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison_2()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison_3()
        {
            NumberText first = "20";
            NumberText second = "21";
            Assert.IsTrue(first.CompareTo(second) < 0);
        }

        [TestMethod]
        public void NumberText_Comparison_4()
        {
            NumberText first = "20";
            NumberText second = "21";
            Assert.IsFalse(second.CompareTo(first) <= 0);
        }

        [TestMethod]
        public void NumberText_Increment_1()
        {
            NumberText number = "hello2";
            number.Increment();
            Assert.AreEqual("hello3", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment_2()
        {
            NumberText number = "hello2goodbye1";
            number.Increment();
            Assert.AreEqual("hello2goodbye2", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment_3()
        {
            NumberText number = "hello2goodbye1";
            number.Increment(0, 1);
            Assert.AreEqual("hello3goodbye1", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment_4()
        {
            NumberText number = "hello002goodbye001";
            number.Increment();
            Assert.AreEqual("hello002goodbye002", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment_5()
        {
            NumberText number = "hello002goodbye001";
            number.Increment(1, 5);
            Assert.AreEqual("hello002goodbye006", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment_6()
        {
            NumberText number = "hello002goodbye001";
            number.Increment(2);
            Assert.AreEqual("hello002goodbye003", number.ToString());
        }

        [TestMethod]
        public void NumberText_Increment_7()
        {
            NumberText number = "hello002goodbye001";
            number.Increment(1, 2L);
            Assert.AreEqual("hello002goodbye003", number.ToString());
        }

        [TestMethod]
        public void NumberText_Max_1()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            NumberText max = NumberText.Max(first, second);
            Assert.AreEqual("hello010", max.ToString());
        }

        [TestMethod]
        public void NumberText_Min_1()
        {
            NumberText first = "hello2";
            NumberText second = "hello010";
            NumberText min = NumberText.Min(first, second);
            Assert.AreEqual("hello2", min.ToString());
        }

        [TestMethod]
        public void NumberText_EqualityOperator_1()
        {
            NumberText left = "hello2";
            NumberText right = "hello002";
            Assert.IsTrue(left == right);

            right = null;
            Assert.IsFalse(left == right);

            right = left;
            Assert.IsTrue(left == right);

            left = null;
            Assert.IsFalse(left == right);
        }

        [TestMethod]
        public void NumberText_EqualityOperator_2()
        {
            string left = "hello2";
            NumberText right = left;
            Assert.IsTrue(left == right);

            right = null;
            Assert.IsFalse(left == right);

            right = left;
            left = null;
            Assert.IsFalse(left == right);
        }

        [TestMethod]
        public void NumberText_EqualityOperator_3()
        {
            NumberText left = "hello2";
            string right = "hello002";
            Assert.IsTrue(left == right);

            right = null;
            Assert.IsFalse(left == right);

            right = "hello002";
            left = null;
            Assert.IsFalse(left == right);
        }

        [TestMethod]
        public void NumberText_EqualityOperator_4()
        {
            NumberText left = "002";
            int right = 2;
            Assert.IsTrue(left == right);

            left = null;
            Assert.IsFalse(left == right);
        }

        [TestMethod]
        public void NumberText_TextOnly_1()
        {
            NumberText number = "Hello1";
            Assert.IsFalse(number.TextOnly);

            number = "Hello";
            Assert.IsTrue(number.TextOnly);

            number = "1";
            Assert.IsFalse(number.TextOnly);
        }

        [TestMethod]
        public void NumberText_ValueOnly_1()
        {
            NumberText number = "Hello1";
            Assert.IsFalse(number.ValueOnly);

            number = "Hello";
            Assert.IsFalse(number.ValueOnly);

            number = "1";
            Assert.IsTrue(number.ValueOnly);
        }

        [TestMethod]
        public void NumberText_HaveChunk_1()
        {
            NumberText number = "Hello1";
            Assert.IsTrue(number.HaveChunk(0));
            Assert.IsFalse(number.HaveChunk(1));
            Assert.IsFalse(number.HaveChunk(-1));
        }

        [TestMethod]
        public void NumberText_AppendChunk_1()
        {
            NumberText number = "Hello1";
            number.AppendChunk("Goodbye");
            Assert.AreEqual("Hello1Goodbye", number.ToString());
        }

        [TestMethod]
        public void NumberText_AppendChunk_2()
        {
            NumberText number = "Hello1";
            number.AppendChunk("Goodbye", 2, 3);
            Assert.AreEqual("Hello1Goodbye002", number.ToString());
        }

        [TestMethod]
        public void NumberText_AppendChunk_3()
        {
            NumberText number = "Hello1";
            number.AppendChunk(100);
            Assert.AreEqual("Hello1100", number.ToString());
        }

        [TestMethod]
        public void NumberText_GetDifference_1()
        {
            NumberText first = "Hello100";
            NumberText second = "Goodbye2";
            int difference = (int) first.GetDifference(second);
            Assert.AreEqual(98, difference);
        }

        [TestMethod]
        public void NumberText_GetLength_1()
        {
            NumberText number = "Hello100";
            int length = number.GetLength(0);
            Assert.AreEqual(3, length);
        }

        [TestMethod]
        public void NumberText_GetPrefix_1()
        {
            NumberText number = "Hello2";
            string prefix = number.GetPrefix(0);
            Assert.AreEqual("Hello", prefix);
        }

        [TestMethod]
        public void NumberText_GetPrefix_2()
        {
            NumberText number = "Hello2";
            string prefix = number.GetPrefix(10);
            Assert.IsNull(prefix);
        }

        [TestMethod]
        public void NumberText_GetValue_1()
        {
            NumberText number = "Hello001";
            int value = (int) number.GetValue(0);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void NumberText_GetValue_2()
        {
            NumberText number = "Hello001";
            int value = (int) number.GetValue(1000);
            Assert.AreEqual(0, value);
        }

        [TestMethod]
        public void NumberText_HavePrefix_1()
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
        public void NumberText_HavePrefix_2()
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
        public void NumberText_HavePrefix_3()
        {
            NumberText number = "1Hello2Goodbye";
            Assert.IsFalse(number.HavePrefix(0));
            Assert.IsTrue(number.HavePrefix(1));
            Assert.IsTrue(number.HavePrefix(2));
            Assert.IsFalse(number.HavePrefix(3));
        }

        [TestMethod]
        public void NumberText_HaveValue_1()
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
        public void NumberText_HaveValue_2()
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
        public void NumberText_ParseRanges_1()
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
        public void NumberText_ParseRanges_2()
        {
            NumberText[] array = NumberText
                .ParseRanges(string.Empty)
                .ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void NumberText_ParseRanges_3()
        {
            NumberText[] array = NumberText
                .ParseRanges("hello1")
                .ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual("hello1", array[0].ToString());
        }

        [TestMethod]
        public void NumberText_ParseRanges_4()
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
        public void NumberText_ParesRanges_5()
        {
            NumberText
                .ParseRanges("-hello2")
                .ToArray();
        }

        [TestMethod]
        public void NumberText_RemoveChunk_1()
        {
            NumberText number = "hello1goodbye2";
            number.RemoveChunk(1);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_RemoveChunk_2()
        {
            NumberText number = "hello1";
            number.RemoveChunk(1);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetLength_1()
        {
            NumberText number = "hello1goodbye2";
            number.SetLength(0, 3);
            Assert.AreEqual("hello001goodbye2", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetLength_2()
        {
            NumberText number = "hello1";
            number.SetLength(1, 3);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetPrefix_1()
        {
            NumberText number = "hello1goodbye2";
            number.SetPrefix(1, "sayonara");
            Assert.AreEqual("hello1sayonara2", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetPrefix_2()
        {
            NumberText number = "hello1";
            number.SetPrefix(1, "sayonara");
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetValue_1()
        {
            NumberText number = "hello1goodbye2";
            number.SetValue(1, 100);
            Assert.AreEqual("hello1goodbye100", number.ToString());
        }

        [TestMethod]
        public void NumberText_SetValue_2()
        {
            NumberText number = "hello1";
            number.SetValue(1, 100);
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_Equals_1()
        {
            NumberText first = "hello1";
            NumberText second = "hello1";
            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Equals_2()
        {
            NumberText first = "hello1";
            NumberText second = "hello2";
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Equals_3()
        {
            NumberText first = "hello1";
            NumberText second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Equals_4()
        {
            NumberText first = "hello1";
            NumberText second = "hello1";
            Assert.IsTrue(first.Equals((object) second));
        }

        [TestMethod]
        public void NumberText_Equals_5()
        {
            NumberText first = "hello1";
            NumberText second = "hello2";
            Assert.IsFalse(first.Equals((object) second));
        }

        [TestMethod]
        public void NumberText_Equals_6()
        {
            NumberText first = "hello1";
            object second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Equals_7()
        {
            NumberText first = "hello1";
            object second = first;
            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void NumberText_Parse_1()
        {
            NumberText number = new NumberText();
            number.Parse("hello1");
            Assert.AreEqual("hello1", number.ToString());
        }

        [TestMethod]
        public void NumberText_Parse_2()
        {
            NumberText number = new NumberText();
            number.Parse(null);
            Assert.IsTrue(number.Empty);
        }

        [TestMethod]
        public void NumberText_Verify_1()
        {
            NumberText number = "hello1";
            Assert.IsTrue(number.Verify(false));
        }

        [TestMethod]
        public void NumberText_Verify_2()
        {
            NumberText number = "hello1";
            Assert.IsTrue(number.Verify(false));
        }

        private void _TestSerialization
            (
                NumberText first
            )
        {
            byte[] bytes = first.SaveToMemory();
            NumberText second = bytes.RestoreObjectFromMemory<NumberText>();
            Assert.AreEqual(first, second);
        }

        [TestMethod]
        public void NumberText_Serialization_1()
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
        public void NumberText_Compare_1()
        {
            Assert.IsTrue(NumberText.Compare("hello10", "hello2") > 0);
            Assert.IsTrue(NumberText.Compare("hello1", "hello2") < 0);
        }

        [TestMethod]
        public void NumberText_Compare_2()
        {
            Assert.IsTrue(NumberText.Compare("hello10", null) > 0);
            Assert.IsTrue(NumberText.Compare(null, "hello2") < 0);
        }

        [TestMethod]
        public void NumberText_Compare_3()
        {
            Assert.IsTrue(NumberText.Compare(null, null) == 0);
        }

        [TestMethod]
        public void NumberText_CompareTo_1()
        {
            NumberText number = new NumberText();
            Assert.IsTrue(number.CompareTo(1) < 0);
        }

        [TestMethod]
        public void NumberText_CompareTo_2()
        {
            NumberText number = new NumberText("Hello1");
            Assert.IsTrue(number.CompareTo(1) > 0);
        }

        [TestMethod]
        public void NumberText_CompareTo_3()
        {
            NumberText number = new NumberText("111");
            Assert.IsTrue(number.CompareTo(111) == 0);
        }

        [TestMethod]
        public void NumberText_CompareTo_4()
        {
            NumberText number = new NumberText("Hello1");
            Assert.IsTrue(number.CompareTo("Hello01") == 0);
        }

        [TestMethod]
        public void NumberText_CompareTo_5()
        {
            NumberText first = new NumberText("Hello1");
            NumberText second = new NumberText("Hello01");
            Assert.IsTrue(first.CompareTo(second) == 0);
        }

        [TestMethod]
        public void NumberText_Addition_1()
        {
            NumberText number = "Hello001";
            number = number + 2;
            Assert.AreEqual("Hello003", number.ToString());

            number = "Hello001Goodbye002";
            number = number + 2;
            Assert.AreEqual("Hello001Goodbye004", number.ToString());
        }

        [TestMethod]
        public void NumberText_Subtraction_1()
        {
            NumberText first = "Hello100";
            NumberText second = "Goodbye2";
            long difference = first - second;
            Assert.AreEqual(98L, difference);
        }

        [TestMethod]
        public void NumberText_NotEqual_1()
        {
            NumberText left = "hello1";
            NumberText right = "hello2";
            Assert.IsTrue(left != right);

            right = null;
            Assert.IsTrue(left != right);

            right = left;
            Assert.IsFalse(left != right);

            left = null;
            Assert.IsTrue(left != right);
        }

        [TestMethod]
        public void NumberText_NotEqual_2()
        {
            NumberText left = "hello1";
            string right = "hello2";
            Assert.IsTrue(left != right);

            right = null;
            Assert.IsTrue(left != right);

            right = "hello2";
            left = null;
            Assert.IsTrue(left != right);
        }

        [TestMethod]
        public void NumberText_NotEqual_3()
        {
            NumberText left = "111";
            int right = 112;
            Assert.IsTrue(left != right);

            left = null;
            Assert.IsTrue(left != null);
        }

        [TestMethod]
        public void NumberText_GreaterThan_1()
        {
            NumberText left = "hello112";
            NumberText right = "hello111";
            Assert.IsTrue(left > right);

            left = null;
            Assert.IsFalse(left > right);

            left = "hello112";
            right = null;
            Assert.IsTrue(left > right);
        }

        [TestMethod]
        public void NumberText_GreaterThan_2()
        {
            NumberText left = "hello112";
            string right = "hello111";
            Assert.IsTrue(left > right);

            left = null;
            Assert.IsFalse(left > right);

            left = "hello112";
            right = null;
            Assert.IsTrue(left > right);
        }

        [TestMethod]
        public void NumberText_GreaterThan_3()
        {
            NumberText left = "112";
            int right = 111;
            Assert.IsTrue(left > right);

            left = null;
            Assert.IsFalse(left > right);
        }

        [TestMethod]
        public void NumberText_GreaterThanOrEqual_1()
        {
            NumberText left = "hello112";
            NumberText right = "hello111";
            Assert.IsTrue(left >= right);

            left = null;
            Assert.IsFalse(left >= right);

            left = "hello112";
            right = null;
            Assert.IsTrue(left >= right);
        }

        [TestMethod]
        public void NumberText_LessThan_1()
        {
            NumberText left = "hello110";
            NumberText right = "hello111";
            Assert.IsTrue(left < right);

            left = null;
            Assert.IsTrue(left < right);

            left = "hello110";
            right = null;
            Assert.IsFalse(left < right);
        }

        [TestMethod]
        public void NumberText_LessThan_2()
        {
            NumberText left = "hello110";
            string right = "hello111";
            Assert.IsTrue(left < right);

            left = null;
            Assert.IsTrue(left < right);

            left = "hello110";
            right = null;
            Assert.IsFalse(left < right);
        }

        [TestMethod]
        public void NumberText_LessThan_3()
        {
            NumberText left = "110";
            int right = 111;
            Assert.IsTrue(left < right);

            left = null;
            Assert.IsTrue(left < right);
        }

        [TestMethod]
        public void NumberText_LessThanOrEqual_1()
        {
            NumberText left = "hello110";
            NumberText right = "hello111";
            Assert.IsTrue(left <= right);

            left = null;
            Assert.IsTrue(left <= right);

            left = "hello110";
            right = null;
            Assert.IsFalse(left <= right);
        }

        [TestMethod]
        public void NumberText_Sort_1()
        {
            List<NumberText> nonSorted = new List<NumberText>
            {
                "hello4", "hello0001", "hello002", "hello03"
            };

            NumberText[] sorted = NumberText.Sort(nonSorted).ToArray();
            Assert.AreEqual(4, sorted.Length);
            Assert.AreEqual("hello0001", sorted[0].ToString());
            Assert.AreEqual("hello002", sorted[1].ToString());
            Assert.AreEqual("hello03", sorted[2].ToString());
            Assert.AreEqual("hello4", sorted[3].ToString());
        }

        [TestMethod]
        public void NumberText_Sort_2()
        {
            string[] nonSorted ={ "hello4", "hello03", "hello002", "hello0001"};
            string[] sorted = NumberText.Sort(nonSorted).ToArray();
            Assert.AreEqual(4, sorted.Length);
            Assert.AreEqual("hello0001", sorted[0]);
            Assert.AreEqual("hello002", sorted[1]);
            Assert.AreEqual("hello03", sorted[2]);
            Assert.AreEqual("hello4", sorted[3]);
        }
    }
}
