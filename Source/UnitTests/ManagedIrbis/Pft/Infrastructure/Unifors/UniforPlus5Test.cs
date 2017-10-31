using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus5Test
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlus5_GetEntry_1()
        {
            Execute(null, 0, "+5Tmhr.mnu", "КХ");
            Execute(null, 0, "+5Fmhr.mnu", "Книгохранилище");
            Execute(null, 1, "+5Tmhr.mnu", "ЦОР");
            Execute(null, 1, "+5Fmhr.mnu", "Центр образовательных ресурсов");
            Execute(null, -1, "+5Tmhr.mnu", "УХТТ");
            Execute(null, -1, "+5Fmhr.mnu", "Усольский химико-технологический техникум");

            // Обработка ошибок
            Execute(null, 1000, "+5Tmhr.mnu", "");
            Execute(null, 0, "+5Qmhr.mnu", "");
            Execute(null, 0, "+5Tmhr.mni", "");
            Execute(null, 0, "+5T", "");
            Execute(null, 0, "+5", "");
        }
    }
}
