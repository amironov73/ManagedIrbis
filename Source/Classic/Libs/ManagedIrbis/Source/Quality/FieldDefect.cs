// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldDefect.cs -- detected defect of the field/subfield
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
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

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Detected defect of the field/subfield.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("defect")]
    [DebuggerDisplay("Field={Field} Value={Value} Message={Message}")]
    public sealed class FieldDefect
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Поле.
        /// </summary>
        [XmlAttribute("field")]
        [JsonProperty("field")]

        public int Field { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [XmlAttribute("repeat")]
        [JsonProperty("repeat")]
        public int Repeat { get; set; }

        /// <summary>
        /// Подполе (если есть).
        /// </summary>
        [CanBeNull]
        [XmlAttribute("subfield")]
        [JsonProperty("subfield")]
        public string Subfield { get; set; }

        /// <summary>
        /// Значение поля/подполя.
        /// </summary>
        [CanBeNull]
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("message")]
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Урон от дефекта.
        /// </summary>
        [XmlAttribute("damage")]
        [JsonProperty("damage")]
        public int Damage { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize <see cref="Damage"/> field?
        /// </summary>
        public bool ShouldSerializeDamage()
        {
            return Damage != 0;
        }

        /// <summary>
        /// Should serialize <see cref="Repeat"/> field?
        /// </summary>
        public bool ShouldSerializeRepeat()
        {
            return Repeat != 0;
        }

        /// <summary>
        /// Should serialize <see cref="Subfield"/> field?
        /// </summary>
        public bool ShouldSerializeSubfield()
        {
            return !string.IsNullOrEmpty(Subfield);
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Field = reader.ReadPackedInt32();
            Repeat = reader.ReadPackedInt32();
            Subfield = reader.ReadNullableString();
            Value = reader.ReadNullableString();
            Message = reader.ReadNullableString();
            Damage = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Field)
                .WritePackedInt32(Repeat)
                .WriteNullable(Subfield)
                .WriteNullable(Value)
                .WriteNullable(Message)
                .WritePackedInt32(Damage);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FieldDefect> verifier
                = new Verifier<FieldDefect>(this, throwOnError);

            verifier
                .Positive(Field, "Field")
                .Assert(Repeat >= 0, "Repeat")
                .NotNullNorEmpty(Message, "Message")
                .Assert(Damage >= 0, "Damage");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Field: {0}, Value: {1}, Message: {2}",
                    Field,
                    Value.ToVisibleString(),
                    Message.ToVisibleString()
               );
        }

        #endregion
    }
}
