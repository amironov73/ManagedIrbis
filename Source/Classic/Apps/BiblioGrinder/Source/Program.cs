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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis.Biblio;
using ManagedIrbis.Client;

#endregion

namespace BiblioGrinder
{
    class Program
    {
        private static string configurationString;
        private static string documentPath;

        private static IrbisProvider provider;
        private static BiblioDocument document;
        private static BiblioContext context;
        private static BiblioProcessor processor;

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("BiblioGrinder <configurationString> <biblioDocument>");
                return;
            }

            configurationString = args[0];
            documentPath = args[1];

            try
            {
                provider = ProviderManager.GetAndConfigureProvider
                    (
                        configurationString
                    );
                document = BiblioDocument.LoadFile(documentPath);
                context = new BiblioContext(document, provider);

                processor = new BiblioProcessor();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }
        }
    }
}
