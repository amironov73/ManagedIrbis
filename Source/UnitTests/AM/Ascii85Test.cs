using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class Ascii85Test
    {
        private void _TestEncode
            (
                [NotNull] byte[] decoded,
                [NotNull] string encoded
            )
        {
            Assert.AreEqual(encoded, Ascii85.Encode(decoded));
            CollectionAssert.AreEqual(decoded, Ascii85.Decode(encoded));
        }

        private void _TestEncode
            (
                [NotNull] string decoded,
                [NotNull] string encoded
            )
        {
            Encoding encoding = Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(decoded);
            Assert.AreEqual(encoded, Ascii85.Encode(bytes));
            bytes = Ascii85.Decode(encoded);
            string s = encoding.GetString(bytes);
            Assert.AreEqual(decoded, s);
        }

        [TestMethod]
        public void Ascii85_Encode_1()
        {
            _TestEncode(new byte[0], string.Empty);
            _TestEncode(new byte[] { 0 }, "!!");
            _TestEncode(new byte[] { 0, 0, 0, 0 }, "z");
            _TestEncode(new byte[] { 1 }, "!<");
            _TestEncode(new byte[] { 1, 1 }, "!<E");
            _TestEncode(new byte[] { 1, 1, 1 }, "!<E3");
            _TestEncode(new byte[] { 1, 1, 1, 1 }, "!<E3%");
            _TestEncode(new byte[] { 10 }, "$3");
            _TestEncode(new byte[] { 10, 10 }, "$46");
            _TestEncode(new byte[] { 10, 10, 10 }, "$47+");
            _TestEncode(new byte[] { 10, 10, 10, 10 }, "$47+I");
            _TestEncode(new byte[] { 100 }, "A,");
            _TestEncode(new byte[] { 100, 100 }, "A7P");
            _TestEncode(new byte[] { 100, 100, 100 }, "A7T3");
            _TestEncode(new byte[] { 100, 100, 100, 100 }, "A7T4]");
            _TestEncode(new byte[] { 255, 255, 255, 255 }, "s8W-!");
        }

        [TestMethod]
        public void Ascii85_Encode_2()
        {
            string decoded = "Man is distinguished, not only by his reason, but by this singular passion from other animals, which is a lust of the mind, that by a perseverance of delight in the continued and indefatigable generation of knowledge, exceeds the short vehemence of any carnal pleasure.";
            string encoded = @"9jqo^BlbD-BleB1DJ+*+F(f,q/0JhKF<GL>Cj@.4Gp$d7F!,L7@<6@)/0JDEF<G%<+EV:2F!,O<DJ+*.@<*K0@<6L(Df-\0Ec5e;DffZ(EZee.Bl.9pF""AGXBPCsi+DGm>@3BB/F*&OCAfu2/AKYi(DIb:@FD,*)+C]U=@3BN#EcYf8ATD3s@q?d$AftVqCh[NqF<G:8+EV:.+Cf>-FD5W8ARlolDIal(DId<j@<?3r@:F%a+D58'ATD4$Bl@l3De:,-DJs`8ARoFb/0JMK@qB4^F!,R<AKZ&-DfTqBG%G>uD.RTpAKYo'+CT/5+Cei#DII?(E,9)oF*2M7/c";
            _TestEncode(decoded, encoded);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Ascii85_Decode_1()
        {
            Ascii85.Decode("abzde");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Ascii85_Decode_2()
        {
            Ascii85.Decode("rstuv");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Ascii85_Decode_3()
        {
            Ascii85.Decode("a");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Ascii85_Decode_4()
        {
            Ascii85.Decode("uuuu");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Ascii85_Decode_5()
        {
            Ascii85.Decode("s8W-<");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Ascii85_Decode_6()
        {
            Ascii85.Decode("s8W");
        }
    }
}
