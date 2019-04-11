using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.Search;
using CM=System.Configuration.ConfigurationManager;

namespace Dundee
{
    public partial class MainForm
        : Form
    {
        public MainForm()
        {
            try
            {
                string connectionString = CM.AppSettings["connection-string"];
                Program.Connection = new IrbisConnection(connectionString);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                MessageBox.Show(exception.ToString());
                Environment.Exit(1);
            }

            InitializeComponent();

            _searchBox.Setup();
            _searchBox.Found += _searchBox_Found;
        }

        private void _searchBox_Found(object sender, EventArgs e)
        {
            _foundBox.Items.Clear();
            _foundBox.Items.AddRange(_searchBox.Records);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                IrbisConnection connection = Program.Connection;
                connection?.Dispose();

                Program.Connection = connection;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            base.OnFormClosed(e);
        }

        private void SetBusy(bool state)
        {
            _busyStripe.Visible = state;
            _busyStripe.Moving = state;
        }

        private async Task<int[]> CountLoans(int mfn)
        {
            int[] result = new int[0];
            try
            {
                SetBusy(true);
                string index = Utility.ConvertMfnToIndex(mfn);
                if (!string.IsNullOrEmpty(index))
                {
                    result = await Task.Run(() => Utility.CountLoans(index));
                }
            }
            finally
            {
                SetBusy(false);
            }

            return result;
        }

        private async void _foundBox_DoubleClick(object sender, EventArgs e)
        {
            _chart.Values = new int[0];
            _chart.Invalidate();
            FoundItem item = _foundBox.SelectedItem as FoundItem;
            if (ReferenceEquals(item, null))
            {
                return;
            }

            _chart.Values = await CountLoans(item.Mfn);
            _chart.Invalidate();
        }
    }
}
