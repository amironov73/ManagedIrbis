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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Text.Output;

using IrbisInterop;

using ManagedIrbis;
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                AbstractOutput logFile = new FileOutput("grinder.log");
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

                string outputText = processor.Output.Text;
                File.WriteAllText
                    (
                        "output.rtf",
                        outputText,
                        IrbisEncoding.Ansi
                    );

                stopwatch.Stop();
                TimeSpan elapsed = stopwatch.Elapsed;
                log.WriteLine
                    (
                        "Elapsed: {0}",
                        elapsed.ToAutoString()
                    );
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
