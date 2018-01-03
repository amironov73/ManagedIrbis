using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor4Test
        : CommonUniforTest
    {
        private void Execute
            (
                int mfn,
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null)
                {
                    Record = provider.ReadRecord(mfn)
                };
                context.SetProvider(provider);
                Unifor unifor = new Unifor();
                string expression = input;
                unifor.Execute(context, null, expression);
                string actual = context.Text.DosToUnix();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void Unifor4_FormatPreviousVersion_1()
        {
            Execute(1, "4", "1");
            Execute(1, "4,v200^a", "Куда пойти учиться?");
            Execute(1, "41,v200^a", "Куда пойти учиться?");
            Execute(1, "4*,v200^a", "Куда пойти учиться?");

            Execute(2, "4", "25");
            Execute(2, "4,v461^c", "Управление банком");

            // Обработка ошибок
            Execute(1, "4Q", "");
            Execute(1, "4,", "");
        }
    }
}
