// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* From CodeJam project
 */
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

//using static CodeJam.PlatformDependent;

namespace UnsafeCode
{
    /// <summary>
    /// Exception factory class
    /// </summary>
    [PublicAPI]
    public static class CodeExceptions
    {
        #region Behavior setup and implementation helpers

        /// <summary>
        /// If true, breaks execution on assertion failure.
        /// Enabled by default.
        /// </summary>
        public static bool BreakOnException { get; set; } = true;

        /// <summary>
        /// BreaksExecution if debugger attached
        /// </summary>
        [DebuggerHidden]
        public static void BreakIfAttached()
        {
            if (BreakOnException && Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        /// <summary>
        /// Formats message or returns <paramref name="messageFormat"/> as it is if <paramref name="args"/> are null or empty
        /// </summary>
        [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        private static string FormatMessage
            (
                [NotNull] string messageFormat,
                [CanBeNull] params object[] args
            )
        {
            return (args == null || args.Length == 0)
                ? messageFormat
                : string.Format(messageFormat, args);
        }

        #endregion

        #region Argument validation
        /// <summary>
        /// Creates <seealso cref="ArgumentNullException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentNullException ArgumentNull([NotNull, InvokerParameterName] string argumentName)
        {
            BreakIfAttached();
            return new ArgumentNullException(argumentName);
        }

        /// <summary>
        /// Creates <seealso cref="ArgumentException"/> for empty string
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentException ArgumentNullOrEmpty([NotNull, InvokerParameterName] string argumentName)
        {
            BreakIfAttached();
            return new ArgumentException(
                string.Format("String '{0}' should be neither null nor empty",
                argumentName));

        }

        /// <summary>
        /// Creates <seealso cref="ArgumentException"/> for empty (or whitespace) string
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentException ArgumentNullOrWhiteSpace([NotNull, InvokerParameterName] string argumentName)
        {
            BreakIfAttached();
            return new ArgumentException(
                string.Format("String '{0}' should be neither null nor whitespace",
                argumentName));
        }

        /// <summary>
        /// Creates <seealso cref="ArgumentOutOfRangeException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentOutOfRangeException ArgumentOutOfRange(
            [NotNull, InvokerParameterName] string argumentName,
            int value, int fromValue, int toValue)
        {
            BreakIfAttached();
            return new ArgumentOutOfRangeException
                (
                    argumentName,
                    value,
                    string.Format
                    (
                        "The value of '{0}' ({1}) should be between {2} and {3}",
                        argumentName,
                        value,
                        fromValue,
                        toValue
                    )
                );
        }

        /// <summary>
        /// Creates <seealso cref="ArgumentOutOfRangeException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentOutOfRangeException ArgumentOutOfRange(
            [NotNull, InvokerParameterName] string argumentName,
            int value, int fromValue)
        {
            BreakIfAttached();
            return new ArgumentOutOfRangeException
                (
                    argumentName,
                    value,
                    string.Format
                    (
                        "The value of '{0}' ({1}) should be greater than {2}",
                        argumentName,
                        value,
                        fromValue
                    )
                );
        }

        /// <summary>
        /// Creates <seealso cref="ArgumentOutOfRangeException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentOutOfRangeException ArgumentOutOfRange<T>(
            [NotNull, InvokerParameterName] string argumentName,
            T value, T fromValue, T toValue)
        {
            BreakIfAttached();
            return new ArgumentOutOfRangeException
                (
                    argumentName,
                    value,
                    string.Format
                    (
                        "The value of '{0}' ('{1}') should be between '{2}' and '{3}'",
                        argumentName,
                        value,
                        fromValue,
                        toValue
                    )
                );
        }

        /// <summary>
        /// Creates <seealso cref="ArgumentOutOfRangeException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentOutOfRangeException ArgumentOutOfRange<T>
            (
                [NotNull, InvokerParameterName] string argumentName,
                T value,
                T fromValue
            )
        {
            BreakIfAttached();
            return new ArgumentOutOfRangeException
                (
                    argumentName,
                    value,
                    string.Format
                        (
                            "The value of '{0}' ('{1}') should be greater than '{2}'.",
                            argumentName,
                            value,
                            fromValue
                        )
                );
        }

        /// <summary>
        /// Creates <seealso cref="IndexOutOfRangeException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        public static IndexOutOfRangeException IndexOutOfRange
            (
                [NotNull, InvokerParameterName] string argumentName,
                int value,
                int startIndex,
                int length
            )
        {
            BreakIfAttached();
            return new IndexOutOfRangeException
                (
                    string.Format
                    (
                        "The value of '{0}' ({1}) should be greater than or equal to {2} and less than {3}.",
                        argumentName,
                        value,
                        startIndex,
                        length
                    )
                );
        }
        #endregion

