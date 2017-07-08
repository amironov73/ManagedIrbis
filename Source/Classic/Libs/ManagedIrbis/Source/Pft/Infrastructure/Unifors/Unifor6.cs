// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor6.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выполнить формат – &uf('6…
    // Вид функции: 6.
    // Назначение: Выполнить формат.
    // Формат(передаваемая строка):
    // 6<имя файла формата>
    // где<имя файла формата> – имя файла формата,
    // указывается без расширения.
    // Файл формата будет найден по заданному имени,
    // обязательному расширению.pft и местоположению:
    // в папке базы данных, а если там нет,
    // то в папке<IRBIS_SERVER_ROOT>\Deposit.
    //

    static class Unifor6
    {
        #region Private members

        #endregion

        #region Public methods

        public static void ExecuteNestedFormat
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string fileName
            )
        {
            //
            // TODO some caching
            //

            if (!string.IsNullOrEmpty(fileName))
            {

                string ext = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(ext))
                {
                    fileName += ".pft";
                }
                FileSpecification specification
                    = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        context.Provider.Database,
                        fileName
                    );
                string source = context.Provider.ReadFile
                    (
                        specification
                );
                if (string.IsNullOrEmpty(source))
                {
                    return;
                }

                PftProgram program = PftUtility.CompileProgram(source);
                program.Execute(context);
            }
        }

        #endregion
    }
}
