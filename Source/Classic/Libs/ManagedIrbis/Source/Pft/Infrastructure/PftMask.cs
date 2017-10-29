// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftMask.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;

using AM;
using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    //
    // Форматный выход: Сравнить заданное значение с маской
    // (сравнение по маске)
    // &uf(‘=!<маска>!<значение>’)
    // ! – уникальный символ двухстороннего ограничения
    // (может быть любым символом).
    // Маска может содержать принятые символы маскирования * и ?.
    // В общем случае маска может содержать несколько масок,
    // отделенных друг от друга символом вертикальной черты (|).
    // Форматный выход возвращает: 1 – в случае положительного
    // результата сравнения; 0 – в случае отрицательного.
    //

    //
    // Examples
    //
    // ----------------------------------
    // | Mask      | Value     | Result |
    // | ----------|-----------|--------|
    // | *         |           | 1      |
    // | ?         |           | 0      |
    // | Hello     | Hello     | 1      |
    // | Hello     | hello     | 0      |
    // | Hello*    | Hello     | 1      |
    // | Hello?    | Hello     | 0      |
    // | Hel*      | Hello     | 1      |
    // | He??o     | Hello     | 1      |
    // | He??o     | Hell      | 0      |
    // ----------------------------------
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftMask
    {
        #region Nested classes

        class Mask
        {
            #region Properties

            // ReSharper disable MemberCanBePrivate.Local
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

            public bool Active { get; set; }

            [NotNull]
            internal string Value { get; private set; }

            [NotNull]
            public TextNavigator Navigator { get; private set; }

            // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local
            // ReSharper enable UnusedAutoPropertyAccessor.Local
            // ReSharper restore MemberCanBePrivate.Local

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Mask
                (
                    [NotNull] string value
                )
            {
                Active = true;
                Value = value;
                Navigator = new TextNavigator(value);
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Alternative masks.
        /// </summary>
        [NotNull]
        public NonNullCollection<string> Alternatives { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMask()
        {
            Alternatives = new NonNullCollection<string>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMask
            (
                [NotNull] string specification
            )
        {
            Code.NotNull(specification, "specification");

            Alternatives = new NonNullCollection<string>();
            string[] parts = StringUtility.SplitString(specification, "|");
            Alternatives.AddRange(parts);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the text matches one of the masks.
        /// </summary>
        public bool Match
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            Mask[] masks = Alternatives.Select(_ => new Mask(_)).ToArray();
            TextNavigator navigator = new TextNavigator(text);

            while (true)
            {
                Mask[] activeMasks = masks.Where(_ => _.Active).ToArray();
                if (activeMasks.Length == 0)
                {
                    return navigator.IsEOF;
                }

                char valueChar = navigator.ReadChar();
                if (valueChar == TextNavigator.EOF)
                {
                    foreach (Mask mask in activeMasks)
                    {
                        char maskChar = mask.Navigator.ReadChar();
                        switch (maskChar)
                        {
                            case TextNavigator.EOF:
                            case '*':
                                goto ALL_DONE;

                            default:
                                return false;
                        }
                    }
                }

                foreach (Mask mask in activeMasks)
                {
                    char maskChar = mask.Navigator.ReadChar();
                    switch (maskChar)
                    {
                        case TextNavigator.EOF:
                            mask.Active = false;
                            continue;

                        case '?':
                            break;

                        case '*':
                            goto ALL_DONE;

                        default:
                            if (maskChar != valueChar)
                            {
                                mask.Active = false;
                            }
                            break;
                    }
                }
            }

            ALL_DONE: return true;
        }

        #endregion
    }
}
