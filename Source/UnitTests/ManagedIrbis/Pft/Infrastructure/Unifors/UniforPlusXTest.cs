using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusXTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusX_SearchIncrement_1()
        {
            //Execute("+XK=ALG#ab13cd", "AB14CD");
            //Execute("+XK=ALG#1", "2");

            // Обработка ошибок
            Execute("+X", "");
            Execute("+X12", "");
            Execute("+X12#", "");
            Execute("+X#12", "");
            Execute("+Xnothing#ab12cd34ef", "AB12CD34EF");
        }
    }
}
