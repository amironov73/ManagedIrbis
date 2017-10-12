// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchReadCommand.cs -- search and read records from IRBIS-server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Linq;

using AM;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Infrastructure.Commands
{
    /// <summary>
    /// Search and read records from IRBIS-server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SearchReadCommand
        : SearchCommand
    {
        #region Properties

        /// <summary>
        /// Use Read command instead.
        /// </summary>
        /// <remarks>
        /// Дело в том, что методу SearchRead ИРБИС-сервер
        /// частенько возвращает «немодифицируемые» записи
        /// (он банально неправильно проставляет флаги
        /// и версию записи, а потом саботирует сохранение
        /// изменений в них). При считывании записи методом
        /// ReadRecord приходят гарантировано «модифицируемые»
        /// записи.
        /// (из переписки с пользователями)
        /// </remarks>
        public static bool UseReadInsteadOfFormat { get; set; }

        /// <summary>
        /// Format specification (always ALL).
        /// </summary>
        public override string FormatSpecification
        {
            get
            {
                return UseReadInsteadOfFormat
                    ? null
                    : IrbisFormat.All;
            }
            set
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Read records.
        /// </summary>
        [CanBeNull]
        public MarcRecord[] Records { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchReadCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
        }

        #endregion

        #region Private members

        private MarcRecord _ConvertRecord
            (
                FoundItem item
            )
        {
            MarcRecord result = new MarcRecord
            {
                HostName = Connection.Host,
                Database = Database ?? Connection.Database
            };
            ProtocolText.ParseResponseForAllFormat
                (
                    item.Text.ThrowIfNull("item.Text"),
                    result
                );
            Debug.Assert
                (
                    item.Mfn == result.Mfn,
                    "item.Mfn == result.Mfn"
                );

            return result;
        }

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                ClientQuery clientQuery
            )
        {
            ServerResponse result = base.Execute(clientQuery);

            if (result.ReturnCode == 0)
            {
                if (UseReadInsteadOfFormat)
                {
                    int[] mfns = FoundItem.ConvertToMfn
                        (
                            Found.ThrowIfNull("Found")
                        );
                    Records = Connection.ReadRecords
                        (
                            Database ?? Connection.Database,
                            mfns
                        );
                }
                else
                {
                    Records = Found
                        .ThrowIfNull("Found")

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

                        .AsParallel()
                        .AsOrdered()

#endif

                        .Select
                            (
                                item => _ConvertRecord(item)
                            )
                        .ToArray();
                }
            }

            return result;
        }

        #endregion
    }
}
