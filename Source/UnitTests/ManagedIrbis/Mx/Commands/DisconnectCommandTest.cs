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
    public class DisconnectCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void DisconnectCommand_Construction_1()
        {
            DisconnectCommand command = new DisconnectCommand();
            Assert.AreEqual("Disconnect", command.Name);
        }

        [TestMethod]
        public void DisconnectCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (DisconnectCommand command = new DisconnectCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void DisconnectCommand_ToString_1()
        {
            DisconnectCommand command = new DisconnectCommand();
            Assert.AreEqual("Disconnect", command.ToString());
        }
    }
}
