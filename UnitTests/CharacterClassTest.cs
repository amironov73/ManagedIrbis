using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

namespace UnitTests
{
    [TestClass]
    public class CharacterClassTest
    {
        [TestMethod]
        public void TestLatinAndCyrillic()
        {
            Assert.AreEqual
                (
                    CharacterClass.BasicLatin, 
                    CharacterClassifier.DetectCharacterClasses
                    (
                        "Russian Federation"
                    )
                );
            Assert.AreEqual
                (
                    CharacterClass.Cyrillic,
                    CharacterClassifier.DetectCharacterClasses
                    (
                        "Россия"
                    )
                );
            Assert.AreEqual
                (
                    CharacterClass.BasicLatin|CharacterClass.Cyrillic,
                    CharacterClassifier.DetectCharacterClasses
                    (
                        "Роcсия"
                    )
                );
        }
    }
}
