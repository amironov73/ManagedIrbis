// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftPacket.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class PftPacket
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        [NotNull]
        public PftContext Context { get; private set; }

        /// <summary>
        /// Current field (if any).
        /// </summary>
        [CanBeNull]
        public FieldSpecification CurrentField { get; set; }

        /// <summary>
        /// In group?
        /// </summary>
        public bool InGroup { get; set; }

        /// <summary>
        /// Breakpoints.
        /// </summary>
        [NotNull]
        public Dictionary<int,object> Breakpoints { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PftPacket
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
            Breakpoints = new Dictionary<int, object>();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Call the debugger.
        /// </summary>
        protected void CallDebugger()
        {
            PftDebugger debugger = Context.Debugger;
            if (!ReferenceEquals(debugger, null))
            {
                PftDebugEventArgs eventArgs = new PftDebugEventArgs
                    (
                        Context,
                        null
                    );
                debugger.Activate(eventArgs);
            }
        }

        /// <summary>
        /// Debugger hook.
        /// </summary>
        protected void DebuggerHook
            (
                int nodeId
            )
        {
            if (Breakpoints.ContainsKey(nodeId))
            {
                CallDebugger();
            }
        }

        /// <summary>
        /// Do the conditional literal.
        /// </summary>
        protected void DoConditionalLiteral
            (
                [CanBeNull] string text,
                string tag,
                char code,
                bool isSuffix
            )
        {
            //bool flag = false;

            //if (isSuffix)
            //{
            //    if (IsLastRepeat(context))
            //    {
            //        _Execute(context, field);
            //    }
            //}
            //else
            //{
            //    if (IsFirstRepeat(context))
            //    {
            //        _Execute(context, field);
            //    }
            //}


            Context.Write(null, text);
        }

        /// <summary>
        /// Do field.
        /// </summary>
        protected void DoField
            (
                [NotNull] FieldSpecification field,
                [CanBeNull] Action leftHand,
                [CanBeNull] Action rightHand
            )
        {
            MarcRecord record = Context.Record;
            if (ReferenceEquals(record, null))
            {
                return;
            }

            CurrentField = field;

            char command = field.Command;
            string tag = field.Tag;
            char code = field.SubField;
            int index = Context.Index;
            string text = GetValue(record, tag, code, index);

            if (command == 'v')
            {
                if (!ReferenceEquals(leftHand, null))
                {
                    leftHand();
                }
                Context.Write(null, text);
                if (!ReferenceEquals(rightHand, null))
                {
                    rightHand();
                }
            }

            CurrentField = null;
        }

        /// <summary>
        /// Do group.
        /// </summary>
        protected void DoGroup
            (
                [NotNull] Action action
            )
        {
            // TODO implement
        }

        /// <summary>
        /// Do the repeatable literal.
        /// </summary>
        protected void DoRepeatableLiteral
            (
                [CanBeNull] string text,
                bool isPrefix,
                bool plus
            )
        {
            // TODO implement
            Context.Write(null, text);
        }

        /// <summary>
        /// Evaluate as string.
        /// </summary>
        [CanBeNull]
        protected string Evaluate
            (
                [NotNull] Action action
            )
        {
            Code.NotNull(action, "action");

            using (PftContextGuard guard = new PftContextGuard(Context))
            {
                Context = guard.ChildContext;
                action();
                string result = Context.ToString();
                Context = guard.ParentContext;

                return result;
            }
        }

        [CanBeNull]
        private string GetValue
            (
                [NotNull] MarcRecord record,
                string tag,
                char code,
                int index
            )
        {
            RecordField field = record.Fields.GetField(tag, index);
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            string result = PftUtility.GetFieldValue
            (
                Context,
                field,
                code,
                default(IndexSpecification)
            );

            return result;
        }

        /// <summary>
        /// Goto new position.
        /// </summary>
        public void Goto
            (
                int newPosition
            )
        {
            int current = Context.Output.GetCaretPosition();
            int delta = newPosition - current;
            if (delta > 0)
            {
                Context.Write
                    (
                        null,
                        new string(' ', delta)
                    );
            }
            else
            {
                Context.WriteLine(null);
                Context.Write
                    (
                        null,
                        new string(' ', newPosition - 1)
                    );
            }
        }

        /// <summary>
        /// Have the field?
        /// </summary>
        protected bool HaveField
            (
                [NotNull] FieldSpecification field
            )
        {
            // TODO implement

            return true;
        }

        /// <summary>
        /// First repeat?
        /// </summary>
        protected bool IsFirstRepeat
            (
                [NotNull] FieldSpecification field
            )
        {
            // TODO implement

            return false;
        }

        /// <summary>
        /// Last repeat?
        /// </summary>
        protected bool IsLastRepeat
            (
                [NotNull] FieldSpecification field
            )
        {
            // TODO implement

            return false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the packet agains the record.
        /// </summary>
        [NotNull]
        public virtual string Execute
            (
                [NotNull] MarcRecord record
            )
        {
            Context.ClearAll();
            Context.Record = record;

            return String.Empty;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Context.ToString();
        }

        #endregion
    }
}
