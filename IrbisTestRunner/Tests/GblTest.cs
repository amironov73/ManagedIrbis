/* TestTemplate.cs -- IRBIS64 test template
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
using ManagedIrbis.Gbl;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class GblTest
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
        public void TestGbl()
        {
            string field3000 = string.Format
                (
                    "'{0}'",
                    DateTime.Now
                );

            GblItem[] items =
            {
                new GblItem
                {
                    Command = GblCode.Add,
                    Format1 = field3000,
                    Format2 = "XXXXXXXXXXX",
                    Parameter1 = "3000",
                    Parameter2 = "*"
                }, 
            };

            string protocol = Connection
                .ThrowIfNull()
                .GlobalCorrection
                (
                    "\"I=37/К88-602720\"",
                    1,
                    0,
                    0,
                    0,
                    null,
                    true,
                    false,
                    true,
                    items
                );
            Write(protocol);
        }

        #endregion
    }
}
