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
        public void TestReadPostings1()
        {
            TermInfo[] terms = Connection.ReadTerms
                (
                    "K=",
                    3,
                    false,
                    null
                );
            TermPosting[] postings = Connection.ReadPostings
                (
                    null,
                    terms[0].Text,
                    0,
                    1,
                    null
                );

            string text = string.Join
                (
                    "| ",
                    postings.Select(p => p.ToString())
                );
            Write(text);
        }

        [TestMethod]
        public void TestReadPostings2()
        {
            TermInfo[] terms = Connection.ReadTerms
                (
                    "K=",
                    3,
                    false,
                    null
                );
            TermPosting[] postings = Connection.ReadPostings
                (
                    null,
                    terms[0].Text,
                    0,
                    1,
                    "@brief"
                );

            string text = string.Join
                (
                    "| ",
                    postings.Select(p=>p.ToString())
                );
            Write(text);
        }

        [TestMethod]
        public void TestReadPostings3()
        {
            TermInfo[] terms = Connection.ReadTerms
                (
                    "K=",
                    3,
                    false,
                    null
                );
            TermPosting[] postings = Connection.ReadPostings
                (
                    null,
                    terms.Select(t=>t.Text).ToArray(),
                    0,
                    1,
                    "@brief"
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
