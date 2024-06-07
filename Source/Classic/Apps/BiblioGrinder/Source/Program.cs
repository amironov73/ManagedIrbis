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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Text.Output;

using IrbisInterop;

using ManagedIrbis;
using ManagedIrbis.Biblio;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure.Commands;

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

        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("BiblioGrinder <configurationString> <biblioDocument.json>");
                return 1;
            }

            log = new TeeOutput(AbstractOutput.Console);

            configurationString = args[0];
            documentPath = args[1];

            ReadRecordCommand.ThrowOnVerify = false;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                AbstractOutput logFile = new FileOutput("grinder.log");
                log.Output.Add(logFile);

                NativeIrbisProvider.Register();

                using (provider = ProviderManager
                    .GetAndConfigureProvider(configurationString))
                {
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

                    // TODO implement properly
                    outputText = outputText
                        .Replace ("№", "\\'B9")
                        .Replace ("\\u8211?", "{\\endash}")
                        .Replace ("\\u8470?", "\\'B9");

                    outputText = Regex.Replace
                        (
                            outputText,
                            @"\s(-|–|\\u8211.)\sIS[BS]N\s[0-9XxХх-]*\.?",
                            string.Empty
                        );
                    outputText = Regex.Replace
                        (
                            outputText,
                            @"([А-Я]\.)\s([А-Я])",
                            "$1\\~$2"
                        );

                    outputText = Regex.Replace
                        (
                            outputText,
                            "\\\"([\\w])",
                            "«$1"
                        );
                    outputText = Regex.Replace
                        (
                            outputText,
                            "([\\w\\.])\\\"",
                            "$1»"
                        );

                    outputText = Regex.Replace
                        (
                            outputText,
                            "\\\"(\\.\\.\\.|\\(|\\[)",
                            "«$1"
                        );
                    outputText = Regex.Replace
                        (
                            outputText,
                            "(\\.\\.\\.|\\!|\\?|\\)|\\])\\\"",
                            "$1»"
                        );

                    outputText = outputText
                        .Replace(" - ", " \\u8211? ")
                        .Replace("}- ", "}\\u8211? ")
                        .Replace(" -}", " \\u8211?}")
                        .Replace(" -\\", " \\u8211?\\")
                        .Replace("\\u8470? ", "\u8470?\\~")
                        .Replace("...", "\\'85")
                        .Replace("С. ", "С.\\~")
                        ;

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
                    log.WriteLine
                        (
                            "Finished at: {0}",
                            DateTime.Now
                        );
                }
            }
            catch (Exception exception)
            {
                log.WriteLine
                    (
                        "Exception: {0}",
                        exception
                    );

                return 1;
            }

            return 0;
        }
    }
}
