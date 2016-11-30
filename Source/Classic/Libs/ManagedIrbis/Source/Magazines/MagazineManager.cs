// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MagazineManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Batch;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Работа с периодикой.
    /// </summary>
    [MoonSharpUserData]
    public sealed class MagazineManager
    {
        #region Constants

        /// <summary>
        /// Вид документа – сводное описание газеты.
        /// </summary>
        public const string Newspaper = "V=01";

        /// <summary>
        /// Вид документа – сводное описание журнала.
        /// </summary>
        public const string Magazine = "V=02";

        #endregion

        #region Properties

        /// <summary>
        /// Клиент для связи с сервером.
        /// </summary>
        [NotNull]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public IrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MagazineManager
            (
                IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Получение перечня всех журналов из базы.
        /// </summary>
        [NotNull]
        public MagazineInfo[] GetAllMagazines()
        {
            List<MagazineInfo> result = new List<MagazineInfo>();

            IEnumerable<MarcRecord> batch = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    Newspaper + " + " + Magazine,
                    1000
                );
            foreach (MarcRecord record in batch)
            {
                if (!ReferenceEquals(record, null))
                {
                    MagazineInfo magazine = MagazineInfo.Parse(record);
                    if (!ReferenceEquals(magazine, null))
                    {
                        result.Add(magazine);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение журнала по его выпуску.
        /// </summary>
        [CanBeNull]
        public MagazineInfo GetMagazine
            (
                [NotNull] MagazineIssueInfo issue
            )
        {
            Code.NotNull(issue, "issue");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение выпуска журнала по статье из этого выпуска.
        /// </summary>
        [CanBeNull]
        public MagazineIssueInfo GetIssue
            (
                [NotNull] MagazineArticleInfo article
            )
        {
            Code.NotNull(article, "article");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение списка выпусков данного журнала.
        /// </summary>
        [NotNull]
        public MagazineIssueInfo[] GetIssues
            (
                [NotNull] MagazineInfo magazine
            )
        {
            Code.NotNull(magazine, "magazine");

            string searchExpression = string.Format
                (
                        "\"I={0}/$\"",
                        magazine.Index
                );
            IEnumerable<MarcRecord> records
                = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    searchExpression,
                    1000
                );

            MagazineIssueInfo[] result = records

#if !WINMOBILE && !PocketPC

                .Select(MagazineIssueInfo.Parse)

#else

                .Select(record => MagazineIssueInfo.Parse(record))

#endif

                .NonNullItems()
                .ToArray();

            return result;
        }

        /// <summary>
        /// Получение списка статей из выпуска.
        /// </summary>
        [NotNull]
        public MagazineArticleInfo[] GetArticles
            (
                [NotNull] MagazineIssueInfo issue
            )
        {
            Code.NotNull(issue, "issue");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Создание журнала в базе по описанию.
        /// </summary>
        [CanBeNull]
        public MagazineManager CreateMagazine
            (
                [NotNull] MagazineInfo magazine
            )
        {
            Code.NotNull(magazine, "magazine");

            throw new NotImplementedException();
        }

        #endregion
    }
}
