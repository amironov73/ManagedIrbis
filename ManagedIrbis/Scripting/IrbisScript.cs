/* IrbisScript.cs -- Lua-interpreter for IRBIS
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;

using AM;

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
        public IrbisConnection Client { get; private set; }

        /// <summary>
        /// Скриптовый движок.
        /// </summary>
        [NotNull]
        public Script Engine { get; private set; }

        /// <summary>
        /// Текущая запись
        /// </summary>
        [CanBeNull]
        public IrbisRecord Record { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        // ReSharper disable NotNullMemberIsNotInitialized
        public IrbisScript()
        {
            Client = new IrbisConnection();
            _Initialize();
            _ownClient = true;
        }
        // ReSharper restore NotNullMemberIsNotInitialized

        /// <summary>
        /// Конструктор с заранее созданным клиентом.
        /// </summary>
        /// <param name="client"></param>
        // ReSharper disable NotNullMemberIsNotInitialized
        public IrbisScript
            (
                [NotNull] IrbisConnection client
            )
        {
            Code.NotNull(client, "client");

            Client = client;
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
            string result = reference.FormatSingle(Record);

            return result;
        }

        /// <summary>
        /// Внутренняя инициализация.
        /// </summary>
        private void _Initialize()
        {
            RegisterIrbisTypes();
            Engine = new Script(CoreModules.Preset_Complete);

            SetGlobal("Client", Client);
            Engine.Globals["v"] = (Func<string,string>)_V;

            foreach (Type type in UserData.GetRegisteredTypes())
            {
                if ((type.Namespace != null)
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

            DynValue function = Engine.Globals.Get(name);
            if (function.Type != DataType.Function)
            {
                throw new ArgumentOutOfRangeException("name");
            }

            DynValue result = Engine.Call
                (
                    function,
                    args
                );

            return result;
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

            DynValue result = Engine.DoFile
                (
                    filename
                );

            return result;
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
            if (string.IsNullOrEmpty(code))
            {
                return DynValue.Nil;
            }
            return Engine.DoString(code);
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
            return Engine.Globals.Get(name);
        }

        /// <summary>
        /// Регистрация типов, помеченных в данной сборке атрибутом
        /// <see cref="MoonSharpUserDataAttribute"/>
        /// </summary>
        public static void RegisterIrbisTypes()
        {
            if (!_typesRegistered)
            {
                // Not supported in .NET Core
                //UserData.RegisterAssembly(typeof(StringUtility).Assembly);
                //UserData.RegisterAssembly(typeof(IrbisScript).Assembly);

                UserData.RegisterType<Version>();
                _typesRegistered = true;
            }
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

            Engine.Globals.Set
                (
                    name,
                    DynValue.FromObject
                    (
                        Engine,
                        value
                    )
                );

            return this;
        }

        /// <summary>
        /// Установка новой текущей записи.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [NotNull]
        public IrbisScript SetRecord
            (
                [CanBeNull] IrbisRecord record
            )
        {
            Record = record;
            SetGlobal("Record", record);
            
            return this;
        }

        #endregion

        #region IDisposable members
        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_ownClient)
            {
                Client.Disconnect();
            }
        }

        #endregion
    }
}
