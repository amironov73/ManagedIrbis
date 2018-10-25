// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AthrcRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Запись в базе данных ATHRC.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AthrcRecord
    {
        #region Properties

        /// <summary>
        /// Основное (унифицированное) наименование коллектива/мероприятия.
        /// Поле 210.
        /// </summary>
        [CanBeNull]
        [Field(210)]
        public object MainTitle { get; set; }

        /// <summary>
        /// Ссылки типа СМ.
        /// Поле 410.
        /// </summary>
        [CanBeNull]
        [Field(410)]
        public object[] See { get; set; }

        /// <summary>
        /// Ссылки типа СМ. ТАКЖЕ.
        /// Поле 510.
        /// </summary>
        [CanBeNull]
        [Field(510)]
        public object[] SeeAlso { get; set; }

        /// <summary>
        /// Связанные основные наименования
        /// коллектива/мероприятия на других языках.
        /// Поле 710.
        /// </summary>
        [CanBeNull]
        [Field(710)]
        public object[] LinkedTitles { get; set; }

        /// <summary>
        /// Вид коллектива/мероприятия.
        /// Поле 900.
        /// </summary>
        [CanBeNull]
        [Field(900)]
        public object CollectiveKind { get; set; }

        /// <summary>
        /// Информационное примечание.
        /// Поле 300.
        /// </summary>
        [CanBeNull]
        [Field(300)]
        public object[] Notes { get; set; }

        /// <summary>
        /// Текстовое ссылочное примечание "см. также".
        /// Поле 305.
        /// </summary>
        [CanBeNull]
        [Field(305)]
        public object[] SeeAlsoNote { get; set; }

        /// <summary>
        /// Текстовое ссылочное примечание "см.".
        /// Поле 310.
        /// </summary>
        [CanBeNull]
        [Field(310)]
        public object SeeNote { get; set; }

        /// <summary>
        /// Примечания об области применения.
        /// Поле 330.
        /// </summary>
        [CanBeNull]
        [Field(330)]
        public object[] UsageNotes { get; set; }

        /// <summary>
        /// Источник составления записи.
        /// Поле 801.
        /// </summary>
        [CanBeNull]
        [Field(801)]
        public object[] InformationSources { get; set; }

        /// <summary>
        /// Источник,в котором выявлена информ. о заголовке.
        /// Поле 810.
        /// </summary>
        [CanBeNull]
        [Field(810)]
        public object[] IdentificationSources { get; set; }

        /// <summary>
        /// Информации об использовании основного наименования коллектива/мероприятия.
        /// Поле 820.
        /// </summary>
        [CanBeNull]
        [Field(820)]
        public object[] UsageInformation { get; set; }

        /// <summary>
        /// Пример,приведенный в примечании.
        /// Поле 825.
        /// </summary>
        [CanBeNull]
        [Field(825)]
        public object[] Example { get; set; }

        /// <summary>
        /// Общее примечание каталогизатора.
        /// Поле 830.
        /// </summary>
        [CanBeNull]
        [Field(830)]
        public object[] CataloguerNotes { get; set; }

        /// <summary>
        /// Информация об исключении основного наименования коллектива/мероприятия.
        /// Поле 835.
        /// </summary>
        [CanBeNull]
        [Field(835)]
        public object[] ExclusionInformation { get; set; }

        /// <summary>
        /// Правила каталогизации и предметизации.
        /// Поле 152.
        /// </summary>
        [CanBeNull]
        [Field(152)]
        public object CataguingRules { get; set; }

        /// <summary>
        /// Ссылка-внешний объект.
        /// Поле 951.
        /// </summary>
        [CanBeNull]
        [Field(951)]
        public object[] ExternalObject { get; set; }

        /// <summary>
        /// Внутренний двоичный ресурс.
        /// Поле 953.
        /// </summary>
        [CanBeNull]
        [Field(953)]
        public object[] BinaryResource { get; set; }

        /// <summary>
        /// Проверку на дублетность производить.
        /// Поле 905.
        /// </summary>
        [CanBeNull]
        [Field(905)]
        public object Settings { get; set; }

        /// <summary>
        /// Каталогизатор, дата.
        /// Поле 907.
        /// </summary>
        [CanBeNull]
        [Field(907)]
        public object[] Technology { get; set; }

        /// <summary>
        /// Имя рабочего листа.
        /// Поле 920.
        /// </summary>
        [CanBeNull]
        [Field(920)]
        public string Worksheet { get; set; }

        #endregion
    }
}