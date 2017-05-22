// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PropertyAccessor.cs -- get access to private properties via reflection
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !PORTABLE

#region Using directives

using System;
using System.Diagnostics;
using System.Reflection;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if WIN81

using MvvmCross.Platform;

#endif

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// Some hacking: get access to private properties (via reflection).
    /// </summary>
    /// <typeparam name="T">Main object type.</typeparam>
    /// <typeparam name="V">Property type.</typeparam>
    /// <example>
    /// <code>
    /// using System;
    /// using AM.Reflection;
    ///
    /// class Canary
    /// {
    ///     public int myProp;
    ///     private int MyProp
    ///     {
    ///         get
    ///         {
    ///             return myProp;
    ///         }
    ///         set
    ///         {
    ///             myProp = value;
    ///         }
    ///     }
    /// }
    /// 
    /// class Program
    /// {
    ///     static void Main ( string [] args )
    ///     {
    ///         Canary canary = new Canary ();
    ///         PropertyAccessor&lt;Canary, int&gt; pa
    ///            = new PropertyAccessor&lt;Canary, int&gt; ( canary, "MyProp" );
    ///         pa.Value = 2;
    ///         Console.WriteLine ( canary.myProp );
    ///     }
    /// }
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public class PropertyAccessor<T, V>
        where T : class
    {
        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public delegate void AccessHandler
            (
                T target, 
                V value
            );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="previousTarget"></param>
        public delegate void TargetHandler
            (
                PropertyAccessor<T, V> accessor, 
                T previousTarget
            );

        /// <summary>
        /// Fired when getting value.
        /// </summary>
        public event AccessHandler GettingValue;

        /// <summary>
        /// Fired when setting value.
        /// </summary>
        public event AccessHandler SettingValue;

        /// <summary>
        /// Fired when target changed.
        /// </summary>
        public event TargetHandler TargetChanged;

        #endregion

        #region Properties

        private T _target;

        ///<summary>
        /// Target.
        ///</summary>
        public virtual T Target
        {
            [DebuggerStepThrough]
            get
            {
                return _target;
            }
            [DebuggerStepThrough]
            set
            {
                T previousTarget = _target;
                _target = value;
                OnTargetChanged(previousTarget);
            }
        }

        private readonly PropertyInfo _info;

        ///<summary>
        /// Property info.
        ///</summary>
        public PropertyInfo Info
        {
            [DebuggerStepThrough]
            get
            {
                return _info;
            }
        }

        /// <summary>
        /// Property value.
        /// </summary>
        public virtual V Value
        {
            get
            {
                if (_getter == null)
                {
                    throw new NotSupportedException();
                }
                return OnGettingValue(_getter());
            }
            set
            {
                if (_setter == null)
                {
                    throw new NotSupportedException();
                }
                _setter(OnSettingValue(value));
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        public PropertyAccessor
            (
                [CanBeNull] T target, 
                [NotNull] string propertyName
            )
        {
            Code.NotNullNorEmpty(propertyName, "propertyName");

            _target = target;
            _info = typeof(T).GetProperty
                (
                    propertyName,

#if WIN81

                    BindingFlags.Public
                    | BindingFlags.Instance | BindingFlags.Static

#else

                    BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.Instance | BindingFlags.Static

#endif
                );
            if (_info == null)
            {
                throw new ArgumentException("propertyName");
            }
            _CreateDelegates();
        }

        #endregion

        #region Private members

        private delegate V _Getter();

        private _Getter _getter;

        private delegate void _Setter(V value);

        private _Setter _setter;

        private void _CreateDelegates()
        {
            if (_info.CanRead)
            {
                MethodInfo methodInfo = _info.GetGetMethod(true);

#if NETCORE || UAP || WIN81

                _getter = (_Getter) methodInfo.CreateDelegate
                    (
                        typeof(_Getter),
                        Target
                    );

#else

                _getter = (_Getter)Delegate.CreateDelegate
                    (
                        typeof(_Getter),
                        Target,
                        methodInfo
                    );

#endif
            }
            if (_info.CanWrite)
            {
                MethodInfo methodInfo = _info.GetSetMethod(true);

#if NETCORE || UAP || WIN81

                _setter = (_Setter) methodInfo.CreateDelegate
                    (
                        typeof(_Setter),
                        Target
                    );

#else

                _setter = (_Setter)Delegate.CreateDelegate
                    (
                        typeof(_Setter),
                        Target,
                        methodInfo
                    );

#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual V OnGettingValue(V value)
        {
            if (GettingValue != null)
            {
                GettingValue(Target, value);
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual V OnSettingValue(V value)
        {
            if (SettingValue != null)
            {
                SettingValue(Target, value);
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="previousTarget"></param>
        protected virtual void OnTargetChanged(T previousTarget)
        {
            _CreateDelegates();
            if (TargetChanged != null)
            {
                TargetChanged(this, previousTarget);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set new <see cref="Target"/>
        /// </summary>
        [NotNull]
        public PropertyAccessor<T, V> SetTarget
            (
                [CanBeNull] T newTarget
            )
        {
            Target = newTarget;

            return this;
        }

        #endregion
    }
}

#endif
