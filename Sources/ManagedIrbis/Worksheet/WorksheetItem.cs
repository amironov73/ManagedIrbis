/* WorksheetItem.cs -- строчка в рабочем листе
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
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Worksheet
{
    //
    // РЛ полей содержит таблицу  с элементами для ввода/корректировки,
    // имеющую следующие столбцы:
    //
    // Метка - числовая метка поля.
    // Вводится произвольно или выбирается из таблицы полного
    // описания полей, вызываемой по кнопке или команде <F2>.
    // Таблицу полного описания полей предварительно следует загрузить.
    //
    // Наименование - наименование поля.
    // Вводится произвольно или выбирается из таблицы полного
    // описания полей, вызываемой по кнопке или команде <F2>.
    // Таблицу полного описания полей предварительно следует загрузить.
    //
    // Повторяемость поля.
    // Выбирается из предлагаемого списка (1 - повторяющееся,
    // 0 - неповторяющееся).
    //
    // Индекс контекстной помощи
    // - ссылка на текст в Инструкции каталогизатора.
    //
    // Режим ввода
    // - использование расширенных средств ввода. Выбирается
    // из предлагаемого списка. Знак «!» перед значением расширенного
    // средства ввода определяет его обязательное использование.
    // Если указать в качестве режима ввода «!0», то это будет
    // означать запрет на корректировку соответствующего поля.
    //
    // Доп.инф
    // - дополнительная информация для Расширенных средств ввода
    // (АРМ Каталогизатор).
    // Значение и структура данного параметра зависит от значения
    // предыдущего параметра – РЕЖИМ ВВОДА
    //
    // ФЛК
    // - формат ФЛК поля (Приложение 4 п. 12). Указывается в виде
    // непосредственного формата или в виде имени предварительно
    // созданного формата (без расширения) с предшествующим символом «@».
    //
    // Подсказка
    // - текст помощи (инструкции), сопровождающий ввод в поле.
    //
    // Значение по умолчанию (статическое)
    // - значение поля по умолчанию при создании новой записи
    // (статическое значение по умолчанию). Указывается непосредственно
    // или через параметр инициализационного файла в виде:
    // @<SECTION>,<NAME>,<DEFAULT>, где:
    // <SECTION>  - секция инициализационного файла;
    // <NAME>        - имя параметра;
    // <DEFAULT>  - значение параметра по умолчанию.
    //
    // Доп.инф. [резерв]
    // - используется при определенных режимах ввода (1, 2, 3, 6, 10, 11).
    // Определяет правила объединения данных при групповом вводе
    // в одно поле. Может иметь вид:
    // RXXX   - вставлять разделители XXX справа от каждого отобранного
    // элемента, кроме последнего;
    // LXXX    - вставлять разделители XXX слева от каждого отобранного
    // элемента;
    // DXXYY - каждый отобранный элемент заключать слева разделителями
    // XX и справа - YY.
    // Если параметр остается пустым - групповой ввод в одно поле запрещен.


    //
    // Столбцы РЛ подполей:
    // 1. Разделитель - односимвольный идентификатор подполя
    // (латиница или цифра, нет разницы между строчными
    // и прописными буквами).
    // 2. Наименование - название подполя.
    // 3. Повторяемость - единственное допустимое значение
    // 0 - неповторяющееся.
    // 4. Индекс контекстной помощи- то же, что и для РЛ полей.
    // 5. Режим ввода - то же, что и для РЛ полей.
    // 6. Доп.инф. - то же, что и для РЛ полей.
    // 7. ФЛК - не используется.
    // 8. Умолчание - не используется.
    // 9. Подсказка - то же, что и для РЛ полей.
    // 10. [резерв] - то же, что и для РЛ полей.


    /// <summary>
    /// Одна строчка в рабочем листе.
    /// </summary>
    [PublicAPI]
    [XmlRoot("line")]
    [MoonSharpUserData]
    [DebuggerDisplay("{Tag} {Title} [{Repeatable}][{InputMode}]")]
    public sealed class WorksheetItem
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Числовая метка поля.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Наименование поля.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Повторяемость поля.
        /// </summary>
        [XmlAttribute("repeatable")]
        [JsonProperty("repeatable")]
        public bool Repeatable { get; set; }

        /// <summary>
        /// Индекс контекстной помощи.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("help")]
        [JsonProperty("help")]
        public string Help { get; set; }

        /// <summary>
        /// Режим ввода.
        /// </summary>
        [XmlAttribute("input-mode")]
        [JsonProperty("input-mode")]
        public InputMode InputMode { get; set; }

        /// <summary>
        /// Дополнительная информация для расширенных
        /// средств ввода.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("input-info")]
        [JsonProperty("input-info")]
        public string InputInfo { get; set; }

        /// <summary>
        /// ФЛК.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("formal-verification")]
        [JsonProperty("formal-verification")]
        public string FormalVerification { get; set; }

        /// <summary>
        /// Подсказка - текст помощи (инструкции),
        /// сопровождающий ввод в поле.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("hint")]
        [JsonProperty("hint")]
        public string Hint { get; set; }

        /// <summary>
        /// Знчение по умолчанию при создании 
        /// новой записи.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("default-value")]
        [JsonProperty("default-value")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Используется при определенных режимах ввода.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("reserved")]
        [JsonProperty("reserved")]
        public string Reserved { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор потока.
        /// </summary>
        [NotNull]
        public static WorksheetItem ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            WorksheetItem result = new WorksheetItem
            {
                Tag = reader.RequireLine(),
                Title = reader.RequireLine().Trim(),
                Repeatable = ConversionUtility.ToBoolean(reader.RequireLine()),
                Help = reader.RequireLine().Trim(),
                InputMode = (InputMode) int.Parse(reader.RequireLine()),
                InputInfo = reader.RequireLine(),
                FormalVerification = reader.RequireLine().Trim(),
                Hint = reader.RequireLine().Trim(),
                DefaultValue = reader.RequireLine().Trim(),
                Reserved = reader.RequireLine().Trim()
                
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Tag = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Repeatable = reader.ReadBoolean();
            Help = reader.ReadNullableString();
            InputMode = (InputMode) reader.ReadPackedInt32();
            InputInfo = reader.ReadNullableString();
            FormalVerification = reader.ReadNullableString();
            Hint = reader.ReadNullableString();
            DefaultValue = reader.ReadNullableString();
            Reserved = reader.ReadNullableString();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Tag)
                .WriteNullable(Title)
                .Write(Repeatable);
            writer
                .WriteNullable(Help)
                .WritePackedInt32((int)InputMode);
            writer
                .WriteNullable(InputInfo)
                .WriteNullable(FormalVerification)
                .WriteNullable(Hint)
                .WriteNullable(DefaultValue)
                .WriteNullable(Reserved);
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
                    "{0}: {1} [{2}][{3}]",
                    Tag,
                    Title,
                    Repeatable,
                    InputMode
                );
        }

        #endregion
    }
}
