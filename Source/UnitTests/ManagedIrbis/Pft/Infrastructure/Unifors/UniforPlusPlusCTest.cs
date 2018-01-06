using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusPlusCTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusPlusC_WorkWithGlobalCounter_1()
        {
            Execute("++C01", "11");
            Execute("++C01#", "11");

            // Обработка ошибок
            Execute("++C", "");
            Execute("++C#1", "");
            Execute("++CnoSuchCounter#1", "");
        }
    }
}
