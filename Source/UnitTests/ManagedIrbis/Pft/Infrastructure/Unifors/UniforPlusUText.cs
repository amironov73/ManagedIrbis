using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusUText
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusU_RepeatString_1()
        {
            Execute("+U3#Привет!", "Привет!Привет!Привет!");
            Execute("+U0#Привет!", "");
            Execute("+U3#!", "!!!");
            Execute("+U3#", "");

            // Обработка ошибок
            Execute("+U", "");
            Execute("+U#text", "");
            Execute("+U11text", "");
            Execute("+U-1#text", "");
            Execute("+UQ#text", "");
            Execute("+U11#", "");
        }
    }
}
