// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using AM.Windows.Forms;

using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.XtraReports.UI;

#endregion

namespace ReportDesigner
{
    static class Program
    {
        static void _ThreadException
        (
            object sender,
            ThreadExceptionEventArgs eventArgs
        )
        {
            ExceptionBox.Show(eventArgs.Exception);
            Environment.FailFast
            (
                "Shutting down",
                eventArgs.Exception
            );
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.ThreadException += _ThreadException;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                BonusSkins.Register();
                SkinManager.EnableFormSkins();
                //UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");
                UserLookAndFeel.Default.SetSkinStyle("Office 2016 Colorful");

                XtraReport report = new XtraReport
                {
                    PaperKind = PaperKind.Custom,
                    ReportUnit = ReportUnit.TenthsOfAMillimeter
                };
                ReportDesignTool tool = new ReportDesignTool(report);
                //tool.ShowDesignerDialog(UserLookAndFeel.Default);
                tool.ShowRibbonDesignerDialog(UserLookAndFeel.Default);
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }
        }
    }
}
