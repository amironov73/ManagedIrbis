/* GblBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

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
    /// GblResult result = new GblBuilder(connection)
    ///        .Add("3079", "'1'")
    ///        .Delete("3011")
    ///        .Execute(new[] {30, 32, 34});
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

        public IrbisConnection Connection { get; set; }

        public string Database { get; set; }

        #endregion

        #region Construction

        public GblBuilder()
        {
            _statements = new List<GblStatement>();
        }

        public GblBuilder
            (
                IrbisConnection connection
            )
            : this()
        {
            Connection = connection;
        }

        public GblBuilder
            (
                IrbisConnection connection,
                string database
            )
            : this()
        {
            Connection = connection;
            Database = database;
        }

        #endregion

        #region Private members

        private const string Filler = "XXXXXXXXXXXXXXXXX";
        private const string All = "*";

        private readonly List<GblStatement> _statements;

        #endregion

        #region Public methods

        public GblBuilder AddCommand
            (
                string command,
                string parameter1,
                string parameter2,
                string format1,
                string format2
            )
        {
            GblStatement item = new GblStatement
            {
                Command = VerifyCommand(command),
                Parameter1 = parameter1,
                Parameter2 = parameter2,
                Format1 = format1,
                Format2 = format2
            };
            _statements.Add(item);
            return this;
        }

        public string VerifyCommand
            (
                string command
            )
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentException();
            }
            return command;
        }

        public string VerifyField
            (
                string field
            )
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentException();
            }
            // ReSharper disable ObjectCreationAsStatement
            if (!ReferenceEquals(Connection, null))
            {
                // Чисто для проверки, что ссылка на поле задана верно
                new FieldReference("v" + field);
            }
            // ReSharper restore ObjectCreationAsStatement
            return field;
        }

        public string VerifyRepeat
            (
                string repeat
            )
        {
            return repeat;
        }

        public string VerifyValue
            (
                string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException();
            }
            return value;
        }

        public GblBuilder Add
            (
                string field,
                string value
            )
        {
            return AddCommand
                (
                    GblCode.Add,
                    VerifyField(field),
                    All,
                    VerifyValue(value),
                    Filler
                );
        }

        public GblBuilder Add
            (
                string field,
                string repeat,
                string value
            )
        {
            return AddCommand
                (
                    GblCode.Add,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(value),
                    Filler
                );
        }

        public GblBuilder Change
            (
                string field,
                string fromValue,
                string toValue
            )
        {
            return AddCommand
                (
                    GblCode.Change,
                    VerifyField(field),
                    All,
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        public GblBuilder Change
            (
                string field,
                string repeat,
                string fromValue,
                string toValue
            )
        {
            return AddCommand
                (
                    GblCode.Change,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(fromValue),
                    VerifyValue(toValue)
                );
        }

        public GblBuilder Delete
            (
                string field,
                string repeat
            )
        {
            return AddCommand
                (
                    GblCode.Delete,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    Filler,
                    Filler
                );
        }

        public GblBuilder Delete
            (
                string field
            )
        {
            return AddCommand
                (
                    GblCode.Delete,
                    VerifyField(field),
                    All,
                    Filler,
                    Filler
                );
        }

        public GblBuilder Fi()
        {
            return AddCommand
                (
                    GblCode.Fi,
                    Filler,
                    Filler,
                    Filler,
                    Filler
                );
        }

        public GblBuilder If
            (
                string condition
            )
        {
            return AddCommand
                (
                    GblCode.If,
                    VerifyValue(condition),
                    Filler,
                    Filler,
                    Filler
                );
        }

        public GblBuilder Replace
            (
                string field,
                string repeat,
                string toValue
            )
        {
            return AddCommand
                (
                    GblCode.Replace,
                    VerifyField(field),
                    VerifyRepeat(repeat),
                    VerifyValue(toValue),
                    Filler
                );
        }

        public GblBuilder Replace
            (
                string field,
                string toValue
            )
        {
            return AddCommand
                (
                    GblCode.Replace,
                    VerifyField(field),
                    All,
                    VerifyValue(toValue),
                    Filler
                );
        }

        public GblBuilder SetConnection
            (
                IrbisConnection connection
            )
        {
            if (ReferenceEquals(connection, null))
            {
                throw new ArgumentException();
            }
            Connection= connection;
            return this;
        }

        public GblBuilder SetDatabase
            (
                string database
            )
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentException();
            }
            Database = database;
            return this;
        }

        public GblStatement[] ToCommands()
        {
            return _statements.ToArray();
        }

        //public GblResult Execute()
        //{
        //    return new GlobalCorrector
        //        (
        //            Client,
        //            Database
        //        )
        //        .ProcessWholeDatabase
        //        (
        //            ToCommands()
        //        );
        //}

        //public GblFinal Execute
        //    (
        //        string searchExpression
        //    )
        //{
        //    return new GlobalCorrector
        //        (
        //            Client,
        //            Database
        //        )
        //        .ProcessSearchResult
        //        (
        //            searchExpression,
        //            ToCommands()
        //        );
        //}

        //public GblFinal Execute
        //    (
        //        int fromMfn,
        //        int toMfn
        //    )
        //{
        //    return new GlobalCorrector
        //        (
        //            Client,
        //            Database
        //        )
        //        .ProcessInterval
        //        (
        //            fromMfn,
        //            toMfn,
        //            ToCommands()
        //        );
        //}

        //public GblFinal Execute
        //    (
        //        IEnumerable<int> recordset
        //    )
        //{
        //    return new GlobalCorrector
        //        (
        //            Client,
        //            Database
        //        )
        //        .ProcessRecordset
        //        (
        //            recordset,
        //            ToCommands()
        //        );
        //}

        #endregion
    }
}
