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
using ManagedIrbis.Search;
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
        public void SearchRaw_Simple()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            SearchParameters parameters = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "K=A$"
            };
            string[] found = connection.SearchRaw(parameters);
            Write
                (
                    string.Join
                        (
                            Environment.NewLine,
                            found
                        )
                );
        }

        [TestMethod]
        public void SearchRaw_Format()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            SearchParameters parameters = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "K=A$",
                FormatSpecification = "v200^a"
            };
            string[] found = connection.SearchRaw(parameters);
            Write
                (
                    string.Join
                        (
                            Environment.NewLine,
                            found
                        )
                );
        }

        [TestMethod]
        public void SearchRaw_Sequential()
        {
            IrbisConnection connection = Connection.ThrowIfNull("Connection");

            SearchParameters parameters = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "K=A$",
                SequentialSpecification = "if v200^a:'A' then '1' else '0' fi",
                FormatSpecification = "@brief"
            };
            string[] found = connection.SearchRaw(parameters);
            Write
                (
                    string.Join
                        (
                            Environment.NewLine,
                            found
                        )
                );
        }

        #endregion
    }
}
