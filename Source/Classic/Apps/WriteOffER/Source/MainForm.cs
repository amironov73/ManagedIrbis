// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MainForm.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Collections;
using AM.Configuration;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Universal;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable UseNullPropagation
// ReSharper disable StringLiteralTypo
// ReSharper disable LocalizableElement

namespace WriteOffER
{
    public partial class MainForm
        : UniversalForm
    {
        #region Properties

        [NotNull]
        public OffPanel Panel { get; private set; }

        #endregion

        #region Construction

        public MainForm()
        {
            Initialize += _Initialize;

            InitializeComponent();

            HideMainMenu();
            HideToolStrip();
            HideStatusStrip();
            Panel = new OffPanel(this);
            SetupCentralControl(Panel);
        }

        #endregion

        #region Private members

        private string _connectionString;

        private string GetConnectionString()
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                return _connectionString;
            }

            string result = ConfigurationUtility
                .GetString("connectionString")
                .ThrowIfNull("connectionString");

            return result;
        }

        public IrbisConnection GetConnection()
        {
            string connectionString = GetConnectionString();

            return new IrbisConnection(connectionString);
        }

        private bool SetupLogin()
        {
            string connectionString = GetConnectionString()
                .ThrowIfNull("connectionString");
            IrbisConnection client = new IrbisConnection();
            client.ParseConnectionString(connectionString);

            if ((string.IsNullOrEmpty(client.Username)
                 || string.IsNullOrEmpty(client.Password)))
            {
                if (!IrbisLoginCenter.Login(client, null))
                {
                    client.Dispose();

                    return false;
                }

                ConnectionSettings settings
                    = ConnectionSettings.FromConnection(client);
                _connectionString = settings.Encode();
            }

            client.Dispose();

            connectionString = GetConnectionString();
            client = new IrbisConnection();
            client.ParseConnectionString(connectionString);

            try
            {
                client.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show
                    (
                        ex.ToString(),
                        "ОШИБКА",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                client.Dispose();

                return false;
            }

            client.Dispose();

            return true;
        }


        private void _Initialize
            (
                object sender,
                EventArgs e
            )
        {
            Icon = Properties.Resources.WriteOff;

            if (!SetupLogin())
            {
                Application.Exit();
                return;
            }

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

            WriteLine("WriteOffER ready");
        }

        #endregion
    }
}
