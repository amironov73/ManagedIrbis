/* SubFieldValue.cs -- subfield value related routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO trim value?
 */

#region Using directives

using AM;

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
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        public static bool ThrowOnVerify { get; set; }

        #endregion

        #region Private members

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
            bool result = true;

            if (!string.IsNullOrEmpty(value))
            {
                if (value.IndexOf(SubField.Delimiter) >= 0)
                {
                    result = false;
                }
            }

            return result;
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

            if (!result && throwOnError)
            {
                throw new VerificationException("SubField.Value");
            }

            return result;
        }

        #endregion
    }
}
