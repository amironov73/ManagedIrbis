using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforKTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforK_GetMenuEntry_1()
        {

            Execute("Kfo.mnu!д/о", "Дневное отделение");
            Execute("Kfo.mnu|д/о", "Дневное отделение");

            // Обработка ошибок
            Execute("Kfo.mnu?д/о", "");
            Execute("K!", "");
            Execute("Kfo.mnu!", "");
        }
    }
}
