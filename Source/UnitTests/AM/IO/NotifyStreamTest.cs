using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class NotifyStreamTest
    {
        [TestMethod]
        public void NotifyStream_StreamChanged()
        {
            Stream baseStream = Stream.Null;
            NotifyStream notifyStream = new NotifyStream(baseStream);
            int counter = 0;
            notifyStream.StreamChanged += (sender, args) =>
            {
                counter++;
            };

            const int expected = 10;
            for (int i = 0; i < expected; i++)
            {
                byte[] bytes = new byte[10];
                notifyStream.Write(bytes, 0, bytes.Length);
            }

            Assert.AreEqual(expected, counter);
        }
    }
}
