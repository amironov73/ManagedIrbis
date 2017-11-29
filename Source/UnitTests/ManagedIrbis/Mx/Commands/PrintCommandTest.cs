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
    public class PrintCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void PrintCommand_Construction_1()
        {
            PrintCommand command = new PrintCommand();
            Assert.AreEqual("Print", command.Name);
        }

        [TestMethod]
        public void PrintCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (PrintCommand command = new PrintCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void PrintCommand_ToString_1()
        {
            PrintCommand command = new PrintCommand();
            Assert.AreEqual("Print", command.ToString());
        }
    }
}
