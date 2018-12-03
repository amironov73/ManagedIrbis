// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeItem32.cs -- record item for L01/N01
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Справочник в N01/L01 является таблицей, определяющей
    /// поисковый термин. Каждый ключ переменной длины, который
    /// есть в записи, представлен в справочнике одним входом,
    /// формат которого описывает следующая структура
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Length={Length}, KeyOffset={KeyOffset}, Text={Text}")]
    public sealed class NodeItem32
    {
        #region Properties

        /// <summary>
        /// Длина ключа
        /// </summary>
        public short Length { get; set; }

        /// <summary>
        /// Смещение ключа от начала записи
        /// </summary>
        public short KeyOffset { get; set; }

        /// <summary>
        /// Младшее слово смещения
        /// </summary>
        public int LowOffset { get; set; }

        /// <summary>
        /// Старшее слово смещения
        /// </summary>
        public int HighOffset { get; set; }

        /// <summary>
        /// Полное смещение
        /// </summary>
        public long FullOffset
        {
            get { return unchecked(((long)HighOffset << 32) + LowOffset); }
        }

        /// <summary>
        /// Ссылается на лист?
        /// </summary>
        public bool RefersToLeaf
        {
            get { return LowOffset < 0; }
        }

        /// <summary>
        /// Текстовое значение ключа
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Length: {0}, KeyOffset: {1}, "
                    + "LowOffset: {2}, HighOffset: {3}, "
                    + "FullOffset: {4}, RefersToLeaf: {5}, "
                    + "Text: {6}",
                    Length,
                    KeyOffset,
                    LowOffset,
                    HighOffset,
                    FullOffset,
                    RefersToLeaf,
                    Text
                );
        }

        #endregion
    }
}
