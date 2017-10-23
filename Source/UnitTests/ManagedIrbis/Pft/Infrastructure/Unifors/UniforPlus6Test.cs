using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus6Test
    {
        private void _6
            (
                bool deleted,
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            MarcRecord record = new MarcRecord
            {
                Deleted = deleted
            };
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforPlus6_GetRecordStatus_1()
        {
            _6(false, "+6", "1");
            _6(true, "+6", "0");
        }
    }
}
