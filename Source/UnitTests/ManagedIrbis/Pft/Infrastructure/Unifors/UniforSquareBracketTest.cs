using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforSquareBracketTest
        : CommonUniforTest
    {
        private void _TestCleanup
            (
                string input,
                string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                context.Write(null, input);
                Unifor unifor = new Unifor();
                unifor.Execute(context, null, "[");
                string actual = context.GetProcessedOutput();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void UniforSquareBracket_CleanContextMarkup_1()
        {
            _TestCleanup
                (
                    "Вот [[b]]жирный[[/b]] текст, а вот [[i]]курсивный[[/i]] текст",
                    "Вот жирный текст, а вот курсивный текст"
                );

            _TestCleanup
                (
                    "Вот [[b]]жирный [[i]]курсивный[[/i]][[/b]] текст",
                    "Вот жирный курсивный текст"
                );

            _TestCleanup("no markup", "no markup");

            // Обработка ошибок
            _TestCleanup("", "");
            _TestCleanup("[[no markup", "[[no markup");
        }
    }
}
