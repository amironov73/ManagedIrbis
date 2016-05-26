/* ManagedClientUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Configuration;
using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ManagedClientUtility
    {
        #region Constants

        #endregion

        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Стандартные наименования для ключа строки подключения
        /// к серверу ИРБИС64.
        /// </summary>
        public static string[] ListStandardConnectionStrings()
        {
            return new[]
            {
                "irbis-connection",
                "irbis-connection-string",
                "irbis64-connection",
                "irbis64",
                "connection-string"
            };
        }

        /// <summary>
        /// Получаем строку подключения в app.settings.
        /// </summary>
        public static string GetStandardConnectionString()
        {
            return ConfigurationUtility.FindSetting
                (
                    ListStandardConnectionStrings()
                );
        }

        /// <summary>
        /// Получаем уже подключенного клиента.
        /// </summary>
        /// <exception cref="ApplicationException">
        /// Если строка подключения в app.settings не найдена.
        /// </exception>
        public static ManagedClient64 GetClientFromConfig()
        {
            string connectionString = GetStandardConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException
                    (
                        "Connection string not specified!"
                    );
            }

            ManagedClient64 result = new ManagedClient64(connectionString);

            return result;
        }

        #endregion
    }
}
