using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class CloneableCollectionTest
    {
        [TestMethod]
        public void CloneableCollection_Clone()
        {
            CloneableCollection<int> source
                = new CloneableCollection<int>
                {
                    212,
                    85,
                    06
                };
            CloneableCollection<int> clone
                = (CloneableCollection<int>) source.Clone();

            Assert.AreEqual(source.Count, clone.Count);
            for (int i = 0; i < source.Count; i++)
            {
                Assert.AreEqual(source[i], clone[i]);
            }
        }
    }
}
