/* CharacterClass.cs -- класс символов Unicode.
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Класс символов Unicode.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum CharacterClass
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Control character.
        /// </summary>
        ControlCharacter = 0x01,

        /// <summary>
        /// Digit.
        /// </summary>
        Digit = 0x02,

        /// <summary>
        /// Basic Latin.
        /// </summary>
        BasicLatin = 0x04,

        /// <summary>
        /// Cyrillic.
        /// </summary>
        Cyrillic = 0x08
    }
}
