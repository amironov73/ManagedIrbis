/* From CodeJam.
 */
using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;


namespace CodeJam
{
    /// <summary>
    /// Assertions class.
    /// </summary>
    [PublicAPI]
    public static class Code
    {
        #region My additions

        /// <summary>
        /// Check whether <paramref name="value"/> is not defined.
        /// </summary>
        /// <typeparam name="T">Parameter type (must be System.Enum
        /// descendant).</typeparam>
        /// <param name="value">Value to check.</param>
        /// <param name="argumentName">Function argument name.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Throws <see cref="T:System.ArgumentOutOfRangeException"/>.
        /// </exception>
        [DebuggerHidden]
        [AssertionMethod]
        public static void Defined<T>
            (
                T value,
                [NotNull] string argumentName
            )
            where T : struct
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Checks whether specified files exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="argumentName">Name of the argument.</param>
        [DebuggerHidden]
        [AssertionMethod]
        public static void FileExists
            (
                [NotNull] string path,
                [NotNull] string argumentName
            )
        {
            NotNull(path, argumentName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException
                    (
                        argumentName
                        + " : "
                        + path
                    );
            }
        }

        /// <summary>
        /// Checks whether <paramref name="argument"/> fits into
        /// specified range <paramref name="from"/> to <paramref name="to"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Throws <see cref="T:System.ArgumentOutOfRangeException"/>.
        /// </exception>
        [DebuggerHidden]
        [AssertionMethod]
        public static void InRange
            (
                double argument,
                double from,
                double to,
                string argumentName
            )
        {
            if ((argument < from)
                 || (argument > to))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is not negative.
        /// </summary>
        /// <param name="argument">Value to check.</param>
        /// <param name="argumentName">Function argument name.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Throws <see cref="T:System.ArgumentOutOfRangeException"/>.
        /// </exception>
        [DebuggerHidden]
        [AssertionMethod]
        public static void Nonnegative
            (
                int argument,
                string argumentName
            )
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is not negative.
        /// </summary>
        /// <param name="argument">Value to check.</param>
        /// <param name="argumentName">Function argument name.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Throws <see cref="T:System.ArgumentOutOfRangeException"/>.
        /// </exception>
        [DebuggerHidden]
        [AssertionMethod]
        public static void Nonnegative
            (
                long argument, 
                string argumentName
            )
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is not negative.
        /// </summary>
        /// <param name="argument">Value to check.</param>
        /// <param name="argumentName">Function argument name.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Throws <see cref="T:System.ArgumentOutOfRangeException"/>.
        /// </exception>
        [DebuggerHidden]
        [AssertionMethod]
        public static void Nonnegative
            (
                double argument, 
                [NotNull] string argumentName
            )
        {
            if (argument < 0.0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is positive.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Throws <see cref="T:System.ArgumentOutOfRangeException"/>.
        /// </exception>
        [DebuggerHidden]
        [AssertionMethod]
        public static void Positive
            (
                int argument,
                [NotNull] string argumentName
            )
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is positive.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Throws <see cref="T:System.ArgumentOutOfRangeException"/>.
        /// </exception>
        [DebuggerHidden]
        [AssertionMethod]
        public static void Positive
            (
                double argument,
                [NotNull] string argumentName
            )
        {
            if (argument <= 0.0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        #endregion


        #region Argument validation
        /// <summary>
        /// Ensures that <paramref name="arg" /> != <c>null</c>
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void NotNull<T>(
            [CanBeNull, NoEnumeration] T arg,
            [NotNull, InvokerParameterName] string argName) where T : class
        {
            if (arg == null)
                throw CodeExceptions.ArgumentNull(argName);
        }

        /// <summary>
        /// Ensures that <paramref name="arg" /> != <c>null</c>
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void NotNull<T>(
            [CanBeNull] T? arg,
            [NotNull, InvokerParameterName] string argName) where T : struct
        {
            if (arg == null)
                throw CodeExceptions.ArgumentNull(argName);
        }

        /// <summary>
        /// Ensures that <paramref name="arg" /> is not null nor empty
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void NotNullNorEmpty(
            [CanBeNull] string arg,
            [NotNull, InvokerParameterName] string argName)
        {
            if (string.IsNullOrEmpty(arg))
                throw CodeExceptions.ArgumentNullOrEmpty(argName);
        }

        /// <summary>
        /// Assertion for the argument value
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void AssertArgument(
            bool condition,
            [NotNull, InvokerParameterName] string argName,
            [NotNull] string message)
        {
            if (!condition)
                throw CodeExceptions.Argument(argName, message);
        }

        /// <summary>
        /// Assertion for the argument value
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void AssertArgument(
            bool condition,
            [NotNull, InvokerParameterName] string argName,
            [NotNull, InstantHandle] Func<string> messageFactory)
        {
            if (!condition)
            {
                NotNull(messageFactory, "messageFactory");
                throw CodeExceptions.Argument(argName, messageFactory());
            }
        }

        /// <summary>
        /// Assertion for the argument value
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod, StringFormatMethod("messageFormat")]
        public static void AssertArgument(
            bool condition,
            [NotNull, InvokerParameterName] string argName,
            [NotNull] string messageFormat,
            [CanBeNull] params object[] args)
        {
            if (!condition)
                throw CodeExceptions.Argument(argName, messageFormat, args);
        }
        #endregion

        #region Argument validation - in range
        /// <summary>
        /// Assertion for the argument in range
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void InRange(
            int value,
            [NotNull, InvokerParameterName] string argName,
            int fromValue,
            int toValue)
        {
            if (value < fromValue || value > toValue)
                throw CodeExceptions.ArgumentOutOfRange(argName, value, fromValue, toValue);
        }

        /// <summary>
        /// Assertion for the argument in range
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void InRange(
            int value,
            [NotNull, InvokerParameterName] string argName,
            int fromValue)
        {
            if (value < fromValue)
                throw CodeExceptions.ArgumentOutOfRange(argName, value, fromValue);
        }

        #endregion

        #region Argument validation - valid index
        /// <summary>
        /// Assertion for index in range
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void ValidIndex(
            int index,
            [NotNull, InvokerParameterName] string argName)
        {
            if (index < 0)
                throw CodeExceptions.IndexOutOfRange(argName, index, 0, int.MaxValue);
        }

        /// <summary>
        /// Assertion for index in range
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void ValidIndex(
            int index,
            [NotNull, InvokerParameterName] string argName,
            int length)
        {
            if (index < 0 || index >= length)
                throw CodeExceptions.IndexOutOfRange(argName, index, 0, length);
        }

        /// <summary>
        /// Assertion for from-to index pair
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void ValidIndexPair(
            int fromIndex,
            [NotNull, InvokerParameterName] string fromIndexName,
            int toIndex,
            [NotNull, InvokerParameterName] string toIndexName,
            int length)
        {
            ValidIndex(fromIndex, fromIndexName, length);

            if (toIndex < fromIndex || toIndex >= length)
                throw CodeExceptions.IndexOutOfRange(toIndexName, toIndex, fromIndex, length);
        }

        /// <summary>
        /// Assertion for startIndex-count pair
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void ValidIndexAndCount(
            int startIndex,
            [NotNull, InvokerParameterName] string startIndexName,
            int count,
            [NotNull, InvokerParameterName] string countName,
            int length)
        {
            ValidIndex(startIndex, startIndexName, length);

            InRange(count, countName, 0, length - startIndex);
        }
        #endregion

        #region State validation
        /// <summary>
        /// State assertion
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod]
        public static void AssertState(
            bool condition,
            [NotNull] string message)
        {
            if (!condition)
                throw CodeExceptions.InvalidOperation(message);
        }

        /// <summary>
        /// State assertion
        /// </summary>
        [DebuggerHidden]
        [AssertionMethod, StringFormatMethod("messageFormat")]
        public static void AssertState(
            bool condition,
            [NotNull] string messageFormat,
            [CanBeNull] params object[] args)
        {
            if (!condition)
                throw CodeExceptions.InvalidOperation(messageFormat, args);
        }
        #endregion
    }
}