// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PredicateBuilder.cs -- tool for predicate creation
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !FW35 && !WINMOBILE

#region Using directives

using System;
using System.Linq.Expressions;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Tool for predicate creation.
    /// </summary>
    /// <remarks>
    /// Borrowed from
    /// <see href="http://www.albahari.com/nutshell/predicatebuilder.aspx"/>
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PredicateBuilder
    {
        #region Public methods

        /// <summary>
        /// Creates a predicate expression from the given lambda expression.
        /// </summary>
        [NotNull]
        public static Expression<Func<T, bool>> Create<T>
            (
                [NotNull] Expression<Func<T, bool>> predicate
            )
        {
            Code.NotNull(predicate, "predicate");

            return predicate;
        }

        /// <summary>
        /// Creates a predicate that evaluates to <c>True</c>.
        /// </summary>
        [NotNull]
        public static Expression<Func<bool>> True()
        {
            return () => true;
        }

        /// <summary>
        /// Creates a predicate that evaluates to <c>True</c>.
        /// </summary>
        [NotNull]
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        /// <summary>
        /// Creates a predicate that evaluates to <c>False</c>.
        /// </summary>
        [NotNull]
        public static Expression<Func<bool>> False()
        {
            return () => false;
        }

        /// <summary>
        /// Creates a predicate that evaluates to <c>False</c>.
        /// </summary>
        [NotNull]
        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        /// <summary>
        /// Combines the first predicate with the second
        /// using the logical <c>OR</c>.
        /// </summary>
        [NotNull]
        public static Expression<Func<T, bool>> Or<T>
            (
                [NotNull] this Expression<Func<T, bool>> expr1,
                [NotNull] Expression<Func<T, bool>> expr2
            )
        {
            Code.NotNull(expr1, "expr1");
            Code.NotNull(expr2, "expr2");

            InvocationExpression invokedExpr = Expression.Invoke
                (
                    expr2,
                    expr1.Parameters
                );

            return Expression.Lambda<Func<T, bool>>
                (
                    Expression.OrElse
                        (
                            expr1.Body,
                            invokedExpr
                        ),
                    expr1.Parameters
                );
        }

        /// <summary>
        /// Combines the first predicate with the second
        /// using the logical <c>AND</c>.
        /// </summary>
        [NotNull]
        public static Expression<Func<T, bool>> And<T>
            (
                [NotNull] this Expression<Func<T, bool>> expr1,
                [NotNull] Expression<Func<T, bool>> expr2
            )
        {
            Code.NotNull(expr1, "expr1");
            Code.NotNull(expr2, "expr2");

            InvocationExpression invokedExpr = Expression.Invoke
                (
                    expr2,
                    expr1.Parameters
                );

            return Expression.Lambda<Func<T, bool>>
                (
                    Expression.AndAlso
                        (
                            expr1.Body,
                            invokedExpr
                        ),
                    expr1.Parameters
                );
        }

        /// <summary>
        /// Negates a given predicate.
        /// </summary>
        [NotNull]
        public static Expression<Func<T, bool>> Not<T>
            (
                [NotNull] this Expression<Func<T, bool>> expression
            )
        {
            Code.NotNull(expression, "expression");

            UnaryExpression negated = Expression.Not(expression.Body);

            return Expression.Lambda<Func<T, bool>>
                (
                    negated,
                    expression.Parameters
                );
        }

        #endregion
    }
}

#endif
