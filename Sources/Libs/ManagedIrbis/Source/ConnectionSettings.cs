/* ConnectionSettings.cs -- connection settings
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.IO;
using AM.Parameters;
using AM.Runtime;
using AM.Threading;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Connection settings.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ConnectionSettings
    {
        #region Constants

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultHost = "127.0.0.1";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultDatabase = "IBIS";

        /// <summary>
        /// 
        /// </summary>
        public const IrbisWorkstation DefaultWorkstation
            = IrbisWorkstation.Cataloger;

        /// <summary>
        /// 
        /// </summary>
        public const int DefaultPort = 6666;

        #endregion

        #region Properties

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        /// <value>Адрес сервера в цифровом виде.</value>
        public string Host { get; set; }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        /// <value>Порт сервера (по умолчанию 6666).</value>
        public int Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        /// <value>Имя пользователя.</value>
        //[DefaultValue(DefaultUsername)]
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        /// <value>Пароль пользователя.</value>
        //[DefaultValue(DefaultPassword)]
        public string Password { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <value>Служебное имя базы данных (например, "IBIS").</value>
        [DefaultValue(DefaultDatabase)]
        public string Database { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        /// <value>По умолчанию <see cref="IrbisWorkstation.Cataloger"/>.
        /// </value>
        [DefaultValue(DefaultWorkstation)]
        public IrbisWorkstation Workstation { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectionSettings()
        {
            Host = DefaultHost;
            Port = DefaultPort;
            Database = DefaultDatabase;
            Username = null;
            Password = null;
            Workstation = DefaultWorkstation;
        }

        #endregion

        #region Private members

        private static void _Add
            (
                List<Parameter> list,
                string name,
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Parameter parameter = new Parameter(name, value);
                list.Add(parameter);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone.
        /// </summary>
        [NotNull]
        public ConnectionSettings Clone()
        {
            return (ConnectionSettings) MemberwiseClone();
        }

        /// <summary>
        /// Encode parameters to text representation.
        /// </summary>
        [NotNull]
        public string Encode()
        {
            List<Parameter> parameters = new List<Parameter>();

            _Add(parameters, "host", Host);
            _Add
                (
                    parameters,
                    "port",
                    Port == 0
                        ? null
                        : Port.ToInvariantString()
                );
            _Add(parameters, "database", Database);
            _Add(parameters, "username", Username);
            _Add(parameters, "password", Password);
            _Add
                (
                    parameters,
                    "workstation",
                    Workstation == 0 
                        ? null
                        : new string( (char)(byte)Workstation, 1)
                );

            string result = ParameterUtility.Encode
                (
                    parameters.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Парсинг строки подключения.
        /// </summary>
        public void ParseConnectionString
            (
                [NotNull] string connectionString
            )
        {
            Code.NotNull(connectionString, "connectionString");

            Parameter[] parameters = ParameterUtility.ParseString
                (
                    connectionString
                );

            foreach (Parameter parameter in parameters)
            {
                string name = parameter.Name
                    .ThrowIfNull("parameter.Name")
                    .ToLower();
                string value = parameter.Value
                    .ThrowIfNull();

                switch (name)
                {
                    case "host":
                    case "server":
                    case "address":
                        Host = value;
                        break;

                    case "port":
                        Port = int.Parse(value);
                        break;

                    case "user":
                    case "username":
                    case "name":
                    case "login":
                        Username = value;
                        break;

                    case "pwd":
                    case "password":
                        Password = value;
                        break;

                    case "db":
                    case "catalog":
                    case "database":
                        Database = value;
                        break;

                    case "arm":
                    case "workstation":
                        Workstation = (IrbisWorkstation)(byte)(value[0]);
                        break;

                    //case "data":
                    //    UserData = value;
                    //    break;

                    //case "debug":
                    //    StartDebug(value);
                    //    break;

                    //case "etr":
                    //case "stage":
                    //    StageOfWork = value;
                    //    break;

                    default:
                        throw new ArgumentException("connectionString");
                }
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
