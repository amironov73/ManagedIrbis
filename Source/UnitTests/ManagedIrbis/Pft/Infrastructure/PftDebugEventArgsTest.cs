using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftDebugEventArgsTest
    {
        [TestMethod]
        public void PftDebugEventArgs_Construction_1()
        {
            PftDebugEventArgs args = new PftDebugEventArgs();
            Assert.IsFalse(args.CancelExecution);
            Assert.IsNull(args.Context);
            Assert.IsNull(args.Node);
            Assert.IsNull(args.Variable);
        }

        [TestMethod]
        public void PftDebugEventArgs_Construction_2()
        {
            PftContext context = new PftContext(null);
            PftNode node = new PftNode();
            PftDebugEventArgs args = new PftDebugEventArgs(context, node);
            Assert.IsFalse(args.CancelExecution);
            Assert.AreSame(context, args.Context);
            Assert.AreSame(node, args.Node);
            Assert.IsNull(args.Variable);
        }

        [TestMethod]
        public void PftDebugEventArgs_CancelExecution_1()
        {
            PftDebugEventArgs args = new PftDebugEventArgs();
            Assert.IsFalse(args.CancelExecution);
            args.CancelExecution = true;
            Assert.IsTrue(args.CancelExecution);
        }

        [TestMethod]
        public void PftDebugEventArgs_Variable_1()
        {
            PftDebugEventArgs args = new PftDebugEventArgs();
            Assert.IsNull(args.Variable);
            PftVariable variable = new PftVariable("name", "value");
            args.Variable = variable;
            Assert.AreSame(variable, args.Variable);
        }
    }
}
