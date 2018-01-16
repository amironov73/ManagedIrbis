// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StreamCollection.cs --  
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using AM.Logging;
using AM.Reflection;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if CLASSIC || NETCORE || DROID

using System.Reflection.Emit;

#endif

#endregion

namespace AM
{
    /// <summary>
    /// Little automation: class that dispose all 
    /// marked fields during finalization.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class DisposableObject
        : IDisposable
    {
        #region Delegates

        private delegate void Disposer(object obj);

        #endregion

        #region Properties

        /// <summary>
        /// Whether the instance disposed.
        /// </summary>
        public bool Disposed { get; private set; }


        ///<summary>
        /// 
        ///</summary>
        public bool DisposeByReflection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisposableObject()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisposableObject
            (
                bool disposeByReflection
            )
        {
#if CLASSIC || DROID

            DisposeByReflection = disposeByReflection;
            if (!disposeByReflection)
            {
                _GenerateDisposer();
            }

#else

            DisposeByReflection = true;

#endif
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }

        #endregion

        #region Private members

        private static Dictionary<Type, Disposer>
            _disposers = new Dictionary<Type, Disposer>();

        private bool _CheckMember
            (
                MemberInfo info,
                Type type
            )
        {
            if (TypeUtility.IsValueType(type))
            {
                return false;
            }

            Type iDisposable = TypeUtility.GetInterface
                (
                    type,
                    "System.IDisposable"
                );
            if (ReferenceEquals(iDisposable, null))
            {
                if (!info.IsDefined(typeof(AutoDisposeAttribute), true))
                {
                    return false;
                }
            }

            return true;
        }

#if CLASSIC || DROID

        private void _ProcessValue
            (
                ILGenerator il
            )
        {
            il.Emit(OpCodes.Dup);
            Type iDisposable = typeof(IDisposable);
            il.Emit(OpCodes.Castclass, iDisposable);
            Label label1 = il.DefineLabel();
            Label label2 = il.DefineLabel();
            il.Emit(OpCodes.Brfalse_S, label1);
            il.EmitCall
                (
                    OpCodes.Callvirt,
                    iDisposable.GetMethod("Dispose"),
                    null
                );
            il.Emit(OpCodes.Br_S, label2);
            il.MarkLabel(label1);
            il.Emit(OpCodes.Pop);
            il.MarkLabel(label2);
        }

        private void _OneField
            (
                ILGenerator il,
                FieldInfo field
            )
        {
            Type type = field.FieldType;
            if (!_CheckMember(field, type))
            {
                return;
            }

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            _ProcessValue(il);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stfld, field);
        }

        private void _OneProperty
            (
                ILGenerator il,
                PropertyInfo prop
            )
        {
            Type type = prop.PropertyType;
            if (!_CheckMember(prop, type))
            {
                return;
            }

            il.Emit(OpCodes.Ldarg_0);
            MethodInfo getter = prop.GetGetMethod();
            il.EmitCall(OpCodes.Callvirt, getter, null);
            _ProcessValue(il);
            MethodInfo setter = prop.GetSetMethod();
            if (!ReferenceEquals(setter, null))
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldnull);
                il.EmitCall(OpCodes.Callvirt, setter, null);
            }
        }

        private void _GenerateDisposer()
        {
            Type type = GetType();

            lock (_disposers)
            {
                if (!_disposers.ContainsKey(type))
                {
                    if (!_disposers.ContainsKey(type))
                    {
                        string methodName = "_dispose" + _disposers.Count;
                        DynamicMethod method = new DynamicMethod
                            (
                                methodName,
                                typeof(void),
                                new Type[] { typeof(object) },
                                type
                            );
                        method.InitLocals = true;

                        ILGenerator il = method.GetILGenerator();

                        FieldInfo[] fields = type.GetFields
                            (
                                BindingFlags.Public 
                                | BindingFlags.Instance
                            );
                        foreach (FieldInfo field in fields)
                        {
                            _OneField(il, field);
                        }

                        PropertyInfo[] props = type.GetProperties
                            (BindingFlags.Public | BindingFlags.Instance);
                        foreach (PropertyInfo prop in props)
                        {
                            _OneProperty(il, prop);
                        }
                        il.Emit(OpCodes.Ret);

                        Disposer disposer = (Disposer)method.CreateDelegate
                            (
                                typeof(Disposer)
                            );
                        _disposers.Add(type, disposer);
                    }
                }
            }
        }

        private static void _DisposeWithLCG
            (
                object obj
            )
        {
            Type type = obj.GetType();
            Disposer disposer;

            lock (_disposers)
            {
                disposer = _disposers[type];
            }
            disposer(obj);
        }

#endif

        private static void _DisposeWithReflection
            (
                object obj
            )
        {
            Type type = obj.GetType();
            FieldInfo[] fields = TypeUtility.GetFields(type);

            foreach (FieldInfo field in fields)
            {
                if (ReflectionUtility.HasAttribute<AutoDisposeAttribute>(field))
                {
                    object val = field.GetValue(obj);
                    IDisposable di = val as IDisposable;

                    if (!ReferenceEquals(di, null))
                    {
                        di.Dispose();
                    }
                    if (TypeUtility.IsComObject(type))
                    {
#if !PORTABLE && !SILVERLIGHT

                        Marshal.ReleaseComObject(val);

#endif
                    }

                    // Is it necessary?
                    field.SetValue(obj, null);
                }
            }
        }

        /// <summary>
        /// Calls <c>Dispose ()</c> for all fields marked with
        /// <see cref="AutoDisposeAttribute">AutoDispose attribute.</see>
        /// </summary>
        private void DisposeFields()
        {
#if CLASSIC || DROID

            if (!DisposeByReflection)
            {
                _DisposeWithLCG(this);
            }
            else
            {
                _DisposeWithReflection(this);
            }

#else

            _DisposeWithReflection(this);

#endif
        }

        /// <summary>
        /// Checks whether this instance disposed.
        /// </summary>
        protected void CheckDisposed()
        {
            if (Disposed)
            {
                Log.Error
                    (
                        "DisposableObject::CheckDisposed"
                    );

                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion

        #region Protected members

        /// <summary>
        /// Can be overriden.
        /// </summary>
        protected virtual void Dispose
            (
                bool disposing
            )
        {
            DisposeFields();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            lock (this)
            {
                if (!Disposed)
                {
                    Disposed = true;
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion
    }

}

