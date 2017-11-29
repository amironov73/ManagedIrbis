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
    public class FormatCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void FormatCommand_Construction_1()
        {
            FormatCommand command = new FormatCommand();
            Assert.AreEqual("Format", command.Name);
        }

        [TestMethod]
        public void FormatCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (FormatCommand command = new FormatCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void FormatCommand_ToString_1()
        {
            FormatCommand command = new FormatCommand();
            Assert.AreEqual("Format", command.ToString());
        }
    }
}
