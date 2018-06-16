using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor8Test
        : CommonUniforTest
    {
        [TestMethod]
        public void Unifor8_FormatWithFst_1()
        {
            Execute
                (
                    "8ISTU,@1,ISTU,200,0",
                    "\nT=Голоценовый вулканизм Срединного хребта Камчатки\n"
                    +"TV=Голоценовый вулканизм Срединного хребта Камчатки - 551.21(571.66)-658726400\n"
                    +"TP=Голоценовый вулканизм Срединного хребта Камчатки/М. М. Певзнер; Рос. акад. наук, Рос. фонд фундам. исслед.,2015.-251 с."
                );

            Execute
                (
                    "8ISTU,/T=Голоценовый вулканизм Срединного хребта Камчатки/,ISTU,200,0",
                    "\nT=Голоценовый вулканизм Срединного хребта Камчатки\n"
                    + "TV=Голоценовый вулканизм Срединного хребта Камчатки - 551.21(571.66)-658726400\n"
                    + "TP=Голоценовый вулканизм Срединного хребта Камчатки/М. М. Певзнер; Рос. акад. наук, Рос. фонд фундам. исслед.,2015.-251 с."
                );

            // Обработка ошибок
            Execute("8", "");
            Execute("8,", "");
            Execute("8ISTU", "");
            Execute("8ISTU,", "");
            Execute("8ISTU,@0,ISTU,200,0", "");
            Execute("8ISTU,,ISTU,200,0", "");
            Execute("8ISTU,@1,,200,0", "");
            Execute("8ISTU,@1,,200,", "");
            Execute("8ISTU,/", "");
            Execute("8ISTU,/T=Голоценовый вулканизм$", "");
            Execute("8ISTU,/T=Голоценовый вулканизм$/", "");
            Execute("8ISTU,//", "");
            Execute("8ISTU,/T=Голоценовый вулканизм$/,", "");
        }
    }
}
