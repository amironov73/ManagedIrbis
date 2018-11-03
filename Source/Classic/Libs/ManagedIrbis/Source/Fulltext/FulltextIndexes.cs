// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FulltextDublin.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fulltext
{
    /// <summary>
    /// Поисковые индексы встроенной базы данных TEXT.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FulltextIndexes
    {
        #region Constants

        /// <summary>
        ///
        /// </summary>
        public const string Text = "TXT=";

        /// <summary>
        ///
        /// </summary>
        public const string BeginText = "TXT1=";

        /// <summary>
        ///
        /// </summary>
        public const string ContinueText = "TXT2=";

        /// <summary>
        ///
        /// </summary>
        public const string Guid = "GUID=";

        #endregion
    }
}
