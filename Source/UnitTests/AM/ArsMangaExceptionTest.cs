using System;

using AM;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class ArsMangaExceptionTest
    {
        [TestMethod]
        public void ArsMagnaException_Construction_1()
        {
            ArsMagnaException exception = new ArsMagnaException();
            Assert.IsNotNull(exception.Message);
            Assert.IsNull(exception.InnerException);
            BinaryAttachment[] attachments = exception.ListAttachments();
            Assert.IsNotNull(attachments);
            Assert.AreEqual(0, attachments.Length);
        }

        [TestMethod]
        public void ArsMagnaException_Construction_2()
        {
            const string message = "Message";
            ArsMagnaException exception = new ArsMagnaException(message);
            Assert.AreEqual(message, exception.Message);
            Assert.IsNull(exception.InnerException);
            BinaryAttachment[] attachments = exception.ListAttachments();
            Assert.IsNotNull(attachments);
            Assert.AreEqual(0, attachments.Length);
        }

        [TestMethod]
        public void ArsMagnaException_Construction_3()
        {
            Exception innerException = new Exception();
            const string message = "Message";
            ArsMagnaException exception = new ArsMagnaException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
            BinaryAttachment[] attachments = exception.ListAttachments();
            Assert.IsNotNull(attachments);
            Assert.AreEqual(0, attachments.Length);
        }

        [TestMethod]
        public void ArsMagnaException_Attach_1()
        {
            ArsMagnaException exception = new ArsMagnaException();
            BinaryAttachment attachment = new BinaryAttachment
                (
                    "first",
                    new byte[] { 1, 2, 3 }
                );
            exception.Attach(attachment);
            BinaryAttachment[] attachments = exception.ListAttachments();
            Assert.AreEqual(1, attachments.Length);
            Assert.AreSame(attachment, attachments[0]);
            Assert.AreEqual("first", attachments[0].Name);
            Assert.AreEqual(3, attachments[0].Content.Length);
        }

        [TestMethod]
        public void ArsMagnaException_Attach_2()
        {
            ArsMagnaException exception = new ArsMagnaException();
            BinaryAttachment attachment1 = new BinaryAttachment
                (
                    "first",
                    new byte[] { 1, 2, 3 }
                );
            exception.Attach(attachment1);
            BinaryAttachment attachment2 = new BinaryAttachment
                (
                    "second",
                    new byte[] { 3, 2, 1 }
                );
            exception.Attach(attachment2);
            BinaryAttachment[] attachments = exception.ListAttachments();
            Assert.AreEqual(2, attachments.Length);
            Assert.AreSame(attachment1, attachments[0]);
            Assert.AreSame(attachment2, attachments[1]);
        }

        [TestMethod]
        public void ArsMagnaException_ToString_1()
        {
            ArsMagnaException exception = new ArsMagnaException("Something went wrong");
            Assert.AreEqual("AM.ArsMagnaException: Something went wrong", exception.ToString().DosToUnix());
        }

        [TestMethod]
        public void ArsMagnaException_ToString_2()
        {
            ArsMagnaException exception = new ArsMagnaException("Something went wrong");
            BinaryAttachment attachment1 = new BinaryAttachment
                (
                    "first",
                    new byte[] { 1, 2, 3 }
                );
            exception.Attach(attachment1);
            BinaryAttachment attachment2 = new BinaryAttachment
                (
                    "second",
                    new byte[] { 3, 2, 1 }
                );
            exception.Attach(attachment2);
            Assert.AreEqual("AM.ArsMagnaException: Something went wrong\n\nAttachment: first\n0000: 01 02 03\n\nAttachment: second\n0000: 03 02 01\n", exception.ToString().DosToUnix());
        }

        [TestMethod]
        public void ArsMagnaException_ToString_3()
        {
            ArsMagnaException exception = new ArsMagnaException("Something went wrong");
            byte[] array = new byte[100];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte) i;
            }
            BinaryAttachment attachment = new BinaryAttachment
                (
                    "first",
                    array
                );
            exception.Attach(attachment);
            Assert.AreEqual("AM.ArsMagnaException: Something went wrong\n\nAttachment: first\n0000: 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F\n0010: 10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 1E 1F\n0020: 20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F\n0030: 30 31 32 33 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F\n0040: 40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 4E 4F\n0050: 50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F\n0060: 60 61 62 63\n", exception.ToString().DosToUnix());
        }
    }
}
