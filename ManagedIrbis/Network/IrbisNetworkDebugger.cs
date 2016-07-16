/* IrbisNetworkDebugger.cs -- debugger for network protocol
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// Debugger for network protocol.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisNetworkDebugger
    {
        #region Properties

        /// <summary>
        /// Active?
        /// </summary>
        public static bool Active { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Break point.
        /// </summary>
        public static void Break
            (
                [CanBeNull] byte[] sendPacket,
                [CanBeNull] byte[] receivedPacket
            )
        {
            if (!Active)
            {
                return;
            }

            Debugger.Break();
        }

        /// <summary>
        /// Write line.
        /// </summary>
        public static void Log
            (
                string text
            )
        {
            Debugger.Log
                (
                    0,
                    "IRBIS",
                    text
                );
        }

        /// <summary>
        /// Write line.
        /// </summary>
        public static void Log
            (
                string[] lines
            )
        {
            foreach (string line in lines)
            {
                Log(line);
            }
        }

        #endregion
    }
}
