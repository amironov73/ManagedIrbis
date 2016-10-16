/* TodayControl.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Suggestions
{
    /// <summary>
    /// Suggests today date.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed partial class TodayControl
        : UserControl
    {
        #region Properties

        /// <summary>
        /// Success.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Date.
        /// </summary>
        public DateTime Date { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TodayControl
            (
                IWindowsFormsEditorService service,
                DateTime? selected
            )
        {
            _service = service;
            InitializeComponent();

            if (selected != null)
            {
                _calendar.SelectionStart = selected.Value;
                _calendar.SelectionEnd = selected.Value;
            }
            _calendar.DoubleClick += _calendar_DoubleClick;
        }

        #endregion

        #region Private members

        private readonly IWindowsFormsEditorService _service;

        private void _calendar_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            Date = _calendar.SelectionStart;
            Success = true;
            _service.CloseDropDown();
        }

        private void _cancelButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Success = false;
            _service.CloseDropDown();
        }

        private void _okButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Date = _calendar.SelectionStart;
            Success = true;
            _service.CloseDropDown();
        }

        #endregion

    }
}