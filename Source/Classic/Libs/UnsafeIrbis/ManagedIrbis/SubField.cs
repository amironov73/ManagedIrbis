// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubField.cs -- MARC record subfield.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// MARC record subfield.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [XmlRoot("subfield")]
    [DebuggerDisplay("Code={Code}, Value={Value}")]
    public sealed class SubField
    {
        #region Constants

        /// <summary>
        /// Нет кода подполя, т. е. код пока не задан.
        /// </summary>
        public const char NoCode = '\0';

        /// <summary>
        /// Нет кода подполя, т. е. код пока не задан.
        /// </summary>
        public const string NoCodeString = "\0";

        /// <summary>
        /// Subfield delimiter.
        /// </summary>
        public const char Delimiter = '^';

        #endregion

        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public char Code
        {
            get;
            set;
        }

        /// <summary>
        /// Значение подполя.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("value")]
        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }

        #endregion
    }
}
