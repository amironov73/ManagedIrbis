﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

using AM.Text.Output;

using IrbisInterop;

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
        private static TeeOutput log;

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("BiblioGrinder <configurationString> <biblioDocument.json>");
                return;
            }

            log = new TeeOutput(AbstractOutput.Console);

            configurationString = args[0];
            documentPath = args[1];

            try
            {
                AbstractOutput logFile = new FileOutput("grinder.txt");
                log.Output.Add(logFile);

                NativeIrbisProvider.Register();

                provider = ProviderManager.GetAndConfigureProvider
                    (
                        configurationString
                    );
                log.WriteLine
                    (
                        "Connected to database {0}, max MFN={1}",
                        provider.Database,
                        provider.GetMaxMfn()
                    );
                document = BiblioDocument.LoadFile(documentPath);
                context = new BiblioContext(document, provider, log);

                processor = new BiblioProcessor();
                processor.Initialize(context);
                processor.BuildDocument(context);
            }
            catch (Exception exception)
            {
                log.WriteLine
                    (
                        "Exception: {0}",
                        exception
                    );

            }
        }
    }
}
