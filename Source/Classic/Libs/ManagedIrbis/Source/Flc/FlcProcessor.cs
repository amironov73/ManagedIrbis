// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FlcProcessor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// FLC processor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FlcProcessor
    {
        #region Public methods

        /// <summary>
        /// Check the record.
        /// </summary>
        [NotNull]
        public FlcResult CheckRecord
            (
                [NotNull] IrbisProvider provider,
                [NotNull] MarcRecord record,
                [NotNull] string format
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(format, format);

            string text = provider.FormatRecord
                (
                    record,
                    format
                );
            FlcResult result = FlcResult.Parse(text);

            return result;
        }

        #endregion
    }
}
