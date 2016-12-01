// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ResourceUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Resources
{
#if NOTDEF

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class ResourceUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Extracts the resource.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="destinationPath">The destination path.</param>
        public static void ExtractResource
            (
                [NotNull] Type type,
                [NotNull] string resourceName,
                [NotNull] string destinationPath
            )
        {
            Code.NotNull(type, "type");
            Code.NotNullNorEmpty(resourceName, "resourceName");
            Code.NotNullNorEmpty(destinationPath, "destinationPath");

            Stream resourceStream = type.Assembly.GetManifestResourceStream
                (
                    type,
                    resourceName
                );
            if (!ReferenceEquals(resourceStream, null))
            {
                using (Stream fileStream
                    = new FileStream(destinationPath, FileMode.Create))
                {
                    StreamUtility.Copy(resourceStream, fileStream);
                }
            }
        }

        #endregion
    }

#endif
}
