using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AM;
using AM.Windows;
using Microsoft.Win32;

namespace SciInvent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            BookLogic.LogWindow = this;
            BookLogic.Connect();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            BookLogic.Disconnect();
        }

        public void Clear()
        {
            LogBox.Clear();
        }

        public void WriteLine
            (
                string format, params object[] args
            )
        {
            string text = string.Format(format, args) + Environment.NewLine;
            LogBox.AppendText(text);
            LogBox.ScrollToEnd();
        }

        private void ReadDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
            string fileName;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this) != true)
            {
                return;
            }

            fileName = openFileDialog.FileName;
            string[] lines = File.ReadAllLines(fileName)
                .NonEmptyLines().ToArray();
            WriteLine("Прочитано строк: {0}", lines.Length);
            Task.Factory.StartNew
                (
                    () =>
                    {
                        BookLogic.MarkBooks(lines);
                    },
                    TaskCreationOptions.LongRunning
                );
        }

        private void GoodBooksButton_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew
                (
                    BookLogic.ListGoodBooks,
                    TaskCreationOptions.LongRunning
                );
        }

        private void MissingBooksButton_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew
                (
                    BookLogic.ListMissingBooks,
                    TaskCreationOptions.LongRunning
                );
        }
    }
}
