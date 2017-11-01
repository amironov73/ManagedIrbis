using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusRTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusR_TrimAtLastDot_1()
        {
            Execute("+R02.64.45", "02.64");
            Execute("+R02.64", "02");

            // Обработка ошибок
            Execute("+R", "");
            Execute("+RHello", "");
        }
    }
}
