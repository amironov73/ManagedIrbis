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

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using static System.Console;

#endregion

// ReSharper disable LocalizableElement

namespace MstStatConsole
{
    class Program
    {
        private static void ProcessingHandler(object sender, EventArgs args)
        {
            MstStat64 stat = (MstStat64) sender;

            if ((stat.ProcessedRecords % 1000) == 0)
            {
                Write($"\r{stat.ProcessedRecords} of {stat.TotalRecordCount}");
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: MstStatConsole <mstFile>");
                return;
            }

            try
            {
                MstStat64 stat = new MstStat64();
                stat.Processing += ProcessingHandler;
                stat.ProcessFile(args[0]);

                WriteLine("\rALL DONE                 ");
                WriteLine($"Total file length          = {stat.TotalFileLength,13:N0}");
                WriteLine($"Total records              = {stat.TotalRecordCount,13:N0}");
                if (stat.TotalRecordCount == 0 || stat.TotalFileLength == 0)
                {
                    return;
                }

                WriteLine($"Physically deleted records = {stat.PhysicallyDeletedCount,13:N0}");
                WriteLine($"Logically deleted records  = {stat.LogicallyDeletedCount,13:N0}");
                WriteLine($"Logically deleted length   = {stat.LogicallyDeletedSize,13:N0}");
                WriteLine($"Previous version count     = {stat.PreviousVersionCount,13:N0}");
                WriteLine($"Previous version length    = {stat.PreviousVersionSize,13:N0}");
                WriteLine($"Useful records length      = {stat.UsefulSize,13:N0}");
                WriteLine("---------------");
                WriteLine($"Average record length = {stat.UsefulSize / (stat.TotalRecordCount - stat.PhysicallyDeletedCount - stat.LogicallyDeletedCount)}");
                WriteLine($"Logically deleted     = {stat.LogicallyDeletedSize * 100 / stat.TotalFileLength}%");
                WriteLine($"Physically deleted    = {stat.PhysicallyDeletedCount * 100 / stat.TotalRecordCount}%");
                WriteLine($"Previous versions     = {stat.PreviousVersionSize * 100 / stat.TotalFileLength}%");
            }
            catch (Exception exception)
            {
                WriteLine(exception);
            }
        }
    }
}
