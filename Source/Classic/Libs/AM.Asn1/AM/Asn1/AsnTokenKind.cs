// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AsnTokenKind.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    public enum AsnTokenKind
    {
        /// <summary>
        /// No token.
        /// </summary>
        None,

        /// <summary>
        /// Identifier name
        /// </summary>
        Name,

        /// <summary>
        /// 123
        /// </summary>
        Number,

        /// <summary>
        /// ::=
        /// </summary>
        Assign,

        /// <summary>
        /// :
        /// </summary>
        Colon,

        /// <summary>
        /// ;
        /// </summary>
        Semicolon,

        /// <summary>
        /// .
        /// </summary>
        Dot,

        /// <summary>
        /// ..
        /// </summary>
        DoubleDot,

        /// <summary>
        /// ...
        /// </summary>
        Ellipsis,

        /// <summary>
        /// ,
        /// </summary>
        Comma,

        /// <summary>
        /// !
        /// </summary>
        Exclamation,

        /// <summary>
        /// `
        /// </summary>
        Apostrophe,

        /// <summary>
        /// &amp;
        /// </summary>
        Ampersand,

        /// <summary>
        /// &lt;
        /// </summary>
        LessThan,

        /// <summary>
        /// &lt;/
        /// </summary>
        LessThanSlash,

        /// <summary>
        /// &gt;
        /// </summary>
        GreaterThan,

        /// <summary>
        /// /&gt;
        /// </summary>
        SlashGreaterThan,

        /// <summary>
        /// {
        /// </summary>
        LeftBrace,

        /// <summary>
        /// }
        /// </summary>
        RightBrace,

        /// <summary>
        /// (
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// )
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// -
        /// </summary>
        Minus,

        /// <summary>
        /// *
        /// </summary>
        Star,

        /// <summary>
        /// ^
        /// </summary>
        Power,

        /// <summary>
        /// |
        /// </summary>
        Pipe,

        /// <summary>
        /// [
        /// </summary>
        LeftBracket,

        /// <summary>
        /// [[
        /// </summary>
        DoubleLeftBracket,

        /// <summary>
        /// ]
        /// </summary>
        RightBracket,

        /// <summary>
        /// ]]
        /// </summary>
        DoubleRightBracket,

        /// <summary>
        /// --
        /// </summary>
        LineComment,

        /// <summary>
        /// '0101'B
        /// </summary>
        BinaryString,

        /// <summary>
        /// '01AB'H
        /// </summary>
        HexString,

        //-----------------------------------------------------------
        // reserved words
        //-----------------------------------------------------------

        /// <summary>
        /// ABSENT
        /// </summary>
        Absent,

        /// <summary>
        /// ABSTRACT SYNTAX
        /// </summary>
        AbstractSyntax,

        /// <summary>
        /// ALL
        /// </summary>
        All,

        /// <summary>
        /// APPLICATION
        /// </summary>
        Application,

        /// <summary>
        /// AUTOMATIC
        /// </summary>
        Automatic,

        /// <summary>
        /// BEGIN
        /// </summary>
        Begin,

        /// <summary>
        /// BIT
        /// </summary>
        Bit,

        /// <summary>
        /// BOOLEAN
        /// </summary>
        Boolean,

        /// <summary>
        /// BY
        /// </summary>
        By,

        /// <summary>
        /// CHARACTER
        /// </summary>
        Character,

        /// <summary>
        /// CHOICE
        /// </summary>
        Choice,

        /// <summary>
        /// CLASS
        /// </summary>
        Class,

        /// <summary>
        /// COMPONENT
        /// </summary>
        Component,

        /// <summary>
        /// COMPONENTS
        /// </summary>
        Components,

        /// <summary>
        /// CONSTRAINED
        /// </summary>
        Constrained,

        /// <summary>
        /// CONTAINING
        /// </summary>
        Containing,

        /// <summary>
        /// DEFAULT
        /// </summary>
        Default,

        /// <summary>
        /// DEFINITIONS
        /// </summary>
        Definitions,

        /// <summary>
        /// EMBEDDED
        /// </summary>
        Embedded,

        /// <summary>
        /// ENCODED
        /// </summary>
        Encoded,

        /// <summary>
        /// END
        /// </summary>
        End,

        /// <summary>
        /// ENUMERATED
        /// </summary>
        Enumerated,

        /// <summary>
        /// EXCEPT
        /// </summary>
        Except,

        /// <summary>
        /// EXPLICIT
        /// </summary>
        Explicit,

        /// <summary>
        /// EXTENSIBILITY
        /// </summary>
        Extensibility,

        /// <summary>
        /// EXTERNAL
        /// </summary>
        External,

        /// <summary>
        /// FALSE
        /// </summary>
        False,

        /// <summary>
        /// false
        /// </summary>
        FalseSmall,

        /// <summary>
        /// FROM
        /// </summary>
        From,

        /// <summary>
        /// IDENTIFIER
        /// </summary>
        Identifier,

        /// <summary>
        /// IMPLIED
        /// </summary>
        Implied,

        /// <summary>
        /// IMPLICIT
        /// </summary>
        Implicit,

        /// <summary>
        /// IMPORTS
        /// </summary>
        Imports,

        /// <summary>
        /// INCLUDES
        /// </summary>
        Includes,

        /// <summary>
        /// INSTANCE
        /// </summary>
        Instance,

        /// <summary>
        /// INTERSECTION
        /// </summary>
        Intersection,

        /// <summary>
        /// MAX
        /// </summary>
        Max,

        /// <summary>
        /// MIN
        /// </summary>
        Min,

        /// <summary>
        /// MINUS-INFINITY
        /// </summary>
        MinusInfinity,

        /// <summary>
        /// NULL
        /// </summary>
        Null,

        /// <summary>
        /// OBJECT
        /// </summary>
        Object,

        /// <summary>
        /// OCTET
        /// </summary>
        Octet,

        /// <summary>
        /// OF
        /// </summary>
        Of,

        /// <summary>
        /// OPTIONAL
        /// </summary>
        Optional,

        /// <summary>
        /// PATTERN
        /// </summary>
        Pattern,

        /// <summary>
        /// PDV
        /// </summary>
        Pdv,

        /// <summary>
        /// PLUS-INFINITY
        /// </summary>
        PlusInfinity,

        /// <summary>
        /// PRESENT
        /// </summary>
        Present,

        /// <summary>
        /// PRIVATE
        /// </summary>
        Private,

        /// <summary>
        /// REAL
        /// </summary>
        Real,

        /// <summary>
        /// RELATIVE-OID
        /// </summary>
        RelativeOid,

        /// <summary>
        /// SET
        /// </summary>
        Set,

        /// <summary>
        /// SEQUENCE
        /// </summary>
        Sequence,

        /// <summary>
        /// SIZE
        /// </summary>
        Size,

        /// <summary>
        /// STRING
        /// </summary>
        String,

        /// <summary>
        /// SYNTAX
        /// </summary>
        Syntax,

        /// <summary>
        /// TAGS
        /// </summary>
        Tags,

        /// <summary>
        /// TRUE
        /// </summary>
        True,

        /// <summary>
        /// true
        /// </summary>
        TrueSmall,

        /// <summary>
        /// UNION
        /// </summary>
        Union,

        /// <summary>
        /// UNIQUE
        /// </summary>
        Unique,

        /// <summary>
        /// WITH
        /// </summary>
        With,
    }
}
