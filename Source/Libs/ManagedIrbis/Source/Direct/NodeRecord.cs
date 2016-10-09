/* NodeRecord.cs -- L01/N01
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Запись в файлах L01 и N01.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Leader={Leader}")]
#endif
    public sealed class NodeRecord
    {
        #region Constants

        /// <summary>
        /// Длина записи в текущей реализации.
        /// </summary>
        public const int RecordSize = 2048;

        #endregion

        #region Properties

        /// <summary>
        /// Лист?
        /// </summary>
        public bool IsLeaf { get; private set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public NodeLeader Leader { get; set; }

        /// <summary>
        /// Ссылки
        /// </summary>
        public List<NodeItem> Items { get { return _items; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор
        /// </summary>
        public NodeRecord()
        {
            Leader = new NodeLeader();
            _items = new List<NodeItem>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public NodeRecord
            (
                bool isLeaf
            )
            : this()
        {
            IsLeaf = isLeaf;
        }

        #endregion

        #region Private members

        private readonly List<NodeItem> _items;

        internal Stream _stream;

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder items = new StringBuilder();
            foreach (NodeItem item in Items)
            {
                items.AppendLine(item.ToString());
            }

            return string.Format
                (
                    "Leader: {0}, Items: {1}", 
                    Leader, 
                    items
                );
        }

        #endregion
    }
}

#endif

