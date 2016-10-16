/* PftContext.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Pft.Infrastructure.Ast;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Контекст форматирования
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftContext
    {
        #region Properties

        ///// <summary>
        ///// Форматтер
        ///// </summary>
        //public PftFormatter Formatter { get { return _formatter; } }

        /// <summary>
        /// Родительский контекст.
        /// </summary>
        public PftContext Parent { get { return _parent; } }

        /// <summary>
        /// Клиент для связи с сервером.
        /// </summary>
        public IrbisConnection Connection { get; set; }

        /// <summary>
        /// Текущая форматируемая запись.
        /// </summary>
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Выходной буфер, в котором накапливается результат
        /// форматирования, а также ошибки и предупреждения.
        /// </summary>
        public PftOutput Output { get; internal set; }

        /// <summary>
        /// Накопленный текст в основном потоке выходного буфера,
        /// т. е. собственно результат расформатирования записи.
        /// </summary>
        public string Text { get { return Output.ToString(); } }

        #region Режим вывода

        /// <summary>
        /// Режим вывода полей.
        /// </summary>
        public PftFieldOutputMode FieldOutputMode { get; set; }

        /// <summary>
        /// Режим перевода текста в верхний регистр при выводе полей.
        /// </summary>
        public bool UpperMode { get; set; }

        #endregion

        #region Работа с группами

        /// <summary>
        /// Текущая группа (если есть).
        /// </summary>
        [CanBeNull]
        public PftGroup CurrentGroup { get; internal set; }

        /// <summary>
        /// Номер повторения в текущей группе.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Флаг, устанавливается при наличии вывода при заданном повторении.
        /// </summary>
        public bool OutputFlag { get; internal set; }

        /// <summary>
        /// Флаг, устанавливается при срабатывании оператора break.
        /// </summary>
        public bool BreakFlag { get; internal set; }

        /// <summary>
        /// Текущее обрабатываемое поле записи, если есть.
        /// </summary>
        [CanBeNull]
        public PftField CurrentField { get; internal set; }

        #endregion

        /// <summary>
        /// Глобальные переменные.
        /// </summary>
        public PftGlobalManager Globals { get; private set; }

        /// <summary>
        /// Нормальные переменные.
        /// </summary>
        public PftVariableManager Variables { get; private set; }

        /// <summary>
        /// Функции, зарегистрированные в данном контексте.
        /// </summary>
        [NotNull]
        public PftFunctionManager Functions { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftContext
            (
                [CanBeNull] PftContext parent
            )
        {
            _parent = parent;

            PftOutput parentBuffer = (parent == null)
                ? null
                : parent.Output;

            Output = new PftOutput(parentBuffer);

            Globals = (parent == null)
                ? new PftGlobalManager()
                : parent.Globals;

            Variables = (parent == null)
                ? new PftVariableManager(null)
                : parent.Variables;

            //// Процедуры в каждом контексте свои
            //Procedures = new PftProcedureManager();

            if (!ReferenceEquals(parent, null))
            {
                CurrentGroup = parent.CurrentGroup;
                CurrentField = parent.CurrentField;
                Index = parent.Index;
            }

            Record = (parent == null)
                ? new MarcRecord()
                : parent.Record;

            Connection = (parent == null)
                ? new IrbisConnection()
                : parent.Connection;

            Functions = new PftFunctionManager();
        }

        #endregion

        #region Private members

        // private PftFormatter _formatter;

        private readonly PftContext _parent;

        //internal void _SetFormatter
        //    (
        //        PftFormatter formatter
        //    )
        //{
        //    _formatter = formatter;
        //}

        #endregion

        #region Public methods

        /// <summary>
        /// Полная очистка всех потоков: и основного,
        /// и предупреждений, и ошибок.
        /// </summary>
        public PftContext ClearAll()
        {
            Output.ClearText();
            Output.ClearError();
            Output.ClearWarning();

            return this;
        }

        /// <summary>
        /// Очистка основного выходного потока.
        /// </summary>
        public PftContext ClearText()
        {
            Output.ClearText();

            return this;
        }

        /// <summary>
        /// Выполнить повторяющуюся группу.
        /// </summary>
        public void DoRepeatableAction
            (
                [NotNull] Action<PftContext> action,
                int count
            )
        {
            Code.NotNull(action, "action");
            Code.Nonnegative(count, "count");

            count = Math.Min(count, PftConfig.MaxRepeat);

            for (Index = 0; Index < count; Index++)
            {
                OutputFlag = false;

                action(this);

                if (!OutputFlag
                    || BreakFlag)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Выполнить повторяющуюся группу
        /// максимально возможное число раз.
        /// </summary>
        public void DoRepeatableAction
            (
                [NotNull] Action<PftContext> action
            )
        {
            DoRepeatableAction(action, int.MaxValue);
        }

        /// <summary>
        /// Временное переключение контекста (например,
        /// при вычислении строковых функций).
        /// </summary>
        [NotNull]
        public PftContext Push()
        {
            //PftContext result = new PftContext(Formatter,this);
            PftContext result = new PftContext(this);

            return result;
        }

        /// <summary>
        /// Pop the context.
        /// </summary>
        public void Pop()
        {
            if (!ReferenceEquals(Parent, null))
            {
                // Nothing to do?
            }
        }

        /// <summary>
        /// Write text.
        /// </summary>
        [NotNull]
        [StringFormatMethod("format")]
        public PftContext Write
            (
                PftNode node,
                string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Output.Write(format, arg);
            }

            return this;
        }

        /// <summary>
        /// Write text.
        /// </summary>
        [NotNull]
        public PftContext Write
            (
                PftNode node,
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Output.Write(value);
            }

            return this;
        }

        /// <summary>
        /// Write line.
        /// </summary>
        [NotNull]
        public PftContext WriteLine
            (
                PftNode node,
                string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Output.WriteLine(format, arg);
            }

            return this;
        }

        /// <summary>
        /// Write line.
        /// </summary>
        [NotNull]
        public PftContext WriteLine
            (
                PftNode node,
                string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Output.WriteLine(value);
            }

            return this;
        }

        /// <summary>
        /// Write line.
        /// </summary>
        [NotNull]
        public PftContext WriteLine
            (
                PftNode node
            )
        {
            Output.WriteLine();

            return this;
        }

        /// <summary>
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        [NotNull]
        public string Evaluate
            (
                [NotNull] PftNode node
            )
        {
            Code.NotNull(node, "node");

            PftContext copy = Push();
            node.Execute(copy);
            string result = copy.ToString();
            Pop();

            return result;
        }

        /// <summary>
        /// Вычисление выражения во временной копии контекста.
        /// </summary>
        [NotNull]
        public string Evaluate
            (
                [NotNull] IEnumerable<PftNode> items
            )
        {
            Code.NotNull(items, "items");

            PftContext copy = Push();
            foreach (PftNode node in items)
            {
                node.Execute(copy);
            }
            string result = copy.ToString();
            Pop();

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> 
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return Output.ToString();
        }

        #endregion
    }
}