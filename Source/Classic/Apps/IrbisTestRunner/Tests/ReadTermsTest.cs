/* ReadTermsTest.cs --
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
    class ReadTermsTest
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
        public void ReadTerms_Forward()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            TermParameters parameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 3,
                ReverseOrder = false,
                StartTerm = "K=",
                Format = null
            };

            TermInfo[] terms = connection.ReadTerms
                (
                    parameters
                );

            string text = string.Join
                (
                    "; ",
                    TermInfo.TrimPrefix(terms, "K=")
                        .Select(term => term.Text)
                        .ToArray()
                );
            Write(text);
        }

        [TestMethod]
        public void ReadTerms_Backward()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            TermParameters parameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 3,
                ReverseOrder = true,
                StartTerm = "K=C",
                Format = null
            };

            TermInfo[] terms = connection.ReadTerms
                (
                    parameters
                );

            string text = string.Join
                (
                    "; ",
                    TermInfo.TrimPrefix(terms, "K=")
                        .Select(term => term.Text)
                        .ToArray()
                );
            Write(text);
        }

        [TestMethod]
        public void ReadTerms_Format()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            TermParameters parameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 10,
                ReverseOrder = false,
                StartTerm = "K=",
                Format = IrbisFormat.Brief
            };

            TermInfoEx[] terms = (TermInfoEx[])connection.ReadTerms
                (
                    parameters
                );

            string text = string.Join
                (
                    "| ",
                    terms.Select
                    (
                        item => item.Formatted.SafeSubstring(0,10)
                    )
                );
            Write(text);
        }

        #endregion
    }
}
