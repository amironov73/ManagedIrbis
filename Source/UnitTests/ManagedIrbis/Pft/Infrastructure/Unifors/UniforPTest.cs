using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforP_UniqueField_1()
        {
            Execute("Pv692^c", "O,V");
            Execute("Pv692^c#1", "O");
            Execute("Pv692^c#*", "V");

            // Обработка ошибок
            Execute("Pv", "");
            Execute("Pv692^", "");
        }
    }
}
