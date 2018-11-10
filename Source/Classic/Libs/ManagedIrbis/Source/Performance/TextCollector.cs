// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextCollector.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Сборщик данных о производительности.
    /// Результат пишется в виде текста, возможно, в консоль.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TextCollector
        : PerformanceCollector
    {
        #region Properties

        /// <summary>
        /// Text writer.
        /// </summary>
        [NotNull]
        public TextWriter Writer { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCollector
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            Writer = writer;
            _ownStream = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCollector
            (
                [NotNull] Stream stream,
                [NotNull] Encoding encoding
            )
            : this (new StreamWriter(stream, encoding))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCollector
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
            : this (new StreamWriter(fileName, true, encoding))
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

            Writer.WriteLine(record.ToString());
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
