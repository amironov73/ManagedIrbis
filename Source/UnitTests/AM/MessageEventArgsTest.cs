using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class MessageEventArgsTest
    {
        [TestMethod]
        public void MessageEventArgs_Construction_1()
        {
            const string message = "Message";
            MessageEventArgs args = new MessageEventArgs(message);
            Assert.AreSame(message, args.Message);
        }
    }
}
