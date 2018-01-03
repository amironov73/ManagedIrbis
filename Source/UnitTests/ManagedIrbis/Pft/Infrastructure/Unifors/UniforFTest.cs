using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforFTest
    {
        private void _F
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
        public void UniforF_GetLastWords_1()
        {
            _F("F3Съешь ещё этих мягких французских булок", " мягких французских булок");
            _F("F0Съешь ещё этих мягких французских булок", "");
            _F("F100Съешь ещё этих мягких французских булок", " ещё этих мягких французских булок");
        }
    }
}
