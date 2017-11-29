using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Mx.Commands
{
    [TestClass]
    public class SortCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void SortCommand_Construction_1()
        {
            SortCommand command = new SortCommand();
            Assert.AreEqual("Sort", command.Name);
        }

        [TestMethod]
        public void SortCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (SortCommand command = new SortCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void SortCommand_ToString_1()
        {
            SortCommand command = new SortCommand();
            Assert.AreEqual("Sort", command.ToString());
        }
    }
}
