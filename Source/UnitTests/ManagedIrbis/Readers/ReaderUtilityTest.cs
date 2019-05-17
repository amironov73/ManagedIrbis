using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Readers;

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class ReaderUtilityTest
    {
        private static void _TestFixName(string name, string expected)
        {
            string actual = ReaderUtility.FixName(name);
            Assert.AreEqual(expected, actual);
        }

        private static void _TestFixTicket(string ticket, string expected)
        {
            string actual = ReaderUtility.FixTicket(ticket);
            Assert.AreEqual(expected, actual);
        }

        private static void _TestFixPhone(string phone, string expected)
        {
            string actual = ReaderUtility.FixPhone(phone);
            Assert.AreEqual(expected, actual);
        }

        private static void _TestFixEmail(string email, string expected)
        {
            string actual = ReaderUtility.FixEmail(email);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReaderUtility_FixEmail_1()
        {
            _TestFixEmail(null, null);
            _TestFixEmail("", "");
            _TestFixEmail(" ", "");
            _TestFixEmail(" nobody @ nowhere.com", "nobody@nowhere.com");
        }

        [TestMethod]
        public void ReaderUtility_FixName_1()
        {
            _TestFixName(null, null);
            _TestFixName("", "");
            _TestFixName(" ", "");
            _TestFixName(" Иванов,  Иван Иванович", "Иванов Иван Иванович");
            _TestFixName(" Dreiser,  Theodore ", "Dreiser Theodore");
        }

        [TestMethod]
        public void ReaderUtility_FixPhone_1()
        {
            _TestFixPhone(null, null);
            _TestFixPhone("", "");
            _TestFixPhone(" ", "");
            _TestFixPhone(" +7(123)456-78 9 ", "8123456789");
        }

        [TestMethod]
        public void ReaderUtility_FixTicket_1()
        {
            _TestFixTicket(null, null);
            _TestFixTicket("", "");
            _TestFixTicket(" ", "");
            _TestFixTicket(" 1 2 3 ", "123");
            _TestFixTicket("C2AB780F", "C2AB780F");
            _TestFixTicket("C2АВ78ОF", "C2AB780F");
        }
    }
}
