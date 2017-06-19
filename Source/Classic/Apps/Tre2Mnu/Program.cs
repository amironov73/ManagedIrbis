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

using ManagedIrbis;
using ManagedIrbis.Menus;

#endregion

namespace Tre2Mnu
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage: Tre2Mnu <input> [output]");

                return 1;
            }

            string inputName = args[0].ThrowIfNull();
            string outputName = args.Length == 1
                ? Path.ChangeExtension(inputName, ".mnu")
                : args[1];

            try
            {
                IrbisTreeFile tree = IrbisTreeFile.ReadLocalFile
                    (
                        inputName,
                        IrbisEncoding.Ansi
                    );
                MenuFile menu = tree.ToMenu();
                File.WriteAllText
                    (
                        outputName,
                        menu.ToText(),
                        IrbisEncoding.Ansi
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return 1;
            }

            return 0;
        }
    }
}
