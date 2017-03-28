// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LoggingDriver.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Logging driver for report debugging.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class LoggingDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Inner driver.
        /// </summary>
        [NotNull]
        public ReportDriver InnerDriver { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LoggingDriver
            (
                [NotNull] ReportDriver innerDriver
            )
        {
            Code.NotNull(innerDriver, "innerDriver");

            InnerDriver = innerDriver;
        }

        #endregion

        #region ReportDriver members

        /// <inheritdoc cref="ReportDriver.BeginCell"/>
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            Log.Trace(string.Format
                (
                    "ReportDriver.BeginCell: {0}",
                    cell
                ));

            InnerDriver.BeginCell(context, cell);
        }

        /// <inheritdoc cref="ReportDriver.BeginDocument"/>
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            Log.Trace(string.Format
                (
                    "ReportDriver.BeginDocument: {0}",
                    report
                ));

            InnerDriver.BeginDocument(context, report);
        }

        /// <inheritdoc cref="ReportDriver.BeginRow"/>
        public override void BeginRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Log.Trace(string.Format
                (
                    "ReportDriver.BeginRow: {0}",
                    band
                ));

            InnerDriver.BeginRow(context, band);
        }

        /// <inheritdoc cref="ReportDriver.EndCell"/>
        public override void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            Log.Trace(string.Format
                (
                    "ReportDriver.EndCell: {0}",
                    cell
                ));

            InnerDriver.EndCell(context, cell);
        }

        /// <inheritdoc cref="ReportDriver.EndDocument"/>
        public override void EndDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            Log.Trace(string.Format
                (
                    "ReportDriver.EndDocument: {0}",
                    report
                ));

            InnerDriver.EndDocument(context, report);
        }

        /// <inheritdoc cref="ReportDriver.EndRow"/>
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Log.Trace(string.Format
                (
                    "ReportDriver.EndRow: {0}",
                    band
                ));

            InnerDriver.EndRow(context, band);
        }

        /// <inheritdoc cref="ReportDriver.Write"/>
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            Log.Trace(string.Format
                (
                    "ReportDriver.Write: {0}",
                    text
                ));

            InnerDriver.Write(context, text);
        }

        #endregion
    }
}
