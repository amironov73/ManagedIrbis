// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchScenario.cs -- search scenario
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
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search
{
    //
    // Для описания одного вида поиска по словарю служат следующие параметры
    // Имя параметра Содержание
    // ItemAdvNN правила автоматического расширения поиска
    // ItemDictionTypeNN тип словаря
    // ItemF8ForNN имя формата (без расширения)
    // ItemHintNN текст подсказки/предупреждения
    // ItemLogicNN логические операторы
    // ItemMenuNN имя файла справочника
    // ItemModByDicAutoNN не задействован
    // ItemModByDicNN способ корректировки по словарю
    // ItemNameNN название поиска
    // ItemNumb общее количество поисков по словарю
    // ItemPftNN имя формата показа документов
    // ItemPrefNN префикс инверсии
    // ItemTrancNN исходное положение переключателя Усечение
    //
    // где NN - порядковый номер вида поиска по словарю
    // в общем списке (начиная с 0).

    //
    // Начиная с 2018.1
    //
    // Для иерархического представления списка видов поиска введены
    // новые параметры для описания вида поиска ItemIdentN и ItemIdentUpN.
    //
    // ItemIdentN - определяет идентификатор вида поиска, целое число
    // больше 0, обязателен для элементов группирования или видов поиска,
    // у которых есть нижестоящие элементы;
    // ItemIdentUpN - определяет идентификатор вышестоящего элемента
    // группирования или вида поиска. Для определения элементов
    // группирования, т.е. элементов списка, которые не являются
    // собственно видами поиска, а служат только для группирования
    // других элементов списка необходимо в качестве префикса
    // указать ItemPrefN=-.
    //

    /// <summary>
    /// Search scenario
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("search")]
    [DebuggerDisplay("{Prefix} {Name}")]
    public sealed class SearchScenario
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Название поиска.
        /// </summary>
        /// <remarks>Например: ItemName5=Заглавие</remarks>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Префикс для соответствующих терминов в словаре.
        /// </summary>
        /// <remarks>Например: ItemPref5=Т=</remarks>
        [CanBeNull]
        [XmlAttribute("prefix")]
        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        /// <summary>
        /// Тип словаря для соответствующего поиска.
        /// </summary>
        /// <remarks>Например: ItemDictionType8=1</remarks>
        [XmlAttribute("type")]
        [JsonProperty("type")]
        public DictionaryType DictionaryType { get; set; }

        /// <summary>
        /// Имя файла справочника.
        /// </summary>
        /// <remarks>Например: ItemMenu8=str.mnu</remarks>
        [CanBeNull]
        [XmlAttribute("menu")]
        [JsonProperty("menu")]
        public string MenuName { get; set; }

        /// <summary>
        /// Имя формата (без расширения).
        /// </summary>
        /// <remarks>Параметр  служит для указания имени формата
        /// (без расширения), который используется при показе
        /// термина словаря полностью (Приложение 4 п.13).
        /// Используется только в ИРБИС32, т.е. в ИРБИС64
        /// не используется.
        /// Используется для длинных терминов (больше 30 символов).
        /// Например: ItemF8For5=!F8TIT.
        /// </remarks>
        [CanBeNull]
        [XmlAttribute("old")]
        [JsonProperty("old")]
        public string OldFormat { get; set; }

        /// <summary>
        /// Способ Корректировки по словарю.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("correction")]
        [JsonProperty("correction")]
        public string Correction { get; set; }

        /// <summary>
        /// Исходное положение переключателя "Усечение".
        /// </summary>
        /// <remarks>Параметр  определяет исходное положение
        /// переключателя "Усечение" для данного вида поиска
        /// (0 - нет; 1 - да) - действует только в АРМе "Каталогизатор".
        /// </remarks>
        [XmlAttribute("truncation")]
        [JsonProperty("truncation")]
        public bool Truncation { get; set; }

        /// <summary>
        /// Текст подсказки/предупреждения.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("hint")]
        [JsonProperty("hint")]
        public string Hint { get; set; }

        /// <summary>
        /// Параметр пока не задействован.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("mod")]
        [JsonProperty("mod")]
        public string ModByDicAuto { get; set; }

        /// <summary>
        /// Логические операторы.
        /// </summary>
        [XmlAttribute("logic")]
        [JsonProperty("logic")]
        public SearchLogicType Logic { get; set; }

        /// <summary>
        /// Правила автоматического расширения поиска
        /// на основе авторитетного файла или тезауруса.
        /// </summary>
        /// <remarks>Параметр имеет следующую структуру:
        /// Dbname,Prefix,Format.
        /// </remarks>
        [CanBeNull]
        [XmlAttribute("advance")]
        [JsonProperty("advance")]
        public string Advance { get; set; }

        /// <summary>
        /// Имя формата показа документов.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("format")]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Идентификатор вида поиска, целое число больше 0.
        /// </summary>
        [XmlAttribute("identifier")]
        [JsonProperty("identifier", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Identifier { get; set; }

        /// <summary>
        /// Идентификатор вышестоящего элемента группирования или вида поиска.
        /// </summary>
        [XmlAttribute("parent")]
        [JsonProperty("parent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Parent { get; set; }

        /// <summary>
        /// Вложенные элементы.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public SearchScenario[] Children { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Arrange the scenarios.
        /// </summary>
        [NotNull][ItemNotNull]
        public static SearchScenario[] Arrange
            (
                [NotNull][ItemNotNull] SearchScenario[] scenarios
            )
        {
            Code.NotNull(scenarios, "scenarios");

            if (scenarios.All(s => s.Parent == 0))
            {
                return scenarios;
            }

            SearchScenario[] topLevel = scenarios
                .Where(s => s.Parent == 0).ToArray();
            foreach (SearchScenario root in topLevel)
            {
                int rootId = root.Identifier;
                if (rootId != 0)
                {
                    root.Children = scenarios
                        .Where(s => s.Parent == rootId).ToArray();
                }
            }

            return topLevel;
        }

        /// <summary>
        /// Clone the <see cref="SearchScenario"/>.
        /// </summary>
        [NotNull]
        public SearchScenario Clone()
        {
            return (SearchScenario)MemberwiseClone();
        }

        /// <summary>
        /// Parse INI file for scenarios.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static SearchScenario[] ParseIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            IniFile.Section section = iniFile["SEARCH"];
            if (ReferenceEquals(section, null))
            {
                return new SearchScenario[0];
            }

            int count = section.GetValue("ItemNumb", 0);
            if (count == 0)
            {
                return new SearchScenario[0];
            }

            List<SearchScenario> result = new List<SearchScenario>(count);

            for (int i = 0; i < count; i++)
            {

                string name = section.GetValue("ItemName" + i, null);
                if (string.IsNullOrEmpty(name))
                {
                    Log.Error
                        (
                            "SearchScenario::ParseIniFile: "
                            + "item name not set: "
                            + i
                        );

                    throw new IrbisException
                        (
                            "Item name not set: " + i
                        );
                }

                SearchScenario scenario = new SearchScenario
                {
                    Name = name,
                    Prefix = section.GetValue("ItemPref" + i, string.Empty),
                    DictionaryType = (DictionaryType)section.GetValue("ItemDictionType" + i, 0),
                    Advance = section.GetValue("ItemAdv" + i, null),
                    Format = section.GetValue("ItemPft" + i, null),
                    Hint = section.GetValue("ItemHint" + i, null),
                    Logic = (SearchLogicType)section.GetValue("ItemLogic" + i, 0),
                    MenuName = section.GetValue("ItemMenu" + i, null),
                    ModByDicAuto = section.GetValue("ItemModByDicAuto" + i, null),
                    Correction = section.GetValue("ModByDic" + i, null),
                    Truncation = Convert.ToBoolean(section.GetValue("ItemTranc" + i, 0)),
                    Identifier = section.GetValue("ItemIdent" + i, 0),
                    Parent = section.GetValue("ItemIdentUp" + i, 0)
                };

                result.Add(scenario);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Load search scenarios from the server.
        /// </summary>
        [NotNull]
        public static SearchScenario[] LoadSearchScenarios
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            throw new NotImplementedException();
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
            Prefix = reader.ReadNullableString();
            DictionaryType = (DictionaryType)reader.ReadPackedInt32();
            MenuName = reader.ReadNullableString();
            OldFormat = reader.ReadNullableString();
            Correction = reader.ReadNullableString();
            Hint = reader.ReadNullableString();
            ModByDicAuto = reader.ReadNullableString();
            Logic = (SearchLogicType)reader.ReadPackedInt32();
            Advance = reader.ReadNullableString();
            Format = reader.ReadNullableString();
            Truncation = reader.ReadBoolean();
            Identifier = reader.ReadPackedInt32();
            Parent = reader.ReadPackedInt32();
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
                .WriteNullable(Prefix)
                .WritePackedInt32((int)DictionaryType)
                .WriteNullable(MenuName)
                .WriteNullable(OldFormat)
                .WriteNullable(Correction)
                .WriteNullable(Hint)
                .WriteNullable(ModByDicAuto)
                .WritePackedInt32((int)Logic)
                .WriteNullable(Advance)
                .WriteNullable(Format)
                .Write(Truncation);
            writer
                .WritePackedInt32(Identifier)
                .WritePackedInt32(Parent);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<SearchScenario> verifier
                = new Verifier<SearchScenario>
                (
                    this,
                    throwOnError
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "{0} {1}",
                    Prefix.ToVisibleString(),
                    Name.ToVisibleString()
                );
        }

        #endregion
    }
}
