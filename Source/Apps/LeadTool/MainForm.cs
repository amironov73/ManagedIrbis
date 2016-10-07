/* MainForm.cs
 */

#region Using directives

using System;
using System.Windows.Forms;

using Microsoft.Win32;

#endregion

namespace LeadTrial
{
    public partial class MainForm 
        : Form
    {
        static int S 
            ( 
                int v 
            )
        {
            return v < 0
                ? 0
                : v < 10
                    ? v
                    : v % 10 + S(v / 10);
        }

        static string Encode 
            (
                int month, 
                int day, 
                int year
            )
        {
            string result = string.Format
                (
                    "{0:00}{1:00}{2:00}{3:0000}",
                    S(99 - month) + S(99 - day) + S(9999 - year),
                    99 - month,
                    99 - day,
                    9999 - year
                );

            return result;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void _goButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            DateTime date = _datePicker.Value;
            string encoded = Encode
                (
                    date.Month,
                    date.Day,
                    date.Year
                );
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey
                (
                    @"SOFTWARE\Tryon\18", 
                    true 
                ))
            {
                if (!ReferenceEquals(key, null))
                {
                    key.SetValue
                        (
                            null,
                            encoded,
                            RegistryValueKind.String
                        );
                }
            }
        }
    }
}
