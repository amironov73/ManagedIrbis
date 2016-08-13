/* GblBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// <para>Инструмент для упрощённого построения заданий на
    /// глобальную корректировку.</para>
    /// <para>Пример построения и выполнения задания:</para>
    /// <code>
    /// GblResult result = new GblBuilder()
    ///        .Add("3079", "'1'")
    ///        .Delete("3011")
    ///        .Execute
    ///             (
    ///                 connection,
    ///                 "IBIS",
    ///                 new[] {30, 32, 34}
    ///             );
    /// Console.WriteLine
    ///     (
    ///         "Processed {0} records",
    ///         result.RecordsProcessed
    ///     );
    /// foreach (ProtocolLine line in result.Protocol)
    /// {
    ///     Console.WriteLine(line);
    /// }
    /// </code>
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblBuilder
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblBuilder()
        {
            _statements = new List<GblStatement>();
        }

        #endregion

        #region Private members

        private const string Filler = "XXXXXXXXXXXXXXXXX";
        private const string All = "*";

        private readonly List<GblStatement> _statements;

        #endregion

        #region Public methods

        /// <summary>
        /// Add an arbitrary statement.
        /// </summary>
        [NotNull]
        public GblBuilder AddStatement
            (
                [NotNull] string code,
                [CanBeNull] string parameter1,
                [CanBeNull] string parameter2,
                [CanBeNull] string format1,
                [CanBeNull] string format2
            )
        {
            GblStatement item = new GblStatement
            {
                Command = VerifyCode(code),
                Parameter1 = parameter1,
                Parameter2 = parameter2,
                Format1 = format1,
                Format2 = format2
            };
            _statements.Add(item);

            return this;
        }

        /// <summary>
        /// Command "ADD".
        /// </summary>
        [NotNull]
        public GblBuilder Add
            (
                [NotNull] string field,
                [NotNull] string value
            )
        {
            return AddStatement
                (
                    GblCode.Add,
                    VerifyField(field),
                    All,
                    VerifyValue(value),
                    Filler
                );
        }

        /// <summary>
        /// Command "ADD".
        /// </summary>
        [NotNull]
        public GblBuilder Add
            (
                [NotNull] string field,
                [NotNull] string repeat,
                [NotNull] string value
            )
        {
            return AddStatement
                (
                    GblCode.Add,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(value),
                    Filler
                );
        }

        /// <summary>
        /// Command "CHA".
        /// </summary>
        [NotNull]
        public GblBuilder Change
            (
                [NotNull] string field,
                [NotNull] string fromValue,
                [NotNull] string toValue
            )
        {
            return AddStatement
                (
                    GblCode.Change,
                    VerifyField(field),
                    All,
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        /// <summary>
        /// Command "CHA".
        /// </summary>
        [NotNull]
        public GblBuilder Change
            (
                [NotNull] string field,
                [NotNull] string repeat,
                [NotNull] string fromValue,
                [NotNull] string toValue
            )
        {
            return AddStatement
                (
                    GblCode.Change,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        /// <summary>
        /// Command "DEL".
        /// </summary>
        [NotNull]
        public GblBuilder Delete
            (
                [NotNull] string field,
                [NotNull] string repeat
            )
        {
            return AddStatement
                (
                    GblCode.Delete,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "DEL".
        /// </summary>
        [NotNull]
        public GblBuilder Delete
            (
                [NotNull] string field
            )
        {
            return AddStatement
                (
                    GblCode.Delete,
                    VerifyField(field),
                    All,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "DELR".
        /// </summary>
        [NotNull]
        public GblBuilder DeleteRecord()
        {
            return AddStatement
                (
                    GblCode.DeleteRecord,
                    Filler,
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Execute the GBL on the given database.
        /// </summary>
        [NotNull]
        public GblResult Execute
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");

            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessWholeDatabase
                (
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on the search result.
        /// </summary>
        [NotNull]
        public GblResult Execute
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            return new GlobalCorrector
                (
                    connection,
                    connection.Database
                )
                .ProcessWholeDatabase
                (
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on the search result.
        /// </summary>
        [NotNull]
        public GblResult Execute
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                [NotNull] string searchExpression
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNullNorEmpty(searchExpression, "searchExpression");

            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessSearchResult
                (
                    searchExpression,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given record interval.
        /// </summary>
        [NotNull]
        public GblResult Execute
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                int fromMfn,
                int toMfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.Nonnegative(fromMfn, "fromMfn");
            Code.Nonnegative(toMfn, "toMfn");

            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessInterval
                (
                    fromMfn,
                    toMfn,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given record interval.
        /// </summary>
        [NotNull]
        public GblResult Execute
            (
                [NotNull] IrbisConnection connection,
                int fromMfn,
                int toMfn
            )
        {
            Code.NotNull(connection, "connection");
            Code.Nonnegative(fromMfn, "fromMfn");
            Code.Nonnegative(toMfn, "toMfn");

            return new GlobalCorrector
                (
                    connection,
                    connection.Database
                )
                .ProcessInterval
                (
                    fromMfn,
                    toMfn,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given recordset.
        /// </summary>
        [NotNull]
        public GblResult Execute
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string database,
                [NotNull] IEnumerable<int> recordset
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(recordset, "recordset");

            return new GlobalCorrector
                (
                    connection,
                    database
                )
                .ProcessRecordset
                (
                    recordset,
                    ToStatements()
                );
        }

        /// <summary>
        /// Execute the GBL on given recordset.
        /// </summary>
        [NotNull]
        public GblResult Execute
            (
                [NotNull] IrbisConnection connection,
                [NotNull] IEnumerable<int> recordset
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(recordset, "recordset");

            return new GlobalCorrector
                (
                    connection,
                    connection.Database
                )
                .ProcessRecordset
                (
                    recordset,
                    ToStatements()
                );
        }

        /// <summary>
        /// Command "FI" - closes "IF".
        /// </summary>
        [NotNull]
        public GblBuilder Fi()
        {
            return AddStatement
                (
                    GblCode.Fi,
                    Filler,
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "IF".
        /// </summary>
        [NotNull]
        public GblBuilder If
            (
                [NotNull] string condition
            )
        {
            return AddStatement
                (
                    GblCode.If,
                    VerifyValue(condition),
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "IF".
        /// </summary>
        [NotNull]
        public GblBuilder If
            (
                [NotNull] string condition,
                params GblStatement[] statements
            )
        {
            If(condition);
            _statements.AddRange(statements);

            return Fi();
        }

        /// <summary>
        /// Command "IF"
        /// </summary>
        [NotNull]
        public GblBuilder If
            (
                [NotNull] string condition,
                [NotNull] GblBuilder builder
            )
        {
            return If
                (
                    condition,
                    builder.ToStatements()
                );
        }

        /// <summary>
        /// Comment.
        /// </summary>
        [NotNull]
        public GblBuilder Nop ()
        {
            return AddStatement
                (
                    GblCode.Comment,
                    Filler,
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Comment.
        /// </summary>
        [NotNull]
        public GblBuilder Nop
            (
                [NotNull] string comment
            )
        {
            return AddStatement
                (
                    GblCode.Comment,
                    VerifyValue(comment),
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Comment.
        /// </summary>
        [NotNull]
        public GblBuilder Nop
            (
                [NotNull] string comment1,
                [NotNull] string comment2
            )
        {
            return AddStatement
                (
                    GblCode.Comment,
                    VerifyValue(comment1),
                    VerifyValue(comment2),
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Command "REP".
        /// </summary>
        [NotNull]
        public GblBuilder Replace
            (
                [NotNull] string field,
                [NotNull] string repeat,
                [NotNull] string toValue
            )
        {
            return AddStatement
                (
                    GblCode.Replace,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(toValue),
                    Filler
                );
        }

        /// <summary>
        /// Command "REP".
        /// </summary>
        [NotNull]
        public GblBuilder Replace
            (
                [NotNull] string field,
                [NotNull] string toValue
            )
        {
            return AddStatement
                (
                    GblCode.Replace,
                    VerifyField(field),
                    All,
                    VerifyValue(toValue),
                    Filler
                );
        }

        /// <summary>
        /// Build statement array.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public GblStatement[] ToStatements()
        {
            return _statements.ToArray();
        }

        /// <summary>
        /// Command "UNDO"
        /// </summary>
        [NotNull]
        public GblBuilder Undo
            (
                [NotNull] string version
            )
        {
            return AddStatement
                (
                    GblCode.Undo,
                    VerifyParameter(version),
                    Filler,
                    Filler,
                    Filler
                );
        }

        /// <summary>
        /// Verify command code.
        /// </summary>
        [NotNull]
        public string VerifyCode
            (
                [NotNull] string code
            )
        {
            Code.NotNullNorEmpty(code, "code");

            // TODO some verification?

            return code;
        }

        /// <summary>
        /// Verify field specification.
        /// </summary>
        [NotNull]
        public string VerifyField
            (
                [NotNull] string field
            )
        {
            Code.NotNullNorEmpty(field, "field");

            // TODO some verification?

            return field;
        }

        /// <summary>
        /// Verify format specification.
        /// </summary>
        [NotNull]
        public string VerifyFormat
            (
                [NotNull] string format
            )
        {
            Code.NotNullNorEmpty(format, "format");

            // TODO some verification?

            return format;
        }

        /// <summary>
        /// Verify command parameter.
        /// </summary>
        [NotNull]
        public string VerifyParameter
            (
                [NotNull] string parameter
            )
        {
            Code.NotNullNorEmpty(parameter, "parameter");

            // TODO some verification?

            return parameter;
        }

        /// <summary>
        /// Verify field repeat specification.
        /// </summary>
        [NotNull]
        public string VerifyRepeat
            (
                [NotNull] string repeat
            )
        {
            Code.NotNullNorEmpty(repeat, "repeat");

            // TODO some verification?

            return repeat;
        }

        /// <summary>
        /// Verify value for command.
        /// </summary>
        [NotNull]
        public string VerifyValue
            (
                [NotNull] string value
            )
        {
            Code.NotNullNorEmpty(value, "value");

            // TODO some verification?

            return value;
        }

        #endregion
    }
}
