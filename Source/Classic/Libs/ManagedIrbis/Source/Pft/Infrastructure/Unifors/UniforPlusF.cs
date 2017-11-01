// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlusF.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Команда постредактуры: очистить результат расформатирования
    // от RTF-конструкций – &uf('+F
    // Вид функции: +F.
    // Назначение: Команда постредактуры: очистить результат
    // расформатирования от RTF-конструкций.
    // Имеет смысл использовать один раз в любом месте формата.
    // Присутствует в версиях ИРБИС с 2004.1.
    // Формат (передаваемая строка):
    // +F
    //
    // Пример:
    //
    // &unifor('+F')
    //

    static class UniforPlusF
    {
        #region Public methods

        /// <summary>
        /// Post processing: cleanup RTF markup.
        /// </summary>
        public static void CleanRtf
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            context.GetRootContext().PostProcessing |= PftCleanup.Rtf;
        }

        #endregion
    }
}
