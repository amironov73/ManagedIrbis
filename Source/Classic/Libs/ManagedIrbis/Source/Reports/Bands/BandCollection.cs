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
using System.Xml.Serialization;

using AM;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

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
        IReadOnly<BandCollection<T>>
        where T: ReportBand
    {
        #region Properties

        /// <summary>
        /// Group.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public GroupBand Group { get; internal set; }

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        public IrbisReport Report { get { return _report; } }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private IrbisReport _report;

        // ReSharper disable InconsistentNaming

        internal BandCollection<T> _SetReport
            (
                IrbisReport report
            )
        {
            _report = report;

            foreach (T band in this)
            {
                band.Report = report;
            }

            return this;
        }

        // ReSharper restore InconsistentNaming

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
            BandCollection<T> result = new BandCollection<T>
            {
                Group = Group,
                _report = Report
            };

            foreach (T band in this)
            {
                T clone = (T)band.Clone();
                clone.Report = Report;
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
        public ReportBand[] FindAll
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

        #endregion

        #region Collection<T> members

        /// <inheritdoc />
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (ReportBand band in this)
            {
                band.Group = null;
                band.Report = null;
            }

            base.ClearItems();
        }

        /// <inheritdoc />
        protected override void InsertItem
            (
                int index,
                [NotNull] T item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Report = Report;

            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if ((index >= 0) && (index < Count))
            {
                ReportBand band  = this[index];
                if (!ReferenceEquals(band, null))
                {
                    band.Group = null;
                    band.Report = null;
                }
            }

            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem
            (
                int index,
                [NotNull] T item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Group = Group;
            item.Report = Report;

            base.SetItem(index, item);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(reader, "reader");

            ClearItems();
            //RecordField[] array = reader.ReadArray<RecordField>();
            //AddRange(array);
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            //writer.WriteArray(this.ToArray());
        }

        #endregion

        #region IReadOnly<T> members

        //[NonSerialized]
        internal bool _readOnly;

        /// <summary>
        /// Whether the collection is read-only?
        /// </summary>
        public bool ReadOnly { get { return _readOnly; } }

        /// <summary>
        /// Create read-only clone of the collection.
        /// </summary>
        public BandCollection<T> AsReadOnly()
        {
            BandCollection<T> result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <summary>
        /// Marks the object as read-only.
        /// </summary>
        public void SetReadOnly()
        {
            _readOnly = true;
        }

        /// <summary>
        /// Throws if read only.
        /// </summary>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }
        }

        #endregion
    }
}
