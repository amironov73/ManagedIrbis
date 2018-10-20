// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ZaprInfo.cs --
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
    /// Информация о постоянном запросе в базе данных ZAPR.
    /// Поле 2.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ZaprInfo
    {
        #region Properties

        /// <summary>
        /// Формулировка запроса на естественном языке.
        /// Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        public string NaturalLanguage { get; set; }

        /// <summary>
        /// Полнотекстовая часть запроса.
        /// Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        public string FullTextQuery { get; set; }

        /// <summary>
        /// Библиографическая часть запроса.
        /// Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        public string SearchQuery { get; set; }

        /// <summary>
        /// Дата создания запроса.
        /// Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        public IrbisDate Date { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record field.
        /// </summary>
        [NotNull]
        public static ZaprInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            ZaprInfo result = new ZaprInfo
            {
                NaturalLanguage = field.GetFirstSubFieldValue('a'),
                FullTextQuery = field.GetFirstSubFieldValue('b'),
                SearchQuery = field.GetFirstSubFieldValue('c'),
                Date = IrbisDate.SafeParse(field.GetFirstSubFieldValue('d'))
            };

            return result;
        }

        #endregion
    }
}
