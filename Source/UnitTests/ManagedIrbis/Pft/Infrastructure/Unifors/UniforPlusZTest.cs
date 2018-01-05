using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusZTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusZ_AnsiToOem_1()
        {
            Execute("+Z0Привет", "ЏаЁўҐв");
            Execute("+Z0╧ЁштхЄ", "Привет");
            Execute("+Z1Привет", "╧ЁштхЄ");
            Execute("+Z1ЏаЁўҐв", "Привет");

            // Обработка ошибок
            Execute("+Z", "");
            Execute("+Z2Привет", "Привет");
        }
    }
}
