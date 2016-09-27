﻿/* PftTokenKind.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Token kind.
    /// </summary>
    public enum PftTokenKind
    {
        /// <summary>
        /// No tokens.
        /// </summary>
        None,

        /// <summary>
        /// a(v200)
        /// </summary>
        A,

        /// <summary>
        /// !
        /// </summary>
        Bang,

        /// <summary>
        /// break
        /// </summary>
        Break,

        /// <summary>
        /// c10
        /// </summary>
        C,

        /// <summary>
        /// chr(32)
        /// </summary>
        Chr,

        /// <summary>
        /// :
        /// </summary>
        Colon,

        /// <summary>
        /// ,
        /// </summary>
        Comma,

        /// <summary>
        /// 'literal'
        /// </summary>
        ConditionalLiteral,

        /// <summary>
        /// else
        /// </summary>
        Else,

        /// <summary>
        /// =
        /// </summary>
        Equals,

        /// <summary>
        /// f(123)
        /// </summary>
        F,

        /// <summary>
        /// f2(123)
        /// </summary>
        F2,

        /// <summary>
        /// fi
        /// </summary>
        Fi,

        /// <summary>
        /// #
        /// </summary>
        Hash,

        /// <summary>
        /// ^
        /// </summary>
        Hat,

        /// <summary>
        /// identifier
        /// </summary>
        Identifier,

        /// <summary>
        /// if
        /// </summary>
        If,

        /// <summary>
        /// l(format)
        /// </summary>
        L,

        /// <summary>
        /// {
        /// </summary>
        LeftCurly,

        /// <summary>
        /// [
        /// </summary>
        LeftSquare,

        /// <summary>
        /// &lt;
        /// </summary>
        Less,

        /// <summary>
        /// &lt;=
        /// </summary>
        LessEqual,

        /// <summary>
        /// (
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// mfn
        /// </summary>
        Mfn,

        /// <summary>
        /// mpl, mhl etc
        /// </summary>
        Mpl,

        /// <summary>
        /// -
        /// </summary>
        Minus,

        /// <summary>
        /// &gt;
        /// </summary>
        More,

        /// <summary>
        /// &gt;=
        /// </summary>
        MoreEqual,

        /// <summary>
        /// not
        /// </summary>
        Not,

        /// <summary>
        /// &lt;&gt;
        /// </summary>
        NotEqual1,

        /// <summary>
        /// !=
        /// </summary>
        NotEqual2,

        /// <summary>
        /// 123.45
        /// </summary>
        Number,

        /// <summary>
        /// or
        /// </summary>
        Or,

        /// <summary>
        /// p(v200)
        /// </summary>
        P,

        /// <summary>
        /// %
        /// </summary>
        Percent,

        /// <summary>
        /// +
        /// </summary>
        Plus,

        /// <summary>
        /// ravr
        /// </summary>
        Ravr,

        /// <summary>
        /// |literal|
        /// </summary>
        RepeatableLiteral,

        /// <summary>
        /// }
        /// </summary>
        RightCurly,

        /// <summary>
        /// ]
        /// </summary>
        RightSquare,

        /// <summary>
        /// )
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// ref
        /// </summary>
        Ref,

        /// <summary>
        /// rmax
        /// </summary>
        Rmax,

        /// <summary>
        /// rmin
        /// </summary>
        Rmin,

        /// <summary>
        /// rsum
        /// </summary>
        Rsum,

        /// <summary>
        /// s(format)
        /// </summary>
        S,

        /// <summary>
        /// ;
        /// </summary>
        Semicolon,

        /// <summary>
        /// /
        /// </summary>
        Slash,

        /// <summary>
        /// *
        /// </summary>
        Star,

        /// <summary>
        /// then
        /// </summary>
        Then,

        /// <summary>
        /// ~
        /// </summary>
        Tilda,

        /// <summary>
        /// &amp;uf('0')
        /// </summary>
        Unifor,

        /// <summary>
        /// 'literal'
        /// </summary>
        UnkonditionalLiteral,

        /// <summary>
        /// v200
        /// </summary>
        V,

        /// <summary>
        /// val
        /// </summary>
        Val,

        /// <summary>
        /// x10
        /// </summary>
        X
    }
}
