using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor7Test
        : CommonUniforTest
    {
        [TestMethod]
        public void Unifor7_FormatDocuments_1()
        {
            Execute("7ISTU,/T=Голоценовый вулканизм$/,v200^a", "Голоценовый вулканизм Срединного хребта Камчатки");
            Execute("7ISTU,/T=Многозначный анализ$/,v200^a", "Многозначный анализ и дифференциальные включения");
            Execute("7ISTU,/T=Металлополимерные гибридные$/,v200^a", "Металлополимерные гибридные нанокомпозиты");

            Execute("7ISTU,/T=Многозначный анализ и дифференциальные включения/,v210^c", "Физматлит");

            Execute("7ISTU,/T=Многозначный анализ и дифференциальные включения/,*", "");

            // Обработка ошибок
            Execute("7", "");
            Execute("7,", "");
            Execute("7ISTU", "");
            Execute("7ISTU,", "");
            Execute("7ISTU,/", "");
            Execute("7ISTU,/T=Голоценовый вулканизм$", "");
            Execute("7ISTU,/T=Голоценовый вулканизм$/", "");
            Execute("7ISTU,//", "");
            Execute("7ISTU,/T=Голоценовый вулканизм$/,", "");
        }
    }
}
