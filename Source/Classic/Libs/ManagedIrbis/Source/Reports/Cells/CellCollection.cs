// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CellCollection.cs -- 
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
    public sealed class CellCollection
        : Collection<ReportCell>,
        IHandmadeSerializable,
        IReadOnly<CellCollection>
    {
        #region Properties

        /// <summary>
        /// Band.
        /// </summary>
        [CanBeNull]
        public ReportBand Band { get; internal set; }

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        public IrbisReport Report { get; internal set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming

        internal CellCollection SetReport
            (
                IrbisReport report
            )
        {
            Report = report;

            foreach (ReportCell cell in this)
            {
                cell.Report = report;
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
                [NotNull] IEnumerable<ReportCell> cells
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(cells, "cells");

            foreach (ReportCell cell in cells)
            {
                Add(cell);
            }
        }

        /// <summary>
        /// Создание клона коллекции.
        /// </summary>
        [NotNull]
        public CellCollection Clone()
        {
            CellCollection result = new CellCollection
            {
                Band = Band,
                Report = Report
            };

            foreach (ReportCell cell in this)
            {
                ReportCell clone = cell.Clone();
                clone.Report = Report;
                result.Add(clone);
            }

            return result;
        }

        /// <summary>
        /// Find first occurrence of the field with given predicate.
        /// </summary>
        [CanBeNull]
        public ReportCell Find
            (
                [NotNull] Predicate<ReportCell> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this.FirstOrDefault
                (
                    cell => predicate(cell)
                );
        }

        /// <summary>
        /// Find all occurrences of the field
        /// with given predicate.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public ReportCell[] FindAll
            (
                [NotNull] Predicate<ReportCell> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return this.Where
                (
                    cell => predicate(cell)
                )
                .ToArray();
        }

        #endregion

        #region Collection<T> members

        /// <inheritdoc />
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (ReportCell cell in this)
            {
                cell.Band = null;
                cell.Report = null;
            }

            base.ClearItems();
        }

        /// <inheritdoc />
        protected override void InsertItem
            (
                int index,
                [NotNull] ReportCell item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Band = Band;
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
                ReportCell cell  = this[index];
                if (!ReferenceEquals(cell, null))
                {
                    cell.Band = null;
                    cell.Report = null;
                }
            }

            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem
            (
                int index,
                [NotNull] ReportCell item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Band = Band;
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
        public CellCollection AsReadOnly()
        {
            CellCollection result = Clone();
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
