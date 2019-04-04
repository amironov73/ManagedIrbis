// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using IrbisUI.Universal;

using JetBrains.Annotations;

#endregion

// ReSharper disable StringLiteralTypo

namespace FastGazettes
{
    public partial class MainForm
        : UniversalForm
    {
        #region Properties

        [NotNull]
        public GazettePanel Panel { get; private set; }

        #endregion

        #region Construction

        public MainForm()
        {
            Initialize += _Initialize;

            InitializeComponent();

            HideMainMenu();
            HideToolStrip();
            HideStatusStrip();
            Panel = new GazettePanel(this);
            SetupCentralControl(Panel);
        }

        #endregion

        #region Private members

        private void _Initialize
            (
                object sender,
                EventArgs e
            )
        {
            Icon = Properties.Resources.Gazette;

            if (TestProviderConnection())
            {
                WriteLine("Connection OK");
                Active = true;
                Controller.EnableControls();

                UniversalCentralControl universal = CentralControl
                    as UniversalCentralControl;
                if (!ReferenceEquals(universal, null))
                {
                    universal.SetDefaultFocus();
                }
            }
            else
            {
                Controller.DisableControls();
                return;
            }

            WriteLine("FastGazettes ready");
            Panel.Phase2();
        }

        #endregion
    }
}
