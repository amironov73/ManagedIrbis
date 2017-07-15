// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftRsum.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftRsum
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Name of the function.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftRsum
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");

            Name = token.Text;
            if (string.IsNullOrEmpty(Name))
            {
                Log.Error
                    (
                        "PftRsum::Constructor: "
                        + "Name="
                        + Name.ToVisibleString()
                    );

                throw new PftSyntaxException("Name");
            }
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

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
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = 0.0;

            if (!string.IsNullOrEmpty(Name))
            {
                string text = context.Evaluate(Children);
                double[] values = PftUtility.ExtractNumericValues(text);
                if (values.Length != 0)
                {
                    switch (Name)
                    {
                        case "rsum":
                            Value = values.Sum();
                            break;

                        case "rmin":
                            Value = values.Min();
                            break;

                        case "rmax":
                            Value = values.Max();
                            break;

                        case "ravr":
                            Value = values.Average();
                            break;

                        default:
                            Log.Error
                                (
                                    "PftRsum::Execute: "
                                    + "unexpected function name="
                                    + Name.ToVisibleString()
                                );

                            throw new PftSyntaxException(this);
                    }
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WriteNullable(Name);
        }

        #endregion
    }
}
