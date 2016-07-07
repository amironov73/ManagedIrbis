/* ObjectDumper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
#if NOTDEF

    /// <summary>
    /// Object dumper for debug purposes.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ObjectDumper
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ObjectDumper"/> class.
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
            while ((_position % 8) != 0)
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
            if (o == null)
            {
                Write("(null)");
            }
            else if ((o is ValueType) || (o is string))
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
                || (element is ValueType)
                || (element is string))
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
                        if ((item is IEnumerable)
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
                    MemberInfo[] members = element
                        .GetType()
                        .GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    Write(prefix);
                    bool propWritten = false;
                    foreach (MemberInfo member in members)
                    {
                        FieldInfo fieldInfo = member as FieldInfo;
                        PropertyInfo propertyInfo = member as PropertyInfo;
                        if ((fieldInfo != null)
                            || (propertyInfo != null))
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
                            Type type = (fieldInfo != null)
                                            ? fieldInfo.FieldType
                                            : propertyInfo.PropertyType;
                            if (type.IsValueType
                                || (type == typeof(string)))
                            {
                                WriteValue((fieldInfo != null)
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
                            if ((fieldInfo != null)
                                || (propertyInfo != null))
                            {

                                Type type = (fieldInfo != null)
                                                ? fieldInfo.FieldType
                                                : propertyInfo.PropertyType;
                                if (!(type.IsValueType
                                    || (type == typeof(string))))
                                {
                                    object value = (fieldInfo != null)
                                                       ? fieldInfo.GetValue(element)
                                                       : propertyInfo.GetValue(element, null);
                                    if (value != null)
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
            Write(element, depth, Console.Out);
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

#endif
}
