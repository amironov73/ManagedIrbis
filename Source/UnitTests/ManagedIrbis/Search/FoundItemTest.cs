using System;
using System.Collections.Generic;
using AM.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class FoundItemTest
    {
        [TestMethod]
        public void TestFoundItem_Construction()
        {
            FoundItem item = new FoundItem();
            Assert.IsNull(item.Text);
            Assert.AreEqual(0, item.Mfn);
        }

        [TestMethod]
        public void TestFoundItem_ToString()
        {
            FoundItem item = new FoundItem();
            Assert.AreEqual
                (
                    "[0] (null)",
                    item.ToString()
                );

            item.Text = string.Empty;
            Assert.AreEqual
                (
                    "[0] (empty)",
                    item.ToString()
                );

            item.Text = "Hello";
            Assert.AreEqual
                (
                    "[0] Hello",
                    item.ToString()
                );
        }

        [TestMethod]
        public void TestFoundItem_ParseLine()
        {
            FoundItem item = FoundItem.ParseLine("123#Hello");
            Assert.AreEqual(123, item.Mfn);
            Assert.AreEqual("Hello", item.Text);
        }

        [TestMethod]
        public void TestFoundItem_ConvertToMfn()
        {
            List<FoundItem> items = new List<FoundItem>()
            {
                new FoundItem {Mfn = 1, Text = "One"},
                new FoundItem {Mfn = 2, Text = "Two"},
                new FoundItem {Mfn = 3, Text = "Three"},
                new FoundItem {Mfn = 4, Text = "Four"},
                new FoundItem {Mfn = 5, Text = "Five"}
            };
            int[] mfns = FoundItem.ConvertToMfn(items);
            Assert.AreEqual(5, mfns.Length);
        }

        [TestMethod]
        public void TestFoundItem_ConvertToText()
        {
            List<FoundItem> items = new List<FoundItem>()
            {
                new FoundItem {Mfn = 1, Text = "One"},
                new FoundItem {Mfn = 2, Text = "Two"},
                new FoundItem {Mfn = 3, Text = "Three"},
                new FoundItem {Mfn = 4, Text = "Four"},
                new FoundItem {Mfn = 5, Text = "Five"}
            };
            string[] lines = FoundItem.ConvertToText(items);
            Assert.AreEqual(5, lines.Length);
        }

        [TestMethod]
        public void TestFoundItem_Verify()
        {
            FoundItem item = new FoundItem();
            Assert.IsFalse(item.Verify(false));

            item.Mfn = 1;
            Assert.IsFalse(item.Verify(false));

            item.Text = "Hello";
            Assert.IsTrue(item.Verify(true));
        }

        private void _TestSerialization
            (
                FoundItem first
            )
        {
            byte[] bytes = first.SaveToMemory();

            FoundItem second
                = bytes.RestoreObjectFromMemory<FoundItem>();

            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Text, second.Text);
        }

        [TestMethod]
        public void TestFoundItem_Serialization()
        {
            FoundItem item = new FoundItem();
            _TestSerialization(item);

            item.Mfn = 1;
            _TestSerialization(item);

            item.Text = "Hello";
            _TestSerialization(item);
        }
    }
}
