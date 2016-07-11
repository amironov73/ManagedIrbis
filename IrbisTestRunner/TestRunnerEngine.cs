/* Program.cs -- application entry point
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedClient;
using Microsoft.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace IrbisTestRunner
{
    /// <summary>
    /// Engine.
    /// </summary>
    [PublicAPI]
    public sealed class TestRunnerEngine
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Connection string for IRBIS64-server.
        /// </summary>
        [CanBeNull]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Path to irbis_server.exe (including filename).
        /// </summary>
        [CanBeNull]
        public string IrbisServerPath { get; set; }

        /// <summary>
        /// Server process (for stopping).
        /// </summary>
        [CanBeNull]
        public Process ServerProcess { get; set; }

        /// <summary>
        /// Path to the tests.
        /// </summary>
        [CanBeNull]
        public string TestPath { get; set; }

        /// <summary>
        /// List of tests.
        /// </summary>
        [NotNull]
        public List<string> TestList { get { return _testList; } }

        /// <summary>
        /// Compiled assembly.
        /// </summary>
        [CanBeNull]
        public Assembly CompiledAssembly { get; set; }

        #endregion

        #region Construction

        public TestRunnerEngine()
        {
            _testList = new List<string>();
        }

        #endregion

        #region Private members

        private const int WM_QUIT = 0x0012;

        private const string DefaultProcessName = "irbis_server";

        [DllImport("User32.dll", SetLastError = false)]
        private static extern int PostThreadMessage
            (
                int threadId,
                int msg,
                int wParam,
                IntPtr lParam
            );

        private readonly List<string> _testList;

        #endregion

        #region Public methods

        /// <summary>
        /// Compile tests.
        /// </summary>
        public void CompileTests ()
        {
            Console.WriteLine("Compiling");

            List<string> sources = new List<string>();
            foreach (string item in TestList)
            {
                string fileName = Path.Combine
                    (
                        TestPath,
                        item
                    );
                sources.Add(fileName);
            }

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters
                (
                    new []
                    {
                        "AM.Core.dll",
                        "Antlr4.Runtime.net45.dll",
                        "JetBrains.Annotations.dll",
                        "ManagedClient.dll",
                        "Microsoft.CSharp.dll",
                        "MoonSharp.Interpreter.dll",
                        "Newtonsoft.Json.dll",
                        "System.dll",
                        "System.Core.dll",
                        "System.Data.dll",
                        "System.Data.DataSetExtensions.dll",
                        "System.Xml.dll",
                        "System.Xml.Linq.dll"
                    }
                );
            CompilerResults results = provider.CompileAssemblyFromFile
                (
                    parameters,
                    sources.ToArray()
                );
            bool haveError = false;
            foreach (var error in results.Errors)
            {
                Console.WriteLine(error);
                haveError = true;
            }

            if (haveError)
            {
                throw new ApplicationException("Can't compile");
            }

            CompiledAssembly = results.CompiledAssembly;

            Console.WriteLine("Tests compiled");
        }

        public void DiscoverTests()
        {
            Console.WriteLine("Discovering tests");

            int count = 0;
            foreach (string item in TestList)
            {
                string fullPath = Path.Combine
                    (
                        TestPath,
                        item
                    );

                Console.Write("{0}: ", item);
                if (File.Exists(fullPath))
                {
                    Console.WriteLine("ok");
                    count++;
                }
                else
                {
                    Console.WriteLine("not found!");
                    throw new FileNotFoundException(fullPath);
                }
            }

            Console.WriteLine("Source files found: {0}", count);
        }

        /// <summary>
        /// Load configuration from local JSON file.
        /// </summary>
        public void LoadConfig
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            JObject root = JObject.Parse
                (
                    File.ReadAllText(fileName, Encoding.UTF8)
                );

            IrbisServerPath = root["irbisServer"]
                .ThrowIfNull()
                .ToString();

            ConnectionString = root["connectionString"]
                .ThrowIfNull()
                .ToString();

            TestPath = root["testPath"]
                .ThrowIfNull()
                .ToString();

            JToken tests = root["tests"];
            foreach (JToken item in tests.Children())
            {
                string testName = item.ToString();
                TestList.Add(testName);
            }
        }

        /// <summary>
        /// Ping the server.
        /// </summary>
        public void PingTheServer()
        {
            Console.Write("Pinging the server... ");

            using (IrbisConnection connection = new IrbisConnection())
            {
                connection.ParseConnectionString(ConnectionString);
                connection.Connect();
            }

            Console.WriteLine("done");
        }

        /// <summary>
        /// Run the tests.
        /// </summary>
        public void RunTests()
        {
            Console.WriteLine("Test execution started");

            Thread.Sleep(1000);

            Console.WriteLine("Test execution finished");
        }

        /// <summary>
        /// Start server.
        /// </summary>
        public void StartServer()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    IrbisServerPath
                )
            {
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Minimized
            };

            ServerProcess = Process.Start(startInfo);

            Console.WriteLine("IRBIS_SERVER.EXE started");

            Console.Write("Giving some time for server initialization... ");

            Thread.Sleep(3000);

            Console.WriteLine("Server initialized");
        }

        /// <summary>
        /// Stop process.
        /// </summary>
        public static bool StopProcess
            (
                [NotNull] Process process
            )
        {
            Code.NotNull(process, "process");

            try
            {
                foreach (ProcessThread thread in process.Threads)
                {
                    PostThreadMessage
                        (
                            thread.Id,
                            WM_QUIT,
                            0,
                            IntPtr.Zero
                        );
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Stop server.
        /// </summary>
        public void StopServer()
        {
            if (ServerProcess == null)
            {
                Console.WriteLine("Server not running");
                return;
            }

            Console.WriteLine("Stopping the server");

            StopProcess(ServerProcess);

            Thread.Sleep(3000);

            Console.WriteLine("IRBIS_SERVER.EXE stopped");
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            bool result = !string.IsNullOrEmpty(IrbisServerPath)
                && !string.IsNullOrEmpty(ConnectionString)
                && !string.IsNullOrEmpty(TestPath);

            if (!result && throwOnError)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion
    }
}
