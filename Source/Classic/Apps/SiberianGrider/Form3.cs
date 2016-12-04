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
using ManagedIrbis.Worksheet;

namespace SiberianGrider
{
    public partial class Form3 
        : Form
    {
        public SiberianSubFieldGrid Grid { get; set; }

        public MarcRecord Record { get; set; }

        public WssFile Worksheet { get; set; }

        public Form3()
        {
            InitializeComponent();

            Grid = new SiberianSubFieldGrid
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(Grid);

            Record = PlainText.ReadOneRecord
                (
                    "record.txt",
                    IrbisEncoding.Utf8
                )
                .ThrowIfNull("Record");
            Worksheet = WssFile.ReadLocalFile
                (
                    "692.WSS",
                    IrbisEncoding.Ansi
                );

            RecordField field = Record.Fields
                .GetFirstField("692");

            Grid.Load
                (
                    Worksheet,
                    field
                );
        }
    }
}
