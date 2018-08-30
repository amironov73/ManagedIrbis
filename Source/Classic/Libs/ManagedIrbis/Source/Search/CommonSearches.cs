// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CommonSearches.cs -- наиболее распространенные поиски.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Наиболее распространенные поиски.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class CommonSearches
    {
        #region Constants

        /// <summary>
        /// Ключевые слова.
        /// </summary>
        public const string KeywordPrefix = "K=";

        /// <summary>
        /// Индивидуальный автор, редактор, составитель.
        /// </summary>
        public const string AuthorPrefix = "A=";

        /// <summary>
        /// Коллектив или мероприятие.
        /// </summary>
        public const string CollectivePrefix = "M=";

        /// <summary>
        /// Заглавие.
        /// </summary>
        public const string TitlePrefix = "T=";

        /// <summary>
        /// Инвентарный номер, штрих-код или радиометка.
        /// </summary>
        public const string InventoryPrefix = "IN=";

        #endregion

        #region Public methods

        /// <summary>
        /// Поиск единственной записи, содержащей экземпляр с указанным номером
        /// (или штрих-кодом или радио-меткой).
        /// Запись может отсуствовать, это не будет считаться ошибкой.
        /// </summary>
        /// <returns></returns>
        public static MarcRecord ByInventory
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string inventory
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(inventory, "inventory");

            return SingleOrDefault(connection, InventoryPrefix, inventory);
        }


        /// <summary>
        /// Поиск первой попавшейся записи, удовлетворяющей указанному условию.
        /// Запись может отсуствовать, это не будет считаться ошибкой.
        /// </summary>
        /// <returns>Найденную запись либо <c>null</c>.</returns>
        [CanBeNull]
        public static MarcRecord FirstOrDefault
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string prefix,
                [NotNull] string value
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(value, "value");

            MarcRecord result = connection.SearchReadOneRecord("\"{0}{1}\"", prefix, value);

            return result;
        }

        /// <summary>
        /// Поиск единственной записи, удовлетворяющей указанному условию.
        /// Запись может отсуствовать, это не будет считаться ошибкой.
        /// </summary>
        /// <returns>Найденную запись либо <c>null</c>.</returns>
        /// <exception cref="IrbisException">Если найдено более одной записи.</exception>
        public static MarcRecord SingleOrDefault
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string prefix,
                [NotNull] string value
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(value, "value");

            string expression = string.Format("\"{0}{1}\"", prefix, value);
            SearchReadCommand command
                = connection.CommandFactory.GetSearchReadCommand();
            command.Database = connection.Database;
            command.SearchExpression = expression;
            command.NumberOfRecords = 2;

            connection.ExecuteCommand(command);

            MarcRecord[] found = command.Records
                .ThrowIfNull("command.Records");
            if (found.Length > 1)
            {
                throw new IrbisException("Too many records found");
            }

            MarcRecord result = found.GetItem(0);

            return result;
        }

        /// <summary>
        /// Поиск единственной записи, удовлетворяющей данному условию.
        /// Запись должна существовать.
        /// </summary>
        /// <returns>Найденную запись.</returns>
        /// <exception cref="IrbisException">Найдено более одной записи,
        /// либо вообще ничего не найдено.</exception>
        public static MarcRecord Required
            (
                [NotNull] this IIrbisConnection connection,
                [NotNull] string prefix,
                [NotNull] string value
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(prefix, "prefix");
            Code.NotNullNorEmpty(value, "value");

            MarcRecord result = SingleOrDefault(connection, prefix, value);
            if (ReferenceEquals(result, null))
            {
                throw new IrbisException
                    (
                        string.Format("Not found: {0}{1}", prefix, value)
                    );
            }

            return result;
        }

        #endregion
    }
}
