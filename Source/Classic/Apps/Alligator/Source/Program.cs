using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Text.Output;
using ManagedIrbis;

namespace Alligator
{
    class Program
    {
        private static string connectionString = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=IBIS;";
        private static AbstractOutput output = AbstractOutput.Console;
        private static IrbisConnection connection;
        private static EffectiveEngine engine;

        static void Main()
        {
            try
            {
                using (connection = new IrbisConnection(connectionString))
                {
                    engine = new EffectiveEngine
                    {
                        Connection = connection,
                        Log = output
                    };
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}
