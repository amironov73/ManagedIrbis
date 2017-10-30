using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor2Test
        : CommonUniforTest
    {
        [TestMethod]
        public void Unifor2_GetMaxMfn_1()
        {

            Execute("2", "0000000333");
            Execute("21", "3");
            Execute("212", "000000000333");

            // Обработка ошибок
            Execute("2Q", "0000000333");
            Execute("20", "");
            Execute("2-1", "");
        }
    }
}
