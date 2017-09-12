// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RepeatSpecification.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Gbl.Infrastructure
{
    //
    // Повторение поля задается одним из способов:
    // 
    // * - все повторения
    // F - если используется корректировка по формату
    // N (число) – если корректируется N-ое повторение поля
    // L – если корректируется последнее повторение поля
    // L-N (число) – если корректируется N-ое с конца повторение поля
    //
    // Нумерация повторений начинается с 1.
    //

    /// <summary>
    /// Спецификация повторения поля.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct RepeatSpecification
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Repeat kind.
        /// </summary>
        [JsonProperty("kind")]
        public RepeatKind Kind { get; set; }

        /// <summary>
        /// Number of the repeat.
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RepeatSpecification
            (
                RepeatKind kind
            )
            : this()
        {
            Code.Defined(kind, "kind");

            Kind = kind;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RepeatSpecification
            (
                int index
            )
            : this()
        {
            Code.Positive(index, "index");

            Kind = RepeatKind.Explicit;
            Index = index;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RepeatSpecification
            (
                RepeatKind kind,
                int index
            )
            : this()
        {
            Code.Defined(kind, "kind");
            Code.Nonnegative(index, "index");

            Kind = kind;
            Index = index;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the text.
        /// </summary>
        public static RepeatSpecification Parse
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            RepeatSpecification result = new RepeatSpecification();
            switch (text)
            {
                case "*":
                    result.Kind = RepeatKind.All;
                    break;

                case "F":
                    result.Kind = RepeatKind.ByFormat;
                    break;

                case "L":
                    result.Kind = RepeatKind.Last;
                    break;

                default:
                    uint index;
                    if (NumericUtility.TryParseUInt32(text, out index))
                    {
                        if (index == 0)
                        {
                            throw new IrbisException
                                (
                                    "Invalid repeat specification: "
                                    + text
                                );
                        }

                        result.Kind = RepeatKind.Explicit;
                        result.Index = (int) index;
                    }
                    else if (text[0] == 'L'
                        && NumericUtility.TryParseUInt32
                            (
                                text.Substring(2),
                                out index
                            ))
                    {
                        result.Kind = RepeatKind.Last;
                        result.Index = (int) index;
                    }
                    else
                    {
                        Log.Error
                            (
                                "RepeatSpecification::Parse: "
                               + "invalid repeat specification="
                               + text
                            );


                        throw new IrbisException
                            (
                                "Invalid repeat specification: "
                                + text
                            );
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Should serialize <see cref="Index"/> field?
        /// </summary>
        public bool ShouldSerializeIndex()
        {
            return Index != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Kind = (RepeatKind) reader.ReadPackedInt32();
            Index = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32((int) Kind)
                .WritePackedInt32(Index);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<RepeatSpecification> verifier
                = new Verifier<RepeatSpecification>(this, throwOnError);

            switch (Kind)
            {
                case RepeatKind.All:
                    verifier.Assert(Index == 0);
                    break;

                case RepeatKind.ByFormat:
                    verifier.Assert(Index == 0);
                    break;

                case RepeatKind.Last:
                    verifier.Assert(Index >= 0);
                    break;

                case RepeatKind.Explicit:
                    verifier.Assert(Index > 0);
                    break;

                default:
                    verifier.Result = false;
                    break;
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            switch (Kind)
            {
                case RepeatKind.All:
                    return "*";

                case RepeatKind.ByFormat:
                    return "F";

                case RepeatKind.Last:
                    return Index == 0
                        ? "L"
                        : "L-" + Index.ToInvariantString();

                case RepeatKind.Explicit:
                    return Index.ToInvariantString();

                default:
                    return string.Format
                        (
                            "Kind={0}, Index={1}",
                            Kind,
                            Index
                        );
            }
        }

        #endregion
    }
}
