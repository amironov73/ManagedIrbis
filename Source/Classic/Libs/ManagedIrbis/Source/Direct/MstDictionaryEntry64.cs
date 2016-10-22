/* MstDictionaryEntry64.cs -- MST file dictionary entry
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Runtime.InteropServices;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Элемент справочника MST-файла,
    /// описывающий поле переменной длины.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [StructLayout(LayoutKind.Sequential)]
    public sealed class MstDictionaryEntry64
    {
        #region Constants

        /// <summary>
        /// Длина элемента справочника MST-файла.
        /// </summary>
        public const int EntrySize = 12;

        #endregion

        #region Properties

        /// <summary>
        /// Field tag.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Data offset.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Data length.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Raw data.
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Decoded data.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString ()
        {
            return string.Format 
                ( 
                    "Tag: {0}, Position: {1}, "
                  + "Length: {2}, Text: {3}", 
                    Tag, 
                    Position, 
                    Length,
                    Text
                );
        }

        #endregion
    }
}
