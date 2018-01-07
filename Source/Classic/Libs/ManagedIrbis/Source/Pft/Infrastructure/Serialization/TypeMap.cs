// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TypeMap.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Reflection;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;

#if !WINMOBILE
using System.Linq.Expressions;
#endif

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    sealed class TypeMap
    {
        #region Properties

        public byte Code;

        public Type Type;

        public Func<PftNode> Create;

        #endregion

        #region Construction

        static TypeMap()
        {
#if !WINMOBILE && !PocketPC

            for (int i = 0; i < Map.Length; i++)
            {
                TypeMap entry = Map[i];

                ConstructorInfo constructor                     = entry.Type.GetConstructor(Type.EmptyTypes);
                if (ReferenceEquals(constructor, null))
                {
                    throw new IrbisException();
                }
                entry.Create = Expression.Lambda<Func<PftNode>>
                    (
                        Expression.New(constructor)
                    )
                    .Compile();
            }

#endif
        }

        #endregion

        #region Public members

        /// <summary>
        /// Важно, чтобы массив был упорядоченным!
        /// </summary>
        public static readonly TypeMap[] Map =
        {
            new TypeMap { Code=1, Type=typeof(PftA) },
            new TypeMap { Code=2, Type=typeof(PftAbs) },
            new TypeMap { Code=3, Type=typeof(PftAll) },
            new TypeMap { Code=4, Type=typeof(PftAny) },
            new TypeMap { Code=5, Type=typeof(PftAssignment) },
            new TypeMap { Code=6, Type=typeof(PftBang) },
            new TypeMap { Code=7, Type=typeof(PftBlank) },
            //new TypeMap { Code=8, Type=typeof(PftBoolean) },
            new TypeMap { Code=9, Type=typeof(PftBreak) },
            new TypeMap { Code=10, Type=typeof(PftC) },
            new TypeMap { Code=11, Type=typeof(PftCeil) },
            new TypeMap { Code=12, Type=typeof(PftCodeBlock) },
            new TypeMap { Code=13, Type=typeof(PftComma) },
            new TypeMap { Code=14, Type=typeof(PftComment) },
            new TypeMap { Code=15, Type=typeof(PftComparison) },
            //new TypeMap { Code=16, Type=typeof(PftCondition) },
            new TypeMap { Code=17, Type=typeof(PftConditionalLiteral) },
            new TypeMap { Code=18, Type=typeof(PftConditionalStatement) },
            new TypeMap { Code=19, Type=typeof(PftConditionAndOr) },
            new TypeMap { Code=20, Type=typeof(PftConditionNot) },
            new TypeMap { Code=21, Type=typeof(PftConditionParenthesis) },
            new TypeMap { Code=22, Type=typeof(PftD) },
            new TypeMap { Code=23, Type=typeof(PftEat) },
            new TypeMap { Code=24, Type=typeof(PftEmpty) },
            new TypeMap { Code=25, Type=typeof(PftF) },
            new TypeMap { Code=26, Type=typeof(PftFmt) },
            new TypeMap { Code=27, Type=typeof(PftFalse) },
            new TypeMap { Code=28, Type=typeof(PftField) },
            new TypeMap { Code=29, Type=typeof(PftFieldAssignment) },
            new TypeMap { Code=30, Type=typeof(PftFirst) },
            new TypeMap { Code=31, Type=typeof(PftFloor) },
            new TypeMap { Code=32, Type=typeof(PftFor) },
            new TypeMap { Code=33, Type=typeof(PftForEach) },
            new TypeMap { Code=34, Type=typeof(PftFrac) },
            new TypeMap { Code=35, Type=typeof(PftFrom) },
            new TypeMap { Code=36, Type=typeof(PftFunctionCall) },
            new TypeMap { Code=37, Type=typeof(PftG) },
            new TypeMap { Code=38, Type=typeof(PftGroup) },
            new TypeMap { Code=39, Type=typeof(PftHash) },
            new TypeMap { Code=40, Type=typeof(PftHave) },
            new TypeMap { Code=41, Type=typeof(PftInclude) },
            new TypeMap { Code=42, Type=typeof(PftL) },
            new TypeMap { Code=43, Type=typeof(PftLast) },
            new TypeMap { Code=44, Type=typeof(PftLocal) },
            new TypeMap { Code=45, Type=typeof(PftMfn) },
            new TypeMap { Code=46, Type=typeof(PftMinus) },
            new TypeMap { Code=47, Type=typeof(PftMode) },
            new TypeMap { Code=48, Type=typeof(PftN) },
            new TypeMap { Code=49, Type=typeof(PftNested) },
            new TypeMap { Code=50, Type=typeof(PftNl) },
            new TypeMap { Code=51, Type=typeof(PftNode) },
            //new TypeMap { Code=52, Type=typeof(PftNumeric) },
            new TypeMap { Code=53, Type=typeof(PftNumericExpression) },
            new TypeMap { Code=54, Type=typeof(PftNumericLiteral) },
            new TypeMap { Code=55, Type=typeof(PftOrphan) },
            new TypeMap { Code=56, Type=typeof(PftP) },
            new TypeMap { Code=57, Type=typeof(PftParallelFor) },
            new TypeMap { Code=58, Type=typeof(PftParallelForEach) },
            new TypeMap { Code=59, Type=typeof(PftParallelGroup) },
            new TypeMap { Code=60, Type=typeof(PftParallelWith) },
            new TypeMap { Code=61, Type=typeof(PftPercent) },
            new TypeMap { Code=62, Type=typeof(PftPow) },
            new TypeMap { Code=63, Type=typeof(PftProcedureDefinition) },
            new TypeMap { Code=64, Type=typeof(PftProgram) },
            new TypeMap { Code=65, Type=typeof(PftRef) },
            new TypeMap { Code=66, Type=typeof(PftRepeatableLiteral) },
            new TypeMap { Code=67, Type=typeof(PftRound) },
            new TypeMap { Code=68, Type=typeof(PftRsum) },
            new TypeMap { Code=69, Type=typeof(PftS) },
            new TypeMap { Code=70, Type=typeof(PftSemicolon) },
            new TypeMap { Code=71, Type=typeof(PftSign) },
            new TypeMap { Code=72, Type=typeof(PftSlash) },
            new TypeMap { Code=73, Type=typeof(PftTrue) },
            new TypeMap { Code=74, Type=typeof(PftTrunc) },
            new TypeMap { Code=75, Type=typeof(PftUnconditionalLiteral) },
            new TypeMap { Code=76, Type=typeof(PftUnifor) },
            new TypeMap { Code=77, Type=typeof(PftV) },
            new TypeMap { Code=78, Type=typeof(PftVal) },
            new TypeMap { Code=79, Type=typeof(PftVariableReference) },
            new TypeMap { Code=80, Type=typeof(PftVerbatim) },
            new TypeMap { Code=81, Type=typeof(PftWhile) },
            new TypeMap { Code=82, Type=typeof(PftWith) },
            new TypeMap { Code=83, Type=typeof(PftX) }
        };

        [CanBeNull]
        public static TypeMap FindCode
            (
                byte code
            )
        {
            int lo = 0, hi = Map.Length - 1;

            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
                int delta = Map[mid].Code - code;
                if (delta == 0)
                {
                    return Map[mid];
                }
                if (delta < 0)
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid - 1;
                }
            }

            //for (int i = 0; i < Map.Length; i++)
            //{
            //    if (code == Map[i].Code)
            //    {
            //        return Map[i];
            //    }
            //}

            return null;
        }

        [CanBeNull]
        public static TypeMap FindType
            (
                [NotNull] Type nodeType
            )
        {
            for (int i = 0; i < Map.Length; i++)
            {
                if (ReferenceEquals(nodeType, Map[i].Type))
                {
                    return Map[i];
                }
            }

            return null;
        }

        #endregion
    }
}
