// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforI.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть параметр из INI-файла - &uf('I')
    // Вид функции: I.
    // Назначение: Вернуть параметр из INI-файла.
    // Формат (передаваемая строка):
    // I<SECTION>,<PAR_NAME>,<DE-FAULT_VALUE>
    //
    // Пример:
    //
    // &unifor('IPRIVATE,NAME,NONAME')
    //

    static class UniforI
    {
        #region Public methods

        /// <summary>
        /// Get INI-file entry.
        /// </summary>
        public static void GetIniFileEntry
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = StringUtility.SplitString
                    (
                        expression,
                        new[] {','},
                        3
                    );

                if (parts.Length >= 2)
                {
                    string section = parts[0];
                    string parameter = parts[1];
                    string defaultValue = parts.Length > 2
                        ? parts[2]
                        : null;

                    if (!string.IsNullOrEmpty(section)
                        && !string.IsNullOrEmpty(parameter))
                    {
                        string result;
                        using (IniFile iniFile
                            = context.Provider.GetUserIniFile())
                        {
                            result = iniFile.GetValue
                                (
                                    section,
                                    parameter,
                                    defaultValue
                                );
                        }
                        if (!string.IsNullOrEmpty(result))
                        {
                            context.Write(node, result);
                            context.OutputFlag = true;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
