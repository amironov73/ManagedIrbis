// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MxPalette.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Mx.Infrastructrure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MxPalette
    {
        #region Properties

        /// <summary>
        /// Фон.
        /// </summary>
        [JsonProperty("background")]
        public ConsoleColor Background { get; set; }

        /// <summary>
        /// Цвет символов по умолчанию.
        /// </summary>
        [JsonProperty("foreground")]
        public ConsoleColor Foreground { get; set; }

        /// <summary>
        /// Вводимые пользователем команды.
        /// </summary>
        [JsonProperty("command")]
        public ConsoleColor Command { get; set; }

        /// <summary>
        /// Служебные сообщения.
        /// </summary>
        [JsonProperty("message")]
        public ConsoleColor Message { get; set; }

        /// <summary>
        /// Сообщения об ошибках.
        /// </summary>
        [JsonProperty("error")]
        public ConsoleColor Error { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Палитра по умолчанию.
        /// </summary>
        [NotNull]
        public static MxPalette GetDefaultPalette()
        {
            return new MxPalette
            {
                Background = ConsoleColor.Black,
                Foreground = ConsoleColor.Gray,
                Command = ConsoleColor.White,
                Message = ConsoleColor.Green,
                Error = ConsoleColor.Red
            };
        }

        #endregion
    }
}
