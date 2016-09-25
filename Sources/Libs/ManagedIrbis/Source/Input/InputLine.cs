/* InputLine.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Input
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [XmlRoot("line")]
    [MoonSharpUserData]
    public sealed class InputLine
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Hint.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("hint")]
        [JsonProperty("hint")]
        public string Hint { get; set; }

        /// <summary>
        /// Mandatory?
        /// </summary>
        [XmlAttribute("mandatory")]
        public bool Mandatory { get; set; }

        /// <summary>
        /// Tag (e. g. "200^a").
        /// </summary>
        [CanBeNull]
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Hint = reader.ReadNullableString();
            Mandatory = reader.ReadBoolean();
            Tag = reader.ReadNullableString();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Hint)
                .Write(Mandatory);
            writer
                .WriteNullable(Tag);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<InputLine> verifier = new Verifier<InputLine>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Tag, "Tag");

            return verifier.Result;
        }

        #endregion
    }
}
