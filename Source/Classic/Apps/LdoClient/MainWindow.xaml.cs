using System;
using System.Collections.ObjectModel;
using System.Windows;

using Newtonsoft.Json.Linq;

using RestSharp;

namespace LdoClient
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

        private void DoSearch()
        {
            FoundBox.ItemsSource = null;

            string term = TermBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(term))
            {
                return;
            }

            RestClient client = new RestClient("https://sparql.rsl.ru/bbkskos");
            string sparql =
                "PREFIX text: <http://jena.apache.org/text#> " +
                "PREFIX skos: <http://www.w3.org/2004/02/skos/core#> " +
                "SELECT DISTINCT ?concept ?notation ?hiddenLabel " +
                "WHERE { " +
                $"?concept text:query (skos:hiddenLabel '{term}') ; " +
                "skos:notation ?notation ; " +
                "skos:hiddenLabel ?hiddenLabel . " +
                "}";

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("query", sparql);
            request.AddHeader("Accept", "application/sparql-results+json;charset=utf-8");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
            RestResponse response = (RestResponse)client.Execute(request);
            string text = response.Content;
            JObject obj = JObject.Parse(text);
            RootObject root = obj.ToObject<RootObject>();
            var list = new ObservableCollection<BbkItem>();
            foreach (var binding in root.Results.Bindings)
            {
                BbkItem item = new BbkItem
                {
                    Index = binding.Notation.Value,
                    Text = binding.HiddenLabel.Value
                };
                list.Add(item);
            }

            FoundBox.ItemsSource = list;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoSearch();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }
    }
}
