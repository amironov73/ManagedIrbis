// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Parameter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Parameters
{
    /// <summary>
    /// Parameter.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("parameter")]
    [DebuggerDisplay("{Name}={Value}")]
    public sealed class Parameter
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        /// <remarks>Can be <c>string.Empty</c>.</remarks>
        [CanBeNull]
        [XmlAttribute("value")]
        [JsonProperty("value")]
        public string Value { get; set; }

        #endregion

        #region Construciton

        /// <summary>
        /// Constructor.
        /// </summary>
        public Parameter()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Parameter
            (
                [NotNull] string name,
                [CanBeNull] string value
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
            Value = value ?? string.Empty;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore the object state from the specified stream.
        /// </summary>
        /// <param name="reader"></param>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <summary>
        /// Save the object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Name)
                .WriteNullable(Value);
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify the object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<Parameter> verifier = new Verifier<Parameter>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Name, "Name")
                .NotNull(Value, "Value");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "{0}={1}",
                    Name,
                    Value
                );
        }

        #endregion
    }
}
