using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ManagedIrbis;

namespace SharpIrbis.Scripts
{
    class HelloWorld
    {
        static void Main()
        {
            try
            {
                using (IrbisConnection connection = new IrbisConnection())
                {
                    connection.ParseConnectionString("host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;");
                    connection.Connect();
                    IrbisVersion version = connection.GetServerVersion();
                    MessageBox.Show(version.ToString(), "SharpIrbis");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SharpIrbis");
            }
        }
    }
}
