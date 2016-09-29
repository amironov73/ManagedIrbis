/* SearchToken.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Token.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Kind} {Text} {Position}")]
    internal sealed class SearchToken
    {
        #region Properties

        /// <summary>
        /// Token kind.
        /// </summary>
        [JsonProperty("kind")]
        public SearchTokenKind Kind { get; set; }

        /// <summary>
        /// Token position.
        /// </summary>
        [JsonProperty("position")]
        public int Position { get; set; }

        /// <summary>
        /// Token text.
        /// </summary>
        [CanBeNull]
        [JsonProperty("text")]
        public string Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchToken()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchToken
            (
                SearchTokenKind kind,
                int position,
                [CanBeNull] string text
            )
        {
            Kind = kind;
            Position = position;
            Text = text;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Text.ToVisibleString();
        }

        #endregion
    }
}
