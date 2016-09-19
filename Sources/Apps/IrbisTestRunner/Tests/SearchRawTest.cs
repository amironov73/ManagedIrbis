/* SearchRawTest.cs -- IRBIS64 test template
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
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class SearchRawTest
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
        public void SearchRaw_Test1()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            string[] result = connection.SearchRaw
                (
                    "IBIS",
                    "K=A$",
                    1,
                    0,
                    null
                );

            string text = string.Join
                (
                    "| ",
                    result
                )
                .SafeSubstring(0, 80);

            Write(text);
        }

        [TestMethod]
        public void SearchRaw_Test2()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            string[] result = connection.SearchRaw
                (
                    "IBIS",
                    "K=A$",
                    1,
                    0,
                    "v200^a"
                );

            string text = string.Join
                (
                    "| ",
                    result
                )
                .SafeSubstring(0, 80);

            Write(text);
        }

        [TestMethod]
        public void SearchRaw_Sequential()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            string[] result = connection.SequentialSearchRaw
                (
                    "IBIS",
                    "K=A$",
                    1,
                    0,
                    0,
                    0,
                    "!if v200^a:'A' then '1' else '0' fi",
                    "@brief"
                );

            string text = string.Join
                (
                    Environment.NewLine,
                    result
                )
                .SafeSubstring(0, 80);

            Write(text);
        }

        #endregion
    }
}
