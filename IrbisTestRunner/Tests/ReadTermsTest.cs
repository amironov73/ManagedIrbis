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
        public void TestReadTerms_Forward()
        {
            TermInfo[] terms = Connection.ReadTerms
                (
                    "K=",
                    3,
                    false,
                    null
                );

            string text = string.Join
                (
                    "; ",
                    TermInfo.TrimPrefix(terms, "K=")
                );
            Write(text);
        }

        [TestMethod]
        public void TestReadTerms_Backward()
        {
            TermInfo[] terms = Connection.ReadTerms
                (
                    "K=C",
                    3,
                    true,
                    null
                );

            string text = string.Join
                (
                    "; ",
                    TermInfo.TrimPrefix(terms, "K=")
                );
            Write(text);
        }

        [TestMethod]
        public void TestReadTerms_Format()
        {
            TermInfoEx[] terms = (TermInfoEx[]) Connection.ReadTerms
                (
                    "K=",
                    10,
                    false,
                    "@brief"
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
