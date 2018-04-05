using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Infrastructure
{
    [TestClass]
    public class ExecutionEventArgsTest
    {
        [TestMethod]
        public void ExecutionEventArgs_Construction_1()
        {
            ExecutionContext context = new ExecutionContext();
            ExecutionEventArgs args = new ExecutionEventArgs(context);
            Assert.AreSame(context, args.Context);
        }
    }
}
