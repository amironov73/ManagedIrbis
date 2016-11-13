/* WssFileTest.cs --
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
    class WssFileTest
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
        public void WssFile_ReadFromServer()
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "210m.wss"
                );
            WssFile wss = WssFile.ReadFromServer
                (
                    connection,
                    specification
                );
            if (!ReferenceEquals(wss, null))
            {
                string[] lines = wss.Items
                    .Select
                    (
                        item => item.Tag + ": " + item.Title
                    )
                    .ToArray();
                string text = string.Join("|", lines);
                Write(text);
            }
        }

        #endregion
    }
}
