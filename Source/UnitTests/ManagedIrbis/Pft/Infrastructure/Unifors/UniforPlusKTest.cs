using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusKTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusK_GetAuthorSign_1()
        {
            Execute("+Khav.mnu!Abstact", "A 16");
            Execute("+Khav.mnu!дополнительный", "Д 68");
            Execute("+Khav.mnu!Маркс", "М 27");
            Execute("+Khav.mnu!Ыстыбаев", "Ы");

            // Обработка ошибок
            Execute("+K", "");
            Execute("+K!", "");
            Execute("+K!дополнительный", "");
            Execute("+Khav.mnu!", "");
            Execute("+Khav.mnu?дополнительный", "");
        }
    }
}
