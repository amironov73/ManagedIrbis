using System.IO;
using System.Net;
using System.Threading.Tasks;

using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Search;
using ManagedIrbis.Server;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public class ServerUtilityTest
        : Common.CommonUnitTest
    {
        [NotNull]
        [ItemNotNull]
        private string[] _GetPaths()
        {
            string[] result =
            {
                Irbis64RootPath,
                Path.Combine(Irbis64RootPath, "Datai"),
                Path.Combine(Irbis64RootPath, "Datai/IBIS")
            };

            return result;
        }

        [TestMethod]
        public void ServerUtility_FindFileOnPath_1()
        {
            string[] paths = _GetPaths();
            string found = ServerUtility.FindFileOnPath("dumb", ".fst", paths);
            Assert.IsNotNull(found);
            found = ServerUtility.FindFileOnPath("dbnam1", ".mnu", paths);
            Assert.IsNotNull(found);
            found = ServerUtility.FindFileOnPath("client_ini", ".mnu", paths);
            Assert.IsNotNull(found);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ServerUtility_FindFileOnPath_2()
        {
            string[] paths = _GetPaths();
            ServerUtility.FindFileOnPath("NoSuchFile", ".ext", paths);
        }

        [TestMethod]
        public void ServerUtility_ExpandInclusion_1()
        {
            string[] paths = _GetPaths();
            string source = "'Before', \x1C_test_hello\x1D, 'After'";
            string actual = ServerUtility.ExpandInclusion
                (
                    source,
                    ".pft",
                    paths
                );
            string expected = "'Before', 'Hello', 'After'";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void ServerUtility_ExpandInclusion_2()
        {
            string[] paths = new string[0];
            string source = "'Before', \x1C_test_hello\x1D, 'After'";
            ServerUtility.ExpandInclusion
                (
                    source,
                    ".pft",
                    paths
                );
        }
    }
}
