/* RelatedField.cs --
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
    /// Related field.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Tag}] {Name}")]
    public sealed class RelatedField
    {
        #region Properties

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// Field.
        /// </summary>
        [CanBeNull]
        public FieldSchema Field { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        [CanBeNull]
        public string Tag { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static RelatedField ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            RelatedField result = new RelatedField
            {
                Tag = element.GetAttributeText("tag", null),
                Name = element.GetAttributeText("name", null),
                Description = element.GetInnerXml("DESCRIPTION")
            };

            return result;
        }

        #endregion
    }
}
