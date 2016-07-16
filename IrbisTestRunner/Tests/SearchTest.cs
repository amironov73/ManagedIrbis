/* SearchTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;

using AM;

using ManagedIrbis.Testing;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class SearchTest
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
        public void TestSearch()
        {
            int[] found = Connection.Search("T=A$");
            Console.Write
                (
                    string.Join
                    (
                        ", ",
                        found.Select(mfn => mfn.ToInvariantString())
                    )
                    .Substring(0,50)
                );
        }


        #endregion
    }
}
