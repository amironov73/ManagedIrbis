// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ObjectDumper.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;

using AM.Reflection;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Object dumper for debug purposes.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ObjectDumper
    {
        #region Properties

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public static TextWriter Output { get; set; }

        #endregion

        #region Construction

        static ObjectDumper()
        {
#if !UAP

            Output = Console.Out;

#else

            Output = TextWriter.Null;

#endif
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private ObjectDumper
            (
                [NotNull] TextWriter writer,
                int depth
            )
        {
            _writer = writer;
            _depth = depth;
        }

        #endregion

        #region Private members

        private readonly TextWriter _writer;
        private readonly int _depth;
        private int _level;
        private int _position;

        private const string _indent = "  ";

        private void Write(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                _writer.Write(s);
                _position += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (int i = 0; i < _level; i++)
            {
                Write(_indent);
            }
        }

        private void WriteTab()
        {
            Write(_indent);
            while (_position % 8 != 0)
            {
                Write(" ");
            }
        }

        private void WriteLine()
        {
            _writer.WriteLine();
            _position = 0;
        }

        private void WriteValue(object o)
        {
            if (ReferenceEquals(o, null))
            {
                Write("(null)");
            }
            else if (o is ValueType || o is string)
            {
                if (o is IFormattable)
                {
                    IFormattable formattable = o as IFormattable;
                    string s = formattable.ToString(null, CultureInfo.InvariantCulture);
                    Write(s);
                }
                else
                {
                    Write(o.ToString());
                }
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }

        /// <summary>
        /// Writes the object.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="element">The element.</param>
        private void WriteObject
            (
                string prefix,
                object element
            )
        {
            if (ReferenceEquals(element, null)
                || element is ValueType
                || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                IEnumerable enumerable = element as IEnumerable;
                if (!ReferenceEquals(enumerable, null))
                {
                    WriteEnumerable(prefix, enumerable);
                }
                else
                {
                    MemberInfo[] members = element
                        .GetType()
                        .GetMembers(BindingFlags.Public | BindingFlags.Instance);

                    Write(prefix);
                    bool propWritten = false;
                    Write("{");
                    foreach (MemberInfo member in members)
                    {
                        FieldInfo fieldInfo = member as FieldInfo;
                        PropertyInfo propertyInfo = member as PropertyInfo;
                        if (!ReferenceEquals(fieldInfo, null)
                            || !ReferenceEquals(propertyInfo, null))
                        {
                            if (propWritten)
                            {
                                WriteTab();
                            }
                            else
                            {
                                propWritten = true;
                            }
                            Write(member.Name);
                            Write("=");
                            Type type = !ReferenceEquals(fieldInfo, null)
                                ? fieldInfo.FieldType
                                : propertyInfo.PropertyType;

                            if (type.Bridge().IsValueType
                                || type == typeof(string))
                            {
                                WriteValue(!ReferenceEquals(fieldInfo, null)
                                    ? fieldInfo.GetValue(element)
                                    : propertyInfo.GetValue(element, null));
                            }
                            else
                            {
                                _level++;
                                WriteObject
                                    (
                                        prefix,
                                        !ReferenceEquals(fieldInfo, null)
                                        ? fieldInfo.GetValue(element)
                                    : propertyInfo.GetValue(element, null)
                                    );
                                _level--;
                            }
                        }
                    }
                    Write("}");

                    if (propWritten)
                    {
                        WriteLine();
                    }

                    if (_level < _depth)
                    {
                        foreach (MemberInfo member in members)
                        {
                            FieldInfo fieldInfo = member as FieldInfo;
                            PropertyInfo propertyInfo = member as PropertyInfo;
                            if (!ReferenceEquals(fieldInfo, null)
                                || !ReferenceEquals(propertyInfo, null))
                            {

                                Type type = !ReferenceEquals(fieldInfo, null)
                                                ? fieldInfo.FieldType
                                                : propertyInfo.PropertyType;
                                if (!(type.Bridge().IsValueType
                                    || type == typeof(string)))
                                {
                                    object value = !ReferenceEquals(fieldInfo, null)
                                                       ? fieldInfo.GetValue(element)
                                                       : propertyInfo.GetValue(element, null);
                                    if (!ReferenceEquals(value, null))
                                    {
                                        _level++;
                                        WriteObject(member.Name + ":", value);
                                        _level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteEnumerable
            (
                string prefix,
                [NotNull] IEnumerable enumerable
            )
        {
            int index = 0;
            foreach (object item in enumerable)
            {
                Write("[" + index + "] ");
                if (item is IEnumerable
                    && !(item is string))
                {
                    WriteIndent();
                    Write(prefix);
                    Write("...");
                    WriteLine();
                    if (_level < _depth)
                    {
                        _level++;
                        WriteObject(prefix, item);
                        _level--;
                    }
                }
                else
                {
                    WriteObject(prefix, item);
                }
                index++;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Writes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="writer">The writer.</param>
        public static void Write
            (
                object element,
                int depth,
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            ObjectDumper dumper = new ObjectDumper(writer, depth);
            dumper.WriteObject(null, element);
        }

        /// <summary>
        /// Writes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="depth">The depth.</param>
        [ExcludeFromCodeCoverage]
        public static void Write
            (
                object element,
                int depth
            )
        {
            Write(element, depth, Output);
        }

        /// <summary>
        /// Writes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        [ExcludeFromCodeCoverage]
        public static void Write
            (
                object element
            )
        {
            Write(element, 0);
        }

        #endregion
    }

}
