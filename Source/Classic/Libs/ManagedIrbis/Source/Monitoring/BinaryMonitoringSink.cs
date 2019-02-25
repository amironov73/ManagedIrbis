// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BinaryMonitoringSink.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Monitoring
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BinaryMonitoringSink
        : MonitoringSink
    {
        #region Properties

        /// <summary>
        /// Writer.
        /// </summary>
        [NotNull]
        public BinaryWriter Writer { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryMonitoringSink
            (
                [NotNull] BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            Writer = writer;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Open file for appending monitoring data.
        /// </summary>
        [NotNull]
        public static BinaryMonitoringSink AppendFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryMonitoringSink result = new BinaryMonitoringSink(writer);

            return result;
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

            data.SaveToStream(Writer);

            return true;
        }

        #endregion
    }
}
