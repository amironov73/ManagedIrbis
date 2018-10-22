// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BbkReference.cs --
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

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Отсылка.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BbkReference
    {
        #region Properties

        /// <summary>
        /// Условие отсылки.
        /// Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        public string Condition { get; set; }

        /// <summary>
        /// Отсылочный код.
        /// Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        public string Content { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        [NotNull]
        public static BbkReference Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            BbkReference result = new BbkReference
            {
                Condition = field.GetFirstSubFieldValue('a'),
                Content = field.GetFirstSubFieldValue('b')
            };

            return result;
        }

        #endregion
    }
}