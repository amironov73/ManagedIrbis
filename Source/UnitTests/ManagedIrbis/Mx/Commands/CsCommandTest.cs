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
    public class CsCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void CsCommand_Construction_1()
        {
            CsCommand command = new CsCommand();
            Assert.AreEqual("CS", command.Name);
        }

        [TestMethod]
        public void CsCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (CsCommand command = new CsCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void CsCommand_ToString_1()
        {
            CsCommand command = new CsCommand();
            Assert.AreEqual("CS", command.ToString());
        }
    }
}
