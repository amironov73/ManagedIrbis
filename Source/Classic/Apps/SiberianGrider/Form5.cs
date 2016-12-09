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

using CM = System.Configuration.ConfigurationManager;

namespace SiberianGrider
{
    public partial class Form5
        : Form
    {
        public SiberianFoundGrid Grid { get; set; }

        public IrbisConnection Connection { get; set; }


        public Form5()
        {
            InitializeComponent();

            string connectionString = CM.AppSettings["connectionString"];
            Connection = new IrbisConnection();
            Connection.ParseConnectionString(connectionString);
            Connection.Connect();

            Grid = new SiberianFoundGrid
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(Grid);

            List<FoundLine> lines = new List<FoundLine>();

            for (int i = 1; i < 100; i++)
            {
                try
                {
                    string description = Connection.FormatRecord("@brief", i);
                    FoundLine line = new FoundLine
                    {
                        Mfn = i,
                        Materialized = true,
                        Description = description
                    };
                    lines.Add(line);
                }
                catch
                {
                    // Nothing to do
                }
            }

            Connection.Dispose();

            Grid.Load(lines.ToArray());

            //FormClosed += _FormClosed;
        }

        //void _FormClosed
        //    (
        //        object sender,
        //        FormClosedEventArgs e
        //    )
        //{
        //    if (Connection != null)
        //    {
        //        Connection.Dispose();
        //    }
        //}


    }
}
