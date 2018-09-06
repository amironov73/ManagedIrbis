
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

using AM.Windows.Irbis;

namespace UnitTests.Experiments
{
    //[TestClass]
    public class LoginCenterExperiments
    {
        //[TestMethod]
        public void TryLogin1()
        {
            using (IIrbisConnection connection = new IrbisConnection())
            {
                connection.Host = "127.0.0.1";
                if (LoginCenter.Login(connection, null))
                {
                    MessageBox.Show("OK", "Login");
                }
            }
        }

        //[TestMethod]
        public void TryLogin2()
        {
            using (IIrbisConnection connection = new IrbisConnection())
            {
                connection.Host = "127.0.0.1";
                if (LoginCenter.ExtendedLogin(connection, null))
                {
                    MessageBox.Show("OK", "Login");
                }
            }
        }
    }
}
