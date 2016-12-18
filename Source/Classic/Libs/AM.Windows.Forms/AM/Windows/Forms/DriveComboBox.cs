// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DriveComboBox.cs -- ComboBox that contains list of installed drives
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using RC = AM.Windows.Forms.Properties.Resources;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="ComboBox"/> that contains list of installed
    /// drives.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolboxBitmap(typeof(DriveComboBox), "Images.drive.bmp")]
    public class DriveComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current drive.
        /// </summary>
        /// <value>The current drive.</value>
        public string CurrentDrive
        {
            [DebuggerStepThrough]
            get
            {
                DriveInfo selectedDrive = CurrentDriveInfo;
                return (selectedDrive == null)
                           ? null
                           : selectedDrive.Name;
            }
            set
            {
                _SetDrive(value);
            }
        }

        /// <summary>
        /// Gets information about the current drive.
        /// </summary>
        /// <value>The current drive info.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DriveInfo CurrentDriveInfo
        {
            [DebuggerStepThrough]
            get
            {
                return (DriveInfo)SelectedItem;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DriveComboBox()
        {
            _LoadImages();
            _SetupControl();
            _RefreshDriveList();
        }

        #endregion

        #region Private members

        private Bitmap _driveBitmap;
        private Bitmap _floppyBitmap;
        private Bitmap _cdromBitmap;
        private Bitmap _networkBitmap;

        private Dictionary<string, string> _driveLabels
            = new Dictionary<string, string>();

        private static readonly Regex _driveNameRegex
            = new Regex(@"^(?<letter>[A-Z]):");

        private static void _DisposeBitmap(ref Bitmap bitmap)
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }

        private Bitmap _GetDriveBitmap(DriveInfo driveInfo)
        {
            Bitmap result = _driveBitmap;
            switch (driveInfo.DriveType)
            {
                case DriveType.CDRom:
                    result = _cdromBitmap;
                    break;
                case DriveType.Network:
                    result = _networkBitmap;
                    break;
                case DriveType.Removable:
                    result = _floppyBitmap;
                    break;
            }
            return result;
        }

        private string _GetDriveLabel(DriveInfo driveInfo)
        {
            string result;
            if (!_driveLabels.TryGetValue(driveInfo.Name, out result))
            {
                if (driveInfo.IsReady)
                {
                    try
                    {
                        result = driveInfo.VolumeLabel;
                    }
                    catch (IOException exception)
                    {
                        Trace.WriteLine(exception.Message);
                    }
                }
                _driveLabels[driveInfo.Name] = result;
            }
            return result;
        }

        private void _LoadImages()
        {
            _floppyBitmap = _MakeTransparent(RC._35floppy);
            _driveBitmap = _MakeTransparent(RC.drive);
            _cdromBitmap = _MakeTransparent(RC.cddrive);
            _networkBitmap = _MakeTransparent(RC.drivenet);
        }

        private static Bitmap _MakeTransparent(Bitmap bitmap)
        {
            bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            return bitmap;
        }

        private void _RefreshDriveList()
        {
            try
            {
                BeginUpdate();
                Items.Clear();
                _driveLabels.Clear();
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo driveInfo in drives)
                {
                    Items.Add(driveInfo);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        private void _SetDrive(string driveName)
        {
            if (string.IsNullOrEmpty(driveName))
            {
                SelectedIndex = -1;
                return;
            }
            driveName = driveName.ToUpper();
            Match match = _driveNameRegex.Match(driveName);
            if (!match.Success)
            {
                throw new ArgumentException("driveName");
            }
            string driveLetter = match.Groups["letter"].Value;
            foreach (DriveInfo driveInfo in Items)
            {
                if (string.Compare(driveLetter, driveInfo.Name,
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    SelectedItem = driveInfo;
                    return;
                }
            }
            throw new ArgumentOutOfRangeException("driveName");
        }

        private void _SetupControl()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        #endregion

        #region Public methods

        #endregion

        #region ComboBox items

        /// <summary>
        /// Gets an object representing the collection of the items contained 
        /// in this <see cref="ComboBox"/>.
        /// </summary>
        /// <returns>A <see cref="ComboBox.ObjectCollection"/>
        /// representing the items in the <see cref="ComboBox"/>.
        /// </returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden
            )]
        public new ObjectCollection Items
        {
            [DebuggerStepThrough]
            get
            {
                return base.Items;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem"/>
        /// event.
        /// </summary>
        /// <param name="e">A 
        /// <see cref="T:System.Windows.Forms.DrawItemEventArgs"/>
        /// that contains the event data. </param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            e.DrawBackground();
            if ((e.Index < 0)
                 || (e.Index >= Items.Count))
            {
                return;
            }
            Graphics graphics = e.Graphics;
            DriveInfo drive = (DriveInfo)Items[e.Index];
            string label = _GetDriveLabel(drive);
            string text = drive.Name;
            if (!string.IsNullOrEmpty(label))
            {
                text += " [" + label + "]";
            }
            Bitmap driveBitmap = _GetDriveBitmap(drive);
            Rectangle rect = e.Bounds;
            int delta = ItemHeight - 1;
            if (driveBitmap != null)
            {
                rect.X++;
                rect.Width--;
                graphics.DrawImage(driveBitmap, rect.X, rect.Y, delta, delta);
                delta += 4;
                rect.X += delta;
                rect.Width -= delta;
            }
            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                graphics.DrawString(text, e.Font, brush, rect);
            }
            e.DrawFocusRectangle();
        }

        #endregion

        #region Component members

        /// <summary>
        /// Releases the unmanaged resources used by the 
        /// <see cref="ComboBox"/> and optionally releases 
        /// the managed resources. 
        /// </summary>
        /// <param name="disposing">true to release both managed 
        /// and unmanaged resources; false to release only unmanaged 
        /// resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _DisposeBitmap(ref _driveBitmap);
            _DisposeBitmap(ref _networkBitmap);
            _DisposeBitmap(ref _floppyBitmap);
            _DisposeBitmap(ref _cdromBitmap);
        }

        #endregion
    }
}
