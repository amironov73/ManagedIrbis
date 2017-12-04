using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class VersionStreamTest
    {
        [TestMethod]
        public void VersionStream_ResetVersion_1()
        {
            Stream baseStream = Stream.Null;
            VersionStream versionStream = new VersionStream(baseStream);

            const int expected = 10;
            for (int i = 0; i < expected; i++)
            {
                byte[] bytes = new byte[10];
                versionStream.Write(bytes, 0, bytes.Length);
            }

            Assert.AreEqual(expected, versionStream.Version);

            versionStream.ResetVersion();

            for (int i = 0; i < expected; i++)
            {
                byte[] bytes = new byte[10];
                versionStream.Write(bytes, 0, bytes.Length);
            }

            Assert.AreEqual(expected, versionStream.Version);
        }
    }
}
