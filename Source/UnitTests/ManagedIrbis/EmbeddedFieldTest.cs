using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class EmbeddedFieldTest
    {
        private RecordField _GetField_1()
        {
            RecordField result = new RecordField(461)
                .AddSubField('1', "2001#")
                .AddSubField('a', "Златая цепь")
                .AddSubField('e', "Записки. Повести. Рассказы")
                .AddSubField('f', "Бондарин С. А.")
                .AddSubField('v', "С. 76-132");

            return result;
        }

        private RecordField _GetField_2()
        {
            RecordField result = new RecordField(461)
                .AddSubField('1', "2001#")
                .AddSubField('a', "Златая цепь")
                .AddSubField('e', "Записки. Повести. Рассказы")
                .AddSubField('f', "Бондарин С. А.")
                .AddSubField('v', "С. 76-132")
                .AddSubField('1', "2001#")
                .AddSubField('a', "Руслан и Людмила")
                .AddSubField('f', "Пушкин А. С.");

            return result;
        }

        private RecordField _GetField_3()
        {
            RecordField result = new RecordField(461)
                .AddSubField('1', null)
                .AddSubField('a', "Златая цепь")
                .AddSubField('e', "Записки. Повести. Рассказы")
                .AddSubField('f', "Бондарин С. А.")
                .AddSubField('v', "С. 76-132");

            return result;
        }

        private RecordField _GetField_4()
        {
            RecordField result = new RecordField(461)
                .AddSubField('1', "0011#")
                .AddSubField('a', "Златая цепь")
                .AddSubField('e', "Записки. Повести. Рассказы")
                .AddSubField('f', "Бондарин С. А.")
                .AddSubField('v', "С. 76-132");

            return result;
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_1()
        {
            RecordField field = _GetField_1();
            RecordField[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(1, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_2()
        {
            RecordField field = _GetField_2();
            RecordField[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(2, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_3()
        {
            RecordField field = _GetField_4();
            RecordField[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(1, embeddedFields.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void EmbeddedField_GetEmbeddedFields_Exception_1()
        {
            RecordField field = _GetField_3();
            RecordField[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(2, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedField_1()
        {
            RecordField field = _GetField_1();
            
            RecordField[] embeddedFields = field.GetEmbeddedField(200);
            Assert.AreEqual(1, embeddedFields.Length);

            embeddedFields = field.GetEmbeddedField(210);
            Assert.AreEqual(0, embeddedFields.Length);
        }
    }
}
