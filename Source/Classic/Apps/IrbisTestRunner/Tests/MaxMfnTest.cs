/* MaxMfnTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;

using ManagedIrbis.Testing;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class MaxMfnTest
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
        public void MaxMfn_Test1()
        {
            int maxMfn = Connection.GetMaxMfn();
            Write(maxMfn);
        }


        #endregion
    }
}
