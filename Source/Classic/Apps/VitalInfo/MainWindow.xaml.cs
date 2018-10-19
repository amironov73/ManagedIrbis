using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

using Microsoft.VisualBasic.Devices;

namespace VitalInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            FillIpAddress();
            UserNameBox.Text = Environment.UserName;
            DomainNameBox.Text = Environment.UserDomainName;
            FillOperatingSystem();
            FillMemory();
            FillDisks();
        }

        private void FillIpAddress()
        {
            StringBuilder builder = new StringBuilder();
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            bool first = true;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (!first)
                    {
                        builder.Append(Environment.NewLine);
                    }
                    builder.Append(ip);
                    first = false;
                }
            }

            Address4Box.Text = builder.ToString();

            builder.Clear();
            first = true;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    if (!first)
                    {
                        builder.Append(Environment.NewLine);
                    }
                    builder.Append(ip);
                    first = false;
                }
            }

            Address6Box.Text = builder.ToString();
        }

        private void FillOperatingSystem()
        {
            ComputerInfo info = new ComputerInfo();
            OperatingSystemBox.Text = string.Format
                (
                    "{0} {1}",
                    info.OSFullName,
                    info.OSVersion
                );
        }

        private void FillMemory()
        {
            const long Megabyte = 1024 * 1024;
            ComputerInfo info = new ComputerInfo();
            MemoryBox.Text = string.Format
                (
                    "Всего {0} Мб, свободно {1} Мб",
                    info.TotalPhysicalMemory / Megabyte,
                    info.AvailablePhysicalMemory / Megabyte
                );
        }

        private void FillDisks()
        {
            const long Gigabyte = 1024 * 1024 * 1024;
            ServerComputer info = new ServerComputer();
            StringBuilder builder = new StringBuilder();
            bool first = true;
            foreach (DriveInfo drive in info.FileSystem.Drives)
            {
                if (drive.DriveType == DriveType.Fixed)
                {
                    if (!first)
                    {
                        builder.Append(Environment.NewLine);
                    }

                    builder.AppendFormat
                        (
                            "{0} всего {1} Гб, свободно {2} Гб",
                            drive.Name,
                            drive.TotalSize / Gigabyte,
                            drive.AvailableFreeSpace / Gigabyte
                        );

                    first = false;
                }
            }

            DisksBox.Text = builder.ToString();
        }
    }
}
