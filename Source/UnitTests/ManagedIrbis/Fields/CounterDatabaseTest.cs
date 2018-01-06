using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Fields;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class CounterDatabaseTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void CounterDatabase_GetCounter_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                CounterDatabase database = new CounterDatabase(provider);
                GlobalCounter counter = database.GetCounter("01");
                Assert.IsNotNull(counter);
                Assert.AreEqual(11, counter.NumericValue);

                counter = database.GetCounter("noSuchCounter");
                Assert.IsNull(counter);
            }
        }
    }
}
