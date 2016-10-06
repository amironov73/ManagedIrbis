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
        /// <param name="text"></param>
        /// <returns></returns>
        [NotNull]
        [ItemNotNull]
        public static ChairInfo[] Parse
            (
                [NotNull] string text
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

            result.Add
                (
                    new ChairInfo
                        {
                            Code = "*",
                            Title = "Все подразделения"
                        }
                );

            return result
                .OrderBy(item => item.Code)
                .ToArray();
        }

        /// <summary>
        /// Загрузка с сервера.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [NotNull]
        [ItemNotNull]
        public static ChairInfo[] Read
            (
                [NotNull] IrbisConnection client,
                [NotNull] string fileName
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

            ChairInfo[] result = Parse(chairText);

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
                    ChairMenu
                );
        }

        #region IHandmadeSerializable

        /// <summary>
        /// Сохранение в поток.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Code);
            writer.WriteNullable(Title);
        }

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public static void SaveToFile
            (
                [NotNull] string fileName,
                [NotNull][ItemNotNull] ChairInfo[] chairs
            )
        {
            chairs.SaveToFile(fileName);
        }

        /// <summary>
        /// Считывание из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code = reader.ReadNullableString();
            Title = reader.ReadNullableString();
        }

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        [CanBeNull]
        [ItemNotNull]
        public static ChairInfo[] ReadChairsFromFile
            (
                [NotNull] string fileName
            )
        {
            ChairInfo[] result = SerializationUtility
                .RestoreArrayFromFile<ChairInfo>
                (
                    fileName
                );

            return result;
        }

        #endregion

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
                    "{0} - {1}",
                    Code,
                    Title
                );
        }

        #endregion
    }
}