        #region General purpose exceptions
        /// <summary>
        /// Creates <seealso cref="ArgumentException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        public static ArgumentException Argument(
            [NotNull, InvokerParameterName] string argumentName,
            [NotNull] string messageFormat,
            [CanBeNull] params object[] args)
        {
            BreakIfAttached();
            return new ArgumentException(FormatMessage(messageFormat, args), argumentName);
        }

        /// <summary>
        /// Creates <seealso cref="InvalidOperationException"/>
        /// </summary>
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        public static InvalidOperationException InvalidOperation(
            [NotNull] string messageFormat,
            [CanBeNull] params object[] args)
        {
            BreakIfAttached();
            return new InvalidOperationException(FormatMessage(messageFormat, args));
        }
        #endregion

        #region Exceptions for specific scenarios
        /// <summary>
        /// Creates <seealso cref="ArgumentOutOfRangeException"/>.
        /// Used to be thrown from the default: switch clause
        /// </summary>
        [DebuggerHidden, NotNull]
        public static ArgumentOutOfRangeException UnexpectedArgumentValue<T>
            (
                [NotNull, InvokerParameterName] string argumentName,
                [CanBeNull] T value
            )
        {
            BreakIfAttached();

            // ReSharper disable CompareNonConstrainedGenericWithNull
            var valueType = (value == null) ? typeof(T) : value.GetType();
            // ReSharper restore CompareNonConstrainedGenericWithNull

            return new ArgumentOutOfRangeException
                (
                    argumentName,
                    value,
                    string.Format
                        (
                            "Unexpected value '{0}' of type '{1}'",
                            value,
                            valueType.FullName
                        )
                );
        }

        /// <summary>
        /// Creates <seealso cref="ArgumentOutOfRangeException"/>.
        /// Used to be thrown from default: switch clause
        /// </summary>
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        public static ArgumentOutOfRangeException UnexpectedArgumentValue<T>
            (
                [NotNull, InvokerParameterName] string argumentName,
                [CanBeNull] T value,
                [NotNull] string messageFormat,
                [CanBeNull] params object[] args
            )
        {
            BreakIfAttached();

            return new ArgumentOutOfRangeException
                (
                    argumentName,
                    value,
                    FormatMessage(messageFormat, args)
                );
        }

        /// <summary>
        /// Creates <seealso cref="InvalidOperationException"/>.
        /// Used to be thrown from the default: switch clause
        /// </summary>
        [DebuggerHidden, NotNull]
        public static InvalidOperationException UnexpectedValue<T>
            (
                [CanBeNull] T value
            )
        {
            BreakIfAttached();

            // ReSharper disable CompareNonConstrainedGenericWithNull
            var valueType = (value == null) ? typeof(T) : value.GetType();
            // ReSharper restore CompareNonConstrainedGenericWithNull

            string message = string.Format
                (
                    "Unexpected value '{0}' of type '{1}'",
                    value,
                    valueType.FullName
                );

            return new InvalidOperationException(message);
        }

        /// <summary>
        /// Creates <seealso cref="InvalidOperationException"/>.
        /// Used to be thrown from default: switch clause
        /// </summary>
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        public static InvalidOperationException UnexpectedValue
            (
                [NotNull] string messageFormat,
                [CanBeNull] params object[] args
            )
        {
            BreakIfAttached();

            return new InvalidOperationException(FormatMessage(messageFormat, args));
        }

        /// <summary>
        /// Throw this if the object is disposed.
        /// </summary>
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        public static ObjectDisposedException ObjectDisposed
            (
                [CanBeNull] Type typeofDisposedObject
            )
        {
            BreakIfAttached();
            string fullName = (typeofDisposedObject == null)
                ? null
                : typeofDisposedObject.FullName;

            return new ObjectDisposedException(fullName);
        }

        /// <summary>
        /// Throw this if the object is disposed.
        /// </summary>
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        public static ObjectDisposedException ObjectDisposed
            (
                [CanBeNull] Type typeofDisposedObject,
                [NotNull] string messageFormat,
                [CanBeNull] params object[] args
            )
        {
            BreakIfAttached();
            string fullName = (typeofDisposedObject == null) ? null : typeofDisposedObject.FullName;

            return new ObjectDisposedException
                (
                    fullName,
                    FormatMessage(messageFormat, args)
                );
        }

        /// <summary>
        /// Used to be thrown in places expected to be unreachable.
        /// </summary>
        [DebuggerHidden, NotNull]
        [StringFormatMethod("messageFormat")]
        public static NotSupportedException Unreachable
            (
                [NotNull] string messageFormat,
                [CanBeNull] params object[] args
            )
        {
            BreakIfAttached();
            return new NotSupportedException
                (
                    FormatMessage
                        (
                            messageFormat,
                            args
                        )
                );
        }
        #endregion
    }
}
