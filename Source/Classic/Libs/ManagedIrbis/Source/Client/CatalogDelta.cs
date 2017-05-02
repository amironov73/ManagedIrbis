// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CatalogDelta.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("catalogDelta")]
    public sealed class CatalogDelta
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// New records.
        /// </summary>
        [CanBeNull]
        [JsonProperty("new")]
        [XmlArray("new")]
        [XmlArrayItem("mfn")]
        public int[] NewRecords { get; set; }

        /// <summary>
        /// Deleted records.
        /// </summary>
        [CanBeNull]
        [JsonProperty("deleted")]
        [XmlArray("deleted")]
        [XmlArrayItem("mfn")]
        public int[] DeletedRecords { get; set; }

        /// <summary>
        /// Altered records.
        /// </summary>
        [CanBeNull]
        [JsonProperty("altered")]
        [XmlArray("altered")]
        [XmlArrayItem("mfn")]
        public int[] AlteredRecords { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            NewRecords = reader.ReadNullableInt32Array();
            DeletedRecords = reader.ReadNullableInt32Array();
            AlteredRecords = reader.ReadNullableInt32Array();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullableArray(NewRecords)
                .WriteNullableArray(DeletedRecords)
                .WriteNullableArray(AlteredRecords);
        }

        #endregion
    }
}
