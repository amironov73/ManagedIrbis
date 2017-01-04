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
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Windows.Forms;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using Newtonsoft.Json;

using CM=System.Configuration.ConfigurationManager;

#endregion

namespace BiblioPolice
{
    public partial class MainForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Connection to IRBIS-server.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

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
            Connection = new IrbisConnection();
        }

        #endregion

        #region Private members

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            Connection.Dispose();
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            WriteLine("Started");

            string connectionString = CM.AppSettings["connectionString"];
        }

        #endregion

        #region Public methods

        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            Code.NotNull(format, "format");

            Log.WriteLine(format, args);
        }

        #endregion
    }
}
