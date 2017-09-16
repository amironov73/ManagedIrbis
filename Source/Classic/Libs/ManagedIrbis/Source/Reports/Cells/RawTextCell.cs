// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RawTextCell.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RawTextCell
        : TextCell
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RawTextCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RawTextCell
            (
                string text
            )
            : base(text)
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportCell

        /// <inheritdoc cref="TextCell.Compute"/>
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

        /// <inheritdoc cref="TextCell.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            Code.NotNull(context, "context");

            string text = Compute(context);

            ReportDriver driver = context.Driver;
            driver.BeginCell(context, this);
            context.Output.Write(text);
            driver.EndCell(context, this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
