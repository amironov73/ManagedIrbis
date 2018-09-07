// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FbBinary.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using AM;
using AM.Logging;
using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.FictionBook
{
    /// <summary>
    /// Вложенные в файл FictionBook двоичные данные.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FbBinary
    {
        #region Properties

        /// <summary>
        /// Content type.
        /// </summary>
        [XmlAttribute("content-type")]
        public string ContentType { get; set; }

        /// <summary>
        /// Id.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        #endregion
    }
}
