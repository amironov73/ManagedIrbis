// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NonCloseableTextReader.cs -- non-closeable TextReader
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.IO.NonCloseable
{
    /// <summary>
    /// Non-closeable <see cref="T:System.IO.TextReader"/>.
    /// Call <see cref="ReallyClose"/> to close it.
    /// </summary>
    public class NonCloseableTextReader
        : TextReader,
          IDisposable
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="NonCloseableTextReader"/> class.
        /// </summary>
        /// <param name="innerReader">The inner reader.</param>
        public NonCloseableTextReader
            (
                [NotNull] TextReader innerReader
            )
        {
            Code.NotNull(innerReader, "innerReader");

            _innerReader = innerReader;
        }

        #endregion

        #region Private members

        private readonly TextReader _innerReader;

        #endregion

        #region Public methods

        /// <summary>
        /// Really closes the reader.
        /// </summary>
        public virtual void ReallyClose()
        {
            _innerReader.Dispose();
        }

        #endregion

        #region TextReader members

        /// <summary>
        /// 
        /// </summary>
        public
#if !NETCORE && !UAP && !WIN81 && !PORTABLE
            override
#endif
            void Close()
        {
            // Nothing to do actually
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // Nothing to do actually
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Peek()
        {
            return _innerReader.Peek();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Read()
        {
            return _innerReader.Read();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(char[] buffer, int index, int count)
        {
            return _innerReader.Read(buffer, index, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int ReadBlock(char[] buffer, int index, int count)
        {
            return _innerReader.ReadBlock(buffer, index, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ReadLine()
        {
            return _innerReader.ReadLine();
        }

        /// <summary>
        /// Reads all characters from the current position to the end 
        /// of the TextReader and returns them as one string.
        /// </summary>
        /// <returns>
        /// A string containing all characters from the current position 
        /// to the end of the TextReader.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue"></see></exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader"></see> is closed. </exception>
        public override string ReadToEnd()
        {
            return _innerReader.ReadToEnd();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        void IDisposable.Dispose()
        {
            // Nothing to do actually
        }

        #endregion
    }
}
