// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerContext.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ServerContext
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Address.
        /// </summary>
        [CanBeNull]
        public string Address { get; set; }

        /// <summary>
        /// Command count.
        /// </summary>
        public int CommandCount { get; set; }

        /// <summary>
        /// Connection established time.
        /// </summary>
        public DateTime Connected { get; set; }

        /// <summary>
        /// Context identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Last activity time.
        /// </summary>
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [CanBeNull]
        public string Password { get; set; }

        /// <summary>
        /// Server.
        /// </summary>
        [NotNull]
        public IrbisSocketServer Server { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        [CanBeNull]
        public string Username { get; set; }

        /// <summary>
        /// Workstation.
        /// </summary>
        public IrbisWorkstation Workstation { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Address = reader.ReadNullableString();
            CommandCount = reader.ReadPackedInt32();
            Connected = reader.ReadDateTime();
            Id = reader.ReadNullableString();
            LastActivity = reader.ReadDateTime();
            Password = reader.ReadNullableString();
            Username = reader.ReadNullableString();
            Workstation = (IrbisWorkstation) reader.ReadPackedInt32();
        }

        /// <inheritdoc/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Address)
                .WritePackedInt32(CommandCount)
                .Write(Connected)
                .WriteNullable(Id)
                .Write(LastActivity)
                .WriteNullable(Password)
                .WriteNullable(Username)
                .WritePackedInt32((int)Workstation);
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Id.ToVisibleString();
        }

        #endregion
    }
}
