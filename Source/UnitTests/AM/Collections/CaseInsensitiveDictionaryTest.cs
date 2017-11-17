using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class CaseInsensitiveDictionaryTest
    {
        [TestMethod]
        public void CaseInsensitiveDictionary_Construction_1()
        {
            CaseInsensitiveDictionary<int> dictionary
                = new CaseInsensitiveDictionary<int>();
            Assert.AreEqual(0, dictionary.Count);
            dictionary.Add("first", 1);
            dictionary.Add("second", 2);
            dictionary.Add("third", 3);
            Assert.AreEqual(3, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("first"));
            Assert.IsTrue(dictionary.ContainsKey("FIRST"));
            Assert.IsTrue(dictionary.ContainsKey("second"));
            Assert.IsTrue(dictionary.ContainsKey("SECOND"));
            Assert.IsTrue(dictionary.ContainsKey("third"));
            Assert.IsTrue(dictionary.ContainsKey("THIRD"));
            Assert.IsFalse(dictionary.ContainsKey("fourth"));
            Assert.IsFalse(dictionary.ContainsKey("FOURTH"));
        }

        [TestMethod]
        public void CaseInsensitiveDictionary_Construction_2()
        {
            CaseInsensitiveDictionary<int> dictionary
                = new CaseInsensitiveDictionary<int>(100);
            Assert.AreEqual(0, dictionary.Count);
            dictionary.Add("first", 1);
            dictionary.Add("second", 2);
            dictionary.Add("third", 3);
            Assert.AreEqual(3, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey("first"));
            Assert.IsTrue(dictionary.ContainsKey("FIRST"));
            Assert.IsTrue(dictionary.ContainsKey("second"));
            Assert.IsTrue(dictionary.ContainsKey("SECOND"));
            Assert.IsTrue(dictionary.ContainsKey("third"));
            Assert.IsTrue(dictionary.ContainsKey("THIRD"));
            Assert.IsFalse(dictionary.ContainsKey("fourth"));
            Assert.IsFalse(dictionary.ContainsKey("FOURTH"));
        }

        [TestMethod]
        public void CaseInsensitiveDictionary_Construction_3()
        {
            CaseInsensitiveDictionary<int> first
                = new CaseInsensitiveDictionary<int>
                {
                    ["first"] = 1,
                    ["second"] = 2,
                    ["third"] = 3,
                };
            CaseInsensitiveDictionary<int> second
                = new CaseInsensitiveDictionary<int>(first);
            Assert.AreEqual(first.Count, second.Count);
            Assert.IsTrue(second.ContainsKey("first"));
            Assert.IsTrue(second.ContainsKey("FIRST"));
            Assert.IsTrue(second.ContainsKey("second"));
            Assert.IsTrue(second.ContainsKey("SECOND"));
            Assert.IsTrue(second.ContainsKey("third"));
            Assert.IsTrue(second.ContainsKey("THIRD"));
            Assert.IsFalse(second.ContainsKey("fourth"));
            Assert.IsFalse(second.ContainsKey("FOURTH"));
        }
    }
}

