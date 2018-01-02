using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusBackslashTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusBackslash_ConvertBackslashes_1()
        {
            Execute(@"+\0c:\folder\example.txt", @"c:\\folder\\example.txt");
            Execute(@"+\1c:\\folder\\example.txt", @"c:\folder\example.txt");

            Execute(@"+\0c:\\folder\\example.txt", @"c:\\folder\\example.txt");
            Execute(@"+\1c:\folder\example.txt", @"c:\folder\example.txt");

            Execute(@"+\0c:\\\folder\\\example.txt", @"c:\\\\\\folder\\\\\\example.txt");
            Execute(@"+\1c:\\\folder\\\example.txt", @"c:\\\folder\\\example.txt");

            Execute(@"+\0c:\\\\folder\\\\example.txt", @"c:\\\\folder\\\\example.txt");
            Execute(@"+\1c:\\\\folder\\\\example.txt", @"c:\\folder\\example.txt");

            Execute(@"+\0c:\folder\\example.txt", @"c:\\folder\\\\example.txt");
            Execute(@"+\1c:\\folder\example.txt", @"c:\\folder\example.txt");

            Execute(@"+\0c:\folder\\\example.txt", @"c:\\folder\\\\\\example.txt");
            Execute(@"+\1c:\\\folder\example.txt", @"c:\\\folder\example.txt");

            Execute(@"+\0c:\\folder\\\example.txt", @"c:\\\\folder\\\\\\example.txt");
            Execute(@"+\1c:\\\folder\\example.txt", @"c:\\\folder\\example.txt");

            Execute(@"+\0no slashes", @"no slashes");
            Execute(@"+\1no slashes", @"no slashes");

            // Обработка ошибок
            Execute(@"+\0", @"");
            Execute(@"+\1", @"");

            Execute(@"+\2c:\folder\example.txt", @"c:\\folder\\example.txt");
            Execute(@"+\qc:\folder\example.txt", @"c:\\folder\\example.txt");
        }
    }
}
