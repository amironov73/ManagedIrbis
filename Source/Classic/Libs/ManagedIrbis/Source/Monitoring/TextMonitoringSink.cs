// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TextMonitoringSink.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Monitoring
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TextMonitoringSink
        : MonitoringSink
    {
        #region Public members

        /// <summary>
        /// Text writer.
        /// </summary>
        [NotNull]
        public TextWriter Writer { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="writer"></param>
        public TextMonitoringSink
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            Writer = writer;
        }

        #endregion

        #region MonitoringSink members

        /// <inheritdoc cref="MonitoringSink.WriteData" />
        public override bool WriteData
            (
                MonitoringData data
            )
        {
            Code.NotNull(data, "data");

            try
            {
                // TODO implement properly

                string text = JsonConvert.SerializeObject
                    (
                        data,
                        Formatting.Indented
                    );
                Writer.WriteLine(text);
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "TextMonitoringSink::WriteData",
                        exception
                    );

                return false;
            }

            return true;
        }

        #endregion
    }
}
