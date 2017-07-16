// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftVariableReference.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftVariableReference
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Name of the variable.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Index.
        /// </summary>
        public IndexSpecification Index { get; set; }

        /// <summary>
        /// Subfield code.
        /// </summary>
        public char SubFieldCode { get; set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftVariableReference
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Variable);

            Name = token.Text;
        }

        #endregion

        #region Private members

        private static string _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                char c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString();
        }

        [NotNull]
        private static RecordField _ParseLine
            (
                [NotNull] string line
            )
        {
            Code.NotNull(line, "line");

            StringReader reader = new StringReader(line);
            RecordField result = new RecordField
            {
                Value = _ReadTo(reader, '^')
            };

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }

                char code = char.ToLower((char)next);
                string text = _ReadTo(reader, '^');
                SubField subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                result.SubFields.Add(subField);
            }

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftVariableReference result
                = (PftVariableReference) base.Clone();

            result.Index = (IndexSpecification) Index.Clone();

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Name = reader.ReadNullableString();
            Index.Deserialize(reader);
            SubFieldCode = reader.ReadChar();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string name = Name.ThrowIfNull("name");
            PftVariable variable
                = context.Variables.GetExistingVariable(name);
            if (ReferenceEquals(variable, null))
            {
                Log.Error
                    (
                        "PftVariableReference::Execute: "
                        + "unknown variable="
                        + name.ToVisibleString()
                    );

                throw new PftSemanticException
                    (
                        "unknown variable: " + name
                    );
            }

            if (variable.IsNumeric)
            {
                Value = variable.NumericValue;
                context.Write
                    (
                        this,
                        variable.NumericValue.ToInvariantString()
                    );
            }
            else
            {
                string output = variable.StringValue;

                if (Index.Kind != IndexKind.None)
                {
                    string[] lines = output.SplitLines();

                    lines = PftUtility.GetArrayItem
                        (
                            context,
                            lines,
                            Index
                        );

                    if (SubFieldCode != SubField.NoCode)
                    {
                        List<string> list = new List<string>();

                        foreach (string line in lines)
                        {
                            RecordField field = _ParseLine(line);
                            string text = field.GetFirstSubFieldValue
                                (
                                    SubFieldCode
                                );
                            list.Add(text);
                        }

                        lines = list.ToArray();
                    }

                    output = string.Join
                        (
                            Environment.NewLine,
                            lines
                        );
                }
                else
                {
                    if (SubFieldCode != SubField.NoCode)
                    {
                        string[] lines = output.SplitLines();

                        List<string> list = new List<string>();

                        foreach (string line in lines)
                        {
                            RecordField field = _ParseLine(line);
                            string text = field.GetFirstSubFieldValue
                                (
                                    SubFieldCode
                                );
                            list.Add(text);
                        }

                        lines = list.ToArray();
                        output = string.Join
                            (
                                Environment.NewLine,
                                lines
                            );
                    }
                }

                context.Write(this, output);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = base.GetNodeInfo();

            if (Index.Kind != IndexKind.None)
            {
                result.Children.Add(Index.GetNodeInfo());
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .Write('$')
                .Write(Name);

            if (Index.Kind != IndexKind.None)
            {
                printer
                    .Write('[')
                    .Write(Index.ToString())
                    .Write(']');
            }

            if (SubFieldCode != SubField.NoCode)
            {
                printer
                    .Write('^')
                    .Write(SubFieldCode);
            }

            printer.Write(' ');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteNullable(Name);
            Index.Serialize(writer);
            writer.Write(SubFieldCode);
        }

        #endregion
    }
}
