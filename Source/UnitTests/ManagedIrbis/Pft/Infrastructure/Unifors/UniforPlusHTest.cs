using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusHTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusH_Take3Of4_1()
        {
            Execute("+H1", "01");
            Execute("+H12", "012");
            Execute("+H123", "1123");
            Execute("+H1234", "1123");
            Execute("+H12345", "21235");
            Execute("+H123456", "212356");
            Execute("+H1234567", "2123567");
            Execute("+H12345678", "2123567");
            Execute("+H123456789", "31235679");
            Execute("+H1234567890", "312356790");
            Execute("+H12345678901", "3123567901");
            Execute("+H123456789012", "3123567901");
            Execute("+H1234567890123", "41235679013");
            Execute("+H12345678901234", "412356790134");
            Execute("+HHello, world!", "4Helo, orl!");

            // Обработка ошибок
            Execute("+H", "");
            Execute("+HП", "0П");
            Execute("+HПривет", "");
        }
    }
}
