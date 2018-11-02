// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FulltextReference.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fulltext
{
    //
    // Ссылка на объект полнотекстового поиска в общем случае
    // содержит следующие структурные элементы:
    //
    // * URL
    // * Путь к текстовому файлу
    // * Номер страницы
    // * Путь к файлу архива
    // * Путь к файлу внутри архива
    // * Имя файла с текстом-подложкой
    // * Полный путь для относительной ссылки (является избыточным
    // и поддерживается по историческим причинам)
    //
    // В зависимости от вида ссылка содержит те или иные элементы.
    //

    //
    // Структура, используемая для хранения ссылки в базе данных
    // представляет собой совокупность подполей ^B^C^I^T^U:
    //
    // B - в зависимости от вида ссылки это относительный, полный
    // или виртуальный путь к файлу полного текста, или же некоторые данные,
    // дополняющие гиперссылку. Относительный путь используется
    // для полнотекстовых документов, хранящихся в папке базы данных
    // (относительный путь начинается с точки). Полные пути используются
    // для ссылок на полнотекстовые документы, находящиеся вне папки
    // базы данных. Виртуальные пути к текстовым документам используются
    // для ссылок на полнотекстовые документы, хранящиеся
    // в архивах .zip и .rar, а также в случае ссылок на отдельные
    // страницы многостраничных документов .pdf и .djvu.
    // Виртуальная ссылка, хранящаяся в этом подполе, позволяет узнать
    // имя файла внутри архива или номер страницы многостраничного документа,
    // но не имя файла архива или многостраничного документа.
    // C - полный путь к файлу zip/rar/pdf/djvu. Данное подполе используется
    // для ссылок на полнотекстовые документы в архиве или отдельные
    // страницы многостраничного документа.
    // I - URL текста, перенесённого из электронного каталога.
    // T - ссылка на файл подложки. Представляет собой имя текстового файла,
    // подразумевается, что местонахождение файла подложки соответствует
    // местонахождению полнотекстового документа.
    // U – введено для технологических целей в версии 2010.1.
    // Подполе ^U было задумано как универсальная замена подполям ^B^C^I
    // с возможностью расширения, однако было признано неудобным
    // с точки зрения его разбора средствами языка форматирования.
    // Как следствие, подполе ^U остаётся вспомогательным,
    // и используется наряду с другими подполями.
    //

    /// <summary>
    /// Ссылка на объект полнотекстового поиска.
    /// Поле 952.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class FullTextReference
    {
        #region Constants

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 952;

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла полного текста. Данное подполе используется
        /// только для хранения ссылок на полнотекстовые документов
        /// в архиве с именем базы данных и расширением .izp,
        /// находящемся в папке базы данных.
        /// Начиная с версии 2010.1 данный вид ссылок не поддерживается.
        /// Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("fileName")]
        [JsonProperty("fileName", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }

        /// <summary>
        /// Дополнительная информация.
        /// Подполе b.
        /// </summary>
        /// <remarks>
        /// Подполе ^B конструируется следующим образом:
        /// [путь к файлу (без имени файла)] + [имя файла (без расширения)]
        /// + [суффикс] + [номер страницы] + [расширение файла]
        /// </remarks>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("info")]
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public string Info { get; set; }

        /// <summary>
        /// Отдельная страница многостраничного документа (путь к файлу).
        /// Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("separatePage")]
        [JsonProperty("separatePage", NullValueHandling = NullValueHandling.Ignore)]
        public string SeparatePage { get; set; }

        /// <summary>
        /// URL.
        /// Подполе i.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("url")]
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        /// <summary>
        /// Имя файла с текстом-подложкой.
        /// Местонахождение файла подложки соответствует местонахождению
        /// основного файла.
        /// Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("substrate")]
        [JsonProperty("substrate", NullValueHandling = NullValueHandling.Ignore)]
        public string Substrate { get; set; }

        /// <summary>
        /// Введено для технологических целей в версии 2010.1.
        /// Подполе ^U было задумано как универсальная замена
        /// подполям ^B^C^I с возможностью расширения, однако было
        /// признано неудобным с точки зрения его разбора средствами
        /// языка форматирования. Как следствие, подполе ^U остаётся
        /// вспомогательным, и используется наряду с другими подполями.
        /// Подполе ^U всегда начинается с префикса
        /// uri:irbis:
        /// Дальнейшее содержимое зависит от объекта полнотекстового поиска.
        /// </summary>
        [CanBeNull]
        [SubField('u')]
        [XmlAttribute("additional")]
        [JsonProperty("additional", NullValueHandling = NullValueHandling.Ignore)]
        public string Additional { get; set; }

        /// <summary>
        /// Associated <see cref="RecordField"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [CanBeNull]
        public static FullTextReference Parse
            (
                [CanBeNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            FullTextReference result = new FullTextReference
            {
                FileName = field.GetFirstSubFieldValue('a'),
                Info = field.GetFirstSubFieldValue('b'),
                SeparatePage = field.GetFirstSubFieldValue('c'),
                Url = field.GetFirstSubFieldValue('i'),
                Substrate = field.GetFirstSubFieldValue('t'),
                Additional = field.GetFirstSubFieldValue('u'),
                Field = field
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Info.ToVisibleString();
        }

        #endregion
    }
}