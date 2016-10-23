using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class CharacterClassifierTest
    {
        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses1()
        {
            string text = string.Empty;
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.AreEqual(CharacterClass.None, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses2()
        {
            string text = "Hello";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.AreEqual(CharacterClass.BasicLatin, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses3()
        {
            string text = "Привет";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.AreEqual(CharacterClass.Cyrillic, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses4()
        {
            string text = "2128506";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.AreEqual(CharacterClass.Digit, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses5()
        {
            string text = "\r\n";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.AreEqual(CharacterClass.ControlCharacter, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses6()
        {
            string text = "Hello, Привет, 2128506\r\n";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.AreEqual
                (
                    CharacterClass.BasicLatin
                    |CharacterClass.ControlCharacter
                    |CharacterClass.Cyrillic
                    |CharacterClass.Digit,
                    classes
                );
        }

        [TestMethod]
        public void CharacterClassifier_IsBothCyrillicAndLatin()
        {
            string text = "Hello, 2128506\r\n";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.IsFalse(CharacterClassifier.IsBothCyrillicAndLatin(classes));

            text = "Привет, 2128506\r\n";
            classes = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.IsFalse(CharacterClassifier.IsBothCyrillicAndLatin(classes));

            text = "Hello, Привет, 2128506\r\n";
            classes = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.IsTrue(CharacterClassifier.IsBothCyrillicAndLatin(classes));

            text = "2128506\r\n";
            classes = CharacterClassifier.DetectCharacterClasses
                (
                    text
                );
            Assert.IsFalse(CharacterClassifier.IsBothCyrillicAndLatin(classes));
        }
    }
}
