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

using AM;
using AM.IO;
using JetBrains.Annotations;

using ManagedIrbis;

namespace WpfAdmin
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

        private DatabaseInfo[] databases;

        private IrbisConnection GetConnection()
        {
            return IrbisConnectionUtility.GetClientFromConfig();
        }

        private void Window_Initialized
            (
                object sender,
                EventArgs e
            )
        {
            string initialDatabase = null;

            using (IrbisConnection connection = GetConnection())
            {
                IniFile iniFile = connection.IniFile;
                string listFile = "dbnam1.mnu";
                if (!ReferenceEquals(iniFile, null))
                {
                    iniFile.GetValue("MAIN", "DBNNAMECAT", "dbnam1.mnu");
                    string ext = System.IO.Path.GetExtension(listFile);
                    if (string.IsNullOrEmpty(ext))
                    {
                        listFile += ".mnu";
                    }

                    initialDatabase = iniFile.GetValue("MAIN", "CURDBN", null);
                }

                databases = connection.ListDatabases(listFile);
                foreach (DatabaseInfo database in databases)
                {
                    DatabaseList.Items.Add(database);
                }
            }

            int initialIndex = 0;
            if (!string.IsNullOrEmpty(initialDatabase))
            {
                for (int i = 0; i < databases.Length; i++)
                {
                    if (initialDatabase.SameString(databases[i].Name))
                    {
                        initialIndex = i;
                        break;
                    }
                }
            }

            if (initialIndex >= 0 && initialIndex < databases.Length)
            {
                DatabaseList.SelectedIndex = initialIndex;
                object selectedItem = DatabaseList.SelectedItem;
                if (!ReferenceEquals(selectedItem, null))
                {
                    DatabaseList.ScrollIntoView(selectedItem);
                }
            }
        }

        private void DatabaseList_SelectionChanged
            (
                object sender,
                SelectionChangedEventArgs e
            )
        {
            DatabaseInfo selectedDatabase = DatabaseList.SelectedItem as DatabaseInfo;
            if (ReferenceEquals(selectedDatabase, null))
            {
                return;
            }

            string name = selectedDatabase.Name.ThrowIfNull();
            DatabaseDescription description = new DatabaseDescription
            {
                Name = name,
                Description = selectedDatabase.Description
            };
            using (IrbisConnection connection = GetConnection())
            {
                DatabaseInfo info = connection.GetDatabaseInfo(name);
                description.MaxMfn = info.MaxMfn - 1;
                description.ExclusiveLock = info.DatabaseLocked;
                description.LockedRecords = info.LockedRecords.SafeLength();
                description.LogicallyDeleted = info.LogicallyDeletedRecords.SafeLength();
                description.PhysicallyDeleted = info.PhysicallyDeletedRecords.SafeLength();
                description.NonactualizedRecords = info.NonActualizedRecords.SafeLength();
            }

            DataContext = description;
        }

        private static bool Contains
            (
                [NotNull] string outer,
                [NotNull] string inner
            )
        {
            return outer.IndexOf(inner, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private void KeyBox_TextChanged
            (
                object sender,
                TextChangedEventArgs e
            )
        {
            TextBox textBox = sender as TextBox;
            if (ReferenceEquals(textBox, null))
            {
                return;
            }

            string text = textBox.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            DatabaseInfo found = null;
            foreach (DatabaseInfo database in databases)
            {
                string name = database.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    if (Contains(name, text))
                    {
                        found = database;
                        break;
                    }
                }

                string description = database.Description;
                if (!string.IsNullOrEmpty(description))
                {
                    if (Contains(description, text))
                    {
                        found = database;
                        break;
                    }
                }
            }

            if (!ReferenceEquals(found, null))
            {
                DatabaseList.SelectedItem = found;
                DatabaseList.ScrollIntoView(found);
            }
        }
    }
}
