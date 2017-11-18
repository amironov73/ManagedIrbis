using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests.AM.Text
{
    [TestClass]
    public class CharacterClassifierTest
    {
        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses_1()
        {
            string text = string.Empty;
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses(text);
            Assert.AreEqual(CharacterClass.None, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses_2()
        {
            string text = "Hello";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses(text);
            Assert.AreEqual(CharacterClass.BasicLatin, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses_3()
        {
            string text = "Привет";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses(text);
            Assert.AreEqual(CharacterClass.Cyrillic, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses_4()
        {
            string text = "2128506";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses(text);
            Assert.AreEqual(CharacterClass.Digit, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses_5()
        {
            string text = "\r\n";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses(text);
            Assert.AreEqual(CharacterClass.ControlCharacter, classes);
        }

        [TestMethod]
        public void CharacterClassifier_DetectCharacterClasses_6()
        {
            string text = "Hello, Привет, 2128506\r\n";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses(text);
            Assert.AreEqual
                (
                    CharacterClass.BasicLatin
                    | CharacterClass.ControlCharacter
                    | CharacterClass.Cyrillic
                    | CharacterClass.Digit,
                    classes
                );
        }

        [TestMethod]
        public void CharacterClassifier_IsBothCyrillicAndLatin_1()
        {
            string text = "Hello, 2128506\r\n";
            CharacterClass classes
                = CharacterClassifier.DetectCharacterClasses(text);
            Assert.IsFalse(CharacterClassifier.IsBothCyrillicAndLatin(classes));

            text = "Привет, 2128506\r\n";
            classes = CharacterClassifier.DetectCharacterClasses(text);
            Assert.IsFalse(CharacterClassifier.IsBothCyrillicAndLatin(classes));

            text = "Hello, Привет, 2128506\r\n";
            classes = CharacterClassifier.DetectCharacterClasses(text);
            Assert.IsTrue(CharacterClassifier.IsBothCyrillicAndLatin(classes));

            text = "2128506\r\n";
            classes = CharacterClassifier.DetectCharacterClasses(text);
            Assert.IsFalse(CharacterClassifier.IsBothCyrillicAndLatin(classes));
        }
    }
}
