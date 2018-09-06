using System;
using System.Collections.Generic;
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

using ManagedIrbis;
using ManagedIrbis.Search;
using Microsoft.Win32;

namespace WpfDupolov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
        : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SetStatus("Приложение запущено");
            ClearLog();
            AppendLog("Приложение запущено");
            if (TryConnection())
            {
                FillDatabases();
            }
        }

        private TheEngine _engine;

        public IIrbisConnection GetConnection()
        {
            return IrbisConnectionUtility.GetClientFromConfig();
        }

        public void SimpleIvoke(Action action)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(action);
                return;
            }

            action();
        }

        public void SimpleIvoke<T>(Action<T> action, T arg1)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(action, arg1);
                return;
            }

            action(arg1);
        }

        public void SimpleIvoke<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(action, arg1, arg2);
                return;
            }

            action(arg1, arg2);
        }

        public void SimpleIvoke<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1,
            T2 arg2, T3 arg3)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(action, arg1, arg2, arg3);
                return;
            }

            action(arg1, arg2, arg3);
        }

        public void SetStatus(string format, params object[] args)
        {
            string message = string.Format(format, args);
            SimpleIvoke(msg => statusLabel.Content = msg, message);
        }

        private void AppendText(string text)
        {
            logBox.AppendText(text + "\n");
            logBox.ScrollToEnd();
        }

        public void AppendLog(string text)
        {
            SimpleIvoke(AppendText, text);
        }

        public void ClearLog()
        {
            SimpleIvoke(() => logBox.Clear());
        }

        public bool TryConnection()
        {
            try
            {
                using (IIrbisConnection connection = GetConnection())
                {
                    connection.NoOp();
                }

                AppendLog("Подключение успешно установлено");
                return true;
            }
            catch (Exception ex)
            {
                AppendLog(ex.ToString());
            }

            return false;
        }

        public void FillDatabases()
        {
            try
            {
                DatabaseInfo[] databases;
                using (IIrbisConnection connection = GetConnection())
                {
                    databases = connection.ListDatabases();
                }

                databaseBox.ItemsSource = databases;
                AppendLog("Всего баз данных: " + databases.Length);
                if (databases.Length != 0)
                {
                    databaseBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.ToString());
            }
        }

        public string GetCurrentDatabase()
        {
            DatabaseInfo database = databaseBox.SelectedValue as DatabaseInfo;
            return database?.Name;
        }

        public string GetCurrentPrefix()
        {
            SearchScenario scenario = termBox.SelectedValue as SearchScenario;
            return scenario?.Prefix;
        }

        public void FillSearchScenarios()
        {
            string database = GetCurrentDatabase();
            if (string.IsNullOrEmpty(database))
            {
                return;
            }

            try
            {
                SearchScenario[] scenarios;
                using (IIrbisConnection connection = GetConnection())
                {
                    string fileName = database + ".mnu";
                    scenarios = connection.ReadSearchScenario(fileName);
                    if (scenarios.Length == 0)
                    {
                        scenarios = SearchScenario.ParseIniFile(connection.IniFile);
                    }
                }

                termBox.ItemsSource = scenarios;
                AppendLog("Поисковых сценариев: " + scenarios.Length);
                if (scenarios.Length != 0)
                {
                    termBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.ToString());
            }
        }

        private void DatabaseBox_OnSelectionChanged
            (
                object sender,
                SelectionChangedEventArgs e
            )
        {
            FillSearchScenarios();
        }

        public void StartEngine()
        {
            ClearLog();

            string database = GetCurrentDatabase();
            if (string.IsNullOrEmpty(database))
            {
                AppendLog("Не задана база данных");
                return;
            }

            string prefix = GetCurrentPrefix();
            if (string.IsNullOrEmpty(prefix))
            {
                AppendLog("Не задан префикс");
                return;
            }

            try
            {
                IIrbisConnection connection = GetConnection();
                connection.Database = database;
                _engine = new TheEngine(this, connection, prefix);
                _engine.NeedDump = dumpBox.IsChecked == true;

                Task task = new Task(_engine.Process);
                task.ContinueWith(StopEngine);
                AppendLog("ЗАДАЧА ЗАПУЩЕНА");
                task.Start();
            }
            catch (Exception ex)
            {
                AppendLog(ex.ToString());
            }
        }

        public void StopEngine(Task task)
        {
            AppendLog("ЗАДАЧА ЗАВЕРШЕНА");
            _engine = null;
            if (!ReferenceEquals(task.Exception, null))
            {
                AppendLog(task.Exception.ToString());
            }
        }

        private void ActionButton_OnClick
            (
                object sender,
                RoutedEventArgs e
            )
        {
            if (ReferenceEquals(_engine, null))
            {
                StartEngine();
            }
            else
            {
                _engine.Cancel = true;
            }
        }

        private void SaveButton_OnClick
            (
                object sender,
                RoutedEventArgs e
            )
        {
            string database = GetCurrentDatabase() ?? "БАЗАДАННЫХ";
            SearchScenario scenario = termBox.SelectedValue as SearchScenario;
            string prefix = scenario?.Name ?? "ПРЕФИКС";
            string proposedFileName = string.Format
                (
                    "{0}_{1}_{2}.txt",
                    database,
                    prefix,
                    DateTime.Today.ToString("yyyy-MM-dd")
                );

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = proposedFileName;
            if (dialog.ShowDialog(this) == true)
            {
                File.WriteAllText(dialog.FileName, logBox.Text, Encoding.UTF8);
            }
        }
    }
}
