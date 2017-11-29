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
    public class AliasCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void AliasCommand_Construction_1()
        {
            AliasCommand command = new AliasCommand();
            Assert.AreEqual("Alias", command.Name);
        }

        [TestMethod]
        public void AliasCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (AliasCommand command = new AliasCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void AliasCommand_ToString_1()
        {
            AliasCommand command = new AliasCommand();
            Assert.AreEqual("Alias", command.ToString());
        }
    }
}
