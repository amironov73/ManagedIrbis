/* FormUtility.cs -- extension methods for System.Windows.Forms
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Extension methods for <see cref="Form"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FormUtility
    {
        #region Public methods

        /// <summary>
        /// Adjusts the placement of the form.
        /// </summary>
        [NotNull]
        public static Form AdjustPlacement
            (
                [NotNull] this Form form,
                [CanBeNull] Screen screen
            )
        {
            Code.NotNull(form, "form");

            const int delta = 1;

            Code.NotNull(form, "form");

            screen = screen ?? Screen.FromControl(form);

            Rectangle area = screen.WorkingArea;

            form.Left = Math.Min
                (
                    form.Left,
                    area.Right - form.Width - delta
                );
            form.Left = Math.Max
                (
                    form.Left,
                    delta
                );
            form.Top = Math.Min
                (
                    form.Top,
                    area.Bottom - form.Height - delta
                );
            form.Top = Math.Max
                (
                    form.Top,
                    delta
                );

            return form;
        }

        /// <summary>
        /// Calculates placement for the window according to
        /// the specified <see cref="WindowPlacement"/>.
        /// </summary>
        public static Point CalculatePlacement
            (
                Size windowSize,
                WindowPlacement placement,
                Size indent
            )
        {
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            Point result;

            switch (placement)
            {
                case WindowPlacement.ScreenCenter:
                    result = new Point
                        (
                            workingArea.Left
                                + (workingArea.Width - windowSize.Width) / 2,
                            workingArea.Top 
                                + (workingArea.Height - windowSize.Height) / 2
                        );
                    break;

                case WindowPlacement.TopLeftCorner:
                    result = new Point(indent);
                    break;

                case WindowPlacement.TopRightCorner:
                    result = new Point
                        (
                            workingArea.Right - windowSize.Width - indent.Width,
                            indent.Height
                        );
                    break;

                case WindowPlacement.TopSide:
                    result = new Point
                        (
                            workingArea.Left 
                                + (workingArea.Width - windowSize.Width) / 2,
                            indent.Height
                        );
                    break;

                case WindowPlacement.LeftSide:
                    result = new Point
                        (
                            indent.Width,
                            workingArea.Top 
                                + (workingArea.Height - windowSize.Height) / 2
                        );
                    break;

                case WindowPlacement.RightSide:
                    result = new Point
                        (
                            workingArea.Right - windowSize.Width - indent.Width,
                            workingArea.Top
                                + (workingArea.Height - windowSize.Height) / 2
                        );
                    break;

                case WindowPlacement.BottomSide:
                    result = new Point
                        (
                            workingArea.Left
                                + (workingArea.Width - windowSize.Width) / 2,
                            workingArea.Height - windowSize.Height - indent.Height
                        );
                    break;

                case WindowPlacement.BottomLeftCorner:
                    result = new Point
                        (
                            indent.Width,
                            workingArea.Height - windowSize.Height - indent.Height
                        );
                    break;

                case WindowPlacement.BottomRightCorner:
                    result = new Point
                        (
                            workingArea.Width - windowSize.Width - indent.Width,
                            workingArea.Height - windowSize.Height - indent.Height
                        );
                    break;

                default:
                    throw new ArgumentOutOfRangeException("placement");
            }

            return result;
        }

        /// <summary>
        /// Show version information in form title.
        /// </summary>
        public static void ShowVersionInfoInTitle
            (
                [NotNull] this Form form
            )
        {
            Code.NotNull(form, "form");

            Assembly assembly = Assembly.GetEntryAssembly();
            Version vi = assembly.GetName().Version;
            if (ReferenceEquals(assembly.Location, null))
            {
                return;
            }
            FileVersionInfo fvi = FileVersionInfo
                .GetVersionInfo(assembly.Location);
            FileInfo fi = new FileInfo(assembly.Location);
            form.Text += string.Format
                (
                    ": version {0} (file {1}) from {2}",
                    vi,
                    fvi.FileVersion,
                    fi.LastWriteTime.ToShortDateString()
                );
        }

        /// <summary>
        /// Print system information in abstract output.
        /// </summary>
        public static void PrintSystemInformation
            (
                [CanBeNull] this AbstractOutput output
            )
        {
            if (!ReferenceEquals(output, null))
            {
                output.WriteLine
                    (
                        "OS version: {0}",
                        Environment.OSVersion
                    );
                output.WriteLine
                    (
                        "Framework version: {0}",
                        Environment.Version
                    );
                Assembly assembly = Assembly.GetEntryAssembly();
                Version vi = assembly.GetName().Version;
                if (ReferenceEquals(assembly.Location, null))
                {
                    return;
                }
                FileInfo fi = new FileInfo(assembly.Location);
                output.WriteLine
                    (
                        "Application version: {0} ({1})",
                        vi,
                        fi.LastWriteTime.ToShortDateString()
                    );
                output.WriteLine
                    (
                        "Memory: {0} Mb",
                        GC.GetTotalMemory(false)/1024
                    );
            }
        }

        #endregion
    }
}
