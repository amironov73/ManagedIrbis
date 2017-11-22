using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Search;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class FoundLineTest
    {
        [TestMethod]
        public void FoundLine_Construction_1()
        {
            FoundLine line = new FoundLine();
            Assert.IsFalse(line.Materialized);
            Assert.AreEqual(0, line.SerialNumber);
            Assert.AreEqual(0, line.Mfn);
            Assert.IsNull(line.Icon);
            Assert.IsFalse(line.Selected);
            Assert.IsNull(line.Description);
            Assert.IsNull(line.Sort);
            Assert.IsNull(line.UserData);
        }

        [TestMethod]
        public void FoundLine_Properties_1()
        {
            FoundLine line = new FoundLine();
            line.Materialized = true;
            Assert.IsTrue(line.Materialized);
            line.Mfn = 123;
            Assert.AreEqual(123, line.Mfn);
            line.SerialNumber = 321;
            Assert.AreEqual(321, line.SerialNumber);
            line.Icon = "icon";
            Assert.AreEqual("icon", line.Icon);
            line.Selected = true;
            Assert.IsTrue(line.Selected);
            line.Description = "Description";
            Assert.AreEqual("Description", line.Description);
            line.Sort = "Sort";
            Assert.AreEqual("Sort", line.Sort);
            line.UserData = "User data";
            Assert.AreEqual("User data", line.UserData);
        }
    }
}
