// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BibTexFile.cs --
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
    // BibTeX использует bib-файлы специального текстового формата
    // для хранения списков библиографических записей. Каждая запись
    // описывает ровно одну публикацию - статью, книгу, диссертацию,
    // и т. д.
    //

    /// <summary>
    /// Файл.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BibTexFile
    {
        #region Properties

        /// <summary>
        /// Записи.
        /// </summary>
        [NotNull]
        public RecordCollection Records { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BibTexFile()
        {
            Records = new RecordCollection();
        }

        #endregion
    }
}
