using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforETest
    {
        private void _E
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
        public void UniforE_GetFirstWords_1()
        {
            _E("E3Съешь ещё этих мягких французских булок", "Съешь ещё этих");
            _E("E0Съешь ещё этих мягких французских булок", "Съешь ещё этих мягких французских булок");
            _E("E100Съешь ещё этих мягких французских булок", "00Съешь");
        }
    }
}
