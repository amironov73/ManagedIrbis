// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StreamCollector.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Сборщик данных о производительности.
    /// Результат пишется в файл.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class StreamCollector
        : PerformanceCollector
    {
        #region Properties

        /// <summary>
        /// Binary writer.
        /// </summary>
        [NotNull]
        public BinaryWriter Writer { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StreamCollector
            (
                [NotNull] BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            Writer = writer;
            _ownStream = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public StreamCollector
            (
                [NotNull] Stream stream
            )
            : this (new BinaryWriter(stream))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public StreamCollector
            (
                [NotNull] string fileName
            )
            : this (File.Create(fileName))
        {
            _ownStream = true;
        }

        #endregion

        #region Private members

        private readonly bool _ownStream;

        #endregion

        #region PerformanceCollector members

        /// <inheritdoc cref="PerformanceCollector.Collect" />
        public override void Collect
            (
                PerfRecord record
            )
        {
            Code.NotNull(record, "record");

            record.SaveToStream(Writer);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="PerformanceCollector.Dispose" />
        public override void Dispose()
        {
            if (_ownStream)
            {
                Writer.Dispose();
            }

            base.Dispose();
        }

        #endregion
    }
}
