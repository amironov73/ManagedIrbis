// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DublinField.cs --
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

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Metadata.DublinCore
{
    /// <summary>
    /// Field of the <see cref="DublinRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DublinField
        : IHandmadeSerializable,
          IVerifiable
    {
        #region Properties

        /// <summary>
        /// Tag.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [XmlAttribute("value")]
        [JsonProperty("value")]
        public string Value { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Tag = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Tag)
                .WriteNullable(Value);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<DublinField> verifier
                = new Verifier<DublinField>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Tag, "Tag")
                .NotNullNorEmpty(Value, "Value");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0}={1}",
                    Tag.ToVisibleString(),
                    Value.ToVisibleString()
                );
        }

        #endregion
    }
}
