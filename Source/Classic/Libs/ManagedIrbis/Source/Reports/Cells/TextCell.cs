// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextCell.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TextCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Static text.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("text")]
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCell
            (
                string text
            )
        {
            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCell
            (
                string text,
                params ReportAttribute[] attributes
            ) : base(attributes)
        {
            Text = text;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Text"/> field?
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeText()
        {
            return !string.IsNullOrEmpty(Text);
        }

        #endregion

        #region ReportCell members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string Compute
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            OnBeforeCompute(context);

            string result = Text;

            OnAfterCompute(context);

            return result;
        }

        /// <inheritdoc cref="ReportCell.Render"/>
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            string text = Compute(context);

            ReportDriver driver = context.Driver;
            driver.BeginCell(context, this);
            driver.Write(context, text);
            driver.EndCell(context, this);
        }

        #endregion
    }
}
