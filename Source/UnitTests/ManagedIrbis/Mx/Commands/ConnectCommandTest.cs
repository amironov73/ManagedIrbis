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
    public class ConnectCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void ConnectCommand_Construction_1()
        {
            ConnectCommand command = new ConnectCommand();
            Assert.AreEqual("Connect", command.Name);
        }

        [TestMethod]
        public void ConnectCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (ConnectCommand command = new ConnectCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void ConnectCommand_ToString_1()
        {
            ConnectCommand command = new ConnectCommand();
            Assert.AreEqual("Connect", command.ToString());
        }
    }
}
