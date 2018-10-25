// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RightRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Drm
{
    //
    // Структура БД типовых прав доступа к полным текстам
    // Имя БД: RIGHT
    // Каждая запись БД описывает типовое право доступа
    // к полным текстам (электронным ресурсам).
    // Структура записи включает в себя следующие элементы данных (поля):
    //
    // Идентификатор записи.
    // Метка поля: 1
    // Поле неповторяющееся, обязательное, содержит уникальное
    // значение, инвертируется с префиксом I=
    //
    // Общий период действия права доступа.
    // Метка поля: 2
    // Поле неповторяющееся, необязательное, содержит два подполя:
    // D – начальная дата периода в виде ГГГГММДД; может отсутствовать
    // E – конечная дата периода в виде ГГГГММДД; может отсутствовать
    //
    // Право доступа.
    // Метка поля: 3
    // Поле повторяющееся, обязательное, содержит следующие подполя:
    // A – элемент доступа; определяет данные, на основании которых
    // решается вопрос о праве доступа; подполе обязательное;
    // принимает значения в соответствии со справочником 3A.MNU:
    // 01 – идентификатор читателя
    // 02 – категория читателя
    // 03 – IP-адрес клиента
    // 04 – доменное имя клиента
    // 05 - Факультет
    // 06 - Семестр
    // 07 - Специальность
    //
    // B – значение элемента доступа (в зависимости от значения
    // подполя A); подполе обязательное; может содержать маскирующий
    // символ *
    // С – значение права доступа; подполе обязательное;
    // принимает следующие значения (в соответствии со справочником 3C.MNU):
    // 0 – доступ к полному тексту запрещен
    // 1 – разрешен постраничный просмотр полного текста
    // 2 – разрешен постраничный просмотр и скачивание полного текста
    // F – количественное ограничение при просмотре/скачивании;
    // содержит только ЧИСЛО; при отсутствии ограничений – остается пустым;
    // G – единицы количественного ограничения; имеет смысл,
    // если подполе F непустое; принимает значения в соответствии
    // со справочником 3G.MNU:
    // (пусто)  - Страницы
    // 1 - Проценты
    // В одной записи не может быть ограничений в разных единицах измерения!
    // D – начальная дата периода доступа для данного права в виде ГГГГММДД;
    // подполе необязательное; имеет смысл, если подполе С имеет
    // значения 1 или 2
    // E – конечная дата периода доступа для данного права в виде ГГГГММДД;
    // подполе необязательное; имеет смысл, если подполе С имеет
    // значения 1 или 2
    //
    // Описание/Название типовой записи права доступа
    // Метка поля: 4
    // Поле необязательное.

    //
    // Алгоритм формирования права доступа к полному тексту
    // Право доступа к конкретному полному тексту для конкретного клиента
    // решается на основе специального формата БД ЭК
    // (по умолчанию - RIGHT_FT_G.PFT). Т.е., запись БД ЭК,
    // соответствующая полному тексту форматируется по формату
    // RIGHT_FT_G.PFT – при этом через глобальные переменные передаются
    // следующие данные:
    // идентификатор читателя – глобальная переменная 30
    // IP-клиента – глобальная переменная 31
    // доменное имя клиента – глобальная переменная 32
    // Результат форматирования может принимать значения:
    // 0 – доступ запрещен
    // 1#NN – разрешен постраничный просмотр
    // 2#NN – разрешен постраничный просмотр и скачивание
    // где NN – ограничение на кол-во страниц; может отсутствовать
    //
    // Формат RIGHT_FT_G.PFT БД ЭК использует в качестве вложенных
    // следующие форматы:
    // - right2_ft_G.pft, right3_ft_G.pft, right4_ft_G.pft – БД ЭК
    // - right0.pft – БД RIGHT
    // - right_rid.pft, right_rkat, right_rfak, right_rsem, right_rspc – БД RDR
    //

    /// <summary>
    /// Запись с правами доступа к ресурсам.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RightRecord
    {
        #region Properties

        /// <summary>
        /// Идентификатор записи. Поле 1.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "0001".
        /// </remarks>
        [CanBeNull]
        [Field(1)]
        [XmlAttribute("id")]
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Общий период действия права доступа. Поле 2.
        /// </summary>
        [CanBeNull]
        [Field(2)]
        [XmlElement("period")]
        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        public ValidityPeriod Period { get; set; }

        /// <summary>
        /// Права доступа. Поле 3 (повторяющееся).
        /// </summary>
        [CanBeNull]
        [Field(3)]
        [XmlElement("right")]
        [JsonProperty("rights", NullValueHandling = NullValueHandling.Ignore)]
        public AccessRight[] Rights { get; set; }

        /// <summary>
        /// Описание/название. Поле 4.
        /// </summary>
        [CanBeNull]
        [Field(4)]
        [XmlAttribute("description")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Associated <see cref="MarcRecord"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

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
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static RightRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            RightRecord result = new RightRecord
            {
                Id = record.FM(1),
                Period = ValidityPeriod.Parse(record.Fields.GetFirstField(2)),
                Rights = AccessRight.Parse(record),
                Description = record.FM(4),
                Record = record
            };

            return result;
        }

        #endregion
    }
}
