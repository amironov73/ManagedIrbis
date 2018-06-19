// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BibTexField.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.BibTex
{
    //
    // Каждая запись содержит некоторый список стандартных полей
    // (можно вводить любые другие поля, которые просто игнорируются
    // стандартными программами).
    //

    /// <summary>
    /// Поле.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BibTexField
    {
        #region Properties

        /// <summary>
        /// Тег поля, см. <see cref="KnownTags"/>.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Значение поля.
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return string.Format("{0}={1}", Tag, Value);
        }

        #endregion
    }
}
