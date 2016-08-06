using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class MultiStreamTest
    {
        [TestMethod]
        public void TestMultiStream()
        {
            Stream baseStream = Stream.Null;
            int counter1 = 0;
            int counter2 = 0;
            NotifyStream stream1 = new NotifyStream(baseStream);
            stream1.StreamChanged += (sender, args) => { counter1++; };
            NotifyStream stream2 = new NotifyStream(baseStream);
            stream2.StreamChanged += (sender, args) => { counter2++; };
            MultiStream multiStream = new MultiStream();
            multiStream.Streams.Add(stream1);
            multiStream.Streams.Add(stream2);

            const int expected = 10;
            for (int i = 0; i < expected; i++)
            {
                byte[] bytes = new byte[10];
                multiStream.Write(bytes, 0, bytes.Length);
            }

            Assert.AreEqual(expected, counter1);
            Assert.AreEqual(expected, counter2);
        }
    }
}
