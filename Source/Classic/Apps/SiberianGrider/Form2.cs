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
    public partial class Form2
        : Form
    {
        public SiberianFieldGrid Grid { get; set; }

        public MarcRecord Record { get; set; }

        public WsFile Worksheet { get; set; }

        public Form2()
        {
            InitializeComponent();

            Grid = new SiberianFieldGrid
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
            Worksheet = WsFile.ReadLocalFile
                (
                    "PAZK31.WS",
                    IrbisEncoding.Ansi
                );

            WorksheetPage page = Worksheet.Pages[1];
            Grid.Load
                (
                    page,
                    Record
                );
        }
    }
}
