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
    public class StoreCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void StoreCommand_Construction_1()
        {
            StoreCommand command = new StoreCommand();
            Assert.AreEqual("Store", command.Name);
        }

        [TestMethod]
        public void StoreCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (StoreCommand command = new StoreCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void StoreCommand_ToString_1()
        {
            StoreCommand command = new StoreCommand();
            Assert.AreEqual("Store", command.ToString());
        }
    }
}
