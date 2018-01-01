using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforHTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforH_ExtractAngleBrackets_1()
        {
            Execute("Habc<def>ijk", "<def>");
            Execute("Habc<def>ijk<lmo>pqr", "<def><lmo>");

            Execute("Hno brackets", "");

            // Обработка ошибок
            Execute("Habc<def", "");
            Execute("Habc<", "");
            Execute("Habc<def>ijk<lm", "<def>");
            Execute("Habc<defijk<lm>opq", "<defijk<lm>");
            Execute("H", "");
        }
    }
}
