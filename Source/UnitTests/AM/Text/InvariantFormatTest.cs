using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class InvariantFormatTest
    {
        [TestMethod]
        public void InvariantFormat_Format_1()
        {
            Assert.AreEqual
                (
                    "10000",
                    InvariantFormat.Format(10000)
                );

            Assert.AreEqual
                (
                    "10000.5",
                    InvariantFormat.Format(10000.5)
                );

            Assert.AreEqual
                (
                    "10000.5",
                    InvariantFormat.Format(10000.5m)
                );
        }

        [TestMethod]
        public void InvariantFormat_Format_2()
        {
            Assert.AreEqual
                (
                    "Length=1000",
                    InvariantFormat.Format("Length={0}", 1000)
                );

            Assert.AreEqual
                (
                    "Length=1000.5",
                    InvariantFormat.Format("Length={0}", 1000.5)
                );

            Assert.AreEqual
                (
                    "Length=1000.5",
                    InvariantFormat.Format
                    (
                        "Length={0}",
                        (object)1000.5
                    )
                );
        }

        [TestMethod]
        public void InvariantFormat_Format_3()
        {
            Assert.AreEqual
                (
                    "Length=1000, Height=2000",
                    InvariantFormat.Format
                    (
                        "Length={0}, Height={1}",
                        1000,
                        2000
                    )
                );

            Assert.AreEqual
                (
                    "Length=1000.5, Height=2000.5",
                    InvariantFormat.Format
                    (
                        "Length={0}, Height={1}",
                        1000.5,
                        2000.5
                    )
                );
        }

        [TestMethod]
        public void InvariantFormat_Format_4()
        {
            Assert.AreEqual
                (
                    "Length=1000, Height=2000, Width=3000",
                    InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}",
                        1000,
                        2000,
                        3000
                    )
                );

            Assert.AreEqual
                (
                    "Length=1000.5, Height=2000.5, Width=3000.5",
                    InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}",
                        1000.5,
                        2000.5,
                        3000.5
                    )
                );
        }

        [TestMethod]
        public void InvariantFormat_Format_5()
        {
            Assert.AreEqual
                (
                    "Length=1000, Height=2000, Width=3000, Weight=4000",
                    InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}, Weight={3}",
                        1000,
                        2000,
                        3000,
                        4000
                    )
                );

            Assert.AreEqual
                (
                    "Length=1000.5, Height=2000.5, Width=3000.5, Weight=4000.5",
                    InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}, Weight={3}",
                        1000.5,
                        2000.5,
                        3000.5,
                        4000.5
                    )
                );
        }
    }
}
