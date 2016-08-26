/* CopyRecordManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 * TODO FST transformations
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Copies and/or moves records from one database
    /// to another.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CopyRecordManager
    {
        #region Properties

        /// <summary>
        /// Connection
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Target database name.
        /// </summary>
        [NotNull]
        public string TargetDatabase { get; private set; }

        /// <summary>
        /// Actualize records?
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Lock records?
        /// </summary>
        public bool LockFlag { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CopyRecordManager
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string targetDatabase
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(targetDatabase, targetDatabase);

            Connection = connection;
            TargetDatabase = targetDatabase;
            Actualize = true;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Copy the record to (other) database.
        /// </summary>
        /// <returns>Record clone.</returns>
        [NotNull]
        public MarcRecord CopyRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MarcRecord result = record.Clone();
            result.Mfn = 0;
            result.Database = TargetDatabase;
            Connection.WriteRecord
                (
                    result,
                    LockFlag,
                    Actualize
                );

            return result;
        }

        // =========================================================

        /// <summary>
        /// Copy the record to (other) database.
        /// </summary>
        [NotNull]
        public MarcRecord CopyRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            MarcRecord record = Connection.ReadRecord(mfn);
            MarcRecord result = CopyRecord(record);

            return result;
        }

        // =========================================================

        /// <summary>
        /// Move the record to (other) database.
        /// </summary>
        /// <returns>Record clone.</returns>
        [NotNull]
        public MarcRecord MoveRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            MarcRecord result = CopyRecord(record);
            int mfn = record.Mfn;
            if (mfn != 0)
            {
                Connection.DeleteRecord(mfn);
            }

            return result;
        }

        // =========================================================

        /// <summary>
        /// Move the record to (other) database.
        /// </summary>
        [NotNull]
        public MarcRecord MoveRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            MarcRecord record = Connection.ReadRecord(mfn);
            MarcRecord result = CopyRecord(record);
            Connection.DeleteRecord(mfn);

            return result;
        }

        #endregion
    }
}
