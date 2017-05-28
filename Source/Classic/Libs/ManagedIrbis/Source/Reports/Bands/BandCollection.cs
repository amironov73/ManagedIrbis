// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BandCollection.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using AM;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BandCollection<T>
        : Collection<T>,
        IHandmadeSerializable,
        IReadOnly<BandCollection<T>>,
        IVerifiable,
        IDisposable
        where T: ReportBand
    {
        #region Properties

        /// <summary>
        /// Parent band.
        /// </summary>
        [CanBeNull]
        public ReportBand Parent
        {
            get { return _parent; }
            internal set { SetParent(value); }
        }

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        public IrbisReport Report
        {
            get { return _report; }
            internal set { SetReport(value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BandCollection()
            : this (null, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BandCollection 
            (
                [CanBeNull] IrbisReport report,
                [CanBeNull] ReportBand parent
            )
        {
            Report = report;
            Parent = parent;
        }

        #endregion

        #region Private members

        private ReportBand _parent;

        private IrbisReport _report;

        internal void SetParent
            (
                [CanBeNull] ReportBand parent
            )
        {
            _parent = parent;

            foreach (T band in this)
            {
                band.Parent = parent;
            }
        }

        internal void SetReport
            (
                IrbisReport report
            )
        {
            _report = report;

            foreach (T band in this)
            {
                band.Report = report;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add range of <see cref="RecordField"/>s.
        /// </summary>
        public void AddRange
            (
                [NotNull] IEnumerable<T> bands
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(bands, "bands");

            foreach (T band in bands)
            {
                Add(band);
            }
        }

        /// <summary>
        /// Создание клона коллекции.
        /// </summary>
        [NotNull]
        public BandCollection<T> Clone()
        {
            BandCollection<T> result
                = new BandCollection<T> (Report, Parent);

            foreach (T band in this)
            {
                T clone = (T)band.Clone();
                result.Add(clone);
            }

            return result;
        }

        /// <summary>
        /// Find first occurrence of the field with given predicate.
        /// </summary>
        [CanBeNull]
        public ReportBand Find
            (
                [NotNull] Predicate<ReportBand> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this.FirstOrDefault
                (
                    band => predicate(band)
                );
        }

        /// <summary>
        /// Find all occurrences of the field
        /// with given predicate.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public T[] FindAll
            (
                [NotNull] Predicate<ReportBand> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this.Where
                (
                    band => predicate(band)
                )
                .ToArray();
        }

        /// <summary>
        /// Render bands.
        /// </summary>
        public void Render
            (
                [NotNull] ReportContext context
            )
        {
            Code.NotNull(context, "context");

            foreach (T band in this)
            {
                band.Render(context);
            }
        }

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.ClearItems" />
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (T band in this)
            {
                band.Report = null;
                band.Parent = null;
            }

            base.ClearItems();
        }

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                [NotNull] T item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Report = Report;
            item.Parent = Parent;

            base.InsertItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if (index >= 0 && index < Count)
            {
                ReportBand band  = this[index];
                if (!ReferenceEquals(band, null))
                {
                    band.Report = null;
                    band.Parent = null;
                }
            }

            base.RemoveItem(index);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                [NotNull] T item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Report = Report;
            item.Parent = Parent;

            base.SetItem(index, item);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(reader, "reader");

            ClearItems();

            // TODO implement

            //RecordField[] array = reader.ReadArray<RecordField>();
            //AddRange(array);

            Log.Error
                (
                    "BandCollection::RestoreFromStream: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            // TODO implement

            //writer.WriteArray(this.ToArray());

            Log.Error
                (
                    "BandCollection::SaveToStream: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        #endregion

        #region IReadOnly<T> members

        // ReSharper disable InconsistentNaming
        internal bool _readOnly;
        // ReSharper restore InconsistentNaming

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        public bool ReadOnly { get { return _readOnly; } }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public BandCollection<T> AsReadOnly()
        {
            BandCollection<T> result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            _readOnly = true;
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Log.Error
                    (
                        "BandCollection::ThrowIfReadOnly"
                    );

                throw new ReadOnlyException();
            }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BandCollection<T>> verifier
                = new Verifier<BandCollection<T>>(this, throwOnError);

            foreach (T band in this)
            {
                verifier.VerifySubObject(band, "band");
            }

            return verifier.Result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            foreach (T item in Items)
            {
                item.Dispose();
            }
        }

        #endregion
    }
}
