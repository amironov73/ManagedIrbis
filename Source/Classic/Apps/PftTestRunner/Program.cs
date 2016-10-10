/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Testing;

using MoonSharp.Interpreter;

#endregion

namespace PftTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("PftTestRunner <folder>");
                return;
            }

            try
            {
                PftTester tester = new PftTester(args[0]);

                tester.DiscoverTests();

                tester.RunTests();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
