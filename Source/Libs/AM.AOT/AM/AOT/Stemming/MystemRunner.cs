/* MystemRunner.cs -- format error codes.
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

using Newtonsoft.Json;

#endregion

namespace AM.AOT.Stemming
{
    /// <summary>
    /// Запускает mystem.exe и разбирает результаты.
    /// </summary>
    public sealed class MystemRunner
    {
        /// <summary>
        /// Путь до mystem.exe, включая exe-файл.
        /// По умолчанию "mystem.exe".
        /// </summary>
        public string MystemPath { get; set; }

        /// <summary>
        /// Опции, передаваемые mystem.exe.
        /// По умолчанию -i -g -d.
        /// </summary>
        public string MystemOptions { get; set; }

        /// <summary>
        /// Кодировка, используемая при передаче.
        /// По умолчанию CP866, т. к. применяется
        /// перенаправление ввода-вывода.
        /// </summary>
        public Encoding TransferEncoding { get; set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public MystemRunner()
        {
            MystemPath = "mystem.exe";
            MystemOptions = "-i -g -d";
            TransferEncoding = Encoding.GetEncoding("cp866");
        }

        /// <summary>
        /// Разбор результатов.
        /// </summary>
        /// <param name="reader">Поток с результатами.</param>
        public MystemResult[] DecodeResults
            (
                StreamReader reader
            )
        {
            List<MystemResult> result = new List<MystemResult>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                MystemResult[] one = JsonConvert
                    .DeserializeObject<MystemResult[]>(line);
                result.AddRange(one);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Запускает анализ текста и выдаёт результаты.
        /// </summary>
        /// <param name="text">Текст для анализа.</param>
        public MystemResult[] Run
            (
                string text
            )
        {
            MystemResult[] result;
            StringBuilder commandLine = new StringBuilder();
            commandLine.Append(MystemOptions);
            commandLine.Append(" -e " +
                               TransferEncoding.HeaderName);
            commandLine.Append(" --format json");

            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    MystemPath,
                    commandLine.ToString()
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = TransferEncoding
            };
            using (Process process = new Process()
            {
                StartInfo = startInfo
            })
            {
                process.Start();
                process.StandardInput.Write(text);
                process.StandardInput.Close();
                process.WaitForExit();
                result = DecodeResults(process.StandardOutput);
            }

            return result;
        }
    }
}
