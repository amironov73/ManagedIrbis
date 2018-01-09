using System.IO;

using AM;

using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusPTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusP_GetPosting_1()
        {
            Execute("+P0,K=ALGEBRAS", "19"); // MFN
            Execute("+P1,K=ALGEBRAS", "1200"); // TAG
            Execute("+P2,K=ALGEBRAS", "1"); // OCC

            // Обработка ошибок
            Execute("+P", "");
            Execute("+P0", "");
            Execute("+P0,", "");
            Execute("+PQ,K=ALGEBRAS", "");
            Execute("+P3,K=ALGEBRAS", "");
            Execute("+P0,K=ALGEBRA", "");
            Execute("+P0,ЯЯЯ", "");
        }
    }
}
