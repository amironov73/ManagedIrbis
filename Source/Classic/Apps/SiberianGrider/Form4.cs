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

using IrbisUI.Grid;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Search;
using ManagedIrbis.Worksheet;

using CM=System.Configuration.ConfigurationManager;

namespace SiberianGrider
{
    public partial class Form4
        : Form
    {
        public SiberianTermGrid Grid { get; set; }

        public IrbisConnection Connection { get; set; }

        public Form4()
        {
            InitializeComponent();

            string connectionString = CM.AppSettings["connectionString"];
            Connection = new IrbisConnection();
            Connection.ParseConnectionString(connectionString);
            Connection.Connect();

            Grid = new SiberianTermGrid
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(Grid);

            TermParameters parameters = new TermParameters
            {
                //Database = "IBIS",
                NumberOfTerms = 100,
                //ReverseOrder = false,
                StartTerm = "K=",
                //Format = null
            };

            TermInfo[] terms = Connection.ReadTerms(parameters);
            terms = TermInfo.TrimPrefix(terms, "K=");

            Grid.Load(terms);

            FormClosed += _FormClosed;
        }

        void _FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            if (Connection != null)
            {
                Connection.Dispose();
            }
        }



    }
}
