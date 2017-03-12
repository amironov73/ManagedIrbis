// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StandardFunctions.Files.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !WIN81

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    static partial class StandardFunctions
    {
        #region Private members

        private static readonly Dictionary<string, FileObject> Files
            = new Dictionary<string, FileObject>
                (
                    StringComparer.CurrentCultureIgnoreCase
                );

        //================================================================
        // INTERNAL METHODS
        //================================================================

        internal static void CloseFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string fullPath = GetFullPath(fileName);

            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                Files.Remove(fullPath);
            }
        }

        [NotNull]
        internal static string GetFullPath
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string result = Path.GetFullPath(fileName);

            return result;
        }

        internal static bool HaveOpenFile
            (
                [CanBeNull] string fileName
            )
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            bool result = Files.ContainsKey(fileName);

            return result;
        }

        [NotNull]
        private static string OpenInternal
            (
                [NotNull] string fileName,
                bool write,
                bool append
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string fullPath = GetFullPath(fileName);
            if (HaveOpenFile(fileName))
            {
                return string.Empty;
            }

            FileObject file = new FileObject(fullPath, write, append);
            Files.Add(fullPath, file);

            return fullPath;
        }

        [NotNull]
        internal static string OpenAppend
            (
                [NotNull] string fileName
            )
        {
            string result = OpenInternal(fileName, true, true);

            return result;
        }

        [NotNull]
        internal static string OpenRead
            (
                [NotNull] string fileName
            )
        {
            string result = OpenInternal(fileName, false, false);

            return result;
        }

        [NotNull]
        internal static string OpenWrite
            (
                [NotNull] string fileName
            )
        {
            string result = OpenInternal(fileName, true, false);

            return result;
        }

        [NotNull]
        internal static string ReadAll
            (
                [NotNull] string fileName
            )
        {
            string fullPath = GetFullPath(fileName);
            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                string result = file.ReadAll();

                return result;
            }

            return string.Empty;
        }

        [NotNull]
        internal static string ReadLine
            (
                [NotNull] string fileName
            )
        {
            string fullPath = GetFullPath(fileName);
            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                string result = file.ReadLine();

                return result;
            }

            return string.Empty;
        }

        [NotNull]
        internal static void Write
            (
                [NotNull] string fileName,
                [NotNull] string text
            )
        {
            string fullPath = GetFullPath(fileName);
            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                file.Write(text);
            }
        }

        [NotNull]
        internal static void WriteLine
            (
                [NotNull] string fileName,
                [NotNull] string text
            )
        {
            string fullPath = GetFullPath(fileName);
            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                file.WriteLine(text);
            }
        }

        //================================================================
        // FILE ORIENTED FUNCTIONS
        //================================================================

        private static void Close(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                CloseFile(fileName);
            }
        }

        private static void IsOpen(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            string output = HaveOpenFile(fileName)
                ? "1"
                : "0";
            context.Write(node, output);
        }

        private static void OpenAppend(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = OpenAppend(fileName);
                context.Write(node, output);
            }
        }

        private static void OpenRead(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = OpenRead(fileName);
                context.Write(node, output);
            }
        }

        private static void OpenWrite(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = OpenWrite(fileName);
                context.Write(node, output);
            }
        }

        private static void ReadAll(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = ReadAll(fileName);
                context.Write(node, output);
            }
        }

        private static void ReadLine(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = ReadLine(fileName);
                context.Write(node, output);
            }
        }

        private static void Write(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            string text = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(fileName)
                && !ReferenceEquals(text, null))
            {
                Write(fileName, text);
            }
        }

        private static void WriteLine(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            string text = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(fileName)
                && !ReferenceEquals(text, null))
            {
                WriteLine(fileName, text);
            }
        }

        #endregion
    }
}

#endif
