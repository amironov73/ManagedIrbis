using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforDTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforD_FormatDocumentDB_1()
        {
            Execute("DISTU,@1,v200^a", "Голоценовый вулканизм Срединного хребта Камчатки");
            Execute("DISTU,@2,v200^a", "Многозначный анализ и дифференциальные включения");
            Execute("DISTU,@3,v200^a", "Металлополимерные гибридные нанокомпозиты");

            // Обработка ошибок
            Execute("D", "");
            Execute("D,", "");
            Execute("DISTU", "");
            Execute("DISTU,", "");
            Execute("DISTU,@", "");
            Execute("DISTU,@qq", "");
            Execute("DISTU,@qq,", "");
            Execute("DISTU,@0", "");
            Execute("DISTU,@-1", "");
            Execute("DISTU,@1", "");
            Execute("DISTU,@1,", "");
        }

        [TestMethod]
        public void UniforD_FormatDocumentDB_2()
        {
            Execute("DISTU,/T=ГОЛОЦЕНОВЫЙ ВУЛКАНИЗМ СРЕДИННОГО ХРЕБТА КАМЧАТКИ/,v200^e", "монография");
            Execute("DISTU,/T=МНОГОЗНАЧНЫЙ АНАЛИЗ И ДИФФЕРЕНЦИАЛЬНЫЕ ВКЛЮЧЕНИЯ/,v200^e", "монография");
            Execute("DISTU,/T=МЕТАЛЛОПОЛИМЕРНЫЕ ГИБРИДНЫЕ НАНОКОМПОЗИТЫ/,v200^e", "монография");

            // Обработка ошибок
            Execute("D", "");
            Execute("D,", "");
            Execute("DISTU", "");
            Execute("DISTU,", "");
            Execute("DISTU,/", "");
            Execute("DISTU,/T=ГОЛОЦЕНОВЫЙ", "");
            Execute("DISTU,/T=ГОЛОЦЕНОВЫЙ/", "");
            Execute("DISTU,//", "");
            Execute("DISTU,/T=ГОЛОЦЕНОВЫЙ ВУЛКАНИЗМ СРЕДИННОГО ХРЕБТА КАМЧАТКИ/,", "");
        }

        [TestMethod]
        public void UniforD_FormatDocumentDB_3()
        {
            Execute("DISTU,/T=ГОЛОЦЕНОВЫЙ ВУЛКАНИЗМ СРЕДИННОГО ХРЕБТА КАМЧАТКИ/,*",
                "^aГолоценовый вулканизм Срединного хребта Камчатки^eмонография^fМ. М. Певзнер^gРос. акад. наук, Рос. фонд фундам. исслед.");
        }
    }
}
