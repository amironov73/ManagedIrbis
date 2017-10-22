using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor9Test
    {
        private void _9
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
        public void Unifor9_RemoveDoubleQuotes_1()
        {
            _9("9", "");
            _9("912345", "12345");
            _9("912\"345", "12345");
            _9("9\"12345\"", "12345");
            _9("9\"\"\"", "");
        }
    }
}
