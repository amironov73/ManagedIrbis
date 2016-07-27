using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;

namespace SharpIrbis.Scripts
{
    class HelloWorld
    {
        static void Main()
        {
            try
            {
                using (IrbisConnection connection = new IrbisConnection())
                {
                    connection.ParseConnectionString("host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;");
                    connection.Connect();
                    IrbisVersion version = connection.GetServerVersion();
                    Console.WriteLine(version);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
