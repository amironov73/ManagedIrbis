/* RuntimeUtility.cs -- some useful methods for runtime
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using JetBrains.Annotations;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// Some useful methods for runtime.
    /// </summary>
    [PublicAPI]
    public static class RuntimeUtility
    {
        #region Properties

        /// <summary>
        /// Путь к файлам текущей версии Net Framework.
        /// </summary>
        /// <remarks>
        /// Типичная выдача:
        /// C:\WINDOWS\Microsoft.NET\Framework\v2.0.50215
        /// </remarks>
        [NotNull]
        public static string FrameworkLocation
        {
            get
            {
                return Path.GetDirectoryName
                    (
                        typeof(int).Assembly.Location
                    );
            }
        }

        /// <summary>
        /// Имя исполняемого процесса.
        /// </summary>
        [NotNull]
        public static string ExecutableFileName
        {
            get
            {
                Process process = Process.GetCurrentProcess();
                ProcessModule module = process.MainModule;
                return module.FileName;
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}