using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusSTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusS_DecodeTitle_1()
        {
            Execute("+S0<1=Первое> апреля", "Первое апреля");
            Execute("+S1<1=Первое> апреля", "1 апреля");

            // Обработка ошибок
            Execute("+S", "");
            Execute("+S0", "");
            Execute("+S1", "");
            Execute("+S2<1=Первое> апреля", "1 апреля");
            Execute("+S<1=Первое> апреля", "1=Первое> апреля");
            Execute("+S0<1=Первое> <апреля=мая>", "Первое мая");
        }
    }
}
