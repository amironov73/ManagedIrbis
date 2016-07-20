using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Threading.Tasks;

namespace UnitTests.AM.Threading.Tasks
{
    [TestClass]
    public class ThreadProcessorTest
    {
        [TestMethod]
        public void TestThreadProcessor()
        {
            List<string> lines = new List<string>();

            TaskProcessor processor = new TaskProcessor(1);
            for (int i = 0; i < 10; i++)
            {
                int number = i;

                Action action = () =>
                {
                    string item = "Hello " + number;
                    lock (lines)
                    {
                        lines.Add(item);
                    }
                };

                processor.Enqueue(action);
            }
            processor.Complete();

            processor.WaitForCompletion();

            Assert.AreEqual(10, lines.Count);
        }
    }
}
