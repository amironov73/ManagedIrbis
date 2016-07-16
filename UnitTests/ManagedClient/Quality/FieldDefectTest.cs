using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Quality;

namespace UnitTests.ManagedClient.Quality
{
    [TestClass]
    public class FieldDefectTest
    {
        private void _TestSerialization
            (
                FieldDefect first
            )
        {
            byte[] bytes = first.SaveToMemory();

            FieldDefect second = bytes
                .RestoreObjectFromMemory<FieldDefect>();

            Assert.AreEqual(first.Damage, second.Damage);
            Assert.AreEqual(first.Field, second.Field);
            Assert.AreEqual(first.FieldRepeat, second.FieldRepeat);
            Assert.AreEqual(first.Message, second.Message);
            Assert.AreEqual(first.Subfield, second.Subfield);
            Assert.AreEqual(first.Value, second.Value);
        }

        [TestMethod]
        public void TestFieldDefectSerialization()
        {
            FieldDefect fieldDefect = new FieldDefect();
            _TestSerialization(fieldDefect);

            fieldDefect.Field = "200";
            fieldDefect.Message = "Отсутствует поле 200";
            fieldDefect.Damage = 100;
            _TestSerialization(fieldDefect);
        }
    }
}
