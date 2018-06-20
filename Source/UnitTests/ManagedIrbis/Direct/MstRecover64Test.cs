using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstRecover64Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetEmptyMst()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "EMPTY.MST"
                );
        }

        [NotNull]
        private string _GetIbisMst()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS/ibis.mst"
                );
        }

        [TestMethod]
        public void MstRecover64_FindRecords_1()
        {
            FoundRecord[] found = MstRecover64.FindRecords(_GetEmptyMst());
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void MstRecover64_FindRecords_2()
        {
            FoundRecord[] found = MstRecover64.FindRecords(_GetIbisMst());
            Assert.IsNotNull(found);
            Assert.AreEqual(332, found.Length);
        }

        [TestMethod]
        public void MstRecover64_BuildXrf_1()
        {
            FoundRecord[] found = MstRecover64.FindRecords(_GetIbisMst());
            string xrfPath = Path.GetTempFileName();
            MstRecover64.BuildXrf(found, xrfPath);
        }

        [TestMethod]
        public void MstRecover64_BuildMst_1()
        {
            string sourceMst = _GetEmptyMst();
            FoundRecord[] found = MstRecover64.FindRecords(sourceMst);
            string destinationMst = Path.GetTempFileName();
            MstRecover64.BuildMst(found, sourceMst, destinationMst);
        }

        [TestMethod]
        public void MstRecover64_BuildIso_1()
        {
            string source = _GetEmptyMst();
            FoundRecord[] found = MstRecover64.FindRecords(source);
            string destination = Path.GetTempFileName();
            MstRecover64.BuildIso(found, source, destination, IrbisEncoding.Ansi);
        }

        [TestMethod]
        public void MstRecover64_BuildText_1()
        {
            string source = _GetEmptyMst();
            FoundRecord[] found = MstRecover64.FindRecords(source);
            string destination = Path.GetTempFileName();
            MstRecover64.BuildText(found, source, destination, IrbisEncoding.Ansi);
        }

        [TestMethod]
        public void MstRecover64_CheckXrf_1()
        {
            string mstPath = _GetIbisMst();
            string xrfPath = Path.ChangeExtension(mstPath, ".xrf");
            string[] result = MstRecover64.CheckXrf(mstPath, xrfPath);
            Assert.IsNotNull(result);
        }
    }
}
