// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NodeRecord.cs -- L01/N01
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable InconsistentNaming

namespace ManagedIrbis.Direct
{
    //
    // Extract from official documentation:
    // http://sntnarciss.ru/irbis/spravka/wtcp006005020.htm
    //
    // Файлы N01 и L01 содержат  в себе индексы словаря поисковых
    // терминов и состоят из записей (блоков) постоянной длины.
    // Записи состоят из трех частей: лидера, справочника
    // и ключей переменной длины.
    //
    // Формат лидера записи
    // Число бит Параметр
    // 32        NUMBER – номер записи(начиная с 1;
    //           в N01 файле номер первой записи равен
    //           номеру корневой записи дерева);
    // 32        PREV – номер предыдущей записи(если нет = -1);
    // 32        NEXT – номер следующей записи(если нет = -1);
    // 16        TERMS – число ключей в записи;
    // 16        OFFSET_FREE – смещение на свободную позицию
    //           в записи(от начала записи);
    //
    // Формат справочника
    //
    // На заметку: Справочник это таблица, определяющая поисковый термин.
    //
    // Каждый ключ переменной длины, который есть в записи,
    // представлен в справочнике одним входом следующего формата:
    // Число бит Параметр
    // 16        LEN – длина ключа;
    // 16        OFFSET_KEY – смещение на ключ(от начала записи);
    // 32        LOW –
    //             В N01 файле:
    //             ссылка на запись файла N01(если LOW > 0) или файла L01
    //             (если LOW < 0), у которых 1-й ключ равен данному.
    //             Положительное значение LOW определяет ветку индекса
    //             иерархически более низкого уровня.Самый низкий уровень
    //             индекса (LOW < 0) соответствует ссылкам на записи
    //             (листья) файла L01;
    //             В L01 файле:
    //             младшее слово 8 байтового смещения на ссылочную
    //             запись в IFP;
    // 32          HIGH –
    //             В N01 файле:
    //             всегда 0;
    //             В L01 файле:
    //             старшее слово 8 байтового смещения на ссылочную запись в IFP
    //
    // Ключи переменной длины
    // записываются начиная с конца записи, так что порядок входов,
    // соответствующих им, определяется алфавитным порядком ключей
    //
    // Сами ключи располагаются вплотную друг к другу без разделителей
    // в порядке поступления на запись.
    //
    // Длина справочника 12*TERMS.
    // Длина ключей = Размер записи – OFSET_FREE.
    // Размер свободного места в записи = 16 + 12 * TERMS - длина ключей.
    // Размер записи зависит от реализации и может быть равен в байтах:
    // 512 ; 1024 ; 2048 ; 4096.
    //

    /// <summary>
    /// Запись в файлах L01 и N01.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Leader={Leader}")]
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

        #region Public methods

        /// <summary>
        /// Dump the record.
        /// </summary>
        public void DumpRecord
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteLine(ToString());
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder items = new StringBuilder();
            foreach (NodeItem item in Items)
            {
                items.AppendLine(item.ToString());
            }

            return string.Format
                (
                    "{0}{1}{2}",
                    Leader,
                    Environment.NewLine,
                    items
                );
        }

        #endregion
    }
}

