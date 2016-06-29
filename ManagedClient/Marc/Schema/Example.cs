/* Example.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Xml.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Marc.Schema
{
    /// <summary>
    /// Example.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("{Text}")]
    public sealed class Example
    {
        #region Properties

        /// <summary>
        /// Number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static Example ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            Example result = new Example
            {
                Number = element.GetAttributeInt32("N", 0),
                Text = element.GetInnerXml()
            };

            return result;
        }

        #endregion
    }
}
