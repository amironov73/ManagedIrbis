// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BanMaster.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Server.Sockets;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BanMaster
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BanMaster()
        {
            _dictionary = new Dictionary<string, object>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, object> _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Ban the address.
        /// </summary>
        public void BanAddress
            (
                [CanBeNull] string address
            )
        {
            if (!string.IsNullOrEmpty(address))
            {
                lock (_dictionary)
                {
                    _dictionary.Remove(address);
                }
            }
        }

        /// <summary>
        /// Whether the address is banned?
        /// </summary>
        public bool IsBanned
            (
                [CanBeNull] string address
            )
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }

            bool result;
            lock (_dictionary)
            {
                result = _dictionary.ContainsKey(address);
            }

            return result;
        }

        /// <summary>
        /// Whether the address is banned.
        /// </summary>
        public bool IsBanned
            (
                [NotNull] IrbisServerSocket socket
            )
        {
            Code.NotNull(socket, "socket");

            string address = socket.GetRemoteAddress();

            return IsBanned(address);
        }

        /// <summary>
        /// Load the ban list from the file.
        /// </summary>
        public void LoadBanList
            (
                [NotNull] string fileName
            )
        {
            Code.FileExists(fileName, "fileName");

            string[] lines = FileUtility.ReadAllLines(fileName, IrbisEncoding.Utf8);
            lock (_dictionary)
            {
                foreach (string line in lines)
                {
                    _dictionary[line] = null;
                }
            }
        }

        /// <summary>
        /// Save the ban list to the file.
        /// </summary>
        public void SaveBanList
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string[] lines;
            lock (_dictionary)
            {
                lines = _dictionary.Keys.ToArray();
            }
            FileUtility.WriteAllLines(fileName, lines, IrbisEncoding.Utf8);
        }

        /// <summary>
        /// Unban the address.
        /// </summary>
        public void UnbanAddress
            (
                [CanBeNull] string address
            )
        {
            if (!string.IsNullOrEmpty(address))
            {
                lock (_dictionary)
                {
                    _dictionary[address] = null;
                }
            }
        }

        /// <summary>
        /// Unban all.
        /// </summary>
        public void UnbanAll()
        {
            lock (_dictionary)
            {
                _dictionary.Clear();
            }
        }

        #endregion
    }
}
