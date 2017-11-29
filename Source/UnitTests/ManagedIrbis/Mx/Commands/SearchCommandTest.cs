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
    public class SearchCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void SearchCommand_Construction_1()
        {
            SearchCommand command = new SearchCommand();
            Assert.AreEqual("Search", command.Name);
        }

        [TestMethod]
        public void SearchCommand_Execute_1()
        {
            using (MxExecutive executive = GetExecutive())
            {
                using (SearchCommand command = new SearchCommand())
                {
                    command.Initialize(executive);

                    MxArgument[] arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void SearchCommand_ToString_1()
        {
            SearchCommand command = new SearchCommand();
            Assert.AreEqual("Search", command.ToString());
        }
    }
}
