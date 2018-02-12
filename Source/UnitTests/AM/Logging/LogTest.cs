using System;

using AM.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.AM.Logging
{
    [TestClass]
    public class LogTest
    {
        private Mock<IAmLogger> _GetLogger()
        {
            Mock<IAmLogger> mock = new Mock<IAmLogger>();
            mock.Setup(m => m.Debug(It.IsAny<string>()));
            mock.Setup(m => m.Error(It.IsAny<string>()));
            mock.Setup(m => m.Fatal(It.IsAny<string>()));
            mock.Setup(m => m.Info(It.IsAny<string>()));
            mock.Setup(m => m.Trace(It.IsAny<string>()));
            mock.Setup(m => m.Warn(It.IsAny<string>()));

            return mock;
        }

        [TestMethod]
        public void Log_Debug_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Debug("message");
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Debug(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Log_Debug_2()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Debug(null);
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Debug(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Log_Error_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Error("message");
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Log_Error_2()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Error(null);
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Log_Fatal_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Fatal("message");
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Fatal(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Log_Fatal_2()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Fatal(null);
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Fatal(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Log_Info_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Info("message");
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Info(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Log_Info_2()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Info(null);
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Info(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Log_Trace_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Trace("message");
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Trace(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Log_Trace_2()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Trace(null);
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Trace(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Log_TraceException_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Exception exception = new Exception("message");
                Log.TraceException("exception", exception);
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Trace(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Log_Warn_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Warn("message");
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Log_Warn_2()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            IAmLogger previous = Log.SetLogger(logger);
            try
            {
                Log.Warn(null);
            }
            finally
            {
                Log.SetLogger(previous);
            }

            mock.Verify(m => m.Warn(It.IsAny<string>()), Times.Never);
        }
    }
}
