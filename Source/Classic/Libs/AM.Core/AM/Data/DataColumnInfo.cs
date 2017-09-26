// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataColumnInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Data
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [XmlRoot("column")]
    [MoonSharpUserData]
    public sealed class DataColumnInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        [CanBeNull]
        [DefaultValue(null)]
        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the column title.
        /// </summary>
        [CanBeNull]
        [DefaultValue(null)]
        [JsonProperty("title")]
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>The width of the column.</value>
        [DefaultValue(0)]
        [XmlAttribute("width")]
        [JsonProperty("width", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the data grid column.
        /// </summary>
        /// <value>The data grid column.</value>
        [DefaultValue(null)]
        [JsonProperty("type")]
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        [DefaultValue(null)]
        [XmlAttribute("defaultValue")]
        [JsonProperty("defaultValue", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// the column is frozen.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("frozen")]
        [JsonProperty("frozen", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Frozen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is invisible.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("invisible")]
        [JsonProperty("invisible", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Invisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// the column is read only.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("readOnly")]
        [JsonProperty("readOnly", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// this column is sorted.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("sorted")]
        [JsonProperty("sorted", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Sorted { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Width = reader.ReadPackedInt32();
            Type = reader.ReadNullableString();
            DefaultValue = reader.ReadNullableString();
            Frozen = reader.ReadBoolean();
            Invisible = reader.ReadBoolean();
            ReadOnly = reader.ReadBoolean();
            Sorted = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Name)
                .WriteNullable(Title)
                .WritePackedInt32(Width)
                .WriteNullable(Type)
                .WriteNullable(DefaultValue);
            writer.Write(Frozen);
            writer.Write(Invisible);
            writer.Write(ReadOnly);
            writer.Write(Sorted);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<DataColumnInfo> verifier
                = new Verifier<DataColumnInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
