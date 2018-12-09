// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldTag.cs -- field tag related routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO check tag length?
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Field tag related routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FieldTag
    {
        #region Public methods

        /// <summary>
        /// Normalization.
        /// </summary>
        public static unsafe string Normalize
            (
                [CanBeNull] string tag
            )
        {
            if (string.IsNullOrEmpty(tag))
            {
                return tag;
            }

            int i = 0;
            fixed (char* ptr = tag)
            {
                int length = tag.Length - 1;
                for (; i < length; i++)
                {
                    if (ptr[i] != '0')
                    {
                        break;
                    }
                }
            }

            string result = tag.Substring(i);

            return result;
        }

        #endregion
    }
}
