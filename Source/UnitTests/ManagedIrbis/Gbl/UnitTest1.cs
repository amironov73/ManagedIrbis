using System;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblEventArgsTest
    {
        [TestMethod]
        public void GblEventArgs_Construction()
        {
            IrbisConnection connection = new IrbisConnection();
            GlobalCorrector corrector = new GlobalCorrector(connection);
            GblEventArgs args = new GblEventArgs(corrector);
            Assert.AreEqual(corrector, args.Corrector);
            Assert.AreEqual(false, args.Cancel);
        }
    }
}
