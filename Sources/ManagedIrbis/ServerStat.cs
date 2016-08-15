/* ServerStat.cs -- IRBIS server stat
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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

        public ClientInfo[] RunningClients { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int TotalCommandCount { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

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
