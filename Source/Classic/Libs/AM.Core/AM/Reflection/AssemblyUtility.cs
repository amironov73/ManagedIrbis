// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AssemblyUtility.cs -- collection of assembly manipulation routines
 *  Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if NETCORE

using System.Runtime.Loader;

#endif

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// Collection of assembly manipulation routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class AssemblyUtility
    {
        #region Public methods

        /// <summary>
        /// Check an assembly to see if it has the given public key token.
        /// </summary>
        /// <remarks>Does not check to make sure the assembly's signature 
        /// is valid.</remarks>
        public static bool CheckForToken
            (
                [NotNull] Assembly assembly,
                [NotNull] byte[] expectedToken
            )
        {
            Code.NotNull(assembly, "assembly");
            Code.NotNull(expectedToken, "expectedToken");

#if SILVERLIGHT

            Log.Error
                (
                    "AssemblyUtility::CheckForToken: "
                    + "not implemented"
                );

            throw new NotImplementedException ();

#else

            byte[] realToken = assembly.GetName().GetPublicKeyToken();
            if (ReferenceEquals(realToken, null))
            {
                return false;
            }

            return ArrayUtility.Compare(realToken, expectedToken) == 0;

#endif
        }

        /// <summary>
        /// Check an assembly to see if it has the given public key token.
        /// </summary>
        public static bool CheckForToken
            (
                [NotNull] string pathToAssembly,
                [NotNull] byte[] expectedToken
            )
        {
            Code.NotNullNorEmpty(pathToAssembly, "pathToAssembly");
            Code.NotNull(expectedToken, "expectedToken");

#if !CLASSIC

            Log.Error
                (
                    "AssmeblyUtility::CheckForToken: "
                    + "not implemented"
                );

            throw new NotImplementedException();

#else

            Assembly assembly
                = Assembly.ReflectionOnlyLoadFrom(pathToAssembly);

            return CheckForToken(assembly, expectedToken);

#endif
        }

        /// <summary>
        /// Get directory path where the assembly resides.
        /// </summary>
        /// <remarks>
        /// Borrowerd from StackOverflow:
        /// https://stackoverflow.com/questions/52797/how-do-i-get-the-path-of-the-assembly-the-code-is-in
        /// </remarks>
        [NotNull]
        public static string GetAssemblyPath
            (
                [NotNull] Assembly assembly
            )
        {
            Code.NotNull(assembly, "assembly");

#if UAP || WIN81 || PORTABLE || WINMOBILE

            Log.Error
                (
                    "AssmeblyUtility::GetAssemblyPath: "
                    + "not implemented"
                );

            throw new NotImplementedException("GetAssemblyPath");

#else

            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string result = Path.GetDirectoryName(path)
                .ThrowIfNull("Path.GetDirectoryName");

            return result;

#endif
        }

        /// <summary>
        /// Determines whether the specified assembly is debug version.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>
        /// 	<c>true</c> if the specified assembly is debug version; 
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDebugAssembly
            (
                [NotNull] Assembly assembly
            )
        {
            Code.NotNull(assembly, "assembly");

#if !CLASSIC

            Log.Error
                (
                    "AssmeblyUtility::IsDebugAssembly: "
                    + "not implemented"
                );

            throw new NotImplementedException();

#else

            object[] attributes = assembly.GetCustomAttributes
                (
                    typeof(DebuggableAttribute),
                    false
                );

            return attributes.Length != 0;

#endif
        }

        /// <summary>
        /// Check an assembly whether it has Microsoft public key token.
        /// </summary>
        public static bool IsMicrosoftSigned
            (
                [NotNull] Assembly assembly
            )
        {
            Code.NotNull(assembly, "assembly");

            return CheckForToken(assembly, PublicKeyTokens.MicrosoftClr())
                   || CheckForToken(assembly, PublicKeyTokens.MicrosoftFX());
        }

        /// <summary>
        /// Check an assembly whether it has Microsoft public key token.
        /// </summary>
        public static bool IsMicrosoftSigned
            (
                [NotNull] string path
            )
        {
            Code.NotNullNorEmpty(path, "path");

            return CheckForToken(path, PublicKeyTokens.MicrosoftClr())
                   || CheckForToken(path, PublicKeyTokens.MicrosoftFX());
        }

        /// <summary>
        /// Load assembly from the file.
        /// </summary>
        [NotNull]
        public static Assembly LoadFile
            (
                [NotNull] string path
            )
        {
            Code.NotNullNorEmpty(path, "path");

#if NETCORE

            Assembly result = AssemblyLoadContext.Default
                .LoadFromAssemblyPath (path);

            return result;

#elif PORTABLE || WIN81 || SILVERLIGHT || UAP || WINMOBILE

            throw new NotSupportedException();

#else

            return Assembly.LoadFile(path);

#endif
        }

        #endregion
    }
}
