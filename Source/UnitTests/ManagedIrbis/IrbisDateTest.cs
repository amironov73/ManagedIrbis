using System;

using AM.Runtime;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class IrbisDateTest
    {
        [TestMethod]
        public void IrbisDate_Constructor_1()
        {
            DateTime today = DateTime.Today;
            IrbisDate date = new IrbisDate();
            Assert.AreEqual
                (
                    today.ToString("yyyyMMdd"),
                    date.Text
                );
        }

        [TestMethod]
        public void IrbisDate_Constructor_2()
        {
            IrbisDate date1 = new IrbisDate("20170225");
            DateTime date2 = date1.Date;
            Assert.AreEqual(2017, date2.Year);
            Assert.AreEqual(2, date2.Month);
            Assert.AreEqual(25, date2.Day);
        }

        [TestMethod]
        public void IrbisDate_Constructor_3()
        {
            string date1 = "20160101";
            IrbisDate date2 = date1;
            string date3 = date2;
            Assert.AreEqual(date1, date3);
        }

        [TestMethod]
        public void IrbisDate_Constructor_4()
        {
            DateTime date1 = new DateTime(2017, 2, 25);
            IrbisDate date2 = new IrbisDate(date1);
            Assert.AreEqual("20170225", date2.Text);
        }

        private void _TestSerialization
            (
                IrbisDate first
            )
        {
            byte[] bytes = first.SaveToMemory();

            IrbisDate second = bytes
                .RestoreObjectFromMemory<IrbisDate>();

            Assert.AreEqual(first.Text, second.Text);
            Assert.AreEqual(first.Date, second.Date);
        }

        [TestMethod]
        public void IrbisDate_Serialization_1()
        {
            IrbisDate date = "20121212";
            _TestSerialization(date);
        }

        [TestMethod]
        public void IrbisDate_ConvertStringToDate_1()
        {
            DateTime date = IrbisDate.ConvertStringToDate(null);
            Assert.AreEqual(DateTime.MinValue, date);
        }

        [TestMethod]
        public void IrbisDate_ConvertStringToDate_2()
        {
            DateTime actual = IrbisDate.ConvertStringToDate("20170225");
            DateTime expected = new DateTime(2017, 2, 25);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisDate_ConvertStringToDate_3()
        {
            DateTime actual = IrbisDate.ConvertStringToDate("201702251015");
            DateTime expected = new DateTime(2017, 2, 25);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisDate_ConvertTimeToString_1()
        {
            string expected = "010203";
            string actual = IrbisDate.ConvertTimeToString(new TimeSpan(1, 2, 3));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisDate_Operator_DateTime_1()
        {
            IrbisDate date1 = "20170225";
            DateTime date2 = date1;
            Assert.AreEqual(2017, date2.Year);
            Assert.AreEqual(2, date2.Month);
            Assert.AreEqual(25, date2.Day);
        }

        [TestMethod]
        public void IrbisDate_Operator_IrbisDate_1()
        {
            DateTime date1 = new DateTime(2017, 2, 25);
            IrbisDate date2 = date1;
            Assert.AreEqual(date1, date2.Date);
        }

        [TestMethod]
        public void IrbisDate_Operator_DateTime_2()
        {
            DateTime expected = new DateTime(2017, 2, 25);
            IrbisDate date = expected;
            DateTime actual = date;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IrbisDate_ToString_1()
        {
            const string expected = "20170225";
            IrbisDate date = expected;
            Assert.AreEqual(expected, date.ToString());
        }

        [TestMethod]
        public void IrbisDate_TodayText_1()
        {
            string expected = DateTime.Today.ToString("yyyyMMdd");
            Assert.AreEqual(expected, IrbisDate.TodayText);
        }

        [TestMethod]
        public void IrbisDate_ConvertStringToTime_1()
        {
            TimeSpan time = IrbisDate.ConvertStringToTime(null);
            Assert.AreEqual(0, time.Ticks);
        }

        [TestMethod]
        public void IrbisDate_ConvertStringToTime_2()
        {
            TimeSpan time = IrbisDate.ConvertStringToTime("1");
            Assert.AreEqual(0, time.Ticks);
        }

        [TestMethod]
        public void IrbisDate_ConvertStringToTime_3()
        {
            TimeSpan time = IrbisDate.ConvertStringToTime("1011");
            Assert.AreEqual(10, time.Hours);
            Assert.AreEqual(11, time.Minutes);
            Assert.AreEqual(0, time.Seconds);
        }

        [TestMethod]
        public void IrbisDate_ConvertStringToTime_4()
        {
            TimeSpan time = IrbisDate.ConvertStringToTime("101112");
            Assert.AreEqual(10, time.Hours);
            Assert.AreEqual(11, time.Minutes);
            Assert.AreEqual(12, time.Seconds);
        }
    }
}
