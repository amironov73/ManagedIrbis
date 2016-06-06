using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedClient.Worksheet;

namespace UnitTests.ManagedClient.Worksheet
{
    [TestClass]
    public class WorksheetItemTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                WorksheetItem first
            )
        {
            byte[] bytes = first.SaveToMemory();

            WorksheetItem second = bytes
                .RestoreObjectFromMemory<WorksheetItem>();

            Assert.AreEqual(first.DefaultValue, second.DefaultValue);
            Assert.AreEqual(first.FormalVerification, second.FormalVerification);
            Assert.AreEqual(first.Help, second.Help);
            Assert.AreEqual(first.Hint, second.Hint);
            Assert.AreEqual(first.InputInfo, second.InputInfo);
            Assert.AreEqual(first.InputMode, second.InputMode);
            Assert.AreEqual(first.Repeatable, second.Repeatable);
            Assert.AreEqual(first.Reserved, second.Reserved);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.Title, second.Title);
        }

        [TestMethod]
        public void TestWorksheetItemSerialization()
        {
            WorksheetItem item = new WorksheetItem();
            _TestSerialization(item);

            item.Tag = "200";
            item.Title = "Область заглавия";
            _TestSerialization(item);
        }
    }
}
