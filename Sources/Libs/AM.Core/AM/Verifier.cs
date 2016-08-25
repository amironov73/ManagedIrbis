/* Verifier.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Helper for <see cref="IVerifiable"/>
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Verifier<T>
        where T: IVerifiable
    {
        #region Properties

        /// <summary>
        /// Prefix.
        /// </summary>
        [CanBeNull]
        private string Prefix { get; set; }

        /// <summary>
        /// Result.
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Target.
        /// </summary>
        public T Target { get { return _target; } }

        /// <summary>
        /// Throw on error.
        /// </summary>
        public bool ThrowOnError { get { return _throwOnError; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Verifier
            (
                [NotNull] T target,
                bool throwOnError
            )
        {
            _target = target;
            _throwOnError = throwOnError;
            Result = true;
        }

        #endregion

        #region Private members

        private readonly T _target;

        private readonly bool _throwOnError;
        private void _Throw ()
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Throw(Prefix);
                }
                else
                {
                    Throw();
                }
            }
        }

        private void _Throw
            (
                string message
            )
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Throw(Prefix + ": " + message);
                }
                else
                {
                    Throw(message);
                }
            }
        }

        private void _Throw
            (
                string format,
                params object[] arguments
            )
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    string message = Prefix + ": "
                        + string.Format
                            (
                                format,
                                arguments
                            );
                    Throw(message);
                }
                else
                {
                    Throw
                        (
                            format,
                            arguments
                        );
                }
            }
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Assert.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition
            )
        {
            Result = Result && condition;
            _Throw();

            return this;
        }

        /// <summary>
        /// Assert.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition,
                string message
            )
        {
            Result = Result && condition;
            _Throw(message);

            return this;
        }

        /// <summary>
        /// Assert.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition,
                string format,
                params object[] arguments
            )
        {
            Result = Result && condition;
            _Throw
                (
                    format,
                    arguments
                );

            return this;
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNull
            (
                object value
            )
        {
            return Assert
                (
                    !ReferenceEquals(value, null)
                );
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNull
            (
                object value,
                string name
            )
        {
            return Assert
                (
                    !ReferenceEquals(value, null),
                    name
                );
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNullNorEmpty
            (
                string value
            )
        {
            return Assert
                (
                    !string.IsNullOrEmpty(value)
                );
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNullNorEmpty
            (
                string value,
                string name
            )
        {
            return Assert
                (
                    !string.IsNullOrEmpty(value),
                    name
                );
        }

        /// <summary>
        /// Throw exception.
        /// </summary>
        public void Throw()
        {
            throw new VerificationException();
        }

        /// <summary>
        /// Throw exception.
        /// </summary>
        public void Throw
            (
                string message
            )
        {
            throw new VerificationException(message);
        }

        /// <summary>
        /// Throw exception.
        /// </summary>
        [StringFormatMethod("format")]
        public void Throw
            (
                string format,
                params object[] arguments
            )
        {
            string message = string.Format
                (
                    format,
                    arguments
                );

            Throw(message);
        }

        /// <summary>
        /// Verify sub-object.
        /// </summary>
        public Verifier<T> VerifySubObject
            (
                [NotNull] IVerifiable verifiable
            )
        {
            Code.NotNull(verifiable, "verifiable");

            Assert(verifiable.Verify(ThrowOnError));

            return this;
        }

        /// <summary>
        /// Verify sub-object.
        /// </summary>
        public Verifier<T> VerifySubObject
            (
                [NotNull] IVerifiable verifiable,
                [NotNull] string name
            )
        {
            Code.NotNull(verifiable, "verifiable");
            Code.NotNullNorEmpty(name, "name");

            Assert(verifiable.Verify(ThrowOnError), name);

            return this;
        }

        #endregion
    }
}
