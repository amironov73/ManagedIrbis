// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisScript.cs -- Lua-interpreter for IRBIS
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Scripting
{
    /// <summary>
    /// Интерпретатор Lua-скриптов с учётом ИРБИС-специфики.
    /// </summary>
    [PublicAPI]
    [CLSCompliant(false)]
    public sealed class IrbisScript
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Клиент для доступа к серверу
        /// </summary>
        [NotNull]
        public IIrbisConnection Connection { get; private set; }

        /// <summary>
        /// Скриптовый движок.
        /// </summary>
        [NotNull]
        public Script Engine { get; private set; }

        /// <summary>
        /// Текущая запись
        /// </summary>
        [CanBeNull]
        public MarcRecord Record { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        // ReSharper disable NotNullMemberIsNotInitialized
        public IrbisScript()
        {
            Connection = new IrbisConnection();
            _Initialize();
            _ownClient = true;
        }
        // ReSharper restore NotNullMemberIsNotInitialized

        /// <summary>
        /// Конструктор с заранее созданным клиентом.
        /// </summary>
        /// <param name="connection"></param>
        // ReSharper disable NotNullMemberIsNotInitialized
        public IrbisScript
            (
                [NotNull] IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            _Initialize();
            _ownClient = false;
        }
        // ReSharper restore NotNullMemberIsNotInitialized

        #endregion

        #region Private members

        private readonly bool _ownClient;
        private static bool _typesRegistered;

        private string _V
            (
                string format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }

            if (ReferenceEquals(Record, null))
            {
                return string.Empty;
            }

            FieldReference reference = FieldReference.Parse(format);
            string result = reference.Format(Record);

            return result;
        }

        /// <summary>
        /// Внутренняя инициализация.
        /// </summary>
        private void _Initialize()
        {
#if !WINMOBILE && !PocketPC

            RegisterIrbisTypes();
            Engine = new Script(CoreModules.Preset_Complete);

            // TODO fix it!
            // SetGlobal("Client", Connection);
            // Engine.Globals["v"] = (Func<string,string>)_V;

            foreach (Type type in UserData.GetRegisteredTypes())
            {
                if (!ReferenceEquals(type.Namespace, null)
                    && type.Namespace.StartsWith("ManagedClient"))
                {
                    SetGlobal
                        (
                            type.Name,
                            type
                        );
                }
            }

            SetRecord(null);

#endif
        }

#endregion

        #region Public methods

        /// <summary>
        /// Вызов Lua-функции и получение результата.
        /// </summary>
        [NotNull]
        public DynValue CallFunction
            (
                [NotNull] string name,
                params object[] args
            )
        {
            Code.NotNullNorEmpty(name, "name");

#if WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            DynValue function = Engine.Globals.Get(name);
            if (function.Type != DataType.Function)
            {
                Log.Error
                    (
                        "IrbisScript::CallFunction: "
                        + "not a function: "
                        + name.ToVisibleString()
                    );

                throw new ArgumentOutOfRangeException("name");
            }

            DynValue result = Engine.Call
                (
                    function,
                    args
                );

            return result;

#endif
        }

        /// <summary>
        /// Исполнение Lua-скрипта из файла
        /// и получение результата.
        /// </summary>
        [NotNull]
        public DynValue DoFile
            (
                [NotNull] string filename
            )
        {
            Code.NotNullNorEmpty(filename, "filename");

#if WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            DynValue result = Engine.DoFile
                (
                    filename
                );

            return result;

#endif
        }

        /// <summary>
        /// Исполнение Lua-кода и получение результата.
        /// </summary>
        [NotNull]
        public DynValue DoString
            (
                [CanBeNull] string code
            )
        {
#if WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            if (string.IsNullOrEmpty(code))
            {
                return DynValue.Nil;
            }

            return Engine.DoString(code);

#endif
        }

        /// <summary>
        /// Получение значения глобальной переменной.
        /// </summary>
        [NotNull]
        public DynValue GetGlobal
            (
                [NotNull] string name
            )
        {
#if WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            return Engine.Globals.Get(name);

#endif
        }

        /// <summary>
        /// Регистрация типов, помеченных в данной сборке атрибутом
        /// <see cref="T:MoonSharp.Interpreter.MoonSharpUserDataAttribute"/>
        /// </summary>
        public static void RegisterIrbisTypes()
        {
#if !WINMOBILE && !PocketPC

            if (!_typesRegistered)
            {
                // Not supported in .NET Core
                //UserData.RegisterAssembly(typeof(StringUtility).Assembly);
                //UserData.RegisterAssembly(typeof(IrbisScript).Assembly);

                UserData.RegisterType<Version>();
                _typesRegistered = true;
            }

#endif
        }


        /// <summary>
        /// Установка глобального значения.
        /// </summary>
        [NotNull]
        public IrbisScript SetGlobal
            (
                [NotNull] string name,
                object value
            )
        {
            Code.NotNullNorEmpty(name, "name");

#if !WINMOBILE && !PocketPC

            Engine.Globals.Set
                (
                    name,
                    DynValue.FromObject
                    (
                        Engine,
                        value
                    )
                );

#endif

            return this;
        }

        /// <summary>
        /// Установка новой текущей записи.
        /// </summary>
        [NotNull]
        public IrbisScript SetRecord
            (
                [CanBeNull] MarcRecord record
            )
        {
            Record = record;
            SetGlobal("Record", record);
            
            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (_ownClient)
            {
                Connection.Dispose();
            }
        }

        #endregion
    }
}
