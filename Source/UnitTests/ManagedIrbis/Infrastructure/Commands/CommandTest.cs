using System;
using System.IO;
using System.Text;

using AM.IO;
using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Infrastructure.Commands
{
    public abstract class CommandTest
        : Common.CommonUnitTest
    {
        [NotNull]
        protected Mock<IIrbisConnection> GetConnectionMock()
        {
            Mock<IIrbisConnection> result = new Mock<IIrbisConnection>();
            IIrbisConnection connection = result.Object;

            result.SetupGet(c => c.Executive)
                .Returns(new StandardEngine(connection, null));
            result.SetupGet(c => c.Socket)
                .Returns(new TestingSocket(connection));


            return result;
        }
    }
}
