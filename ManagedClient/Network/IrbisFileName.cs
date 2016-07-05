/* IrbisFileName.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Runtime;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Network
{
    /// <summary>
    /// Irbis file name.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Path={Path} Database={Database} FileName={FileName}")]
    public sealed class IrbisFileName
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Path.
        /// </summary>
        public IrbisPath Path { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        #endregion

        #region Public methods
        
        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object stat from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
