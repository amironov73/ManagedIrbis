// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniversalCentralControl.cs -- 
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
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Logging;
using AM.Text.Output;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Universal
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]

    public partial class UniversalCentralControl 
        : UserControl
    {
        #region Properties

        /// <summary>
        /// Main form.
        /// </summary>
        [CanBeNull]
        public UniversalForm MainForm { get; private set; }

        /// <summary>
        /// Output.
        /// </summary>
        [CanBeNull]
        public AbstractOutput Output
        {
            get
            {
                return ReferenceEquals(MainForm, null)
                    ? null
                    : MainForm.Output;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected UniversalCentralControl()
        {
            // Constructor for WinForms Designer only
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UniversalCentralControl
            (
                [CanBeNull] UniversalForm mainForm
            )
        {
            MainForm = mainForm;

            InitializeComponent();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Run the specified action.
        /// </summary>
        public bool Run
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            bool result = false;

            UniversalForm mainForm = MainForm;
            if (!ReferenceEquals(mainForm, null))
            {
                result = mainForm.Controller.Run(action);
            }

            return result;
        }

        /// <summary>
        /// Set default focus to control.
        /// </summary>
        public virtual void SetDefaultFocus()
        {
            // Nothing to do here
        }

        /// <summary>
        /// Write line.
        /// </summary>
        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            if (!ReferenceEquals(Output, null))
            {
                Output.WriteLine(format, args);
            }
        }


        #endregion
    }
}
