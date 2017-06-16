// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Text.Output;

#endregion

namespace Sigler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("SIGLER <sigla.txt> <connectionString>");
                return;
            }

            string fileName = args[0];
            string connectionString = args[1];

            AbstractOutput output = AbstractOutput.Console;
            using (SiglaStamper stamper = new SiglaStamper
                (
                    connectionString,
                    output
                ))
            {
                stamper.ProcessFile(fileName);
            }
        }
    }
}
