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
    public class ReportTestTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFolder001()
        {
            return Path.Combine(TestDataPath, "Reports\\001");
        }

        [NotNull]
        private string _GetRootFolder()
        {
            return Path.Combine(TestDataPath, "Reports");
        }

        [TestMethod]
        public void ReportTest_Construction_1()
        {
            string folder = _GetFolder001();
            ReportTest test = new ReportTest(folder);
            Assert.AreEqual(folder, test.Folder);
        }

        [TestMethod]
        public void ReportTest_IsDirectoryContainsTest_1()
        {
            string folder = _GetFolder001();
            Assert.IsTrue(ReportTest.IsDirectoryContainsTest(folder));

            folder = _GetRootFolder();
            Assert.IsFalse(ReportTest.IsDirectoryContainsTest(folder));
        }

        [TestMethod]
        public void ReportTest_Run_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                string folder = _GetFolder001();
                ReportTest test = new ReportTest(folder)
                {
                    Provider = provider
                };
                ReportTestResult result = test.Run("001");
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Failed);
            }
        }
    }
}
