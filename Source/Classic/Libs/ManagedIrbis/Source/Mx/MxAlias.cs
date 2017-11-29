// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MxAlias.cs -- 
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
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Mx
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("alias")]
    public sealed class MxAlias
        : IHandmadeSerializable,
        IVerifiable,
        IEquatable<MxAlias>
    {
        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("value")]
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxAlias()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxAlias
            (
                [CanBeNull] string name,
                [CanBeNull] string value
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNullNorEmpty(value, "value");

            Name = name;
            Value = value;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the line.
        /// </summary>
        [NotNull]
        [MustUseReturnValue]
        public static MxAlias Parse
            (
                [NotNull] string line
            )
        {
            Code.NotNullNorEmpty(line, "line");

            TextNavigator navigator = new TextNavigator(line);
            string name = navigator.ReadUntil('=');
            if (string.IsNullOrEmpty(name))
            {
                throw new IrbisException();
            }
            name = name.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new IrbisException();
            }
            if (navigator.ReadChar() != '=')
            {
                throw new IrbisException();
            }
            string value = navigator.GetRemainingText();
            if (string.IsNullOrEmpty(value))
            {
                throw new IrbisException();
            }
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                throw new IrbisException();
            }

            MxAlias result = new MxAlias(name, value);

            return result;
        }

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
                .WriteNullable(Name)
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
            Verifier<MxAlias> verifier = new Verifier<MxAlias>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name")
                .NotNullNorEmpty(Value, "Value");

            return verifier.Result;
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals
            (
                MxAlias other
            )
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Name == other.Name
                && Value == other.Value;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            if (ReferenceEquals(obj, this))
            {
                return true;
            }
            MxAlias alias = obj as MxAlias;
            if (ReferenceEquals(alias, null))
            {
                return false;
            }

            return Equals(alias);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return ReferenceEquals(Name, null)
                ? 0
                : Name.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString() + "=" + Value.ToVisibleString();
        }

        #endregion
    }
}
