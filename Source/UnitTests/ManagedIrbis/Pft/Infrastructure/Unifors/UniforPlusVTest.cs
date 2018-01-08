using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusVTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusV_Substring_1()
        {
            Execute("+V10#Hello, world!", "Hello, wor");
            Execute("+V10#Hello, world!", "Hello, wor");
            Execute("+V1#Hello, world!", "H");
            Execute("+V100#Hello, world!", "Hello, world!");

            // Обработка ошибок
            Execute("+V", "");
            Execute("+V-10#Hello, world!", "");
            Execute("+VHello, world!", "");
            Execute("+V#1", "");
            Execute("+VAA#Hello", "");
            Execute("+V1#", "");
        }
    }
}
