// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsnType.cs --
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
    /// Enumeration of possible ASN.1 universal class tags
    /// in Identifier octet.
    /// </summary>
    [PublicAPI]
    public enum AsnType
    {
        /// <summary>
        /// UniversalClass
        /// </summary>
        Eoc = 0,
        /// <summary>
        /// Boolean
        /// </summary>
        Boolean = 1,

        /// <summary>
        /// Integer
        /// </summary>
        Integer = 2,

        /// <summary>
        /// BitString
        /// </summary>
        BitString = 3,

        /// <summary>
        /// OctetString
        /// </summary>
        OctetString = 4,

        /// <summary>
        /// NULL
        /// </summary>
        Null = 5,

        /// <summary>
        /// ObjectIdentifier
        /// </summary>
        ObjectIdentifier = 6,

        /// <summary>
        /// ObjectDescriptor
        /// </summary>
        ObjectDescriptor = 7,

        /// <summary>
        /// External
        /// </summary>
        External = 8,

        /// <summary>
        /// Real
        /// </summary>
        Real = 9,

        /// <summary>
        /// Enumerated
        /// </summary>
        Enumerated = 10,

        /// <summary>
        /// EmbeddedPdv
        /// </summary>
        EmbeddedPdv = 11,

        /// <summary>
        /// Utf8String
        /// </summary>
        Utf8String = 12,

        /// <summary>
        /// RelativeOid
        /// </summary>
        RelativeOid = 13,

        /// <summary>
        /// Sequence
        /// </summary>
        Sequence = 16,

        /// <summary>
        /// Set
        /// </summary>
        Set = 17,

        /// <summary>
        /// NumericString
        /// </summary>
        NumericString = 18,

        /// <summary>
        /// PrintableString
        /// </summary>
        PrintableString = 19,

        /// <summary>
        /// T61String
        /// </summary>
        T61String = 20,

        /// <summary>
        /// VideotexString
        /// </summary>
        VideotexString = 21,

        /// <summary>
        /// Ia5String
        /// </summary>
        Ia5String = 22,

        /// <summary>
        /// UtcTime
        /// </summary>
        UtcTime = 23,

        /// <summary>
        /// GeneralizedTime
        /// </summary>
        GeneralizedTime = 24,

        /// <summary>
        /// GraphicString
        /// </summary>
        GraphicString = 25,

        /// <summary>
        /// VisibleString
        /// </summary>
        VisibleString = 26,

        /// <summary>
        /// GeneralString
        /// </summary>
        GeneralString = 27,

        /// <summary>
        /// UniversalString
        /// </summary>
        UniversalString = 28,

        /// <summary>
        /// CharacterString
        /// </summary>
        CharacterString = 29,

        /// <summary>
        /// BmpString
        /// </summary>
        BmpString = 30,

        /// <summary>
        /// LongForm
        /// </summary>
        LongForm = 31
    }
}
