using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusPlusATest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusPlusA_GetPhrase_1()
        {
            Execute("++AС Новым годом!", "Новым годом!");
            Execute("++AС новым годом!", "");
            Execute("++Aтолько Рюмка водки на столе!", "Рюмка водки на столе!");
            Execute("++AЁжики плакали, кололись, но продолжали есть мышек", "Ёжики плакали, кололись, но продолжали есть мышек");
            Execute("++AHappy New Year!", "Happy New Year!");
            Execute("++Ahappy New Year!", "Year!");
            Execute("++ADo more harm!", "");
            Execute("++Aпо-сербски Ђурађ Бранковић Смедеревац", "Ђурађ Бранковић Смедеревац");

            // Обработка ошибок
            Execute("++A", "");
            Execute("++A!!!", "");
            Execute("++A123456", "");
        }
    }
}
