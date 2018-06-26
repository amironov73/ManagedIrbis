// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstWriter64.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Readers;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MstWriter64
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// MST file name.
        /// </summary>
        [NotNull]
        public string MstFileName { get; private set; }

        /// <summary>
        /// XRF file name.
        /// </summary>
        [NotNull]
        public string XrfFileName { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MstWriter64
            (
                [NotNull] string outputPath
            )
        {
            Code.NotNull(outputPath, "outputPath");

            MstFileName = Path.ChangeExtension(outputPath, ".mst")
                .ThrowIfNull("MstFileName");
            XrfFileName = Path.ChangeExtension(outputPath, ".xrf")
                .ThrowIfNull("XrfFileName");

            bool created = false;
            if (!File.Exists(MstFileName) || !File.Exists(XrfFileName))
            {
                DirectUtility.CreateMasterFile64(outputPath);
                created = true;
            }

            _mst = new FileStream
                (
                    MstFileName,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
            MstControlRecord64 control = MstControlRecord64.Read(_mst);
            _mfn = created ? control.NextMfn : 1;
            _mstPosition = created ? control.NextPosition : MstControlRecord64.RecordSize;
            _xrf = new FileStream
                (
                    XrfFileName,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.None
                );
            _xrfPosition = (_mfn - 1) * XrfRecord64.RecordSize;
            _xrf.Seek(_xrfPosition, SeekOrigin.Begin);
        }

        #endregion

        #region Private members

        private bool _needUpdate;

        private readonly FileStream _mst, _xrf;

        private int _mfn;

        private long _mstPosition, _xrfPosition;

        #endregion

        #region Public methods

        /// <summary>
        /// Update control record.
        /// </summary>
        public void UpdateControlRecord()
        {
            long nextPosition = _mst.Length;
            _mst.Seek(0, SeekOrigin.Begin);
            MstControlRecord64 control = MstControlRecord64.Read(_mst);
            control.Blocked = 0;
            control.NextMfn = _mfn;
            control.NextPosition = nextPosition;

            _mst.Seek(0, SeekOrigin.Begin);
            control.Write(_mst);
        }

        /// <summary>
        /// Write the record.
        /// </summary>
        public int WriteRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            int result = _mfn;

            record.Mfn = _mfn;
            record.Version = 1;
            record.Status = RecordStatus.Last;
            long position = _mstPosition;
            MstRecord64 mstRecord = MstRecord64.EncodeRecord(record);
            mstRecord.Prepare();
            mstRecord.Write(_mst);

            _xrf.WriteInt64Network(position);
            _xrf.WriteInt32Network((int)RecordStatus.NonActualized);
            _xrfPosition += XrfRecord64.RecordSize;

            _mfn++;
            _needUpdate = true;

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (!ReferenceEquals(_mst, null))
            {
                if (_needUpdate)
                {
                    UpdateControlRecord();
                }
                _mst.Dispose();
            }

            if (!ReferenceEquals(_xrf, null))
            {
                _xrf.Dispose();
            }
        }

        #endregion
    }
}
