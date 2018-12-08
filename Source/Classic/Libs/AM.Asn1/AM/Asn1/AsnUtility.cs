// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsnUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public static class AsnUtility
    {
        #region Public methods

        //=================================================

        // ReSharper disable InconsistentNaming
        private static readonly string[] _reservedWords =
        // ReSharper restore InconsistentNaming
        {
            "ABSENT",
            "ABSTRACT",
            "ALL",
            "APPLICATION",
            "AUTOMATIC",
            "BEGIN",
            "BIT",
            "BOOLEAN",
            "BY",
            "CHARACTER",
            "CHOICE",
            "CLASS",
            "COMPONENT",
            "COMPONENTS",
            "CONSTRAINED",
            "CONTAINING",
            "DEFAULT",
            "DEFINITIONS",
            "EMBEDDED",
            "ENCODED",
            "END",
            "ENUMERATED",
            "EXCEPT",
            "EXPLICIT",
            "EXTENSIBILITY",
            "EXTERNAL",
            "FALSE",
            "false",
            "FROM",
            "IDENTIFIER",
            "IMPLIED",
            "IMPLICIT",
            "IMPORTS",
            "INCLUDES",
            "INFINITY",
            "INSTANCE",
            "INTERSECTION",
            "MAX",
            "MIN",
            "MINUS",
            "NULL",
            "OBJECT",
            "OCTET",
            "OID",
            "OF",
            "OPTIONAL",
            "PATTERN",
            "PDV",
            "PLUS",
            "PRESENT",
            "PRIVATE",
            "REAL",
            "RELATIVE",
            "SET",
            "SEQUENCE",
            "SIZE",
            "STRING",
            "TAGS",
            "TRUE",
            "true",
            "UNION",
            "UNIQUE",
            "WITH",
        };

        /// <summary>
        /// Get array of reserved words.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static string[] GetReservedWords()
        {
            return _reservedWords;
        }

        //=================================================

        #endregion
    }
}
