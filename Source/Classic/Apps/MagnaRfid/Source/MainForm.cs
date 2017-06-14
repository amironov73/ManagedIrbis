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

using AM;
using AM.Rfid;
using AM.Windows.Forms;

#endregion

namespace MagnaRfid
{
    public partial class MainForm
        : Form
    {
        #region Properties

        public bool Active { get; set; }

        public RfidDriver Driver { get; set; }

        #endregion

        #region Construction

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private bool _busy = false;

        private string _readerName;

        private string _lastTag;

        private DateTime _lastMoment;

        #endregion

        #region Methods

        public void OpenDevice()
        {
            CloseDevice();

            RfidCardmanDriver driver = new RfidCardmanDriver();
            string[] readers = driver.GetReaders();
            _readerName = readers[1];
            Driver = driver;
        }

        public void CloseDevice()
        {
            if (!ReferenceEquals(Driver, null))
            {
                try
                {
                    Driver.Dispose();
                }
                catch
                {
                    // Nothing to do here
                }
                Driver = null;
            }
        }

        public void SendTag
            (
                string tag
            )
        {
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            if (tag.SameString(_lastTag))
            {
                TimeSpan howLong = DateTime.Now - _lastMoment;
                if (howLong.TotalSeconds < 30)
                {
                    return;
                }
            }

            bool english = _englishCheckBox.Checked;
            //InputLanguage previousLanguage 
            //    = InputLanguage.CurrentInputLanguage;
            //bool needSwitch = !previousLanguage.Culture
            //    .TwoLetterISOLanguageName.SameString("EN");

            if (english)
            {
                //InputLanguageUtility.SwitchToEnglish();

                //if (needSwitch)
                //{
                    SendKeys.SendWait("%+");
                    Thread.Sleep(100);
                //}

                //Application.DoEvents();
            }

            SendKeys.SendWait(tag);
            _lastTag = tag;
            _lastMoment = DateTime.Now;

            if (_crlfBox.Checked)
            {
                SendKeys.SendWait("{ENTER}");
            }

            if (english)
            {
                //InputLanguage.CurrentInputLanguage = previousLanguage;

                //if (needSwitch)
                //{
                    SendKeys.SendWait("%+");
                    Thread.Sleep(100);
                //}

                //Application.DoEvents();
            }
        }

        #endregion

        private void _openButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            OpenDevice();
        }

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            CloseDevice();
        }

        private void _startButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Active = true;
        }

        private void _stopButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Active = false;
        }

        private void _timer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            if (!Active
                || _busy
                || ReferenceEquals(Driver, null)
               )
            {
                return;
            }

            try
            {
                _busy = true;

                Driver.Connect(_readerName);
                string[] tags = Driver.Inventory();
                //Driver.Disconnect();
                if (tags.Length != 1)
                {
                    return;
                }

                SendTag(tags[0]);
            }
            catch
            {
                // Nothing to do here
            }
            finally
            {
                _busy = false;
            }
        }
    }
}
