using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class UserInfoTest
    {
        [NotNull]
        private UserInfo _GetUserInfo()
        {
            return new UserInfo
            {
                Name = "TylerDurden",
                Password = "FightClub",
                Cataloger = "Tyler.ini"
            };
        }

        [TestMethod]
        public void UserInfo_Constructor_1()
        {
            UserInfo user = new UserInfo();
            Assert.IsNull(user.Name);
            Assert.IsNull(user.Password);
            Assert.IsNull(user.Cataloger);
            Assert.IsNull(user.Reader);
            Assert.IsNull(user.Circulation);
            Assert.IsNull(user.Acquisitions);
            Assert.IsNull(user.Provision);
            Assert.IsNull(user.Administrator);
            Assert.IsNull(user.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] UserInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            UserInfo second = bytes.RestoreObjectFromMemory<UserInfo>();
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Password, second.Password);
            Assert.AreEqual(first.Cataloger, second.Cataloger);
            Assert.AreEqual(first.Circulation, second.Circulation);
            Assert.AreEqual(first.Acquisitions, second.Acquisitions);
            Assert.AreEqual(first.Provision, second.Provision);
            Assert.AreEqual(first.Administrator, second.Administrator);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void UserInfo_Serialization_1()
        {
            UserInfo user = new UserInfo();
            _TestSerialization(user);

            user.UserData = "User data";
            _TestSerialization(user);

            user = _GetUserInfo();
            _TestSerialization(user);
        }

        [TestMethod]
        public void UserInfo_Encode_1()
        {
            const string reader = "irbisr.ini";

            UserInfo user = new UserInfo();
            Assert.AreEqual
                (
                    "\n\nC=;R=;B=;M=;K=;A=;",
                    user.Encode().DosToUnix()
                );

            user = _GetUserInfo();
            Assert.AreEqual
                (
                    "TylerDurden\nFightClub\nC=Tyler.ini;R=;B=;M=;K=;A=;",
                    user.Encode().DosToUnix()
                );

            user.Reader = reader;
            Assert.AreEqual
                (
                    "TylerDurden\nFightClub\nC=Tyler.ini;B=;M=;K=;A=;",
                    user.Encode().DosToUnix()
                );
        }

        [TestMethod]
        public void UserInfo_Parse_1()
        {
            const string name = "TylerDurden";
            const string password = "FightClub";
            const string cataloger = "Tyler.ini";

            ResponseBuilder builder = new ResponseBuilder()
                .Append(1).NewLine()
                .Append(9).NewLine()
                .Append(1).NewLine()
                .AppendAnsi(name).NewLine()
                .AppendAnsi(password).NewLine()
                .AppendAnsi(cataloger).NewLine() // Cataloger
                .NewLine() // Reader
                .NewLine() // Circulation
                .NewLine() // Acquisitions
                .NewLine() // Provision
                .NewLine(); // Administrator

            IrbisConnection connection = new IrbisConnection();
            byte[] answer = builder.Encode();
            byte[] request = new byte[0];
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    request,
                    true
                );
            UserInfo[] users = UserInfo.Parse(response);
            Assert.AreEqual(1, users.Length);
            Assert.AreEqual(name, users[0].Name);
            Assert.AreEqual(password, users[0].Password);
            Assert.AreEqual(cataloger, users[0].Cataloger);
            Assert.IsNull(users[0].Reader);
            Assert.IsNull(users[0].Circulation);
            Assert.IsNull(users[0].Acquisitions);
            Assert.IsNull(users[0].Provision);
            Assert.IsNull(users[0].Administrator);
            Assert.IsNull(users[0].UserData);
        }

        [TestMethod]
        public void UserInfo_ToXml_1()
        {
            UserInfo user = new UserInfo();
            Assert.AreEqual("<user />", XmlUtility.SerializeShort(user));

            user = _GetUserInfo();
            Assert.AreEqual("<user><name>TylerDurden</name><password>FightClub</password><cataloguer>Tyler.ini</cataloguer></user>", XmlUtility.SerializeShort(user));
        }

        [TestMethod]
        public void UserInfo_ToJson_1()
        {
            UserInfo user = new UserInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(user));

            user = _GetUserInfo();
            Assert.AreEqual("{'name':'TylerDurden','password':'FightClub','cataloguer':'Tyler.ini'}", JsonUtility.SerializeShort(user));
        }

        [TestMethod]
        public void UserInfo_Verify_1()
        {
            UserInfo user = new UserInfo();
            Assert.IsFalse(user.Verify(false));

            user = _GetUserInfo();
            Assert.IsTrue(user.Verify(false));
        }

        [TestMethod]
        public void UserInfo_ToString_1()
        {
            UserInfo user = new UserInfo();
            Assert.AreEqual
                (
                    "Number: (null), Name: (null), Password: (null), Cataloger: (null), Reader: (null), Circulation: (null), Acquisitions: (null), Provision: (null), Administrator: (null)",
                    user.ToString().DosToUnix()
                );

            user = _GetUserInfo();
            Assert.AreEqual
                (
                    "Number: (null), Name: TylerDurden, Password: FightClub, Cataloger: Tyler.ini, Reader: (null), Circulation: (null), Acquisitions: (null), Provision: (null), Administrator: (null)",
                    user.ToString().DosToUnix()
                );
        }
    }
}