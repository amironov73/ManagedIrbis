// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NonCloseableStreamReader.cs -- non-closeable stream reader
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Non-closeable stream reader.
    /// </summary>
    [PublicAPI]
    public class NonCloseableStreamReader
        : StreamReader,
          IDisposable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream
            )
            : base(stream)
        {
        }

#if !NETCORE && !UAP && !WIN81 && !PORTABLE

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path
            )
            : base(path)
        {
        }

#endif

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream,
                bool detectEncodingFromByteOrderMarks
            )
            : base(stream, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream,
                Encoding encoding
            )
            : base(stream, encoding)
        {
        }

#if !NETCORE && !UAP && !WIN81 && !PORTABLE

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path,
                bool detectEncodingFromByteOrderMarks
            )
            : base(path, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path,
                Encoding encoding
            )
            : base(path, encoding)
        {
        }

#endif

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream,
                Encoding encoding,
                bool detectEncodingFromByteOrderMarks
            )
            : base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
        }

#if !NETCORE && !UAP && !WIN81 && !PORTABLE

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path,
                Encoding encoding,
                bool detectEncodingFromByteOrderMarks
            )
            : base(path, encoding, detectEncodingFromByteOrderMarks)
        {
        }

#endif

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public NonCloseableStreamReader
            (
                StreamReader reader
            )
            : base(reader.BaseStream, reader.CurrentEncoding)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Really close the reader.
        /// </summary>
        public virtual void ReallyClose()
        {
            base.Dispose();
        }

        #endregion

        #region StreamReader members

        /// <summary>
        /// NOT closes the <see cref="T:System.IO.StreamReader"></see> 
        /// object and the underlying stream, and releases any system resources 
        /// associated with the reader.
        /// </summary>
        public
#if !NETCORE && !UAP && !WIN81 && !PORTABLE
            override
#endif
            void Close()
        {
            // Nothing to do actually
        }

        #endregion

        #region IDisposable Members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        void IDisposable.Dispose()
        {
            // Nothing to do actually
        }

        #endregion
    }
}
