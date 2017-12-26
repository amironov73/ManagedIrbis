using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforLTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforL_ContinueTerm_1()
        {
            Execute("LJAZ=рус", "СКИЙ");
        }

        [TestMethod]
        public void UniforL_ContinueTerm_2()
        {
            Execute("L", "");
        }

        [TestMethod]
        public void UniforL_ContinueTerm_3()
        {
            Execute("LJUK=", "");
        }
    }
}
