// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MstControlRecord64.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Первая запись в файле документов – управляющая 
    /// запись, которая формируется (в момент определения 
    /// базы данных или при ее инициализации) и поддерживается 
    /// автоматически.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct MstControlRecord64
    {
        #region Constants

        /// <summary>
        /// Размер управляющей записи.
        /// </summary>
        public const int RecordSize = 32;

        /// <summary>
        /// Позиция индикатора блокировки базы данных
        /// в управляющей записи.
        /// </summary>
        public const long LockFlagPosition = 28;

        #endregion

        #region Properties

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv1 { get; set; }

        /// <summary>
        /// Номер записи файла документов, назначаемый 
        /// для следующей записи, создаваемой в базе данных.
        /// </summary>
        public int NextMfn { get; set; }

        /// <summary>
        /// Смещение свободного места в файле; (всегда указывает
        /// на конец файла MST).
        /// </summary>
        public long NextPosition { get; set; }

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv2 { get; set; }

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv3 { get; set; }

        /// <summary>
        /// Резерв.
        /// </summary>
        public int Reserv4 { get; set; }

        /// <summary>
        /// Индикатор блокировки базы данных.
        /// </summary>
        public int Blocked { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Read the control record from specified stream.
        /// </summary>
        [NotNull]
        public static MstControlRecord64 Read
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            MstControlRecord64 result = new MstControlRecord64
            {
                Reserv1 = stream.ReadInt32Network(),
                NextMfn = stream.ReadInt32Network(),
                NextPosition = stream.ReadInt64Network(),
                Reserv2 = stream.ReadInt32Network(),
                Reserv3 = stream.ReadInt32Network(),
                Reserv4 = stream.ReadInt32Network(),
                Blocked = stream.ReadInt32Network()
            };

            return result;
        }

        /// <summary>
        /// Write the control record to specified stream.
        /// </summary>
        public void Write
            (
                [NotNull] Stream stream
            )
        {
            Code.NotNull(stream, "stream");

            stream.WriteInt32Network(Reserv1);
            stream.WriteInt32Network(NextMfn);
            stream.WriteInt64Network(NextPosition);
            stream.WriteInt32Network(Reserv2);
            stream.WriteInt32Network(Reserv3);
            stream.WriteInt32Network(Reserv4);
            stream.WriteInt32Network(Blocked);
        }

        #endregion
    }
}
