using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class RetryManagerTest
    {
        [TestMethod]
        public void TestRetryManagerAction1()
        {
            int counter = 0;
            Action action = () =>
            {
                counter++;
                if (counter < 2)
                {
                    throw new Exception();
                }
            };

            RetryManager retryManager = new RetryManager(3);
            retryManager.Try(action);

            Assert.IsTrue(counter == 2);
        }

        [TestMethod]
        public void TestRetryManagerAction2()
        {
            int counter = 0;
            Action<int> action = argument =>
            {
                counter++;
                if (counter < 2)
                {
                    throw new Exception();
                }
            };

            RetryManager retryManager = new RetryManager(3);
            retryManager.Try(action, 1);

            Assert.IsTrue(counter == 2);
        }

        [TestMethod]
        public void TestRetryManagerAction3()
        {
            int counter = 0;
            Action<int> action = argument =>
            {
                counter++;
                if (counter < 2)
                {
                    throw new Exception();
                }
            };
            Func<Exception, bool> resolver =
                ex => ex.GetType() == typeof (Exception);

            RetryManager retryManager = new RetryManager(3, resolver);
            retryManager.Try(action, 1);

            Assert.IsTrue(counter == 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArsMagnaException))]
        public void TestRetryManagerAction4()
        {
            Action<int> action = argument =>
            {
                throw new ArgumentException();
            };
            Func<Exception, bool> resolver =
                ex => ex.GetType() == typeof(Exception);

            RetryManager retryManager = new RetryManager(3, resolver);
            retryManager.Try(action, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArsMagnaException))]
        public void TestRetryManagerAction5()
        {
            Action<int> action = argument =>
            {
                throw new ArgumentException();
            };

            RetryManager retryManager = new RetryManager(3);
            retryManager.Try(action, 1);
        }

        [TestMethod]
        public void TestRetryManagerFunc1()
        {
            const int expected = 2;
            int counter = 0;
            Func<int> func = () =>
            {
                counter++;
                if (counter < expected)
                {
                    throw new Exception();
                }

                return counter;
            };

            RetryManager retryManager = new RetryManager(3);
            int actual = retryManager.Try(func);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRetryManagerFunc2()
        {
            const int expected = 2;
            int counter = 0;
            Func<int,int> func = argument =>
            {
                counter++;
                if (counter < expected)
                {
                    throw new Exception();
                }

                return counter;
            };

            RetryManager retryManager = new RetryManager(3);
            int actual = retryManager.Try(func, 1);
            Assert.AreEqual(expected, actual);
        }
    }
}
