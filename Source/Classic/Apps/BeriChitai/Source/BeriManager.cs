// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BeriManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Mapping;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace BeriChitai
{
    class BeriManager
    {
        #region Constants

        /// <summary>
        /// Статус экземпляра.
        /// </summary>
        public const string StatusPrefix = "BERI=";

        /// <summary>
        /// Книга, доступная для заказа.
        /// </summary>
        public const string FreeBook = "0";

        /// <summary>
        /// Заказанная кем-либо книга.
        /// </summary>
        public const string ReservedBook = "1";

        /// <summary>
        /// Отданная читателю книга.
        /// </summary>
        public const string SurrenderedBook = "2";

        /// <summary>
        /// Дата бронирования.
        /// </summary>
        public const string DatePrefix = "DAB=";

        /// <summary>
        /// Читательский билет.
        /// </summary>
        public const string TicletPrefix = "CAB=";

        /// <summary>
        /// Дата выдачи.
        /// </summary>
        public const string IssuePrefix = "DAV=";

        /// <summary>
        /// Населенный пункт.
        /// </summary>
        public const string LocalityPrefix = "NAP=";

        #endregion

        #region Properties

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; set; }

        /// <summary>
        /// Формат библиографического описания.
        /// </summary>
        public string Format { get; set; }

        #endregion

        #region Construction

        public BeriManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            Format = "@";
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Расширение информации о книгах.
        /// </summary>
        public void ExtendInfo
            (
                [NotNull][ItemNotNull] IEnumerable<BeriInfo> books
            )
        {
            Code.NotNull(books, "books");

            if (!string.IsNullOrEmpty(Format))
            {
                foreach (BeriInfo book in books)
                {
                    if (!ReferenceEquals(book.Record, null))
                    {
                        book.Description = Connection.FormatRecord
                            (
                                Format,
                                book.Record.Mfn
                            );
                    }
                }
            }
        }

        /// <summary>
        /// Получение списка книг с указанным статусом.
        /// </summary>
        public BeriInfo[] GetBooksWithStatus(string status)
        {
            string expression = string.Format
                (
                    "\"{0}{1}\"",
                    StatusPrefix,
                    status
                );

            BatchRecordReader reader
                = (BatchRecordReader)BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    expression,
                    500
                );

            BeriInfo[] result = reader
                .SelectMany(record => BeriInfo.Parse(record))
                .ToArray();

            return result;
        }

        /// <summary>
        /// Получение списка доступных для заказа книг.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public BeriInfo[] GetFreeBooks()
        {
            return GetBooksWithStatus(FreeBook);
        }

        /// <summary>
        /// Получение списка заказанных книг.
        /// </summary>
        public BeriInfo[] GetReservedBooks()
        {
            return GetBooksWithStatus(ReservedBook);
        }

        public BeriInfo[] GetSurrenderedBooks()
        {
            return GetBooksWithStatus(SurrenderedBook);
        }

        #endregion
    }
}
