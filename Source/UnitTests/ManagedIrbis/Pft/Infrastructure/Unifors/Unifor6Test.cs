using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor6Test
        : CommonUniforTest
    {
        [TestMethod]
        public void Unifor6_ExecuteNestedFormat_1()
        {
            Execute("6_test_hello", "Hello");
            Execute("6_test_onearg#700", "Field v700 absent");
            Execute("6_test_twoarg#700,701", "Field v700 absent\nField v701 absent");

            // Обработка ошибок
            Execute("6", "");
            Execute("6#700", "");
            Execute("6notexist", "");
        }
    }
}
