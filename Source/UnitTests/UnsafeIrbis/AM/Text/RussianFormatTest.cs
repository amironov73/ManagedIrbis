using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Text;

namespace UnitTests.UnsafeAM.Text
{
    [TestClass]
    public class RussianFormatTest
    {
        [TestMethod]
        public void RussianFormat_Format_1()
        {
            Assert.AreEqual
                (
                    "10000",
                    RussianFormat.Format(10000)
                );

            Assert.AreEqual
                (
                    "10000,5",
                    RussianFormat.Format(10000.5)
                );

            Assert.AreEqual
                (
                    "10000,5",
                    RussianFormat.Format(10000.5m)
                );
        }

        [TestMethod]
        public void RussianFormat_Format_2()
        {
            Assert.AreEqual
                (
                    "Length=1000",
                    RussianFormat.Format("Length={0}", 1000)
                );

            Assert.AreEqual
                (
                    "Length=1000,5",
                    RussianFormat.Format("Length={0}", 1000.5)
                );

            Assert.AreEqual
                (
                    "Length=1000,5",
                    RussianFormat.Format
                    (
                        "Length={0}",
                        (object)1000.5
                    )
                );
        }

        [TestMethod]
        public void RussianFormat_Format_3()
        {
            Assert.AreEqual
                (
                    "Length=1000, Height=2000",
                    RussianFormat.Format
                    (
                        "Length={0}, Height={1}",
                        1000,
                        2000
                    )
                );

            Assert.AreEqual
                (
                    "Length=1000,5, Height=2000,5",
                    RussianFormat.Format
                    (
                        "Length={0}, Height={1}",
                        1000.5,
                        2000.5
                    )
                );
        }

        [TestMethod]
        public void RussianFormat_Format_4()
        {
            Assert.AreEqual
                (
                    "Length=1000, Height=2000, Width=3000",
                    RussianFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}",
                        1000,
                        2000,
                        3000
                    )
                );

            Assert.AreEqual
                (
                    "Length=1000,5, Height=2000,5, Width=3000,5",
                    RussianFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}",
                        1000.5,
                        2000.5,
                        3000.5
                    )
                );
        }

        [TestMethod]
        public void RussianFormat_Format_5()
        {
            Assert.AreEqual
                (
                    "Length=1000, Height=2000, Width=3000, Weight=4000",
                    RussianFormat.Format
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
                    "Length=1000,5, Height=2000,5, Width=3000,5, Weight=4000,5",
                    RussianFormat.Format
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
