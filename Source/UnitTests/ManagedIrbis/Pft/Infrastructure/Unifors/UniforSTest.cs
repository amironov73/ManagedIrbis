using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforSTest
        : CommonUniforTest
    {
        private void _S
            (
                [NotNull] string input,
                int startValue,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                UniversalCounter = startValue
            };
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        private void _S
            (
                [NotNull] string input,
                int startValue,
                int expected
            )
        {
            PftContext context = new PftContext(null)
            {
                UniversalCounter = startValue
            };
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            int actual = context.UniversalCounter;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void UniforS_Add_1()
        {
            _S("S1", 10, 11);
            _S("S9", 10, 19);
        }

        [TestMethod]
        public void UniforS_Add_2()
        {
            _S("S1A", 10, "11");
            _S("S9X", 10, "XIX");
        }

        [TestMethod]
        public void UniforS_Arabic_1()
        {
            _S("SA", 123, "123");
        }

        [TestMethod]
        public void UniforS_Clear_1()
        {
            _S("S0", 123, 0);
        }

        [TestMethod]
        public void UniforS_Roman_1()
        {
            _S("SX", 4, "IV");
            _S("SX", 6, "VI");
            _S("SX", 9, "IX");
            _S("SX", 23, "XXIII");
            _S("SX", 43, "XLIII");
            _S("SX", 53, "LIII");
            _S("SX", 93, "XCIII");
            _S("SX", 123, "CXXIII");
            _S("SX", 423, "CDXXIII");
            _S("SX", 523, "DXXIII");
            _S("SX", 923, "CMXXIII");
            _S("SX", 1234, "MCCXXXIV");
            _S("SX", 3234, "MMMCCXXXIV");
            _S("SX", 6234, "");
        }
    }
}
