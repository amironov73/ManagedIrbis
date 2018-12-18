using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class DisposableCollectionTest
    {
        private static int _count;

        class Dummy
            : IDisposable
        {
            public void Dispose()
            {
                _count++;
            }
        }

        [TestMethod]
        public void DisposableCollection_Dispose()
        {
            _count = 0;

            DisposableCollection<Dummy> collection
                = new DisposableCollection<Dummy>
            {
                new Dummy(),
                new Dummy(),
                new Dummy()
            };

            collection.Dispose();

            Assert.AreEqual(3, _count);
        }
    }
}
