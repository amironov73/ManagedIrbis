using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforTTest
    {
        private void _T
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
        public void UniforT_Transliterate_1()
        {
            _T("T0", "");
            _T("T01234567890", "1234567890");
            _T("T0Съешь ещё этих мягких французских булок, да выпей, дружок, чаю!", "S\"esh\' eshchio etikh miagkikh frantsuzskikh bulok, da vypei, druzhok, chaiu!");
        }

        [TestMethod]
        public void UniforT_Transliterate_2()
        {
            _T("T1", "");
            _T("T11234567890", "1234567890");
            _T("T1Съешь ещё этих мягких французских булок, да выпей, дружок, чаю!", "S\"esh\' eshchio etikh miagkikh frantsuzskikh bulok, da vypei, druzhok, chaiu!");
        }

        [TestMethod]
        public void UniforT_Transliterate_3()
        {
            _T("T2", "");
            _T("T21234567890", "1234567890");
            _T("T2Съешь ещё этих мягких французских булок, да выпей, дружок, чаю!", "S\"esh\' eshchio etikh miagkikh frantsuzskikh bulok, da vypei, druzhok, chaiu!");
        }
    }
}
