using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Configuration;

namespace Protector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                string source = args[0];
                if (source == "-")
                {
                    source = Console.ReadLine();
                }
                string encrypted = ConfigurationUtility.Protect(source);
                Console.WriteLine(encrypted);
            }
        }
    }
}
