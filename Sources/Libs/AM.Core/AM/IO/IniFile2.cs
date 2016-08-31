/* IniFile.cs -- simple INI-file reader/writer
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    // ReSharper disable ClassWithVirtualMembersNeverInherited.Global

    /// <summary>
    /// Simple INI-file reader/writer.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    //[DebuggerDisplay("{FileName}")]
    public class IniFile2
    {
        #region Nested classes

        /// <summary>
        /// Line (element) of the INI-file.
        /// </summary>
        [PublicAPI]
        [MoonSharpUserData]
        [DebuggerDisplay("{Name}={Value} [{Modified}]")]
        public sealed class Line
            : IHandmadeSerializable
        {
            #region Properties

            /// <summary>
            /// Key (name) of the element.
            /// </summary>
            [NotNull]
            public string Name
            {
                get { return _name; }
                //private set
                //{
                //    CheckName(value);
                //    _name = value;
                //}
            }

            /// <summary>
            /// Value of the element.
            /// </summary>
            [CanBeNull]
            public string Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    Modified = true;
                }
            }

            /// <summary>
            /// Modification flag.
            /// </summary>
            public bool Modified { get; set; }

            #endregion

            #region Construction

            /// <summary>
            /// Default constructor.
            /// </summary>
            public Line()
            {
                // Leave Name=null for a while.
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Line
                (
                    [NotNull] string name,
                    [CanBeNull] string value
                )
            {
                CheckName(name);

                _name = name;
                _value = value;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Line
                (
                    string name,
                    string value,
                    bool modified
                )
            {
                CheckName(name);

                _name = name;
                _value = value;
                Modified = modified;
            }

            #endregion

            #region Private members

            private string _name;
            private string _value;

            #endregion

            #region IHandmadeSerializable members

            /// <inheritdoc />
            public void RestoreFromStream
                (
                    BinaryReader reader
                )
            {
                Code.NotNull(reader, "reader");

                _name = reader.ReadNullableString();
                _value = reader.ReadNullableString();
                Modified = reader.ReadBoolean();
            }

            /// <inheritdoc />
            public void SaveToStream
                (
                    BinaryWriter writer
                )
            {
                Code.NotNull(writer, "writer");

                writer
                    .WriteNullable(Name)
                    .WriteNullable(Value)
                    .Write(Modified);
            }

            #endregion

            #region Object members

            /// <inheritdoc />
            public override string ToString()
            {
                string result = string.Format
                    (
                        "{0}={1}{2}",
                        Name,
                        Value,
                        Modified ? " [modified]" : string.Empty
                    );

                return result;
            }

            #endregion
        }

        // =========================================================

        /// <summary>
        /// INI-file section.
        /// </summary>
        [PublicAPI]
        [MoonSharpUserData]
        public sealed class Section
        {
            #region Properties



            #endregion

            #region Construction

            internal Section()
            {
                
            }

            #endregion

            #region Private members

            

            #endregion
        }

        #endregion

        #region Properties



        #endregion

        #region Construction



        #endregion

        #region Private members

        internal static void CheckName
            (
                string name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }
        }

        #endregion
    }
}
