/* IndicatorSchema.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
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
    [DebuggerDisplay("{Name}")]
    public sealed class IndicatorSchema
    {
        #region Properties

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Options.
        /// </summary>
        [NotNull]
        public NonNullCollection<Option> Options
        {
            get { return _options; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndicatorSchema()
        {
            _options = new NonNullCollection<Option>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<Option> _options;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        [NotNull]
        public static IndicatorSchema ParseElement
            (
                [NotNull] XElement element
            )
        {
            Code.NotNull(element, "element");

            IndicatorSchema result = new IndicatorSchema
            {
                Name = element.GetAttributeText("name", null),
                Description = element.GetElementText("DESCRIPTION", null)
            };

            foreach (XElement subElement in element.Elements("OPTION"))
            {
                Option option = Option.ParseElement(subElement);
                result.Options.Add(option);
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
