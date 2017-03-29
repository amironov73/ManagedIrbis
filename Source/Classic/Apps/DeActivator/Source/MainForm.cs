// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Rfid;

using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace DeActivator
{
    public partial class MainForm 
        : Form
    {
        #region Properties

        /// <summary>
        /// Log output.
        /// </summary>
        [NotNull]
        public TextBoxOutput Log { get; private set; }


        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            Log = new TextBoxOutput(_logBox);
        }

        #endregion

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            this.ShowVersionInfoInTitle();
            Log.PrintSystemInformation();
            WriteDelimiter();
        }

        private void DoThis
            (
                bool activate
            )
        {
            int count = 0;

            try
            {
                Log.Clear();

                using (RfidFeigDriver driver = new RfidFeigDriver())
                {
                    driver.ConnectUSB();

                    string[] labels;

                    AGAIN:
                    try
                    {
                        labels = driver.Inventory();
                    }
                    catch (RfidException)
                    {
                        if (count == 10)
                        {
                            WriteLine("Сдаюсь, ничего не получается");
                            WriteDelimiter();
                            return;
                        }

                        WriteLine
                            (
                                "Не прочиталось, ещё попытка: {0}",
                                ++count
                            );
                        Application.DoEvents();
                        Thread.Sleep(500);
                        goto AGAIN;
                    }
                    if (labels.Length != 0)
                    {
                        foreach (string label in labels)
                        {
                            driver.SetEAS(label, activate);

                            string may = activate
                                ? "Нельзя выносить"
                                : "Можно выносить";

                            WriteLine
                                (
                                    "Метка {0}: {1}",
                                    label,
                                    may
                                );
                        }
                    }
                    else
                    {
                        WriteLine("Не удалось прочитать ни одной метки");
                        WriteDelimiter();
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(this, exception);
            }
        }

        public void WriteDelimiter()
        {
            Log.WriteLine(new string('=', 60));
        }

        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            Log.WriteLine(format, args);
        }

        private void _deactivateButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            DoThis(false);
        }

        private void _activateButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            DoThis(true);
        }
    }
}
