using System;
using System.Globalization;
using System.IO;
using AM.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Direct;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class DirectUtilityTest
    {
        [TestMethod]
        public void DirectUlility_CreateDatabase32()
        {
            Random random = new Random();
            string directory = Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToString
                    (
                        CultureInfo.InvariantCulture
                    ) + "_1"
                );
            Directory.CreateDirectory(directory);
            string path = Path.Combine
                (
                    directory,
                    "database"
                );
            DirectUtility.CreateDatabase32(path);
            string[] files = Directory.GetFiles(directory);
            Assert.AreEqual(8, files.Length);
        }

        [TestMethod]
        public void DirectUlility_CreateDatabase64()
        {
            Random random = new Random();
            string directory = Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToString
                    (
                        CultureInfo.InvariantCulture
                    ) + "_2"
                );
            Directory.CreateDirectory(directory);
            string path = Path.Combine
                (
                    directory,
                    "database"
                );
            DirectUtility.CreateDatabase64(path);
            string[] files = Directory.GetFiles(directory);
            Assert.AreEqual(5, files.Length);
        }
    }
}
