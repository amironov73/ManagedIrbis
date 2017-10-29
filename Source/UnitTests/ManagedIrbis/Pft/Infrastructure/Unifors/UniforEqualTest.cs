using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforEqualTest
    {
        private void _Execute
            (
                [NotNull] string left,
                [NotNull] string right,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            Unifor unifor = new Unifor();
            string input = "=!" + left + "!" + right;
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforEqual_CompareWithMask_1()
        {
            _Execute("", "", "1");
            _Execute("?", "", "0");
            _Execute("*", "", "1");
            _Execute("", "1", "1");
            _Execute("Hello", "Hello", "1");
            _Execute("Hello", "Hell", "0");
            _Execute("Hello*", "Hello", "1");
            _Execute("Hello?", "Hello", "0");
            _Execute("Hello", "hello", "0");
            _Execute("Hello|hello", "Hello", "1");
            _Execute("Hello|hello", "hello", "1");
            _Execute("Hello|hello", "zello", "0");
            _Execute("Hel?o", "Hello", "1");
            _Execute("Hel?o", "Hel_o", "1");
            _Execute("Hel?o", "hello", "0");
            _Execute("*123", "", "1");
            _Execute("*123", "Hello", "1");
            _Execute("?123", "Hello", "0");
        }
    }
}
