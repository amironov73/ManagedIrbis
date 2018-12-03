using System;
using System.IO;

using AM;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Direct;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class InvertedFile64Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS/ibis.ifp"
                );
        }

        [NotNull]
        private string _CreateDatabase()
        {
            Random random = new Random();
            string directory = Path.Combine
            (
                Path.GetTempPath(),
                random.Next().ToInvariantString()
            );
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, "database");
            DirectUtility.CreateDatabase64(path);
            string result = path + ".mst";

            return result;
        }

        [TestMethod]
        public void InvertedFile64_Construction_1()
        {
            string fileName = _GetFileName();
            using (InvertedFile64 inverted = new InvertedFile64(fileName, DirectAccessMode.ReadOnly))
            {
                Assert.IsFalse(inverted.Fragmented);
                Assert.AreSame(fileName, inverted.FileName);
                Assert.AreEqual(DirectAccessMode.ReadOnly, inverted.Mode);
                Assert.IsNotNull(inverted.IfpControlRecord);
                Assert.IsNotNull(inverted.Ifp);
                Assert.IsNotNull(inverted.L01);
                Assert.IsNotNull(inverted.N01);
                Assert.IsNull(inverted.AdditionalControlRecord);
                Assert.IsNull(inverted.AdditionalIfp);
                Assert.IsNull(inverted.AdditionalL01);
                Assert.IsNull(inverted.AdditionalN01);
            }
        }
    }
}
