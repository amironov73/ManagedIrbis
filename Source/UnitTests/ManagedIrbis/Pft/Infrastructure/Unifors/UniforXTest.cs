using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Unifors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforXTest
    {
        private void _X
            (
                [CanBeNull] string text,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            UniforX.RemoveAngleBrackets(context, null, text);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforX_RemoveAngleBrackets_1()
        {
            _X(null, string.Empty);
            _X(string.Empty, string.Empty);
            _X("abc", "abc");
            _X("a<bc", "a<bc");
            _X("a<b>c", "ac");
            _X("a<<b>c", "ac");
            _X("<abc>", string.Empty);
            _X("<>abc<>", "abc");
            _X("<>a<b>c<>", "ac");
            _X("<>", string.Empty);
            _X("<<>", string.Empty);
            _X("<><>", string.Empty);
        }
    }
}
