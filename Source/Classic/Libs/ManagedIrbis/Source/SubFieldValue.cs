// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SubFieldValue.cs -- subfield value related routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO trim value?
 */

#region Using directives

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Subfield value related routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SubFieldValue
    {
        #region Properties

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        public static bool ThrowOnVerify { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the value valid.
        /// </summary>
        public static bool IsValidValue
            (
                [CanBeNull] string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (char c in value)
                {
                    if (c == SubField.Delimiter)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// SubField value normalization.
        /// </summary>
        [CanBeNull]
        public static string Normalize
            (
                [CanBeNull] string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            // TODO normalize

            //string result = value.Trim();
            //return result;

            return value;
        }


        /// <summary>
        /// Verify subfield value.
        /// </summary>
        public static bool Verify
            (
                [CanBeNull] string value
            )
        {
            return Verify(value, ThrowOnVerify);
        }

        /// <summary>
        /// Verify subfield value.
        /// </summary>
        public static bool Verify
            (
                [CanBeNull] string value,
                bool throwOnError
            )
        {
            bool result = IsValidValue(value);

            if (!result)
            {
                Log.Error
                    (
                        "SubFieldValue::Verify: "
                        + value.ToVisibleString()
                    );

                if (throwOnError)
                {
                    throw new VerificationException("SubField.Value");
                }
            }

            return result;
        }

        #endregion
    }
}
