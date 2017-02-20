using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Run32bit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                MessageBox.Show("Usage: Run32bit <program.exe>");
                return 1;
            }

            try
            {
                string fileName = Path.GetFullPath(args[0]);
                Assembly assembly = Assembly.LoadFile(fileName);
                MethodInfo entryPoint = assembly.EntryPoint;
                ParameterInfo[] parameters = entryPoint.GetParameters();

                if (parameters.Length == 0)
                {
                    entryPoint.Invoke(null, null);
                }
                else
                {
                    string[] args2 = args.Skip(1).ToArray();

                    entryPoint.Invoke(null, new object[] {args2});
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 1;
            }

            return 0;
        }
    }
}
