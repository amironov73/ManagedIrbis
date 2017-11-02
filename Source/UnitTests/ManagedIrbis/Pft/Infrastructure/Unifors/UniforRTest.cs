using AM.PlatformAbstraction;
using AM.Text;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforRTest
        : CommonUniforTest
    {
        private void _Execute
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                provider.PlatformAbstraction = new TestingPlatformAbstraction();
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                Unifor unifor = new Unifor();
                string expression = input;
                unifor.Execute(context, null, expression);
                string actual = context.Text.DosToUnix();
                Assert.AreEqual(expected, actual);
            }

        }

        [TestMethod]
        public void UniforR_RandomNumber_1()
        {
            _Execute("R", "984556");
            _Execute("R5", "98455");

            // Обработка ошибок
            _Execute("R0", "");
            _Execute("R10", "");
            _Execute("R-1", "");
        }
    }
}
