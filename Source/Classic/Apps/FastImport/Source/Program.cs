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
using System.Globalization;
using System.IO;
using System.Linq;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Readers;

#endregion

// ReSharper disable LocalizableElement
// ReSharper disable InconsistentNaming
// ReSharper disable InlineOutVariableDeclaration

namespace FastImport
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need two arguments");
                return;
            }

            string inputFileName = args[0]
                .ThrowIfNull("inputFIleName");
            string outputFileName = args[1]
                .ThrowIfNull("outputFileName");
            string mstFileName = Path.ChangeExtension(outputFileName, ".mst")
                .ThrowIfNull("mstFileName");
            string xrfFileName = Path.ChangeExtension(outputFileName, ".xrf")
                .ThrowIfNull("xrfFileName");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                DirectUtility.CreateMasterFile64(outputFileName);

                int mfn = 0;
                using (Stream mstFile = new FileStream(mstFileName,
                    FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                using (Stream xrfFile = File.OpenWrite(xrfFileName))
                using (StreamReader stringReader = File.OpenText(inputFileName))
                {
                    mstFile.Seek(0, SeekOrigin.Current);
                    long position = mstFile.Position;
                    MarcRecord marcRecord;
                    while ((marcRecord = PlainText.ReadRecord(stringReader)) != null)
                    {
                        if (mfn % 100 == 0)
                        {
                            Console.Write(" {0} ", mfn);
                        }

                        mfn++;
                        position = mstFile.Position;
                        MstRecord64 mstRecord = MstRecord64.EncodeRecord(marcRecord);
                        mstRecord.Prepare();
                        mstRecord.Write(mstFile);

                        xrfFile.WriteInt64Network(position);
                        xrfFile.WriteInt32Network((int)RecordStatus.NonActualized);
                    }

                    if (mfn != 0)
                    {
                        // Update the control record
                        long nextPosition = mstFile.Length;
                        if (nextPosition % 1 != 0)
                        {
                            nextPosition++;
                        }

                        mstFile.Seek(0, SeekOrigin.Begin);
                        MstControlRecord64 control = MstControlRecord64.Read(mstFile);
                        control.Blocked = 0;
                        control.NextMfn = mfn;
                        control.NextPosition = nextPosition;

                        mstFile.Seek(0, SeekOrigin.Begin);
                        control.Write(mstFile);
                    }
                }

                Console.WriteLine(" Total: {0}", mfn);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            Console.WriteLine("Elapsed: {0}", elapsed.ToAutoString());
        }
    }
}
