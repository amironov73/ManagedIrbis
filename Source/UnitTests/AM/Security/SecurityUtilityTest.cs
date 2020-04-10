using AM.Security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Security
{
    [TestClass]
    public class SecurityUtilityTest
    {
        [TestMethod]
        public void SecurityUtility_Encrypt_1()
        {
            var secretText = "У попа была собака";
            var password = "ИРБИС";
            var encryptedText = SecurityUtility.Encrypt(secretText, password);
            Assert.AreEqual("LuRnYV8942IU3RGJzWlbJMIl2wSTxGG5Tr7RbY/g6uM4pJWY4lajEdudNOzDVhhF", encryptedText);
        }

        [TestMethod]
        public void SecurityUtility_Decrypt_1()
        {
            var encryptedText = "LuRnYV8942IU3RGJzWlbJMIl2wSTxGG5Tr7RbY/g6uM4pJWY4lajEdudNOzDVhhF";
            var password = "ИРБИС";
            var decryptedText = SecurityUtility.Decrypt(encryptedText, password);
            Assert.AreEqual("У попа была собака", decryptedText);
        }
    }
}
