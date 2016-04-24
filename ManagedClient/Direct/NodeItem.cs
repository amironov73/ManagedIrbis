/* NodeItem.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Справочник в N01/L01 является таблицей, определяющей
    /// поисковый термин. Каждый ключ переменной длины, который
    /// есть в записи, представлен в справочнике одним входом,
    /// формат которого описывает следующая структура
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Length={Length}, KeyOffset={KeyOffset}, Text={Text}")]
    public sealed class NodeItem
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

        public long FullOffset
        {
            get { return unchecked ((((long) HighOffset) << 32) + LowOffset); }
        }

        /// <summary>
        /// Ссылается на лист?
        /// </summary>
        public bool RefersToLeaf
        {
            get { return (LowOffset < 0); }
        }

        /// <summary>
        /// Текстовое значение ключа
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Object members

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
