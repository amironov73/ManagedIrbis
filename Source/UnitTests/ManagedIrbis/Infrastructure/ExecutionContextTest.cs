using System;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Infrastructure
{
    [TestClass]
    public class ExecutionContextTest
    {
        [TestMethod]
        public void ExecutionContext_Construction_1()
        {
            ExecutionContext context = new ExecutionContext();
            Assert.IsNull(context.Command);
            Assert.IsNull(context.Connection);
            Assert.IsNull(context.Exception);
            Assert.IsFalse(context.ExceptionHandled);
            Assert.IsNull(context.Response);
            Assert.IsNull(context.UserData);
        }

        [TestMethod]
        public void ExecutionContext_Construction_2()
        {
            IIrbisConnection connection = new IrbisConnection();
            AbstractCommand command = new NopCommand(connection);
            ExecutionContext context = new ExecutionContext(connection, command);
            Assert.AreSame(command, context.Command);
            Assert.AreSame(connection, context.Connection);
            Assert.IsNull(context.Exception);
            Assert.IsFalse(context.ExceptionHandled);
            Assert.IsNull(context.Response);
            Assert.IsNull(context.UserData);
        }

        [TestMethod]
        public void ExecutionContext_Properties_1()
        {
            IIrbisConnection connection = new IrbisConnection();
            AbstractCommand command = new NopCommand(connection);
            ExecutionContext context = new ExecutionContext(connection, command);

            Exception exception = new Exception();
            context.Exception = exception;
            Assert.AreSame(exception, context.Exception);

            context.ExceptionHandled = true;
            Assert.IsTrue(context.ExceptionHandled);

            ServerResponse response = new ServerResponse
                (
                    connection,
                    new byte[0],
                    new [] { new byte[0], new byte[0] },
                    true
                );
            context.Response = response;
            Assert.AreSame(response, context.Response);

            object userData = new object();
            context.UserData = userData;
            Assert.AreSame(userData, context.UserData);
        }

        [TestMethod]
        public void ExecutionContext_Verify_1()
        {
            IIrbisConnection connection = new IrbisConnection();
            AbstractCommand command = new NopCommand(connection);
            ExecutionContext context = new ExecutionContext(connection, command);
            Assert.IsTrue(context.Verify(false));

            context = new ExecutionContext();
            Assert.IsFalse(context.Verify(false));
        }
    }
}
