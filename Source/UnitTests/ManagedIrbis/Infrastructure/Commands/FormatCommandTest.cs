﻿using System;
using System.IO;
using System.Text;

using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Infrastructure.Commands
{
    [TestClass]
    public class FormatCommandTest
    {
        [TestMethod]
        public void FormatCommand_Construciton_1()
        {
            IIrbisConnection connection = new IrbisConnection();
            FormatCommand command
                = new FormatCommand(connection);
            Assert.AreSame(connection, command.Connection);
        }
    }
}
