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
    public class ExitCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void ExitCommand_Construction_1()
        {
            ExitCommand command = new ExitCommand();
            Assert.AreEqual("Exit", command.Name);
        }

        [TestMethod]
        public void ExitCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (ExitCommand command = new ExitCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void ExitCommand_ToString_1()
        {
            ExitCommand command = new ExitCommand();
            Assert.AreEqual("Exit", command.ToString());
        }
    }
}
