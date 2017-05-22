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
using System.IO;
using System.Reflection;

using AM.Reflection;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if WIN81

using MvvmCross.Platform;

#endif

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
        #region Construction

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
                Write(o.ToString());
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
                    foreach (object item in enumerable)
                    {
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
                    }
                }
                else
                {
#if PORTABLE || WIN81

                    MemberInfo[] members = new MemberInfo[0];

#else

                    MemberInfo[] members = element
                        .GetType()
                        .GetMembers(BindingFlags.Public | BindingFlags.Instance);

#endif

                    Write(prefix);
                    bool propWritten = false;
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

#if !PORTABLE

                            if (type.Bridge().IsValueType
                                || type == typeof(string))
                            {
                                WriteValue(!ReferenceEquals(fieldInfo, null)
                                    ? fieldInfo.GetValue(element)
                                    : propertyInfo.GetValue(element, null));
                            }
                            else
                            {
                                Write
                                    (
                                    typeof(IEnumerable)
                                    .IsAssignableFrom(type)
                                        ? "..."
                                        : "{ }"
                                    );
                            }

#endif
                        }
                    }

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
                TextWriter writer
            )
        {
            ObjectDumper dumper = new ObjectDumper(writer, depth);
            dumper.WriteObject(null, element);
        }

        /// <summary>
        /// Writes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="depth">The depth.</param>
        public static void Write
            (
                object element,
                int depth
            )
        {
#if !UAP && !SILVERLIGHT && !PORTABLE && !WIN81

            Write(element, depth, Console.Out);

#endif
        }

        /// <summary>
        /// Writes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
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
