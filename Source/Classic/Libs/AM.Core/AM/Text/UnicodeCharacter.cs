// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UnicodeCharacter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class UnicodeCharacter
    {
        #region Constants

        /// <summary>
        /// Space character that prevents an automatic line break
        /// at its position. In some formats, including HTML,
        /// it also prevents consecutive whitespace characters
        /// from collapsing into a single space.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Non-breaking_space
        /// </remarks>
        public const char NoBreakSpace = (char)0xA0;

        /// <summary>
        /// Typographical character for referencing individual
        /// numbered sections of a document, frequently used when
        /// referring to legal code.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Section_sign
        /// </remarks>
        public const char SectionSign = (char)0xA7;

        /// <summary>
        /// Symbol used in copyright notices for works other
        /// than sound recordings.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Copyright_symbol
        /// </remarks>
        public const char CopyrightSign = (char)0xA9;

        /// <summary>
        /// Serves as an invisible marker used to specify a place
        /// in text where a hyphenated break is allowed without
        /// forcing a line break in an inconvenient place if the
        /// text is re-flowed. It becomes visible only after
        /// word wrapping at the end of a line.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Hyphen
        /// </remarks>
        public const char SoftHyphen = (char)0xAD;

        /// <summary>
        /// Symbol that provides notice that the preceding word
        /// or symbol is a trademark or service mark that has been
        /// registered with a national trademark office.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Registered_trademark_symbol
        /// </remarks>
        public const char RegisteredSign = (char)0xAE;

        /// <summary>
        /// Typographical symbol that is used, among other things, 
        /// to represent degrees of arc (e.g. in geographic coordinate
        /// systems), hours (in the medical field), degrees 
        /// of temperature, alcohol proof, or diminished quality 
        /// in musical harmony.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Degree_symbol
        /// </remarks>
        public const char DegreeSign = (char)0xB0;

        /// <summary>
        /// Mathematical symbol with multiple meanings.
        /// <list type="bullet">
        /// <item><description>In mathematics, it generally indicates
        /// a choice of exactly two possible values, one of which 
        /// is the negation of the other.</description></item>
        /// <item><description>In experimental sciences, the sign
        /// commonly indicates the confidence interval or error
        /// in a measurement, often the standard deviation
        /// or standard error. The sign may also represent 
        /// an inclusive range of values that a reading might have.
        /// </description></item>
        /// <item><description>In engineering the sign indicates
        /// the tolerance, which is the range of values that
        /// are considered to be acceptable, safe, or which comply
        /// with some standard, or with a contract.</description></item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Plus-minus_sign
        /// </remarks>
        public const char PlusMinusSign = (char)0xB1;

        /// <summary>
        /// Multiplication sign, also known as the times sign
        /// or the dimension sign.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Multiplication_sign
        /// </remarks>
        public const char MultiplicationSign = (char) 0xD7;

        /// <summary>
        /// Typographic unit equal to the size of a single typographic
        /// figure (numeral or letter), minus leading. Its size can
        /// fluctuate somewhat depending on which font is being used.
        /// This is the preferred space to use in numbers.
        /// It has the same width as a digit and keeps the number
        /// together for the purpose of line breaking.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Figure_space
        /// </remarks>
        public const char FigureSpace = (char) 0x2007;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Whitespace_character#Hair_spaces_around_dashes
        /// </remarks>
        public const char HairSpace = (char)0x200A;

        /// <summary>
        /// Non-printing character used in computerized typesetting
        /// to indicate word boundaries to text processing systems
        /// when using scripts that do not use explicit spacing,
        /// or after characters (such as the slash) that are not
        /// followed by a visible space but after which there may
        /// nevertheless be a line break. Normally, it is not
        /// a visible separation, but it may expand in passages
        /// that are fully justified.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Zero-width_space
        /// </remarks>
        public const char ZeroWidthSpace = (char)0x200B;

        /// <summary>
        /// Punctuation mark used to join words and to separate
        /// syllables of a single word.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Hyphen
        /// </remarks>
        public const char Hyphen = (char)0x2010;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Hyphen#Nonbreaking_hyphens
        /// </remarks>
        public const char NonBreakingHyphen = (char)0x2011;

        /// <summary>
        /// Series of dots (typically three, such as "…") that
        /// usually indicates an intentional omission of a word,
        /// sentence, or whole section from a text without altering
        /// its original meaning.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Ellipsis
        /// </remarks>
        public const char HorizontalEllipsis = (char)0x2026;

        /// <summary>
        /// Thin space. Space character that is usually  1/5 or  1/6
        /// of an em in width. It is used to add a narrow space,
        /// such as between nested quotation marks or to separate
        /// glyphs that interfere with one another.
        /// It is not as narrow as the hair space.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Thin_space
        /// </remarks>
        public const char NarrowNoBreakSpace = (char)0x202F;

        /// <summary>
        /// Thin space. Space character that is usually  1/5 or  1/6
        /// of an em in width. It is used to add a narrow space,
        /// such as between nested quotation marks or to separate
        /// glyphs that interfere with one another.
        /// It is not as narrow as the hair space.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Thin_space
        /// </remarks>
        public const char ThinSpace = (char)0x202F;

        /// <summary>
        /// Sign indicating parts per thousand.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Per_mille
        /// </remarks>
        public const char PerMilleSign = (char)0x2030;

        /// <summary>
        /// Symbol used to designate units and for other purposes
        /// in mathematics, the sciences, linguistics and music.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Prime_(symbol)
        /// </remarks>
        public const char Prime = (char)0x2032;

        /// <summary>
        /// Symbol used to designate units and for other purposes
        /// in mathematics, the sciences, linguistics and music.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Prime_(symbol)
        /// </remarks>
        public const char DoublePrime = (char)0x2033;

        /// <summary>
        /// Symbol used to designate units and for other purposes
        /// in mathematics, the sciences, linguistics and music.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Prime_(symbol)
        /// </remarks>
        public const char TriplePrime = (char)0x2034;

        /// <summary>
        /// Symbol used to designate units and for other purposes
        /// in mathematics, the sciences, linguistics and music.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Prime_(symbol)
        /// </remarks>
        public const char ReversedPrime = (char)0x2035;

        /// <summary>
        /// Symbol used to designate units and for other purposes
        /// in mathematics, the sciences, linguistics and music.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Prime_(symbol)
        /// </remarks>
        public const char ReversedDoublePrime = (char)0x2036;

        /// <summary>
        /// Symbol used to designate units and for other purposes
        /// in mathematics, the sciences, linguistics and music.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Prime_(symbol)
        /// </remarks>
        public const char ReversedTriplePrime = (char)0x2037;

        /// <summary>
        /// Code point in Unicode used to indicate that word
        /// separation should not occur at a position, when using
        /// scripts that do not use explicit spacing. The word joiner
        /// does not produce any space, and prohibits a line break
        /// at its position.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Word_joiner
        /// </remarks>
        public const char WordJoiner = (char) 0x2060;

        /// <summary>
        /// Typographic abbreviation of the word number(s)
        /// indicating ordinal numeration, especially in names
        /// and titles.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Numero_sign
        /// </remarks>
        public const char NumeroSign = (char)0x2116;

        /// <summary>
        /// Symbol to indicate that the preceding mark is a trademark.
        /// It is usually used for unregistered trademarks,
        /// as opposed to the registered trademark symbol which 
        /// is reserved for registered trademarks.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Trademark_symbol
        /// </remarks>
        public const char TradeMarkSign = (char)0x2122;

        /// <summary>
        /// Series of dots (typically three, such as "…") that
        /// usually indicates an intentional omission of a word,
        /// sentence, or whole section from a text without altering
        /// its original meaning.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Ellipsis
        /// </remarks>
        public const char VerticalEllipsis = (char)0x22EE;

        /// <summary>
        /// Series of dots (typically three, such as "…") that
        /// usually indicates an intentional omission of a word,
        /// sentence, or whole section from a text without altering
        /// its original meaning.
        /// </summary>
        /// <remarks>
        /// See https://en.wikipedia.org/wiki/Ellipsis
        /// </remarks>
        public const char HorizontalMidlineEllipsis = (char)0x22EF;

        #endregion
    }
}
