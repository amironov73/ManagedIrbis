// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerStat.cs -- IRBIS server stat
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// IRBIS server stat
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ServerStat
    {
        #region Properties

        /// <summary>
        /// List of running client.
        /// </summary>
        public ClientInfo[] RunningClients { get; set; }

        /// <summary>
        /// Unknown field.
        /// </summary>
        public int Unknown1 { get; set; }

        /// <summary>
        /// Unknown field.
        /// </summary>
        public int Unknown2 { get; set; }

        /// <summary>
        /// Total commands executed since server start.
        /// </summary>
        public int TotalCommandCount { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        public static ServerStat Parse
            (
                [NotNull] ServerResponse response
            )
        {
            ServerStat result = new ServerStat
            {
                TotalCommandCount = response.RequireInt32(),
                Unknown1 = response.RequireInt32(),
                Unknown2 = response.RequireInt32()
            };

            List<ClientInfo> clients = new List<ClientInfo>();

            while (true)
            {
                string number = response.GetAnsiString();
                string ipAddress = response.GetAnsiString();
                if (string.IsNullOrEmpty(number)
                    || string.IsNullOrEmpty(ipAddress))
                {
                    break;
                }

                ClientInfo client = new ClientInfo
                {
                    Number = number,
                    IPAddress = ipAddress,
                    Port = response.RequireAnsiString(),
                    Name = response.RequireAnsiString(),
                    ID = response.RequireAnsiString(),
                    Workstation = response.RequireAnsiString(),
                    Registered = response.RequireAnsiString(),
                    Acknowledged = response.RequireAnsiString(),
                    LastCommand = response.RequireAnsiString(),
                    CommandNumber = response.RequireAnsiString()
                };
                clients.Add(client);
            }
            result.RunningClients = clients.ToArray();

            return result;
        }


        #endregion

        #region Object members
        
        #endregion
    }
}
