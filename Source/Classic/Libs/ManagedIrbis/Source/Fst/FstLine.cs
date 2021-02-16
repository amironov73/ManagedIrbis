// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* FstLine.cs -- FST file line
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fst
{
    //
    // ТВП состоит из набора строк, каждая из которых содержит
    // следующие три параметра, разделенные знаком пробел:
    // * формат выборки данных, представленный на языке форматирования системы,
    // * идентификатор поля(ИП),
    // * метод индексирования(МИ).
    //

    /// <summary>
    /// FST file line.
    /// </summary>
    [PublicAPI]
    [XmlRoot("line")]
    [MoonSharpUserData]
    [DebuggerDisplay("{Tag} {Method} {Format}")]
    public sealed class FstLine
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Line number.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public int LineNumber { get; set; }

        /// <summary>
        /// Field tag.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonProperty("tag", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Description("Метка поля")]
        [DisplayName("Метка поля")]
        public int Tag { get; set; }

        /// <summary>
        /// Index method.
        /// </summary>
        [XmlAttribute("method")]
        [JsonProperty("method", DefaultValueHandling = DefaultValueHandling.Include)]
        [Description("Метод")]
        [DisplayName("Метод")]
        public FstIndexMethod Method { get; set; }

        /// <summary>
        /// Format itself.
        /// </summary>
        [CanBeNull]
        [XmlElement("format")]
        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        [Description("Формат")]
        [DisplayName("Формат")]
        public string Format { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse one line from the stream.
        /// </summary>
        [CanBeNull]
        public static FstLine ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            string line;
            while (true)
            {
                line = reader.ReadLine();
                if (ReferenceEquals(line, null))
                {
                    return null;
                }
                line = line.Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    break;
                }
            }

            line = line.Replace('\x1A', ' ');
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            char[] delimiters = {' ', '\t'};

#if !WINMOBILE && !PocketPC

            string[] parts = line.Split
                (
                    delimiters,
                    3,
                    StringSplitOptions.RemoveEmptyEntries
                );

#else
            // TODO Implement it properly

            string[] parts = line.Split
                (
                    delimiters
                );

#endif

            if (parts.Length != 3)
            {
                Log.Error
                    (
                        "FstLine::ParseStream: "
                        + "bad line: "
                        + line.ToVisibleString()
                    );

                throw new FormatException
                    (
                        "Bad FST line: "
                        + line.ToVisibleString()
                    );
            }

            FstLine result = new FstLine
            {
                Tag = NumericUtility.ParseInt32(parts[0]),
                Method = (FstIndexMethod)int.Parse(parts[1]),
                Format = parts[2]
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Tag"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTag()
        {
            return Tag != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Format"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFormat()
        {
            return !string.IsNullOrEmpty(Format);
        }

        /// <summary>
        /// Convert line to the IRBIS format.
        /// </summary>
        [NotNull]
        public string ToFormat()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "mpl,'{0}',/,",
                    Tag
                );
            result.Append
                (
                    IrbisFormat.PrepareFormat(Format)
                );
            result.Append(",'\x07'");

            return result.ToString();
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

            LineNumber = reader.ReadPackedInt32();
            Tag = reader.ReadPackedInt32();
            Method = (FstIndexMethod) reader.ReadPackedInt32();
            Format = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt32(LineNumber)
                .WritePackedInt32(Tag)
                .WritePackedInt32((int)Method)
                .WriteNullable(Format);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FstLine> verifier = new Verifier<FstLine>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Format, "Format");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0} {1} {2}",
                    Tag,
                    (int)Method,
                    Format.ToVisibleString()
                );
        }

        #endregion
    }
}
