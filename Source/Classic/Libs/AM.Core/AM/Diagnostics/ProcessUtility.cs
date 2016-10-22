/* ProcessUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !NETCORE

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ProcessUtility
    {
        #region Private members


        #endregion

        #region Public methods

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="standardInput">Если не null, задает
        /// содержимое стандартного ввода.</param>
        /// <returns></returns>
        public static string RunAndReadStandardOutput
            (
                ProcessStartInfo info,
                string standardInput
            )
        {
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            if (standardInput != null)
            {
                info.RedirectStandardInput = true;
            }
            using (Process process = new Process())
            {
                process.StartInfo = info;
                if (standardInput != null)
                {
                    process.StandardInput.Write(standardInput);
                }
                process.Start();
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string RunAndReadStandardOutput
            (
                ProcessStartInfo info
            )
        {
            return RunAndReadStandardOutput
                (
                    info,
                    null
                );
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="standardInput"></param>
        /// <returns></returns>
        public static string RunAndReadStandardOutput
            (
                string fileName,
                string arguments,
                string standardInput
            )
        {
            ProcessStartInfo psi = new ProcessStartInfo
                (
                    fileName,
                    arguments
                  );
            return RunAndReadStandardOutput(psi, standardInput);
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string RunAndReadStandardOutput
            (
            string fileName,
            string arguments)
        {
            return RunAndReadStandardOutput
                (
                    fileName,
                    arguments,
                    null
                  );
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="standardInput">Если не null, задает
        /// содержимое стандартного ввода.</param>
        /// <returns></returns>
        public static string[] RunAndReadStandardOutputAndError
            (
                ProcessStartInfo info,
                string standardInput
            )
        {
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            if (standardInput != null)
            {
                info.RedirectStandardInput = true;
            }
            using (Process process = new Process())
            {
                process.StartInfo = info;
                if (standardInput != null)
                {
                    process.StandardInput.Write(standardInput);
                }
                process.Start();
                process.WaitForExit();
                string stdout = process.StandardOutput.ReadToEnd();
                string stderr = process.StandardError.ReadToEnd();
                return new string[]
                    {
                        stdout, stderr
                    };
            }
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string[] RunAndReadStandardOutputAndError
            (
                ProcessStartInfo info
            )
        {
            return RunAndReadStandardOutputAndError(info, null);
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="standardInfo"></param>
        /// <returns></returns>
        public static string[] RunAndReadStandardOutputAndError
            (
                string fileName,
                string arguments,
                string standardInfo
            )
        {
            ProcessStartInfo psi = new ProcessStartInfo
                (fileName,
                  arguments);
            return RunAndReadStandardOutputAndError(psi, standardInfo);
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string[] RunAndReadStandardOutputAndError
            (
                string fileName,
                string arguments
            )
        {
            return RunAndReadStandardOutputAndError(fileName, arguments, null);
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="milliseconds">Сколько ожидать.
        /// Неположительные значения = бесконечно.</param>
        /// <returns>Код, вовращенный процессом.
        /// Если с процессом не сложилось, возвращается -1.</returns>
        public static int RunAndWait
            (
                ProcessStartInfo info,
                int milliseconds
            )
        {
            using (Process process = new Process())
            {
                process.StartInfo = info;
                process.Start();
                if (milliseconds > 0)
                {
                    if (!process.WaitForExit(milliseconds))
                    {
                        return -1;
                    }
                }
                else
                {
                    process.WaitForExit();
                }
                return process.ExitCode;
            }
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="milliseconds">Сколько ожидать.
        /// Неположительные значения = бесконечно.</param>
        /// <returns>Код, возвращенный процессом. 
        /// Если с процессом как-то не сложилось, возвращается -1.</returns>
        public static int RunAndWait
            (
                string fileName,
                string arguments,
                int milliseconds
            )
        {
            ProcessStartInfo psi = new ProcessStartInfo
                (
                    fileName,
                    arguments
                )
            {
                UseShellExecute = false
            };
            return RunAndWait(psi, milliseconds);
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static int RunAndWait
            (
                string fileName,
                string arguments
            )
        {
            return RunAndWait(fileName, arguments, -1);
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int RunAndWait
            (
                string fileName
            )
        {
            ProcessStartInfo psi = new ProcessStartInfo(fileName);
            return RunAndWait(psi, -1);
        }

        #endregion
    }
}

#endif