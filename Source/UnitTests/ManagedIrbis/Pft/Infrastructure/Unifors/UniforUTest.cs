using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforUTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforW_Check_1()
        {
            Execute("W12,10-17", "1");
            Execute("W21,10-17", "0");
        }

        [TestMethod]
        public void UniforU_Cumulate_1()
        {
            Execute("U10-15,16,17", "10-17");
        }

        [TestMethod]
        public void UniforU_Cumulate_2()
        {
            // Обработка ошибок
            Execute("U10-,16,17", "10-,16,17");
        }

        [TestMethod]
        public void UniforV_Decumulate_1()
        {
            Execute("V10-17", "10,11,12,13,14,15,16,17");
        }
    }
}
