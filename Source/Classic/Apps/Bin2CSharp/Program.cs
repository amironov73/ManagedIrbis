// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM.Text;

#endregion

namespace Bin2CSharp
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Bin2CSharp <file>");
                return 1;
            }

            try
            {
                byte[] array = File.ReadAllBytes(args[0]);
                string text = SourceCodeUtility.ToSourceCode
                    (
                        array
                    );
                Console.WriteLine(text);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 1;
            }

            return 0;
        }
    }
}
