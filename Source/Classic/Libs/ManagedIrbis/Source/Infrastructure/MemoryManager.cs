// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MemoryManager.cs -- memory manager for IRBIS client
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Memory manager for IRBIS client.
    /// </summary>
    static class MemoryManager
    {
        #region Construction

        static MemoryManager()
        {
#if !WINMOBILE && !PocketPC

            MemoryManager.Manager
                = new Microsoft.IO.RecyclableMemoryStreamManager ();

#endif
        }

        #endregion

        #region Private members

#if !WINMOBILE && !PocketPC

        private const string Tag = "IRBIS";

        private static readonly Microsoft.IO.RecyclableMemoryStreamManager Manager;

#endif

        #endregion

        #region Public methods

        /// <summary>
        /// Get the memory stream.
        /// </summary>
        [NotNull]
        public static MemoryStream GetMemoryStream()
        {
#if WINMOBILE || PocketPC

            return new MemoryStream();

#else

            return Manager.GetStream(Tag);

#endif

        }

        /// <summary>
        /// Get the memory stream.
        /// </summary>
        [NotNull]
        public static MemoryStream GetMemoryStream
            (
                int initialSize
            )
        {
#if WINMOBILE || PocketPC

            return new MemoryStream(initialSize);

#else

            return Manager.GetStream(Tag, initialSize);

#endif

        }

        /// <summary>
        /// Recycle the memory stream.
        /// </summary>
        public static void RecycleMemoryStream
            (
                [CanBeNull] MemoryStream stream
            )
        {
            if (!ReferenceEquals(stream, null))
            {
                stream.Dispose();
            }
        }

        #endregion
    }
}
