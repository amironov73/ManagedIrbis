// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SrwFile.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Tables
{
    //
    // Official documentation
    //
    // Файл сортировки имеет расширение .srw.
    // В нем задается количество заголовков в документе,
    // их содержание и форматирование.
    //
    // Файл содержит 3 секции:
    // HeaderNumber
    // HeaderFormat
    // KeyOptions
    //
    // Первые две секции отвечают за содержание
    // заголовков сортировки.
    // Поскольку сортировка может быть множественной,
    // то и для каждого уровня сортировки возможно
    // задать свой заголовок.
    // Ключи сортировки задаются в третьей секции KeyOptions.
    // Секция KeyOptions может состоять из нескольких строк.
    // Однако стоит помнить, что количество строк в этой
    // секции должно быть кратно тройке, поскольку
    // каждый ключ сортировки описывается 3-я строками:
    // длина ключа сортировки, режим сортировки
    // и формат выбора значения сортировки.
    // Длина ключа задается целым числом,
    // режим сортировки может быть 0 (единственный ключ)
    // или 1 (множественный ключ).
    // В режиме "единственный ключ" только первая строка
    // (если она есть) результата форматирования становится
    // ключом сортировки.
    // В режиме "множественный ключ" каждая строка
    // результата форматирования становится ключом сортировки.
    //
    // При написании формата заголовков могут быть использованы
    // условные поля - Vi, где i - номер ключа сортировки.
    // Форматы заголовков (если их больше одного)
    // указываются через разделитель "/".
    //

    /// <summary>
    /// Файл сортировки таблицы.
    /// </summary>
    [PublicAPI]
    [XmlRoot("sorting")]
    [MoonSharpUserData]
    public sealed class SrwFile
    {
        #region Properties

        /// <summary>
        /// Key definitions.
        /// </summary>
        [NotNull]
        [XmlElement("key")]
        [JsonProperty("keys")]
        public NonNullCollection<KeyDefinition> Keys { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SrwFile()
        {
            Keys = new NonNullCollection<KeyDefinition>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
