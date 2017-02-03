using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;

using RestfulIrbis;

namespace RestfulClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IrbisRestClient client = new IrbisRestClient("http://localhost:1234");
                DatabaseInfo[] databases = client.ListDatabases();
                foreach (DatabaseInfo database in databases)
                {
                    Console.WriteLine(database);
                }

                int[] mfns = {1, 2, 3};
                string[] formatted = client.FormatRecords(mfns, "@brief");
                foreach (string s in formatted)
                {
                    Console.WriteLine(s);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
