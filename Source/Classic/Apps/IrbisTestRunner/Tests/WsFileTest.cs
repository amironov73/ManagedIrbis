/* WsFileTest.cs --
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
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Testing;
using ManagedIrbis.Worksheet;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class WsFileTest
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
        public void WsFile_ReadFromServer1()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "PAZK31.WS"
                );
            WsFile wss = WsFile.ReadFromServer
                (
                    connection,
                    specification
                );
            if (!ReferenceEquals(wss, null))
            {
                string[] lines = wss.Pages
                    .Select
                    (
                        item => item.Name
                    )
                    .ToArray();
                string text = string.Join("|", lines);
                Write(text);
            }
        }

        [TestMethod]
        public void WsFile_ReadFromServer2()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "PAZK42.WS"
                );
            WsFile wss = WsFile.ReadFromServer
                (
                    connection,
                    specification
                );
            if (!ReferenceEquals(wss, null))
            {
                string[] lines = wss.Pages
                    .Select
                    (
                        item => item.Name
                    )
                    .ToArray();
                string text = string.Join("|", lines);
                Write(text);
            }
        }

        #endregion
    }
}
