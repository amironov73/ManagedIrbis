/* UnmanagedLibrary.cs -- helper for calling unmanaged methods from managed code
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using CodeJam;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Helper for calling unmanaged methods from
    /// managed code.
    /// </summary>
    /// <remarks>
    /// Inspired by Mike Stall's.Net Debugging Blog
    /// http://blogs.msdn.com/jmstall/default.aspx
    /// </remarks>
    /// <example>
    /// <para>Sample usage may be:</para>
    /// <code>
    /// using ( Unmanaged library lib = new UnmanagedLibrary ( "kernel32" ) )
    /// {
    ///	Action&lt;string&gt; function 
    ///		= lib.GetFunction&lt;Action&lt;string&gt;&gt;("DeleteFile");
    ///	function ( "c:\\tmp.dat" );
    /// }
    /// </code>
    /// </example>
    public class UnmanagedLibrary
        : IDisposable
    {
        #region Properties

        private SafeLibraryHandle _handle;

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <value>The handle.</value>
        public SafeLibraryHandle Handle
        {
            [DebuggerStepThrough]
            get
            {
                return _handle;
            }
        }

        private string _name;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return _name;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="UnmanagedLibrary"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public UnmanagedLibrary(string name)
        {
            Code.NotNullNorEmpty(name, "name");
            _name = name;
            _handle = new SafeLibraryHandle(Kernel32.LoadLibrary(name));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the function.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns></returns>
        public T GetFunction<T>(string functionName)
            where T : class
        {
            Code.NotNullNorEmpty(functionName, "functionName");
            IntPtr ptr = Kernel32.GetProcAddress
                (
                Handle.DangerousGetHandle(),
                functionName
                );
            Delegate function = Marshal.GetDelegateForFunctionPointer
                (
                ptr,
                typeof(T)
                );
            return (T)(object)function;
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!Handle.IsClosed)
            {
                Handle.Close();
            }
        }

        #endregion
    }
}