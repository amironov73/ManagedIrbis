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
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Batch;
using ManagedIrbis.Fields;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

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
        public IIrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MagazineManager
            (
                IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

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
                    "VRL=J",
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

            Log.Error
                (
                    "MagazineManager::GetMagazine: "
                    + "not implemented"
                );

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

            Log.Error
                (
                    "MagazineManager::GetIssue: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение выпуска журнала с указанным номером.
        /// </summary>
        [CanBeNull]
        public MagazineIssueInfo GetIssue
            (
                [NotNull] MagazineInfo magazine,
                [NotNull] string year,
                [NotNull] string number
            )
        {
            Code.NotNull(magazine, "magazine");
            Code.NotNullNorEmpty(year, "year");
            Code.NotNullNorEmpty(number, "number");

            string index = magazine.Index + "/" + year + "/" + number;
            MarcRecord record = Connection.SearchReadOneRecord("\"I={0}\"", index);
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            return MagazineIssueInfo.Parse(record);
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
                        "\"I933={0}/$\"",
                        magazine.Index
                );
            IEnumerable<MarcRecord> records = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    searchExpression,
                    1000
                );

            MagazineIssueInfo[] result = records
                .Select(record => MagazineIssueInfo.Parse(record))
                .NonNullItems()
                .ToArray();

            return result;
        }

        /// <summary>
        /// Получение списка выпусков данного журнала.
        /// </summary>
        [NotNull]
        public MagazineIssueInfo[] GetIssues
            (
                [NotNull] MagazineInfo magazine,
                [NotNull] string year
            )
        {
            Code.NotNull(magazine, "magazine");
            Code.NotNullNorEmpty(year, "year");

            string searchExpression = string.Format
                (
                        "\"I={0}/{1}/$\"",
                        magazine.Index,
                        year
                );
            IEnumerable<MarcRecord> records = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    searchExpression,
                    1000
                );

            MagazineIssueInfo[] result = records
                .Select(record => MagazineIssueInfo.Parse(record))
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

            string searchExpression = string.Format
                (
                    "\"II={0}\"",
                    issue.Index
                );
            IEnumerable<MarcRecord> records = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    searchExpression,
                    1000
                );

            MagazineArticleInfo[] result = records
                .Select(record => MagazineArticleInfo.ParseAsp(record))
                .NonNullItems()
                .ToArray();

            return result;
        }

        /// <summary>
        /// Подсчёт числа статей, расписанных в рабочем листе ASP.
        /// </summary>
        public int CountExternalArticles
            (
                [NotNull] MagazineIssueInfo issue
            )
        {
            Code.NotNull(issue, "issue");

            string searchExpression = string.Format
                (
                    "\"II={0}\"",
                    issue.Index
                );
            int result = Connection.SearchCount(searchExpression);

            return result;
        }

        /// <summary>
        /// Создание журнала в базе по описанию.
        /// </summary>
        [NotNull]
        public MagazineIssueInfo CreateMagazine
            (
                [NotNull] MagazineInfo magazine,
                [NotNull] string year,
                [NotNull] string issue,
                [CanBeNull] ExemplarInfo[] exemplars
            )
        {
            Code.NotNull(magazine, "magazine");
            Code.NotNullNorEmpty(year, "year");
            Code.NotNullNorEmpty(issue, "issue");

            string fullIndex = magazine.Index + "/" + year + "/" + issue;
            MagazineIssueInfo result = new MagazineIssueInfo
            {
                Index = fullIndex,
                DocumentCode = fullIndex,
                MagazineCode = magazine.Index,
                Year = year,
                Number = issue,
                Worksheet = "NJ",
                Exemplars = exemplars
            };

            return result;
        }

        #endregion
    }
}
