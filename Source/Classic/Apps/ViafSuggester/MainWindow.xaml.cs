using System;
using System.Collections.Generic;
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

using RestfulIrbis.Viaf;

namespace ViafSuggester
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

        private void GoButton_OnClick(object sender, RoutedEventArgs e)
        {
            resultBox.Clear();
            string name = termBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            try
            {
                ViafClient client = new ViafClient();
                ViafSuggestResult[] suggestions = client.GetSuggestions(name);
                string text = string.Join
                    (
                        Environment.NewLine,
                        suggestions.Select(s => s.DisplayForm)
                            .OrderBy(s => s)
                    );
                resultBox.Text = text;
            }
            catch (Exception exception)
            {
                resultBox.Text = exception.ToString();
            }
        }
    }
}
