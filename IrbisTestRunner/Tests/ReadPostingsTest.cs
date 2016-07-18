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
        public void TestReadPostings()
        {
            TermInfo[] terms = Connection.ReadTerms
                (
                    "K=",
                    3,
                    false
                );
            TermPosting[] postings = Connection.ReadPostings
                (
                    null,
                    terms[0].Text,
                    0,
                    1
                );


            string text = string.Join
                (
                    ", ",
                    postings.Select(p=>p.Mfn.ToString())
                );
            Console.Write(text);
        }

        #endregion
    }
}
