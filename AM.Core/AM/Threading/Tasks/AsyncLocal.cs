/* AsyncLocal.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if NOTDEF

#region Using directives

using System;
using System.Runtime.Remoting.Messaging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class AsyncLocal<TType>
    {
        #region Properties

        /// <summary>
        /// Returns a value indicating whether the value of this async-local variable has been set for the local context.
        /// </summary>
        public bool IsValueSet
        {
            get { return CallContext.LogicalGetData(_slotName) != null; }
        }

        /// <summary>
        /// Gets or sets the value of this async-local variable for the local context.
        /// </summary>
        [CanBeNull]
        public TType Value
        {
            get
            {
                var ret = CallContext.LogicalGetData(_slotName);
                if (ret == null)
                    return _defaultValue;
                return (TType)ret;
            }

            set
            {
                CallContext.LogicalSetData(_slotName, value);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Creates a new async-local variable with the default value of <typeparamref name="TType"/>.
        /// </summary>
        public AsyncLocal()
            : this(default(TType))
        {
        }

        /// <summary>
        /// Creates a new async-local variable with the specified default value.
        /// </summary>
        public AsyncLocal(TType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Our unique slot name.
        /// </summary>
        private readonly string _slotName = Guid.NewGuid().ToString("N");

        /// <summary>
        /// The default value when none has been set.
        /// </summary>
        private readonly TType _defaultValue;

        #endregion

        #region Public methods

        /// <summary>
        /// Clears the value of this async-local variable for the local context.
        /// </summary>
        public void ClearValue()
        {
            CallContext.FreeNamedDataSlot(_slotName);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Value: {0}", Value);
        }

        #endregion
    }
}

#endif
