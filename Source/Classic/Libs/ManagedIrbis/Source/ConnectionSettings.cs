// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConnectionSettings.cs -- connection settings
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Parameters;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Connection settings.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("connection")]
    public sealed class ConnectionSettings
        : IHandmadeSerializable,
        IVerifiable
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
        /// IP-address of the server.
        /// </summary>
        /// <remarks>Default value is "127.0.0.1".</remarks>
        [CanBeNull]
        [XmlAttribute("host")]
        [JsonProperty("host")]
        public string Host { get; set; }

        /// <summary>
        /// IP-port of the server.
        /// </summary>
        /// <remarks>Default value is 6666.</remarks>
        [XmlAttribute("port")]
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>
        /// User logon name.
        /// </summary>
        /// <remarks>Default value is <c>null</c>,
        /// so connection can't be made.</remarks>
        [CanBeNull]
        [XmlAttribute("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// User logon password.
        /// </summary>
        /// <remarks>Default value is <c>null</c>,
        /// so connection can't be made.</remarks>
        [CanBeNull]
        [XmlAttribute("password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Database name to connect.
        /// </summary>
        /// <remarks>Default value is "IBIS".
        /// Database with such a name can be
        /// non-existent.
        /// </remarks>
        [CanBeNull]
        [DefaultValue(DefaultDatabase)]
        [XmlAttribute("database")]
        [JsonProperty("database")]
        public string Database { get; set; }

        /// <summary>
        /// Workstation application kind.
        /// </summary>
        /// <remarks>Default value is
        /// <see cref="IrbisWorkstation.Cataloger"/>.
        /// </remarks>
        [DefaultValue(DefaultWorkstation)]
        [XmlAttribute("workstation")]
        [JsonProperty("workstation")]
        public IrbisWorkstation Workstation { get; set; }

        /// <summary>
        /// Turn on network logging.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("log")]
        [JsonProperty("log")]
        public string NetworkLogging { get; set; }

        /// <summary>
        /// Type name for ClientSocket.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("socket")]
        [JsonProperty("socket")]
        public string SocketTypeName { get; set; }

        /// <summary>
        /// Type name for CommandFactory.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("factory")]
        [JsonProperty("factory")]
        public string FactoryTypeName { get; set; }

        /// <summary>
        /// Type name for execution engine.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("engine")]
        [JsonProperty("engine")]
        public string EngineTypeName { get; set; }

        /// <summary>
        /// Retry count.
        /// </summary>
        [XmlAttribute("retry")]
        [JsonProperty("retry")]
        public int RetryCount { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("userdata")]
        [JsonProperty("userdata")]
        public string UserData { get; set; }

        /// <summary>
        /// Saved "connected" state.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool Connected { get; set; }

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

        private static string _Select
            (
                string first,
                string second
            )
        {
            return string.IsNullOrEmpty(first)
                ? second
                : first;
        }

        private static int _Select
            (
                int first,
                int second
            )
        {
            return first != 0
                ? first
                : second;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply settings to the <see cref="IrbisConnection"/>.
        /// </summary>
        public void ApplyToConnection
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            connection.Host = _Select(Host, connection.Host);
            connection.Port = _Select(Port, connection.Port);
            connection.Username = _Select(Username, connection.Username);
            connection.Password = _Select(Password, connection.Password);
            connection.Database = _Select(Database, connection.Database);
            connection.Workstation = (IrbisWorkstation) _Select
                (
                    (int) Workstation,
                    (int) connection.Workstation
                );

            if (!string.IsNullOrEmpty(EngineTypeName))
            {
                connection.SetEngine(EngineTypeName);
            }

            if (!string.IsNullOrEmpty(SocketTypeName))
            {
                connection.SetSocket(SocketTypeName);
            }

            if (!string.IsNullOrEmpty(NetworkLogging))
            {
                connection.SetNetworkLogging(NetworkLogging);
            }

            if (!string.IsNullOrEmpty(FactoryTypeName))
            {
                connection.SetCommandFactory(FactoryTypeName);
            }

            if (RetryCount != 0)
            {
                connection.SetRetry(RetryCount, null);
            }

            if (!string.IsNullOrEmpty(UserData))
            {
                connection.UserData = UserData;
            }

            connection._connected = Connected;
        }

        /// <summary>
        /// Clone.
        /// </summary>
        [NotNull]
        public ConnectionSettings Clone()
        {
            return (ConnectionSettings) MemberwiseClone();
        }

        /// <summary>
        /// Decrypt the connection settings.
        /// </summary>
        public static ConnectionSettings Decrypt
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            byte[] bytes = Convert.FromBase64String(text);

#if !SILVERLIGHT

            bytes = CompressionUtility.Decompress(bytes);

#endif

            ConnectionSettings result
                = bytes.RestoreObjectFromMemory<ConnectionSettings>();

            return result;
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
            _Add(parameters, "socket", SocketTypeName);
            _Add(parameters, "engine", EngineTypeName);
            _Add(parameters, "factory", FactoryTypeName);
            _Add(parameters, "log", NetworkLogging);
            _Add
                (
                    parameters,
                    "retry",
                    RetryCount == 0
                    ? null
                    : RetryCount.ToInvariantString()
                );
            _Add(parameters, "data", UserData);

            string result = ParameterUtility.Encode
                (
                    parameters.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Encrypt the connection settings.
        /// </summary>
        [NotNull]
        public string Encrypt()
        {
            byte[] bytes = this.SaveToMemory();

#if !SILVERLIGHT

            bytes = CompressionUtility.Compress(bytes);

#endif

            string result = Convert.ToBase64String(bytes);

            return result;
        }

        /// <summary>
        /// Construct <see cref="ConnectionSettings"/>
        /// from <see cref="IrbisConnection"/>.
        /// </summary>
        [NotNull]
        public static ConnectionSettings FromConnection
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            ConnectionSettings result = new ConnectionSettings
            {
                Host = connection.Host,
                Port = connection.Port,
                Username = connection.Username,
                Password = connection.Password,
                Database = connection.Database,
                Workstation = connection.Workstation,
                UserData = connection.UserData as string,
                Connected = connection.Connected
            };

#if !WINMOBILE && !PocketPC && !WIN81 && !PORTABLE

            LoggingClientSocket loggingSocket
                = connection.Socket as LoggingClientSocket;
            if (loggingSocket != null)
            {
                result.NetworkLogging = loggingSocket.DebugPath;
            }

#endif

            RetryClientSocket retrySocket
                = connection.Socket as RetryClientSocket;
            if (retrySocket != null)
            {
                result.RetryCount = retrySocket.RetryManager.RetryCount;
            }

#if !WINMOBILE && !PocketPC && !WIN81 && !PORTABLE

            if ((connection.Socket.GetType() != typeof(SimpleClientSocket))
                && (retrySocket == null)
                && (loggingSocket == null)
            )
            {
                result.SocketTypeName = connection.Socket
                    .GetType().AssemblyQualifiedName;
            }

#else

            if ((connection.Socket.GetType() != typeof (SimpleClientSocket))
                && (retrySocket == null))
            {
                result.SocketTypeName = connection.Socket
                    .GetType().AssemblyQualifiedName;
            }

#endif

            if (connection.CommandFactory.GetType() !=
                typeof(CommandFactory))
            {
                result.FactoryTypeName = connection.CommandFactory
                    .GetType().AssemblyQualifiedName;
            }

            if (connection.Executive.GetType() !=
                typeof(StandardEngine))
            {
                result.EngineTypeName = connection.Executive
                    .GetType().AssemblyQualifiedName;
            }

            return result;
        }

        /// <summary>
        /// Get missing elements from the settings.
        /// </summary>
        public ConnectionElement GetMissingElements()
        {
            ConnectionElement result = ConnectionElement.None;

            if (string.IsNullOrEmpty(Host))
            {
                result |= ConnectionElement.Host;
            }
            if (Port == 0)
            {
                result |= ConnectionElement.Port;
            }
            if (string.IsNullOrEmpty(Username))
            {
                result |= ConnectionElement.Username;
            }
            if (string.IsNullOrEmpty(Password))
            {
                result |= ConnectionElement.Password;
            }
            if (Workstation == IrbisWorkstation.None)
            {
                result |= ConnectionElement.Workstation;
            }

            return result;
        }

        /// <summary>
        /// Парсинг строки подключения.
        /// </summary>
        public ConnectionSettings ParseConnectionString
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
                    .ThrowIfNull("parameter.Value");

                switch (name)
                {
                    case "provider":
                        // Nothing to do
                        break;

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
                        Workstation 
                            = (IrbisWorkstation)(byte)value[0];
                        break;

                    case "socket":
                        SocketTypeName = value;
                        break;

                    case "engine":
                        EngineTypeName = value;
                        break;

                    case "factory":
                        FactoryTypeName = value;
                        break;

                    case "log":
                        NetworkLogging = value;
                        break;

                    case "retry":
                        RetryCount = int.Parse(value);
                        break;

                    case "userdata":
                    case "data":
                        UserData = value;
                        break;

                    default:
                        string message = string.Format
                            (
                                "Unknown parameter: {0}",
                                name
                            );
                        throw new ArgumentException(message);
                }
            }

            return this;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Host = reader.ReadNullableString();
            Port = reader.ReadPackedInt32();
            Username = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Workstation = (IrbisWorkstation) reader.ReadPackedInt32();
            NetworkLogging = reader.ReadNullableString();
            SocketTypeName = reader.ReadNullableString();
            FactoryTypeName = reader.ReadNullableString();
            EngineTypeName = reader.ReadNullableString();
            RetryCount = reader.ReadPackedInt32();
            UserData = reader.ReadNullableString();
            Connected = reader.ReadBoolean();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Host)
                .WritePackedInt32(Port)
                .WriteNullable(Username)
                .WriteNullable(Password)
                .WriteNullable(Database)
                .WritePackedInt32((int) Workstation)
                .WriteNullable(NetworkLogging)
                .WriteNullable(SocketTypeName)
                .WriteNullable(FactoryTypeName)
                .WriteNullable(EngineTypeName)
                .WritePackedInt32(RetryCount)
                .WriteNullable(UserData)
                .Write(Connected);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ConnectionSettings> verifier
                = new Verifier<ConnectionSettings>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Host, "Host")
                .Assert(Port > 0 && Port < 0x10000, "Port")
                .NotNullNorEmpty(Username, "Username")
                .NotNullNorEmpty(Password, "Password")
                .Assert
                    (
                        Workstation != IrbisWorkstation.None,
                        "Workstation"
                    );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return Encode();
        }

        #endregion
    }
}
