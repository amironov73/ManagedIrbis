// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PinnedStructure.cs -- helper for structure interop
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// A helper class for working with structures that need to be
    /// pinned to prevent the GC from moving them.
    /// </summary>
    /// <remarks>
    /// Inspired by Marc Clifton article at CodeProject
    /// http://www.codeproject.com/useritems/PinnedObject.asp
    /// </remarks>
    /// <example>
    /// <para>This example illustrates manipulation a 
    /// <see cref="PinnedStructure{T}"/> using C++
    /// style pointer syntax withing an unsafe code block.
    /// The code illustrates:
    /// </para>
    /// <list type="bullet">
    /// <item>Assigning a new structure to the pinned object
    /// (testing the ManagedObject setter).
    /// </item>
    /// <item>Getting the pointer and manipulating the structure
    /// via the pointer (testing the Pointer getter).
    /// </item>
    /// <item>Getting the structure (testing the ManagedObject getter).
    /// </item>
    /// </list>
    /// <code>
    /// using System;
    /// 
    /// using AM.Runtime;
    /// 
    /// public struct DummyStructure
    /// {
    ///	public int Afield;
    /// }
    /// 
    /// public static class Test
    /// {
    ///	public static void Main ()
    ///	{
    ///		PinnedStructure&lt;DummyStructure&gt; pin
    ///			= new PinnedStructure&lt;DummyStructure&gt; ();
    ///		DummyStructure dummy = new DummyStructure ();
    ///		dummy.AField = 1;
    ///		pin.ManagedObject = dummy;
    ///		
    ///		unsafe
    ///		{
    ///			DummyStructure *p = (DummyStructure*) pin.Pointer;
    ///			++p->AField;
    ///		}
    ///		
    ///		Console.WriteLine ( pin.ManagedObject.AField );
    ///	}
    /// }
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public class PinnedStructure<T>
        : IDisposable
        where T : struct
    {
        #region Properties

        private bool _isDisposed;

        /// <summary>
        /// Gets a value indicating whether 
        /// this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed
        {
            [DebuggerStepThrough]
            get
            {
                return _isDisposed;
            }
        }

        private T _managedObject;

        /// <summary>
        /// Gets or sets the managed object.
        /// </summary>
        public T ManagedObject
        {
            get
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException("PinnedStructure");
                }
                return (T)_handle.Target;
            }
            set
            {
                Marshal.StructureToPtr
                    (
                        value,
                        _pointer,
                        false
                    );
            }
        }

        private IntPtr _pointer;

        /// <summary>
        /// Gets the pointer to the managed object.
        /// </summary>
        public IntPtr Pointer
        {
            [DebuggerStepThrough]
            get
            {
                if (IsDisposed)
                {
                    throw new ObjectDisposedException("PinnedStructure");
                }
                return _pointer;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="PinnedStructure{T}"/> class.
        /// </summary>
        public PinnedStructure()
        {
            _managedObject = new T();
            _handle = GCHandle.Alloc
                (
                    _managedObject,
                    GCHandleType.Pinned
                );
            _pointer = _handle.AddrOfPinnedObject();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other 
        /// cleanup operations before the
        /// <see cref="PinnedStructure{T}"/> 
        /// is reclaimed by garbage collection.
        /// </summary>
        ~PinnedStructure()
        {
            Dispose();
        }

        #endregion

        #region Private members

        private GCHandle _handle;

        #endregion

        #region Public methods

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                _handle.Free();
                _pointer = IntPtr.Zero;
                _isDisposed = true;
            }
        }

        #endregion
    }
}
