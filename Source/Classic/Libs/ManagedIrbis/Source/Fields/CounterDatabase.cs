// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CounterDatabase.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// База данных глобальных счётчиков.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CounterDatabase
    {
        #region Constants

        /// <summary>
        /// Имя базы данных по умолчанию.
        /// </summary>
        public const string DefaultName = "COUNT";

        /// <summary>
        /// Префикс для поиска по индексу счётчика.
        /// </summary>
        public const string IndexPrefix = "I=";

        /// <summary>
        /// Префикс для поиска по шаблону счётчика.
        /// </summary>
        public const string TemplatePrefix = "S=";

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [NotNull]
        public string Name { get; private set; }

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CounterDatabase
            (
                [NotNull] IrbisProvider provider
            )
            : this(provider, DefaultName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CounterDatabase
            (
                [NotNull] IrbisProvider provider,
                [NotNull] string name
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNullNorEmpty(name, "name");

            Provider = provider;
            Name = name;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public GlobalCounter CreateCounter
            (
                [NotNull] string index,
                [NotNull] string template
            )
        {
            Code.NotNullNorEmpty(index, "index");
            Code.NotNullNorEmpty(template, "template");

            string saveDatabase = Provider.Database;
            try
            {
                Provider.Database = Name;
                string expression = string.Format
                    (
                        "\"{0}{1}\"",
                        IndexPrefix,
                        index
                    );
                int[] found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    throw new IrbisException();
                }

                GlobalCounter result = new GlobalCounter
                {
                    Index = index,
                    Template = template,
                    NumericValue = 0
                };
                MarcRecord record = result.ToRecord();
                record.Database = Name;
                result.Record = record;
                Provider.WriteRecord(record);

                return result;
            }
            finally
            {
                Provider.Database = saveDatabase;
            }
        }

        /// <summary>
        /// Get the <see cref="GlobalCounter"/> by its index.
        /// </summary>
        [CanBeNull]
        public GlobalCounter GetCounter
            (
                [NotNull] string index
            )
        {
            Code.NotNullNorEmpty(index, "index");

            string saveDatabase = Provider.Database;
            try
            {
                Provider.Database = Name;
                string expression = string.Format
                    (
                        "\"{0}{1}\"",
                        IndexPrefix,
                        index
                    );
                int[] found = Provider.Search(expression);
                if (found.Length == 0)
                {
                    return null;
                }

                MarcRecord record = Provider.ReadRecord(found[0]);
                if (ReferenceEquals(record, null))
                {
                    return null;
                }

                GlobalCounter result = GlobalCounter.Parse(record);

                return result;
            }
            finally
            {
                Provider.Database = saveDatabase;
            }
        }

        /// <summary>
        /// Update the <see cref="GlobalCounter"/>.
        /// </summary>
        public void UpdateCounter
            (
                [NotNull] GlobalCounter counter
            )
        {
            Code.NotNull(counter, "counter");

            string index = counter.Index;
            if (string.IsNullOrEmpty(index))
            {
                throw new IrbisException();
            }

            counter.Verify(true);

            string saveDatabase = Provider.Database;
            try
            {
                Provider.Database = Name;
                MarcRecord record = counter.Record;
                if (ReferenceEquals(record, null))
                {
                    string expression = string.Format
                        (
                            "\"{0}{1}\"",
                            IndexPrefix,
                            index
                        );
                    int[] found = Provider.Search(expression);
                    if (found.Length == 0)
                    {
                        record = counter.ToRecord();
                    }
                    else
                    {
                        record = Provider.ReadRecord(found[0]);
                        if (ReferenceEquals(record, null))
                        {
                            throw new IrbisException();
                        }
                    }

                    record.Database = Name;
                    counter.Record = record;
                }

                Provider.WriteRecord(record);
            }
            finally
            {
                Provider.Database = saveDatabase;
            }
        }

        #endregion
    }
}
