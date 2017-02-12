// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ControlCenter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Text.Output;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestfulIrbis;
using RestfulIrbis.OsmiCards;
using CM = System.Configuration.ConfigurationManager;

#endregion

namespace OsmiRegistration
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class ControlCenter
    {
        #region Properties

        /// <summary>
        /// OSMICards client.
        /// </summary>
        public static OsmiCardsClient Client { get; set; }

        /// <summary>
        /// Connection string for IRBIS-server.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Debug output.
        /// </summary>
        public static AbstractOutput Output { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get <see cref="IrbisConnection"/>.
        /// </summary>
        [NotNull]
        public static IrbisConnection GetIrbisConnection()
        {
            IrbisConnection result 
                = new IrbisConnection(ConnectionString);

            return result;
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public static void Initialize()
        {
            ConnectionString = CM.AppSettings["connectionString"];

            string baseUri = CM.AppSettings["baseUri"];
            string apiId = CM.AppSettings["apiID"];
            string apiKey = CM.AppSettings["apiKey"];
            Client = new OsmiCardsClient
                (
                    baseUri,
                    apiId,
                    apiKey
                );
        }

        public static bool Ping()
        {
            using (IrbisConnection connection = GetIrbisConnection())
            {
                WriteLine("Pinging IRBIS-server");
                connection.NoOp();
            }

            WriteLine("Pinging OSMI-server");
            Client.Ping();

            return true;
        }

        /// <summary>
        /// Shutdown.
        /// </summary>
        public static void Shutdown()
        {

        }

        /// <summary>
        /// Write debug line.
        /// </summary>
        public static void WriteLine
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Code.NotNull(format, "format");

            if (!ReferenceEquals(Output, null))
            {
                Output.WriteLine(format, arguments);
            }
        }

        #endregion
    }
}
