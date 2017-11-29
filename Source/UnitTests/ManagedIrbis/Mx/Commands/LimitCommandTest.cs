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
    public class LimitCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void LimitCommand_Construction_1()
        {
            LimitCommand command = new LimitCommand();
            Assert.AreEqual("Limit", command.Name);
        }

        [TestMethod]
        public void LimitCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (LimitCommand command = new LimitCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void LimitCommand_ToString_1()
        {
            LimitCommand command = new LimitCommand();
            Assert.AreEqual("Limit", command.ToString());
        }
    }
}
