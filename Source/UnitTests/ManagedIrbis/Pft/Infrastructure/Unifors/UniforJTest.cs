using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforJTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforJ_GetTermRecordCountDB_1()
        {
            Execute("J,K=BARBARICUM", "2");
            Execute("JIBIS,K=BARBARICUM", "2");

            // Обработка ошибок
            Execute("J", "");
            Execute("JK=BARBARICUM", "");
            Execute("JIBIS,", "");
            Execute("J,", "");
        }
    }
}
