using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Text.Output;

using ManagedIrbis;
using ManagedIrbis.Client;

using CM=System.Configuration.ConfigurationManager;

namespace ClientEngineRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            try
            {
                string clientIniName = args[0];
                using (ClientEngine engine = new ClientEngine(clientIniName))
                {
                    engine.Output = AbstractOutput.Console;
                    engine.Connect(null);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
