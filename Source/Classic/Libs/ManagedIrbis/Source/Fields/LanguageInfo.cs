// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LanguageInfo.cs -- 
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
    /// Язык документа (дополнительные данные). Поле 919.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LanguageInfo
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "abefgklnoz";

        /// <summary>
        /// Tag.
        /// </summary>
        public const string Tag = "919";

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        public string CatalogingLanguage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>See
        /// <see cref="ManagedIrbis.Fields.CatalogingRules"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('k')]
        public string CatalogingRules { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>See <see cref="CharacterSetCode"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('n')]
        public string CharacterSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>See <see cref="Fields.TitleCharacterSet"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('g')]
        public string TitleCharacterSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>See <see cref="LanguageCode"/> class.
        /// </remarks>
        [CanBeNull]
        [SubField('b')]
        public string IntermediateTranslationLanguage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [SubField('o')]
        public string OriginalLanguage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        public string TocLanguage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        public string TitlePageLanguage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [SubField('z')]
        public string MainTitleLanguage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        public string AccompanyingMaterialLanguage { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
