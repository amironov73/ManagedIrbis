// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

/*
 * Простая утилита, превращающая текстовые дампы, подобные такому
 *
 * 0000: 47 0A 43 0A 47 0A 39 32 31 30 33 35 31 0A 37 0A
 * 0010: 62 30 39 73 68 30 35 6F 6B 0A 72 74 61 0A 0A 0A
 * 0020: 0A 49 42 49 53 0A 40 0A 31 0A 38 36 33
 *
 * в соответствующие двоичные файлы
 *
 */

namespace Dump2Bin
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Dump2Bin <input=file> <output-file>");

                return;
            }

            try
            {
                using (StreamReader input = new StreamReader(args[0], Encoding.ASCII))
                using (FileStream output = new FileStream(args[1], FileMode.Create, FileAccess.Write))
                {
                    string line;
                    while ((line = input.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        line = line.Split(':')[1].Trim();
                        string[] parts = line.Split(' ');
                        foreach (string part in parts)
                        {
                            int byteValue = Int32.Parse(part, NumberStyles.HexNumber, CultureInfo.InstalledUICulture);
                            output.WriteByte(unchecked((byte)byteValue));
                        }
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
