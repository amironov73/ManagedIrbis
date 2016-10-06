/* ComparableObject.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !NETCORE && !PocketPC

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

            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length == 0)
            {
                
            }

            // compound AND expression using short-circuit evaluation
            Expression last = null;
            foreach (PropertyInfo property in properties)
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

            // use fields if no properties present
            if (properties.Length == 0)
            {
                FieldInfo[] fields = type.GetFields();
                foreach (FieldInfo field in fields)
                {
                    BinaryExpression equalExpression = Expression.Equal
                    (
                        Expression.Field(pCastThis, field),
                        Expression.Field(pCastThat, field)
                    );

                    last = last == null
                        ? equalExpression
                        : Expression.AndAlso
                            (
                                last,
                                equalExpression
                            );
                }
            }

            if (ReferenceEquals(last, null))
            {
                last = Expression.Constant(false, typeof(bool));
            }

            // call Object.Equals if second parameter doesn't match type
            last = Expression.Condition
                (
                    Expression.AndAlso
                    (
                        Expression.NotEqual
                        (
                            pThat, 
                            Expression.Constant(null)
                        ),
                        Expression.TypeIs(pThat, type)
                    ),
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

            PropertyInfo[] properties = type.GetProperties();

            Expression last = null;
            foreach (PropertyInfo property in properties)
            {
                Expression hash = Expression.Call
                (
                    typeof(ComparableObject).GetMethod
                    (
                        "GetHashCode",
                        new[] { typeof(object) }
                    ),
                    Expression.Convert
                    (
                        Expression.Property(pCastThis, property),
                        typeof(object)
                    )
                );

                last = last == null ?
                    hash
                    : Expression.ExclusiveOr(last, hash);
            }

            // use fields if no properties present
            if (properties.Length == 0)
            {
                FieldInfo[] fields = type.GetFields();

                foreach (FieldInfo field in fields)
                {
                    Expression hash = Expression.Call
                    (
                        typeof(ComparableObject).GetMethod
                        (
                            "GetHashCode",
                            new[] { typeof(object) }
                        ),
                        Expression.Convert
                        (
                            Expression.Field(pCastThis, field),
                            typeof(object)
                        )
                    );

                    last = last == null ?
                        hash
                        : Expression.ExclusiveOr(last, hash);
                }
            }

            if (ReferenceEquals(last, null))
            {
                last = Expression.Constant(0, typeof(int));
            }

            return Expression.Lambda<Func<object, int>>(last, pThis)
                .Compile();
        }

        /// <summary>
        /// Get hash code for arbitrary object.
        /// </summary>
        public static int GetHashCode
            (
                [CanBeNull] object obj
            )
        {
            return ReferenceEquals(obj, null)
                ? 0
                : obj.GetHashCode();
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
        public override bool Equals
            (
                object obj
            )
        {
            if (ReferenceEquals(obj, null)
                || obj.GetType() != GetType())
            {
                return false;
            }

            return _functions[GetType()].EqualsFunc(this, obj);
        }

        #endregion
    }
}

#endif
