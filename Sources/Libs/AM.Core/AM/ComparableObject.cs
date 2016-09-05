/* ComparableObject.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !NETCORE

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Class with automatic Equals method.
    /// </summary>
    /// <remarks>Borrowed from Brad Smith's coding blog:
    /// http://www.brad-smith.info/blog/archives/385
    /// </remarks>
    [PublicAPI]
    [MoonSharpUserData]
    public class ComparableObject
    {
        #region Nested classes

        /// <summary>
        /// Used to hold delegates for the compiled methods.
        /// </summary>
        private class MemberwiseFunctions
        {
            /// <summary>
            /// Delegate for the Equals method.
            /// </summary>
            public Func<object, object, bool> EqualsFunc;
            /// <summary>
            /// Delegate for the GetHashCode method.
            /// </summary>
            public Func<object, int> GetHashCodeFunc;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ComparableObject()
        {
            _functions = new Dictionary<Type, MemberwiseFunctions>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>Dynamically compiles the Equals/GetHashCode
        /// functions on the 
        /// first call to a subclass constructor.
        /// </remarks>
        public ComparableObject()
        {
            Type type = GetType();

            if (!_functions.ContainsKey(type))
            {
                MemberwiseFunctions functions = new MemberwiseFunctions
                {
                    EqualsFunc = MakeEqualsMethod(type),
                    GetHashCodeFunc = MakeGetHashCodeMethod(type)
                };

                _functions[type] = functions;
            }
        }

        #endregion

        #region Private members

        // ReSharper disable once InconsistentNaming
        static readonly Dictionary<Type, MemberwiseFunctions> _functions;

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the Equals() method.
        /// </summary>
        [NotNull]
        public static Func<object, object, bool> MakeEqualsMethod
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            ParameterExpression pThis = Expression.Parameter
                (
                    typeof(object),
                    "x"
                );
            ParameterExpression pThat = Expression.Parameter
                (
                    typeof(object),
                    "y"
                );

            // cast to the subclass type
            UnaryExpression pCastThis = Expression.Convert(pThis, type);
            UnaryExpression pCastThat = Expression.Convert(pThat, type);

            // compound AND expression using short-circuit evaluation
            Expression last = null;
            foreach (PropertyInfo property in type.GetProperties())
            {
                BinaryExpression equalExpression = Expression.Equal
                    (
                        Expression.Property(pCastThis, property),
                        Expression.Property(pCastThat, property)
                    );

                last = last == null
                    ? equalExpression
                    : Expression.AndAlso
                        (
                            last,
                            equalExpression
                        );
            }

            // call Object.Equals if second parameter doesn't match type
            last = Expression.Condition
                (
                    Expression.TypeIs(pThat, type),
                    last,
                    Expression.Equal(pThis, pThat)
                );

            // compile method
            return Expression.Lambda<Func<object, object, bool>>
                (
                    last,
                    pThis,
                    pThat
                )
                .Compile();
        }

        /// <summary>
        /// Creates the GetHashCode() method.
        /// </summary>
        [NotNull]
        static Func<object, int> MakeGetHashCodeMethod
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            ParameterExpression pThis = Expression.Parameter
                (
                    typeof(object),
                    "x"
                );
            UnaryExpression pCastThis = Expression.Convert
                (
                    pThis,
                    type
                );

            Expression last = null;
            foreach (PropertyInfo property in type.GetProperties())
            {
                MethodCallExpression hash = Expression.Call
                    (
                        Expression.Property(pCastThis, property),
                        "GetHashCode",
                        Type.EmptyTypes
                    );

                if (last == null)
                {
                    last = hash;
                }
                else
                {
                    last = Expression.ExclusiveOr(last, hash);
                }
            }

            return Expression.Lambda<Func<object, int>>(last, pThis)
                .Compile();
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return _functions[GetType()].GetHashCodeFunc(this);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return _functions[GetType()].EqualsFunc(this, obj);
        }

        #endregion
    }
}

#endif
