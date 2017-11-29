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
    public class CsFileCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void CsFileCommand_Construction_1()
        {
            CsFileCommand command = new CsFileCommand();
            Assert.AreEqual("CSFile", command.Name);
        }

        [TestMethod]
        public void CsFileCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (CsFileCommand command = new CsFileCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void CsFileCommand_ToString_1()
        {
            CsFileCommand command = new CsFileCommand();
            Assert.AreEqual("CSFile", command.ToString());
        }
    }
}
