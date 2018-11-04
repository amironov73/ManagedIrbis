using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Reports;

namespace Crocodile
{
    class EffectiveReport
        : IrbisReport
    {
        #region Properties

        [NotNull]
        public IrbisProvider Provider { get; private set; }

        [NotNull]
        public ReportContext Context { get; private set; }

        [NotNull]
        // ReSharper disable once NotNullMemberIsNotInitialized
        public static EffectiveReport Instance { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public EffectiveReport
            (
                [NotNull] IrbisProvider provider
            )
        {
            Provider = provider;
            Context = new ReportContext(provider);

            Header = new HeaderBand();
            Header.Cells.Add(new TextCell("КСУ",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Дата",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Сигла",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("ББК",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Назв.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Экз.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Руб.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Выдач",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Выд./экз.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Руб/выд.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Выд/день",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Чит. эффект.",
                new ReportAttribute(ReportAttribute.Bold, true)));
            Header.Cells.Add(new TextCell("Фин. эффект.",
                new ReportAttribute(ReportAttribute.Bold, true)));
        }

        #endregion

        #region Public methods

        public static void AddLine(string text)
        {
            Instance.Body.Add(new ReportBand(new TextCell(text)));
        }

        public static void BoldLine(string text)
        {
            Instance.Body.Add(new ReportBand(new TextCell(text,
                new ReportAttribute(ReportAttribute.Bold, true))));
        }

        #endregion
    }
}
