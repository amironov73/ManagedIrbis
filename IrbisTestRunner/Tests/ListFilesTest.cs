/* ListFilesTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Network;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class ListFilesTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [TestMethod]
        public void TestListFiles1()
        {
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "*.pft"
                );

            string[] files = Connection.ListFiles(specification);

            string text = string.Join
                (
                    ", ",
                    files
                );
            Console.Write(text);
        }

        [TestMethod]
        public void TestListFiles2()
        {
            FileSpecification specification1 = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "*.pft"
                );
            FileSpecification specification2 = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "*.wss"
                );

            string[] files = Connection.ListFiles
                (
                    new FileSpecification[] { specification1, specification2 }
                );

            string text = string.Join
                (
                    ", ",
                    files
                );
            Console.Write(text);
        }

        #endregion
    }
}
