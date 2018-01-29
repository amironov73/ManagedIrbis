using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusWTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusW_Increment_1()
        {
            Execute("+Wnothing#ab13cd", "AB14CD");
            Execute("+Wnothing#1", "2");

            // Обработка ошибок
            Execute("+W", "");
            Execute("+W12", "");
            Execute("+W12#", "");
            Execute("+W#12", "");
            Execute("+Wnothing#ab12cd34ef", "AB12CD34EF");
        }
    }
}
