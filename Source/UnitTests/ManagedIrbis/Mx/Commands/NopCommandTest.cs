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
    public class NopCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void NopCommand_Construction_1()
        {
            NopCommand command = new NopCommand();
            Assert.AreEqual("Nop", command.Name);
        }

        [TestMethod]
        public void NopCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (NopCommand command = new NopCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void NopCommand_ToString_1()
        {
            NopCommand command = new NopCommand();
            Assert.AreEqual("Nop", command.ToString());
        }
    }
}
