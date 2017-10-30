using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforITest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforI_GetIniFileEntry_1()
        {
            Execute("IPRIVATE,NAME,NONAME", "NONAME");
            Execute("IPRIVATE,FIO,NONAME", "kladovka");
            Execute("IPRIVATE,FIO", "kladovka");

            // Обработка ошибок
            Execute("IPRIVATE", "");
            Execute("I", "");
        }
    }
}
