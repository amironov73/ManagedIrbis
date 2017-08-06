// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CompilerUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Reflection;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class CompilerUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert boolean to string according C# rules.
        /// </summary>
        [NotNull]
        public static string BooleanToText
            (
                bool value
            )
        {
            return value ? "true" : "false";
        }

        /// <summary>
        /// Escape the text according C# rules.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [CanBeNull]
        public static string Escape
            (
                [CanBeNull] string text
            )
        {
            // TODO implement properly

            return text;
        }

        /// <summary>
        /// Find entry point of the assembly.
        /// </summary>
        [NotNull]
        public static Func<PftContext, PftPacket> GetEntryPoint
            (
                [NotNull] Assembly assembly
            )
        {
            Code.NotNull(assembly, "assembly");

#if WIN81 || PORTABLE

            Func<PftContext, PftPacket> result = null;

#else

            Type[] types = assembly.GetTypes();
            if (types.Length != 1)
            {
                throw new PftCompilerException();
            }
            Type type = types[0];
            if (!type.Bridge().IsSubclassOf(typeof(PftPacket)))
            {
                throw new PftCompilerException();
            }

#if NETCORE

            MethodInfo method = type.Bridge().GetMethod
                (
                    "CreateInstance",
                    new Type[] { typeof(PftContext) },
                    null
                );

            Func<PftContext, PftPacket> result
                = (Func<PftContext, PftPacket>)method.CreateDelegate
                    (
                        typeof(Func<PftContext, PftPacket>)
                    );

#elif UAP

            MethodInfo method = type.GetMethod
                (
                    "CreateInstance",
                    new [] { typeof(PftContext) }
                );

            Func<PftContext, PftPacket> result
                = (Func<PftContext, PftPacket>)method.CreateDelegate
                    (
                        typeof(Func<PftContext, PftPacket>)
                    );

#elif WINMOBILE

            MethodInfo method = type.Bridge().GetMethod
                (
                    "CreateInstance",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new Type[] { typeof(PftContext) },
                    null
                );

            Func<PftContext, PftPacket> result
                = (Func<PftContext, PftPacket>)Delegate.CreateDelegate
                    (
                        typeof(Func<PftContext, PftPacket>),
                        null,
                        method
                    );

#else

            MethodInfo method = type.Bridge().GetMethod
                (
                    "CreateInstance",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new Type[] { typeof(PftContext) },
                    null
                );

            Func<PftContext, PftPacket> result
                = (Func<PftContext, PftPacket>)Delegate.CreateDelegate
                    (
                        typeof(Func<PftContext, PftPacket>),
                        method,
                        true
                    );

#endif

            if (ReferenceEquals(result, null))
            {
                throw new PftCompilerException();
            }

#endif

            return result;
        }

        /// <summary>
        /// Shorten the text.
        /// </summary>
        [NotNull]
        public static string ShortenText
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string result = text.Replace('\r', ' ')
                .Replace('\n', ' ')
                .SafeSubstring(0, 25)
                .IfEmpty(string.Empty);

            return result;
        }

        #endregion
    }
}
