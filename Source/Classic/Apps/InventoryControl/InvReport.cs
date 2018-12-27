/* InvReport.cs
 */

#region Using directives

using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace InventoryControl
{
    public partial class InvReport 
        : XtraReport
    {
        public InvReport()
        {
            InitializeComponent();
        }

        public static void ShowModal
            (
                IWin32Window owner,
                DataAccessLevel dal,
                BookInfo[] books,
                string title,
                bool sortByTitle,
                bool wrapDescriptions,
                bool numberIndex,
                int startNumber
            )
        {
            dal.SortBooks
                (
                    books,
                    sortByTitle,
                    startNumber
                );
            InvReport report = new InvReport
            {
                DataSource = books,
                ShowPrintMarginsWarning = false,
                RequestParameters = false
            };
            report.Parameters["MainTitle"].Value = title;
            report.PrintingSystem.ShowMarginsWarning = false;
            if (wrapDescriptions)
            {
                report._descriptionCell.WordWrap = true;
            }
            if (numberIndex)
            {
                report._indexCell.DataBindings.Clear();
                report._indexCell.DataBindings.Add
                    (
                        "Text",
                        books,
                        "Issue"
                    );
                report._indexCell.Font = report._yearCell.Font;
                report._indexCell.TextAlignment = TextAlignment.MiddleRight;
                //report._indexCell.WordWrap = true;
                //report._indexCell.CanGrow = true;
                report._indexTitleCell.Text = "Номер";
            }
            report.CreateDocument();
            ReportPrintTool tool = new ReportPrintTool(report)
            {
                AutoShowParametersPanel = false,
            };
            tool.PreviewForm.ShowInTaskbar = false;
            tool.PreviewForm.Text = title;
            tool.ShowPreviewDialog(owner, null);
            
        }
    }
}
