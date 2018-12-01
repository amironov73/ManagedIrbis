// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordGuard.cs -- ensures that the record will be saved
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Ensures that the record(s) will be saved.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordGuard
        : IDisposable,
        IVerifiable
    {
        #region Events

        /// <summary>
        /// Raised on <see cref="Commit"/> call.
        /// </summary>
        public event EventHandler CommitChanges;

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [CanBeNull]
        public IIrbisConnection Connection { get; private set; }

        /// <summary>
        /// Provider.
        /// </summary>
        [CanBeNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Records to guard.
        /// </summary>
        [NotNull]
        public IEnumerable<MarcRecord> Records
        {
            get { return _records; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordGuard
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(record, "record");

            Connection = connection;
            _records = new NonNullCollection<MarcRecord>
            {
                record
            };
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordGuard
            (
                [NotNull] IIrbisConnection connection,
                params MarcRecord[] records
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            _records = new NonNullCollection<MarcRecord>();
            _records.AddRange(records);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordGuard
            (
                [NotNull] IrbisProvider provider,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(record, "record");

            Provider = provider;
            _records = new NonNullCollection<MarcRecord>
            {
                record
            };
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordGuard
            (
                [NotNull] IrbisProvider provider,
                params MarcRecord[] records
            )
        {
            Code.NotNull(provider, "provider");

            Provider = provider;
            _records = new NonNullCollection<MarcRecord>();
            _records.AddRange(records);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~RecordGuard()
        {
            Log.Error
                (
                    "RecordGuard::Destructor: "
                    + "destructor called!"
                );

            Dispose();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<MarcRecord> _records;

        #endregion

        #region Public methods

        /// <summary>
        /// Add the record to the guard.
        /// </summary>
        [NotNull]
        public RecordGuard AddRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            if (_records.Contains(record))
            {
                Log.Trace
                    (
                        "RecordGuard::AddRecord: "
                        + "already have record="
                        + record
                    );
            }
            else
            {
                _records.Add(record);
            }

            return this;
        }

        /// <summary>
        /// Commit changes if any.
        /// </summary>
        [NotNull]
        public RecordGuard Commit()
        {
            bool result = false;

            CommitChanges.Raise(this);
            foreach (MarcRecord record in Records)
            {
                if (record.Modified)
                {
                    if (!ReferenceEquals(Provider, null))
                    {
                        Provider.WriteRecord(record);
                        result = true;
                    }
                    else if (!ReferenceEquals(Connection, null))
                    {
                        Connection.WriteRecord(record);
                        result = true;
                    }

                    record.Modified = false;
                }
            }

            Log.Trace
                (
                    "RecordGuard::Commit: "
                    + "result="
                    + result
                );

            return this;
        }

        /// <summary>
        /// Remove the record.
        /// </summary>
        [NotNull]
        public RecordGuard RemoveRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            _records.Remove(record);

            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Log.Trace
                (
                    "RecordGuard::Dispose"
                );

            Commit();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<RecordGuard> verifier
                = new Verifier<RecordGuard>(this, throwOnError);

            verifier
                .Assert(!ReferenceEquals(Provider, null)
                        || !ReferenceEquals(Connection, null));

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            MarcRecord record = _records.FirstOrDefault();
            if (!ReferenceEquals(record, null))
            {
                result.AppendFormat
                    (
                        "{0} {1} {2}",
                        record.Database,
                        record.Mfn,
                        record.Modified
                    );
            }

            return result.ToString();
        }

        #endregion
    }
}
