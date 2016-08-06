/* CharacterClassifier.cs -- классификатор символов Unicode
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Text
{
    /// <summary>
    /// Классы символов Unicode.
    /// </summary>
    [PublicAPI]
    public static class CharacterClassifier
    {
        #region Public methods

        /// <summary>
        /// Выявление классов символов.
        /// </summary>
        public static CharacterClass DetectCharacterClasses
            (
                [CanBeNull] string text
            )
        {
            CharacterClass result = CharacterClass.None;

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            foreach (char c in text)
            {
                int index = c;
                if (index < 0x20)
                {
                    result |= CharacterClass.ControlCharacter;
                }
                else if ((index >= 0x30) && (index < 0x3A))
                {
                    result |= CharacterClass.Digit;
                }
                else if ((index >= 0x40) && (index < 0x80))
                {
                    result |= CharacterClass.BasicLatin;
                }
                else if ((index >= 0x0400) && (index < 0x0500))
                {
                    result |= CharacterClass.Cyrillic;
                }
            }

            return result;
        }

        /// <summary>
        /// Смешаны ли в тексте латиница с кириллицей?
        /// </summary>
        public static bool IsBothCyrillicAndLatin
            (
                CharacterClass value
            )
        {
            return ((value & (CharacterClass.BasicLatin | CharacterClass.Cyrillic))
                    == (CharacterClass.BasicLatin | CharacterClass.Cyrillic));
        }

        #endregion
    }
}
