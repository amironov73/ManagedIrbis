using AM.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.AM.Logging
{
    [TestClass]
    public class TeeLoggerTest
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
        public void TeeLogger_Construction_1()
        {
            TeeLogger tee = new TeeLogger();
            Assert.IsNotNull(tee.Loggers);
            Assert.AreEqual(0, tee.Loggers.Count);
        }

        [TestMethod]
        public void TeeLogger_Debug_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            TeeLogger tee = new TeeLogger
            {
                Loggers = { logger }
            };
            tee.Debug("message");

            mock.Verify(m => m.Debug(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TeeLogger_Error_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            TeeLogger tee = new TeeLogger
            {
                Loggers = { logger }
            };
            tee.Error("message");

            mock.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TeeLogger_Fatal_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            TeeLogger tee = new TeeLogger
            {
                Loggers = { logger }
            };
            tee.Fatal("message");

            mock.Verify(m => m.Fatal(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TeeLogger_Info_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            TeeLogger tee = new TeeLogger
            {
                Loggers = { logger }
            };
            tee.Info("message");

            mock.Verify(m => m.Info(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TeeLogger_Trace_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            TeeLogger tee = new TeeLogger
            {
                Loggers = { logger }
            };
            tee.Trace("message");

            mock.Verify(m => m.Trace(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TeeLogger_Warn_1()
        {
            Mock<IAmLogger> mock = _GetLogger();
            IAmLogger logger = mock.Object;
            TeeLogger tee = new TeeLogger
            {
                Loggers = { logger }
            };
            tee.Warn("message");

            mock.Verify(m => m.Warn(It.IsAny<string>()), Times.Once);
        }
    }
}
