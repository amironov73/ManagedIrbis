using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftContextSaverTest
    {
        [TestMethod]
        public void PftContextSaver_Dispose_1()
        {
            MarcRecord originalRecord = new MarcRecord();
            PftGroup originalGroup = new PftGroup();
            PftField originalField = new PftField();
            const int originalIndex = 10;
            PftContext context = new PftContext(null)
            {
                Record = originalRecord,
                Index = originalIndex,
                CurrentGroup = originalGroup,
                CurrentField = originalField
            };

            using (new PftContextSaver(context, false))
            {
                context.Record = new MarcRecord();
                context.Index = 11;
                context.CurrentGroup = new PftGroup();
                context.CurrentField = new PftField();
            }

            Assert.AreSame(originalRecord, context.Record);
            Assert.AreEqual(originalIndex, context.Index);
            Assert.AreSame(originalGroup, context.CurrentGroup);
            Assert.AreSame(originalField, context.CurrentField);
        }

        [TestMethod]
        public void PftContextSaver_Dispose_2()
        {
            MarcRecord originalRecord = new MarcRecord();
            PftGroup originalGroup = new PftGroup();
            PftField originalField = new PftField();
            const int originalIndex = 10;
            PftContext context = new PftContext(null)
            {
                Record = originalRecord,
                Index = originalIndex,
                CurrentGroup = originalGroup,
                CurrentField = originalField
            };

            using (new PftContextSaver(context, true))
            {
                Assert.AreEqual(0, context.Index);
                Assert.IsNull(context.CurrentGroup);
                Assert.IsNull(context.CurrentField);

                context.Record = new MarcRecord();
                context.Index = 11;
                context.CurrentGroup = new PftGroup();
                context.CurrentField = new PftField();
            }

            Assert.AreSame(originalRecord, context.Record);
            Assert.AreEqual(originalIndex, context.Index);
            Assert.AreSame(originalGroup, context.CurrentGroup);
            Assert.AreSame(originalField, context.CurrentField);
        }
    }
}
