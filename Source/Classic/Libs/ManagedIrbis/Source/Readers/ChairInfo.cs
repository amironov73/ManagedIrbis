// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ChairInfo.cs -- кафедра обслуживания.
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
using System.Xml.Serialization;

using AM;
using AM.IO;
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
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ChairInfo()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="code">Код.</param>
        /// <param name="title">Название.</param>
        public ChairInfo
            (
                [NotNull] string code,
                [NotNull] string title
            )
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code");
            }
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

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
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            List<ChairInfo> result = new List<ChairInfo>();

            text = text.Replace("\r", string.Empty);
            string[] lines = text.Split('\n');

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
                [NotNull] IrbisConnection client,
                [NotNull] string fileName,
                bool addAllItem
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            string chairText = client.ReadTextFile
                (
                    IrbisPath.MasterFile,
                    fileName
                );

            if (string.IsNullOrEmpty(chairText))
            {
                throw new IrbisException();
            }

            ChairInfo[] result = Parse(chairText, addAllItem);

            return result;
        }

        /// <summary>
        /// Загрузка с сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [NotNull]
        [ItemNotNull]
        public static ChairInfo[] Read
            (
                [NotNull] IrbisConnection client
            )
        {
            return Read
                (
                    client,
                    ChairMenu,
                    true
                );
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Code);
            writer.WriteNullable(Title);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return Code.ToVisibleString();
            }

            return string.Format
                (
                    "{0} - {1}",
                    Code,
                    Title
                );
        }

        #endregion
    }
}
