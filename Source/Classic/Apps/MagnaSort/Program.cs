// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

#endregion

namespace MagnaSort
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: MagnaSort <input> <output>");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines
                    (
                        args[0],
                        Encoding.UTF8
                    );

                string[] sorted = NumberText.Sort(lines).ToArray();

                File.WriteAllLines
                    (
                        args[1],
                        sorted,
                        Encoding.UTF8
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
