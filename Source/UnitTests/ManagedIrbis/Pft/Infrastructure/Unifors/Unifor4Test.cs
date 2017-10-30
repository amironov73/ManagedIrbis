using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor4Test
        : CommonUniforTest
    {
        [TestMethod]
        public void Unifor4_FormatPreviousVersion_1()
        {
            Execute("4,v200^a", "Куда пойти учиться?");
            Execute("41,v200^a", "Куда пойти учиться?");
            Execute("4*,v200^a", "Куда пойти учиться?");

            // Обработка ошибок
            Execute("4", "");
            Execute("4Q", "");
            Execute("4,", "");
        }
    }
}
