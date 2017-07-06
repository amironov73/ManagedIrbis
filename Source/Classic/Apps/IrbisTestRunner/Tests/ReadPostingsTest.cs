/* ReadPostingsTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
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
    class ReadPostingsTest
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
        public void ReadPostings_Test1()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            TermParameters termParameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 3,
                ReverseOrder = false,
                StartTerm = "K=",
                Format = null
            };

            TermInfo[] terms = connection.ReadTerms
                (
                    termParameters
                );

            PostingParameters postingParameters = new PostingParameters
            {
                Database = "IBIS",
                Term = terms.ThrowIfNullOrEmpty("terms")[0].Text,
                NumberOfPostings = 3
            };

            TermPosting[] postings = Connection.ReadPostings
                (
                    postingParameters
                );

            string text = string.Join
                (
                    "| ",
                    postings.Select(p => p.ToString())
                );
            Write(text);
        }

        [TestMethod]
        public void ReadPostings_Test2()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            TermParameters termParameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 3,
                ReverseOrder = false,
                StartTerm = "K=",
                Format = null
            };

            TermInfo[] terms = connection.ReadTerms
                (
                    termParameters
                );

            PostingParameters postingParameters = new PostingParameters
            {
                Database = "IBIS",
                Term = terms.ThrowIfNullOrEmpty("terms")[0].Text,
                NumberOfPostings = 3,
                Format = IrbisFormat.Brief
            };

            TermPosting[] postings = Connection.ReadPostings
                (
                    postingParameters
                );

            string text = string.Join
                (
                    "| ",
                    postings.Select(p=>p.ToString())
                );
            Write(text);
        }

        [TestMethod]
        public void ReadPostings_Test3()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            TermParameters termParameters = new TermParameters
            {
                Database = "IBIS",
                NumberOfTerms = 3,
                ReverseOrder = false,
                StartTerm = "K=",
                Format = null
            };

            TermInfo[] terms = connection.ReadTerms
                (
                    termParameters
                );

            PostingParameters postingParameters = new PostingParameters
            {
                Database = "IBIS",
                ListOfTerms = terms.ThrowIfNullOrEmpty("terms")
                    .Select(t => t.Text).ToArray(),
                NumberOfPostings = 3,
                Format = IrbisFormat.Brief
            };

            TermPosting[] postings = Connection.ReadPostings
                (
                    postingParameters
                );

            string text = string.Join
                (
                    "| ",
                    postings.Select(p => p.ToString())
                );
            Write(text);
        }

        #endregion
    }
}
