// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FbAuthor.cs --
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
    /// Данные об авторе FictionBook.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FbAuthor
    {
        #region Properties

        /// <summary>
        /// Имя.
        /// </summary>
        [XmlElement("first-name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [XmlElement("middle-name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [XmlElement("last-name")]
        public string LastName { get; set; }

        #endregion
    }
}
