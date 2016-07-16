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
        public void TestMaxMfn()
        {
            int maxMfn = Connection.GetMaxMfn();
            Console.Write(maxMfn);
        }


        #endregion
    }
}
