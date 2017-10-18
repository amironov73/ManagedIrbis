using System.Text;
using AM.Text;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Unifors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforZTest
    {
        private void _Z
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            MarcRecord record = new MarcRecord();
            context.Record = record;
            RecordField field = RecordField.Parse(910, input);
            record.Fields.Add(field);

            Unifor unifor = new Unifor();
            unifor.Execute(context, null, "Z");

            StringBuilder builder = new StringBuilder();
            foreach (RecordField oneField in record.Fields)
            {
                builder.AppendLine(oneField.ToText());
            }
            string actual = builder.ToString().TrimEnd().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforZ_GenerateExemplars_1()
        {
            _Z("^AR^Qwert", "^a0^qwert");
            _Z("^AR^B0/111", string.Empty);
            _Z("^AR^B-1/111", string.Empty);
            _Z("^AR^B5/111", "^a0^b111\n^a0^b112\n^a0^b113\n^a0^b114\n^a0^b115");
            _Z("^AR^B5", "^a0^b5");
            _Z("^AR^B5/", "^a0^b1\n^a0^b2\n^a0^b3\n^a0^b4\n^a0^b5");
            _Z("^AR^B5/111^H222", "^a0^b111^h222\n^a0^b112^h223\n^a0^b113^h224\n^a0^b114^h225\n^a0^b115^h226");
            _Z("^AR^B5/111^H222^Qwert", "^a0^b111^h222^qwert\n^a0^b112^h223^qwert\n^a0^b113^h224^qwert\n^a0^b114^h225^qwert\n^a0^b115^h226^qwert");
            _Z("^AR^B/5", "^a0^b5");
        }
    }
}
