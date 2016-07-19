/* IrbisConnectionUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AM.Configuration;
using AM.IO;
using AM.Runtime;
using CodeJam;
using JetBrains.Annotations;
using ManagedIrbis.Menus;
using ManagedIrbis.Network;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisConnectionUtility
    {
        #region Constants

        #endregion

        #region Properties

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Read menu from server.
        /// </summary>
        [NotNull]
        public static MenuFile ReadMenu
            (
                [NotNull] this IrbisConnection connection,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification fileSpecification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    connection.Database,
                    fileName
                );
            string text = connection.ReadTextFile(fileSpecification);
            MenuFile result = MenuFile.ParseServerResponse(text);

            return result;
        }

        ///// <summary>
        ///// Стандартные наименования для ключа строки подключения
        ///// к серверу ИРБИС64.
        ///// </summary>
        //public static string[] ListStandardConnectionStrings()
        //{
        //    return new[]
        //    {
        //        "irbis-connection",
        //        "irbis-connection-string",
        //        "irbis64-connection",
        //        "irbis64",
        //        "connection-string"
        //    };
        //}

        ///// <summary>
        ///// Получаем строку подключения в app.settings.
        ///// </summary>
        //public static string GetStandardConnectionString()
        //{
        //    return ConfigurationUtility.FindSetting
        //        (
        //            ListStandardConnectionStrings()
        //        );
        //}

        ///// <summary>
        ///// Получаем уже подключенного клиента.
        ///// </summary>
        ///// <exception cref="IrbisException">
        ///// Если строка подключения в app.settings не найдена.
        ///// </exception>
        //public static IrbisConnection GetClientFromConfig()
        //{
        //    string connectionString = GetStandardConnectionString();
        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        throw new IrbisException
        //            (
        //                "Connection string not specified!"
        //            );
        //    }

        //    IrbisConnection result = new IrbisConnection(connectionString);

        //    return result;
        //}

        #endregion
    }
}
