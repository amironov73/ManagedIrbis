// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChairInfo.cs -- кафедра обслуживания.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о кафедре обслуживания.
    /// </summary>
    [PublicAPI]
    [XmlRoot("chair")]
    [DebuggerDisplay("{Code} {Title}")]
    public sealed class ChairInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Имя меню с кафедрами по умолчанию.
        /// </summary>
        public const string ChairMenu = "kv.mnu";

        /// <summary>
        /// Имя меню с местами хранения по умолчанию.
        /// </summary>
        public const string PlacesMenu = "mhr.mnu";

        #endregion

        #region Properties

        /// <summary>
        /// Код.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("code")]
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChairInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">Chair code.</param>
        public ChairInfo
            (
                [NotNull] string code
            )
        {
            CodeJam.Code.NotNull(code, "code");

            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">Chair code.</param>
        /// <param name="title">Chair title.</param>
        public ChairInfo
            (
                [NotNull] string code,
                [NotNull] string title
            )
        {
            CodeJam.Code.NotNull(code, "code");
            CodeJam.Code.NotNull(title, "title");

            Code = code;
            Title = title;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текста меню-файла.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ChairInfo[] Parse
            (
                [NotNull] string text,
                bool addAllItem
            )
        {
            CodeJam.Code.NotNullNorEmpty(text, "text");

            List<ChairInfo> result = new List<ChairInfo>();
            string[] lines = text.SplitLines();
            for (int i = 0; i < lines.Length; i += 2)
            {
                if (lines[i].StartsWith("*"))
                {
                    break;
                }
                ChairInfo item = new ChairInfo
                    {
                        Code = lines[i],
                        Title = lines[i + 1]
                    };
                result.Add(item);
            }

            if (addAllItem)
            {
                result.Add
                    (
                        new ChairInfo
                            {
                                Code = "*",
                                Title = "Все подразделения"
                            }
                    );
            }

            return result
                .OrderBy(item => item.Code)
                .ToArray();
        }

        /// <summary>
        /// Загрузка с сервера.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ChairInfo[] Read
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] string fileName,
                bool addAllItem
            )
        {
            CodeJam.Code.NotNull(connection, "connection");
            CodeJam.Code.NotNullNorEmpty(fileName, "fileName");

            string chairText = connection.ReadTextFile
                (
                    IrbisPath.MasterFile,
                    fileName
                );

            if (string.IsNullOrEmpty(chairText))
            {
                Log.Error
                    (
                        "ChairInfo::Read: "
                        + "file is missing or empty: "
                        + fileName
                    );

                throw new IrbisException();
            }

            ChairInfo[] result = Parse(chairText, addAllItem);

            return result;
        }

        /// <summary>
        /// Загрузка с сервера.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ChairInfo[] Read
            (
                [NotNull] IIrbisConnection connection
            )
        {
            return Read
                (
                    connection,
                    ChairMenu,
                    true
                );
        }

        /// <summary>
        /// Should serialize the <see cref="Title"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeTitle()
        {
            return !string.IsNullOrEmpty(Title);
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Code);
            writer.WriteNullable(Title);
        }

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code = reader.ReadNullableString();
            Title = reader.ReadNullableString();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return Code.ToVisibleString();
            }

            return string.Format
                (
                    "{0} - {1}",
                    Code.ToVisibleString(),
                    Title.ToVisibleString()
                );
        }

        #endregion
    }
}
