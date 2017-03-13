// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StandardFunctions.Objects.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

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

        private static readonly Dictionary<string, OuterObject> Objects
            = new Dictionary<string, OuterObject>
                (
                    StringComparer.CurrentCultureIgnoreCase
                );

        //================================================================
        // INTERNAL METHODS
        //================================================================

        /// <summary>
        /// Create object of given type.
        /// </summary>
        [NotNull]
        internal static OuterObject CreateObject
            (
                [NotNull] string className
            )
        {
            Code.NotNullNorEmpty(className, "className");

#if WIN81 || PocketPC || WINMOBILE

            throw new NotImplementedException();

#else

            Type type = Type.GetType(className, true, true);
            string name = Guid.NewGuid().ToString("N");
            OuterObject result 
                = (OuterObject) Activator.CreateInstance(type, name);
            result.IncreaseCounter();

            RegisterObject(result);

            return result;

#endif
        }

        /// <summary>
        /// Get object by the name.
        /// </summary>
        [CanBeNull]
        internal static OuterObject GetObject
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            OuterObject result;
            Objects.TryGetValue(name, out result);

            return result;
        }

        internal static void RegisterObject
            (
                [NotNull] OuterObject obj
            )
        {
            Code.NotNull(obj, "obj");

            Objects.Add(obj.Name, obj);
        }

        internal static void UnregisterObject
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Objects.Remove(name);
        }

        //================================================================
        // OBJECT ORIENTED FUNCTIONS
        //================================================================

        private static void CallObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            string objName = context.GetStringArgument(arguments, 0);
            string methodName = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(objName)
                && !string.IsNullOrEmpty(methodName))
            {
                List<string> callParameters = new List<string>();
                for (int i = 2; i < arguments.Length; i++)
                {
                    string arg = context.GetStringArgument(arguments, i);
                    if (ReferenceEquals(arg, null))
                    {
                        break;
                    }
                    callParameters.Add(arg);
                }

                OuterObject obj = GetObject(objName);
                if (!ReferenceEquals(obj, null))
                {
                    obj.CallMethod
                        (
                            methodName,

                            // ReSharper disable CoVariantArrayConversion
                            callParameters.ToArray()
                            // ReSharper restore CoVariantArrayConversion
                        );
                }
            }
        }

        private static void CloseObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            string name = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(name))
            {
                OuterObject obj = GetObject(name);
                if (!ReferenceEquals(obj, null))
                {
                    if (obj.DecreaseCounter() <= 0)
                    {
                        UnregisterObject(name);
                    }
                }
            }
        }

        private static void CreateObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            string className = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(className))
            {
                OuterObject obj = CreateObject(className);

                context.Write(node, obj.Name);
            }
        }

        private static void OpenObject(PftContext context, PftNode node, PftNode[] arguments)
        {
            string name = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(name))
            {
                OuterObject obj = GetObject(name);
                if (!ReferenceEquals(obj, null))
                {
                    obj.IncreaseCounter();

                    context.Write(node, obj.Name);
                }
            }
        }

        #endregion
    }
}
