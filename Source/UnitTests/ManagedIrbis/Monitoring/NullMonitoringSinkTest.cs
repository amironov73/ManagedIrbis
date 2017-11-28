using ManagedIrbis.Monitoring;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Monitoring
{
    [TestClass]
    public class NullMonitoringSinkTest
    {
        [TestMethod]
        public void NullMonitoringSink_Construction_1()
        {
            NullMonitoringSink sink = new NullMonitoringSink();
            Assert.IsNotNull(sink);
        }

        [TestMethod]
        public void NullMonitoringSink_WriteData_1()
        {
            NullMonitoringSink sink = new NullMonitoringSink();
            MonitoringData data = new MonitoringData();
            Assert.IsTrue(sink.WriteData(data));
        }
    }
}
