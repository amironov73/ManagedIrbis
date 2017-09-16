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
using System.Xml.Serialization;

using AM;
using AM.Logging;
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
    public sealed class CellCollection
        : Collection<ReportCell>,
        IHandmadeSerializable,
        IReadOnly<CellCollection>,
        IVerifiable,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Band.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReportBand Band { get; internal set; }

        /// <summary>
        /// Record.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
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

        /// <inheritdoc cref="Collection{T}.ClearItems" />
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

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                ReportCell item
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(item, "item");

            item.Band = Band;
            item.Report = Report;

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
                ReportCell cell  = this[index];
                if (!ReferenceEquals(cell, null))
                {
                    cell.Band = null;
                    cell.Report = null;
                }
            }

            base.RemoveItem(index);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                ReportCell item
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

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            Code.NotNull(reader, "reader");

            // TODO implement

            ClearItems();
            //RecordField[] array = reader.ReadArray<RecordField>();
            //AddRange(array);

            Log.Error
                (
                    "CellCollection::RestoreFromStream: "
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
                    "CellCollection::SaveToStream: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        #endregion

        #region IReadOnly<T> members

        internal bool _readOnly;

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        public bool ReadOnly { get { return _readOnly; } }

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public CellCollection AsReadOnly()
        {
            CellCollection result = Clone();
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
                        "CellCollection::ThrowIfReadOnly"
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
            Verifier<CellCollection> verifier
                = new Verifier<CellCollection>(this, throwOnError);

            foreach (ReportCell cell in this)
            {
                verifier.VerifySubObject(cell, "cell");
            }

            return verifier.Result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            foreach (ReportCell cell in this)
            {
                cell.Dispose();
            }
        }

        #endregion
    }
}
