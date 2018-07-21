using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedIrbis;

namespace UnitTests.NetworkDebugging
{
    public class NetworkTestBase
    {
        public IrbisConnection GetConnection()
        {
            string connectionString = "host=127.0.0.1;port=6666;user=1;password=1;db=ISTU;arm=A;";

            return new IrbisConnection(connectionString);
        }
    }
}
