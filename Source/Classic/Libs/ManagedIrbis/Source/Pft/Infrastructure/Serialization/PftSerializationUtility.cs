// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftSerializationUtility.cs -- 
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PftSerializationUtility
    {
        #region Public methods

        /// <summary>
        /// Compare two lists of nodes.
        /// </summary>
        public static void CompareLists
            (
                [NotNull] IList<PftNode> left,
                [NotNull] IList<PftNode> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            if (left.Count != right.Count)
            {
                throw new PftSyntaxException();
            }

            for (int i = 0; i < left.Count; i++)
            {
                CompareNodes
                    (
                        left[i],
                        right[i]
                    );
            }
        }

        /// <summary>
        /// Compare two lists of <see cref="FieldSpecification"/>.
        /// </summary>
        public static void CompareLists
            (
                [NotNull] IList<FieldSpecification> left,
                [NotNull] IList<FieldSpecification> right
            )
        {
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

            if (left.Count != right.Count)
            {
                throw new PftSyntaxException();
            }

            for (int i = 0; i < left.Count; i++)
            {
                if (!FieldSpecification.Compare(left[i], right[i]))
                {
                    throw new PftSerializationException();
                }
            }
        }

        /// <summary>
        /// Compare two nodes.
        /// </summary>
        public static void CompareNodes
            (
                [CanBeNull] PftNode left,
                [CanBeNull] PftNode right
            )
        {
            bool result;

            if (ReferenceEquals(left, null))
            {
                result = ReferenceEquals(right, null);
            }
            else
            {
                result = !ReferenceEquals(right, null)
                    && ReferenceEquals
                    (
                        left.GetType(),
                        right.GetType()
                    );
                if (result)
                {
                    left.CompareNode(right);
                }
            }

            if (!result)
            {
                throw new PftSerializationException();
            }
        }

        /// <summary>
        /// Compare two strings.
        /// </summary>
        public static bool CompareStrings
            (
                [CanBeNull] string left,
                [CanBeNull] string right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return string.CompareOrdinal(left, right) == 0;
        }

        /// <summary>
        /// Verify deserialized <see cref="PftProgram"/>.
        /// </summary>
        public static void VerifyDeserializedProgram
            (
                [NotNull] PftProgram ethalon,
                [NotNull] PftProgram deserialized
            )
        {
            Code.NotNull(ethalon, "ethalon");
            Code.NotNull(deserialized, "deserialized");

            ethalon.CompareNode(deserialized);
        }

        #endregion
    }
}
