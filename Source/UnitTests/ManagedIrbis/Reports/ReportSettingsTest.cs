using System;
using System.IO;

using AM;
using AM.Runtime;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Reports;

namespace UnitTests.ManagedIrbis.Reports
{
    [TestClass]
    public class ReportSettingsTest
    {
        [TestMethod]
        public void ReportSettings_Construction_1()
        {
            ReportSettings settings = new ReportSettings();
            Assert.IsNotNull(settings.Assemblies);
            Assert.AreEqual(0, settings.Assemblies.Count);
            Assert.IsNull(settings.DriverName);
            Assert.IsNull(settings.DriverSettings);
            Assert.IsNull(settings.Filter);
            Assert.IsNull(settings.OutputFile);
            Assert.IsNull(settings.PageSettings);
            Assert.IsNull(settings.PrinterName);
            Assert.IsNull(settings.ProviderName);
            Assert.IsNull(settings.ProviderSettings);
            Assert.IsNull(settings.RegisterDriver);
            Assert.IsNull(settings.RegisterProvider);
        }
    }
}
