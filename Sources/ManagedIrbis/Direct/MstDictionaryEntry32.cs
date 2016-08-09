/* MstDictionaryEntry32.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Элемент справочника MST-файла,
    /// описывающий поле переменной длины.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public sealed class MstDictionaryEntry32
    {
        #region Constants

        /// <summary>
        /// Длина элемента справочника MST-файла.
        /// </summary>
        public const int EntrySize = 12;

        #endregion

        #region Properties

        public int Tag { get; set; }

        public int Position { get; set; }

        public int Length { get; set; }

        public byte[] Bytes { get; set; }

        public string Text { get; set; }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format
                (
                    "Tag: {0}, Position: {1}, Length: {2}, Text: {3}",
                    Tag,
                    Position,
                    Length,
                    Text
                );
        }

        #endregion
    }
}
