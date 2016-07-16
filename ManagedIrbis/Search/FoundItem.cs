/* FoundItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedClient.Network;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Search
{
    /// <summary>
    /// Found item.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Mfn} {Text}")]
    public sealed class FoundItem
    {
        #region Constants

        /// <summary>
        /// Delimiter.
        /// </summary>
        public const char Delimiter = '#';

        #endregion

        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        // ReSharper disable InconsistentNaming
        private static readonly char[] _delimiters = {Delimiter};
        // ReSharper restore InconsistentNaming

        #endregion

        #region Public methods

        /// <summary>
        /// Convert to MFN array.
        /// </summary>
        [NotNull]
        public static int[] ConvertToMfn
            (
                [NotNull][ItemNotNull] FoundItem[] found
            )
        {
            Code.NotNull(found, "found");

            int[] result = new int[found.Length];
            for (int i = 0; i < found.Length; i++)
            {
                result[i] = found[i].Mfn;
            }

            return result;
        }

        /// <summary>
        /// Convert to string array.
        /// </summary>
        [NotNull]
        [ItemCanBeNull]
        public static string[] ConvertToText
            (
                [NotNull][ItemNotNull] FoundItem[] found
            )
        {
            Code.NotNull(found, "found");

            string[] result = new string[found.Length];
            for (int i = 0; i < found.Length; i++)
            {
                result[i] = found[i].Text.EmptyNull();
            }

            return result;
        }

        /// <summary>
        /// Parse text line.
        /// </summary>
        [NotNull]
        public static FoundItem ParseLine
            (
                [NotNull] string line
            )
        {
            Code.NotNull(line, "line");

            string[] parts = line.Split(_delimiters, 2);
            FoundItem result = new FoundItem
            {
                Mfn = int.Parse(parts[0])
            };
            if (parts.Length > 1)
            {
                string text = result.Text.EmptyNull();
                text = IrbisText.IrbisToWindows(text);
                result.Text = text;
            }

            return result;
        }

        /// <summary>
        /// Parse server response for MFN only.
        /// </summary>
        [NotNull]
        public static int[] ParseMfnOnly
            (
                [NotNull] IrbisServerResponse response
            )
        {
            Code.NotNull(response, "response");

            FoundItem[] parsed = ParseServerResponse(response);
            int[] result = ConvertToMfn(parsed);

            return result;
        }

        /// <summary>
        /// Parse server response for text only.
        /// </summary>
        [NotNull]
        [ItemCanBeNull]
        public static string[] ParseTextOnly
            (
                [NotNull] IrbisServerResponse response
            )
        {
            Code.NotNull(response, "response");

            FoundItem[] parsed = ParseServerResponse(response);
            string[] result = ConvertToText(parsed);

            return result;
        }

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static FoundItem[] ParseServerResponse
            (
                [NotNull] IrbisServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<FoundItem> result = new List<FoundItem>();
            string line;
            while ((line = response.GetUtfString()) != null)
            {
                FoundItem item = ParseLine(line);
                result.Add(item);
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format
                (
                    "{0} {1}",
                    Mfn,
                    Text
                );
        }

        #endregion
    }
}
