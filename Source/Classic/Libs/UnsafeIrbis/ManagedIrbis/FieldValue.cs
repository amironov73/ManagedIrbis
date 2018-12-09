// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldValue.cs -- field value related routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO trim value?
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Field value related routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FieldValue
    {
        #region Public methods

        /// <summary>
        /// Whether the value valid.
        /// </summary>
        public static unsafe bool IsValidValue
            (
                [CanBeNull] string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                int length = value.Length;
                fixed (char* ptr = value)
                {
                    for (int i = 0; i < length; i++)
                    {
                        char c = ptr[i];
                        if (c == SubField.Delimiter || c < ' ')
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
