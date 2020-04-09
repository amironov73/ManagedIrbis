using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Security
{
    [TestClass]
    class SecurityUtilityTest
    {
        [TestMethod]
        public void SecurityUtility_Encrypt_1()
        {
            var secretText = "У попа была собака";
            var password = "ИРБИС";
            var encryptedText = SecurityUtility.Encrypt(secretText, password);
            Assert.AreEqual("", encryptedText);
        }
    }
}
