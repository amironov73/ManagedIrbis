// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

#endregion

// ReSharper disable LocalizableElement

namespace MstDoctor
{
    class Program
    {
        static void WriteRecord
            (
                [NotNull] StreamWriter writer,
                [NotNull] MarcRecord record
            )
        {
            CultureInfo culture = CultureInfo.InvariantCulture;

            foreach (RecordField field in record.Fields)
            {
                writer.Write(string.Format(culture, "#{0}: ", field.Tag));
                if (!string.IsNullOrEmpty(field.Value))
                {
                    writer.Write(field.Value);
                }

                foreach (SubField subField in field.SubFields)
                {
                    writer.Write("^{0}{1}", subField.Code, subField.Value);
                }

                writer.WriteLine();
            }

            writer.WriteLine("*****");
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need two arguments");
                return;
            }

            string inputFileName = args[0];
            string outputFileName = args[1];

            try
            {
                FoundRecord[] found = MstRecover64
                    .FindRecords(inputFileName)
                    .Where(record => (record.Flags & (int)RecordStatus.LogicallyDeleted) == 0)
                    .ToArray();
                Console.WriteLine("Records found: {0}", found.Length);

                using (StreamWriter output = File.CreateText(outputFileName))
                using (MstFile64 input = new MstFile64(inputFileName,
                    DirectAccessMode.ReadOnly))
                {
                    foreach (FoundRecord info in found)
                    {
                        Console.WriteLine
                            (
                                "MFN {0}: ver {1} status {2}",
                                info.Mfn,
                                info.Version,
                                info.Flags
                            );

                        MstRecord64 mstRecord = input.ReadRecord(info.Position);
                        MarcRecord marcRecord = mstRecord.DecodeRecord();

                        WriteRecord(output, marcRecord);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
