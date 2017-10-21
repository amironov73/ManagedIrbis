using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforCTest
    {
        private void _C
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforC_CheckIsbn_1()
        {
            // Пустой ISBN считается правильным
            _C("C", "1");


            _C("C5-02-003157-7", "0");
            _C("C5-02-003206-9", "0");
            _C("C5-02-003206-1", "1");
            _C("C0033-765X", "0");
            _C("C0033-7651", "1");
        }
    }
}
