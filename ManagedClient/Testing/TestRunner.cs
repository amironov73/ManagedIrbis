/* TestRunner.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#if FW45

#region Using directives

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedClient;
using ManagedClient.Testing;

using Microsoft.CSharp;

using Newtonsoft.Json.Linq;

#endregion

namespace IrbisTestRunner
{
    /// <summary>
    /// Test runner.
    /// </summary>
    [PublicAPI]
    public sealed class TestRunner
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [CanBeNull]
        public IrbisConnection Connection { get; set; }

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
        /// Path to the test data.
        /// </summary>
        [CanBeNull]
        public string DataPath { get; set; }

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

        /// <summary>
        /// Assembly references.
        /// </summary>
        [NotNull]
        public List<string> References { get { return _references; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TestRunner()
        {
            _testList = new List<string>();
            _references = new List<string>();
        }

        #endregion

        #region Private members

        private const int WM_CLOSE = 0x0010;
        private const int WM_QUIT = 0x0012;

        private const string DefaultProcessName = "irbis_server";
        private const string ServerWindowTitle = "TCP/IP-Сервер ИРБИС 64";
        private const string ServerWindowClass = "TLabelServer";

        [DllImport("User32.dll", SetLastError = false)]
        private static extern int PostThreadMessage
            (
                int threadId,
                int msg,
                int wParam,
                IntPtr lParam
            );

        [DllImport("User32.dll", SetLastError = false)]
        private static extern IntPtr FindWindow
            (
                string className,
                string windowTitle
            );

        [DllImport("User32.dll", SetLastError = false)]
        private static extern IntPtr SendMessage
            (
                IntPtr hwnd,
                int msg,
                int lParam,
                int wParam
            );

        private readonly List<string> _testList;
        private readonly List<string> _references;

        private Type[] _GetTestClasses
            (
                Assembly assembly
            )
        {
            Type[] types = assembly.GetTypes();
            List<Type> result = new List<Type>();
            foreach (Type type in types)
            {
                TestClassAttribute classAttribute
                    = type.GetCustomAttribute<TestClassAttribute>();
                if (classAttribute == null)
                {
                    continue;
                }
                if (!type.IsSubclassOf(typeof(AbstractTest)))
                {
                    continue;
                }

                if (_GetTestMethods(type).Length == 0)
                {
                    continue;
                }

                result.Add(type);
            }

            return result.ToArray();
        }

        private MethodInfo[] _GetTestMethods
            (
                Type type
            )
        {
            List<MethodInfo> result = new List<MethodInfo>();

            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                TestMethodAttribute methodAttribute
                    = method.GetCustomAttribute<TestMethodAttribute>();
                if (methodAttribute == null)
                {
                    continue;
                }

                if (method.GetParameters().Length != 0)
                {
                    continue;
                }

                if (method.ReturnType != typeof(void))
                {
                    continue;
                }

                result.Add(method);
            }

            return result.ToArray();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compile tests.
        /// </summary>
        public void CompileTests()
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
                    References.ToArray()
                )
            {
                CompilerOptions = "/d:DEBUG",
                IncludeDebugInformation = true
            };
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

        /// <summary>
        /// Discover tests.
        /// </summary>
        public void DiscoverTests()
        {
            WriteLine(ConsoleColor.Yellow, "Discovering tests");

            int count = 0;
            foreach (string item in TestList)
            {
                string fullPath = Path.Combine
                    (
                        TestPath,
                        item
                    );

                Console.Write("\t{0}: ", item);
                if (File.Exists(fullPath))
                {
                    WriteLine(ConsoleColor.Green, "OK");
                    count++;
                }
                else
                {
                    WriteLine(ConsoleColor.Red, "not found!");
                    throw new FileNotFoundException(fullPath);
                }
            }

            WriteLine(ConsoleColor.Yellow, "Source files found: {0}", count);
        }

        /// <summary>
        /// Find running local server process.
        /// </summary>
        public bool FindLocalServer()
        {
            Process[] processes = Process.GetProcessesByName
                (
                    "irbis_server"
                );

            return processes.Length != 0;
        }

        /// <summary>
        /// Hide server window.
        /// </summary>
        public void HideServerWindow()
        {
            if (ServerProcess != null)
            {
                HideWindow
                    (
                        ServerWindowClass,
                        ServerWindowTitle
                    );
            }
        }

        /// <summary>
        /// Hide window
        /// </summary>
        public static void HideWindow
            (
                string className,
                string windowTitle
            )
        {
            Code.NotNull(className, "className");
            Code.NotNull(windowTitle, "windowTitle");

            IntPtr hwnd = FindWindow(className, windowTitle);
            if (hwnd != IntPtr.Zero)
            {
                SendMessage(hwnd, WM_CLOSE, 0, 0);
            }
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

            DataPath = root["dataPath"]
                .ThrowIfNull()
                .ToString();

            TestPath = root["testPath"]
                .ThrowIfNull()
                .ToString();

            JToken tests = root["tests"];
            foreach (JToken child in tests.Children())
            {
                string testName = child.ToString();
                TestList.Add(testName);
            }

            JToken references = root["references"];
            foreach (JToken child in references.Children())
            {
                string assemblyReferences = child.ToString();
                References.Add(assemblyReferences);
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
            Type[] testClasses = _GetTestClasses(CompiledAssembly);

            WriteLine(ConsoleColor.White, "Test execution started");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (Connection = new IrbisConnection())
            {
                Connection.ParseConnectionString(ConnectionString);
                Connection.Connect();

                foreach (Type testClass in testClasses)
                {
                    RunTests(testClass);
                }
            }
            stopwatch.Stop();

            WriteLine(ConsoleColor.White, "Test execution finished");
            WriteLine(ConsoleColor.Gray, "Time elapsed: {0}", stopwatch.Elapsed);
        }

        /// <summary>
        /// Run tests in the class.
        /// </summary>
        public void RunTests
            (
                Type testClass
            )
        {
            MethodInfo[] testMethods = _GetTestMethods(testClass);

            AbstractTest testObject
                = (AbstractTest)Activator.CreateInstance(testClass);
            testObject.Connection = Connection;
            testObject.DataPath = DataPath;

            foreach (MethodInfo method in testMethods)
            {
                RunTests
                    (
                        testObject,
                        method
                    );

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Run tests in the class.
        /// </summary>
        public void RunTests
            (
                AbstractTest testObject,
                MethodInfo method
            )
        {
            Write(ConsoleColor.Cyan, "{0} ", method.Name);

            Action action = (Action)method.CreateDelegate
                (
                    typeof(Action),
                    testObject
                );

            action();

            WriteLine(ConsoleColor.Green, " OK");
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

            WriteLine(ConsoleColor.Yellow, "IRBIS_SERVER.EXE started");

            Write(ConsoleColor.Gray, "Giving some time for server initialization... ");

            Thread.Sleep(3000);

            WriteLine(ConsoleColor.Gray, "Server initialized");
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
                WriteLine(ConsoleColor.Yellow, "Server not started");
                return;
            }

            Write(ConsoleColor.Yellow, "Stopping the server... ");

            StopProcess(ServerProcess);

            Thread.Sleep(3000);

            WriteLine(ConsoleColor.Green, "IRBIS_SERVER.EXE stopped");
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void Write
            (
                ConsoleColor color,
                string text
            )
        {
            ConsoleColor savedColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = savedColor;
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        [StringFormatMethod("format")]
        public void Write
            (
                ConsoleColor color,
                string format,
                params object[] arguments
            )
        {
            ConsoleColor savedColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(format, arguments);
            Console.ForegroundColor = savedColor;
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                ConsoleColor color,
                string text
            )
        {
            ConsoleColor savedColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = savedColor;
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        [StringFormatMethod("format")]
        public void WriteLine
            (
                ConsoleColor color,
                string format,
                params object[] arguments
            )
        {
            ConsoleColor savedColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, arguments);
            Console.ForegroundColor = savedColor;
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

#endif
