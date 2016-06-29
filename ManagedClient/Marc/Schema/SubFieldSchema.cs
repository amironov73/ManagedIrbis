/* SubFieldSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using AM.Collections;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Marc.Schema
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("[{Code}] {Name}")]
    public sealed class SubFieldSchema
    {
        #region Properties

        /// <summary>
        /// Code.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Display.
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Mandatory?
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MandatoryText { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Repeatable?
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RepeatableText { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members


        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static SubFieldSchema ParseElement
            (
                [NotNull] XElement element
            )
        {
            CodeJam.Code.NotNull(element, "element");

            SubFieldSchema result = new SubFieldSchema
            {
                Code = element.GetAttributeCharacter("tag"),
                Name = element.GetAttributeText("name", null),
                Mandatory = element.GetAttributeBoolean("mandatory", false),
                MandatoryText = element.GetAttributeText("nm", null),
                Repeatable = element.GetAttributeBoolean("repeatable", false),
                RepeatableText = element.GetAttributeText("nr", null),
                Description = element.GetInnerXml("DESCRIPTION"),
                Display = element.GetAttributeBoolean("display", false)
            };

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
