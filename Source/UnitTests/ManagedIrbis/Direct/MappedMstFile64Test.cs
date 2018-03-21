using System.IO;

using JetBrains.Annotations;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MappedMstFile64Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS/ibis.mst"
                );
        }

        [TestMethod]
        public void MappedMstFile64_ReadRecord_1()
        {
            string fileName = _GetFileName();
            using (MappedMstFile64 file = new MappedMstFile64(fileName))
            {
                Assert.AreSame(fileName, file.FileName);
                Assert.AreEqual(333, file.ControlRecord.NextMfn);
                MstRecord64 record = file.ReadRecord(22951100L);
                Assert.AreEqual(100, record.Dictionary.Count);
                string expected = "Tag: 200, Position: 2652, Length: 173, Text: ^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
                Assert.AreEqual(expected, record.Dictionary[87].ToString());
            }
        }
    }
}
