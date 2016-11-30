// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MethodRunner.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Method runner
    /// </summary>
    static class MethodRunner
    {
        #region Properties

        /// <summary>
        /// Buffer size, bytes.
        /// </summary>
        public static int BufferSize = 100*1024;

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Run process and get its output.
        /// </summary>
        public static MethodResult RunMethod
            (
                [NotNull] string dllName,
                [NotNull] string methodName,
                bool winApi,
                [NotNull] string input
            )
        {
            Code.NotNullNorEmpty(dllName, "dllName");
            Code.NotNullNorEmpty(methodName, "methodName");
            Code.NotNull(input, "input");

            MethodResult result = new MethodResult
            {
                Input = input
            };

#if CLASSIC || DESKTOP

            StringBuilder buffer = new StringBuilder(BufferSize);
            buffer.Append(input); // ???

            try
            {

                if (winApi)
                {
                    WinapiPluginFunction winapiPlugin
                        = (WinapiPluginFunction) DllCache
                        .CreateDelegate
                        (
                            dllName,
                            methodName,
                            typeof(WinapiPluginFunction)
                        );
                    result.ReturnCode = winapiPlugin
                        (
                            input,
                            buffer,
                            buffer.Capacity
                        );
                }
                else
                {
                    CdeclPluginFunction cdeclPlugin
                        = (CdeclPluginFunction) DllCache
                        .CreateDelegate
                        (
                            dllName,
                            methodName,
                            typeof(CdeclPluginFunction)
                        );
                    result.ReturnCode = cdeclPlugin
                        (
                            input,
                            buffer,
                            buffer.Capacity
                        );
                }

                result.Output = buffer.ToString();
            }
            catch
            {
                // Nothing to do
            }

#endif

            return result;

        }

        #endregion

    }
}
