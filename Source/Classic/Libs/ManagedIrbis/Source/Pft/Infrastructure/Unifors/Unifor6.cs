// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor6.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выполнить формат – &uf('6
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

    //
    // 2015.1:
    // http://irbis.gpntb.ru/read.php?3,94172
    //
    // Расширена возможность форматного выхода &uf('6<имя_формата>')
    // - новая конструкция имеет вид: 
    //
    // &uf('6<имя_шаблон-формата>#<параметр1>,<параметр2>,...<параметрN>')
    //
    // где: 
    // <параметр1>,<параметр2>,...<параметрN> - список значений переменных параметров
    //
    // Шаблон-формат - формат, содержащий переменные параметры в виде %N - где
    // N - номер параметра в списке значений
    //
    // В качестве переменных параметров шаблон-формата могут выступать
    // ЛЮБЫЕ конструкции языка форматирования(метки полей/подполей,
    // литералы и т.д.)
    //
    // Данная конструкция &uf('6... позволяет сократить тексты
    // форматов - например, в тех случаях, когда для вывода разных
    // элементов данных используются идентичные конструкции.
    //
    // Пример (упрощенный). 
    // Для вывода сведения об индивидуальной ответственности (авторов)
    // из различных полей (700, 701, 702, 330 и т.д.) используются
    // идентичные конструкции, отличающиеся только значением метки поля.
    // Поступаем следующим образом.
    // Создаем шаблон-формат AUTHOR, в котором в качестве метки
    // используем переменный параметр %1 
    //
    // (if p(v%1) then |A=|v%1^a,| |v%1^d,|, |v%1^g,if a(v%1^g) then |, |d%1^b,if v%1^b:'. 'or(not(v%1^b:'.')) then v%1^b else &unifor('G0.'v%1^b),'. '&unifor('G2.'v%1^b) fi fi,if &uf('Ag700#1')='1' then else if s(v%1^1, v%1^c, v%1^f)<>''then' (',v%1^1,if s(v%1^1)<>''then| ; |d%1^c fi, v%1^c,if s(v%1^1, v%1^c)<>''then| ; |d%1^f fi, v%1^f,')' fi,|\|v%1^4*4,|, |v%1^5*4,|, |v%1^6*4,|(|v%1^7|)|,|\|d%1^4 fi fi,|%|d%1/) 
    //
    // Теперь для вывода сведений из конкретного поля используем конструкции 
    // &uf('6author#700')
    // или 
    // &uf('6author#701')
    // и т.д.
    //

    static class Unifor6
    {
        #region Public methods

        public static void ExecuteNestedFormat
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            //
            // TODO some caching
            //

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            string fileName = navigator.ReadUntil('#');
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            List<string> parameters = new List<string>();
            if (navigator.ReadChar() == '#')
            {
                while (!navigator.IsEOF)
                {
                    string item = navigator.ReadUntil(',');
                    parameters.Add(item);
                    navigator.ReadChar();
                }
            }

            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
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

            if (parameters.Count != 0)
            {
                if (parameters.Count == 1)
                {
                    string fromText = "v1";
                    string toText = parameters.First();
                    source = source.Replace(fromText, toText);
                }
                else
                {
                    StringBuilder builder = new StringBuilder(source);
                    int index = 1;
                    foreach (string parameter in parameters)
                    {
                        string fromText = "v" + index.ToInvariantString();
                        string toText = parameter;
                        builder.Replace(fromText, toText);
                        index++;
                    }
                    source = builder.ToString();
                }
            }

            PftProgram program = PftUtility.CompileProgram(source);
            program.Execute(context);
        }

        #endregion
    }
}
