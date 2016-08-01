/* FieldTag.cs -- field tag related routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO check tag length?
 */

#region Using directives

using AM;
using AM.Collections;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Field tag related routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FieldTag
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Бросать исключения при валидации?
        /// </summary>
        public static bool ThrowOnValidate { get; set; }

        #endregion

        #region Construction

        static FieldTag()
        {
            _goodCharacters = new CharSet().AddRange('0', '9');
        }

        #endregion

        #region Private members

        private static readonly CharSet _goodCharacters;

        #endregion

        #region Public methods

        /// <summary>
        /// Whether given tag is valid?
        /// </summary>
        public static bool IsValidTag
            (
                [CanBeNull] string tag
            )
        {
            if (string.IsNullOrEmpty(tag))
            {
                return false;
            }

            bool result = _goodCharacters.CheckText(tag)
                && Normalize(tag) != "0"
                && tag.Length < 6; // ???

            return result;
        }

        /// <summary>
        /// Normalization.
        /// </summary>
        public static string Normalize
            (
                [CanBeNull] string tag
            )
        {
            if (string.IsNullOrEmpty(tag))
            {
                return tag;
            }

            string result = tag;
            while (result.Length > 1
                && result.StartsWith("0"))
            {
                result = result.Substring(1);
            }

            return result;
        }

        /// <summary>
        /// Verify the tag value.
        /// </summary>
        public static bool Verify
            (
                [CanBeNull] string tag,
                bool throwOnError
            )
        {
            bool result = IsValidTag(tag);

            if (!result && throwOnError)
            {
                throw new VerificationException("bad tag: " + tag);
            }

            return result;
        }

        /// <summary>
        /// Verify the tag value.
        /// </summary>
        public static bool Verify
            (
                [CanBeNull] string tag
            )
        {
            return Verify
                (
                    tag,
                    ThrowOnValidate
                );
        }

        #endregion
    }
}
