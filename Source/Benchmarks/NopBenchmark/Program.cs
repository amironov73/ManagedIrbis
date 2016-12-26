using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;

namespace NopBenchmark
{
    class Program
    {
        static string ConnectionString { get; set; }

        static int RetryCount;

        static void DoBenchmark()
        {
            using (IrbisConnection connection = new IrbisConnection())
            {
                connection.ParseConnectionString(ConnectionString);
                connection.Connect();
                Console.WriteLine("Connected");

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                for (int i = 0; i < RetryCount; i++)
                {
                    connection.NoOp();
                }

                stopwatch.Stop();
                connection.Dispose();
                Console.WriteLine("Disconnected");

                long ms = stopwatch.ElapsedMilliseconds;
                Console.WriteLine("Elapsed: {0} ms", ms);
                long one = ms/RetryCount;
                Console.WriteLine("One: {0} ms", one);
            }
        }

        static void Main(string[] args)
        {
            RetryCount = 10000;

            if (args.Length == 0)
            {
                return;
            }

            ConnectionString = args[0];
            if (args.Length > 1)
            {
                RetryCount = int.Parse(args[1]);
            }

            Console.WriteLine("Count={0}", RetryCount);

            try
            {
                DoBenchmark();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
