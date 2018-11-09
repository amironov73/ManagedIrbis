// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BitrixConnection.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

using JetBrains.Annotations;

#endregion

namespace BitrixAdapter
{
    /// <summary>
    /// Подключение к Bitrix.
    /// </summary>
    [PublicAPI]
    public class BitrixConnection
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// База данных.
        /// </summary>
        public DbManager Db { get; private set; }

        /// <summary>
        /// Инфоблоки.
        /// </summary>
        public Table<InfoBlock> Blocks
        {
            get { return Db.GetTable<InfoBlock>(); }
        }

        /// <summary>
        /// Элементы инфоблоков.
        /// </summary>
        public Table<InfoBlockElement> Elements
        {
            get { return Db.GetTable<InfoBlockElement>(); }
        }

        /// <summary>
        /// Свойства элементов.
        /// </summary>
        public Table<InfoBlockElementProperty> ElementProperties
        {
            get{ return Db.GetTable<InfoBlockElementProperty>(); }
        }

        /// <summary>
        /// Свойства инфоблоков.
        /// </summary>
        public Table<InfoBlockProperty> Properties
        {
            get { return Db.GetTable<InfoBlockProperty>(); }
        }

        /// <summary>
        ///
        /// </summary>
        public Table<InfoBlockPropertyEnum> PropertyEnums
        {
            get { return Db.GetTable<InfoBlockPropertyEnum>(); }
        }

        #endregion

        #region Construction

        public BitrixConnection()
        {
            Db = new DbManager();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Db.Dispose();
        }

        #endregion
    }
}
