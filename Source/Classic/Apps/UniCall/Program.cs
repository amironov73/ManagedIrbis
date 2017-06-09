// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Project.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Reflection;

#endregion

namespace UniCall
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine
                    (
                        "Usage: UniCall <assembly> <type> <method>"
                    );
                return;
            }

            try
            {

                string assemblyPath = args[0];
                Assembly assembly = Assembly.LoadFile(assemblyPath);

                string typeName = args[1];
                Type type = assembly.GetType(typeName, true);

                string methodName = args[2];
                MethodInfo method = type.GetMethod
                    (
                        methodName,
                        BindingFlags.Instance | BindingFlags.Static
                        | BindingFlags.Public | BindingFlags.NonPublic
                    );
                if (ReferenceEquals(method, null))
                {
                    Console.WriteLine
                        (
                            "Can't find method {0}",
                            methodName
                        );
                    return;
                }

                object result;

                if (method.IsStatic)
                {
                    result = method.Invoke(null, null);
                }
                else
                {
                    object instance = Activator.CreateInstance(type);
                    if (ReferenceEquals(instance, null))
                    {
                        Console.WriteLine
                            (
                                "Can't create instance of {0}",
                                type
                            );
                    }

                    result = method.Invoke(instance, null);
                }

                if (ReferenceEquals(result, null))
                {
                    result = "(null)";
                }

                Console.WriteLine
                    (
                        "Result = {0}",
                        result
                    );
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
