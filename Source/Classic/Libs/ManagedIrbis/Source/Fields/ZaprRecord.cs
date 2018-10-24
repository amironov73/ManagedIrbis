// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ZaprRecord.cs --
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
    /// Запись в базе данных ZAPR.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ZaprRecord
    {
        #region Properties

        /// <summary>
        /// Идентификатор читателя.
        /// </summary>
        [Field(1)]
        [CanBeNull]
        public string Ticket { get; set; }

        /// <summary>
        /// Постоянные запросы.
        /// </summary>
        [Field(2)]
        [CanBeNull]
        public ZaprInfo[] Requests { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static ZaprRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            ZaprRecord result = new ZaprRecord
            {
                Ticket = record.FM(1),
                Requests = record.Fields
                    .GetField(2)
                    .Select(f => ZaprInfo.Parse(f))
                    .ToArray()
            };

            return result;
        }

        #endregion
    }
}
