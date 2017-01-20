// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftG.cs -- global variable reference
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Global variable reference
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftG
        : PftField
    {
        #region Properties

        /// <summary>
        /// Number of the variable.
        /// </summary>
        public int Number { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            FieldSpecification specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                throw new PftSyntaxException();
            }

            Apply(specification);

            Number = int.Parse
                (
                    Tag.ThrowIfNull("Tag")
                );
        }

        #endregion

        #region Private members

        private RecordField[] _fields;

        private int _count;

        private void _Execute
            (
                PftContext context
            )
        {
            try
            {
                context.CurrentField = this;

                context.Execute(LeftHand);

                string value = GetValue(context);
                if (!string.IsNullOrEmpty(value))
                {
                    if (Indent != 0
                        && IsFirstRepeat(context))
                    {
                        value = new string(' ', Indent) + value;
                    }

                    context.Write(this, value);
                }
                if (HaveRepeat(context))
                {
                    context.OutputFlag = true;
                }

                context.Execute(RightHand);
            }
            finally
            {
                context.CurrentField = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Число повторений _поля_ в записи.
        /// </summary>
        public int GetCount
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            _fields = context.Globals.Get(Number);
            int result = _fields.Length;

            return result;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        public override string GetValue
            (
                PftContext context
            )
        {
            Code.NotNull(context, "context");

            if (_count == 0)
            {
                return null;
            }

            int index = context.Index;

            RecordField field = _fields.GetOccurrence(index);
            if (field == null)
            {
                return null;
            }

            string result;

            if (SubField == NoSubField)
            {
                result = field.FormatField
                    (
                        context.FieldOutputMode,
                        context.UpperMode
                    );
            }
            else if (SubField == '*')
            {
                result = field.Value;
            }
            else
            {
                result = field.GetFirstSubFieldValue(SubField);
            }

            result = LimitText(result);

            return result;
        }

        /// <summary>
        /// Have value?
        /// </summary>
        public override bool HaveRepeat
            (
                PftContext context
            )
        {
            Code.NotNull(context, "context");

            return context.Index < GetCount(context);
        }


        #endregion

        #region PftNode members

        /// <inheritdoc/>
        public override bool IsLastRepeat
            (
                PftContext context
            )
        {
            return context.Index >= (_count - 1);
        }

        /// <inheritdoc />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (context.CurrentField != null)
            {
                throw new PftSemanticException("nested field");
            }

            if (Number == 0)
            {
                Number = int.Parse
                    (
                        Tag.ThrowIfNull("Tag")
                    );
            }

            if (context.CurrentGroup != null)
            {
                if (IsFirstRepeat(context))
                {
                    _count = GetCount(context);
                }

                _Execute(context);
            }
            else
            {
                _count = GetCount(context);

                context.DoRepeatableAction(_Execute);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc />
        public override void Write
            (
                StreamWriter writer
            )
        {
            foreach (PftNode node in LeftHand)
            {
                node.Write(writer);
            }

            writer.Write(ToString());

            foreach (PftNode node in RightHand)
            {
                node.Write(writer);
            }
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc />
        public override object Clone()
        {
            PftG result = (PftG) base.Clone();

            result._fields = null;

            //if (!ReferenceEquals(_fields, null))
            //{
            //    result._fields = (RecordField[]) _fields.Clone();
            //    for (int i = 0; i < _fields.Length; i++)
            //    {
            //        result._fields[i] = _fields[i].Clone();
            //    }
            //}

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToSpecification().ToString();
        }

        #endregion
    }
}
