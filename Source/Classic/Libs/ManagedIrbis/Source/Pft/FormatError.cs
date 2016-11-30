// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FormatError.cs -- format error codes.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Format error codes.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FormatError
    {
        #region Constants

        #endregion

        #region Nested classes

        class Pair
        {
            public int Code { get; set; }

            public string Message { get; set; }
        }

        #endregion

        #region Private members

        private static Pair[] _knownCodes =
        {
            new Pair {Code = 1, Message = "Обнаружен конец формата " +
                "в процессе обработки повторяющейся группы. Возможно, " +
                "пропущена закрывающая скобка повторяющейся группы." },
            new Pair {Code = 2, Message = "Вложенность повторяющейся " +
                "группы (т.е. одна повторяющаяся группа расположена " +
                "внутри другой повторяющейся группы)."},
            new Pair {Code = 8, Message = "Команда IF без THEN." },
            new Pair {Code = 19, Message = "Непарная открывающаяся " +
                "скобка (." },
            new Pair {Code = 20, Message = "Непарная закрывающаяся " +
                "скобка ). Также может быть вызвано наличием " +
                "неправильного операнда в выражении."},
            new Pair {Code = 26, Message = "Два операнда различных " +
                "типов в одном операторе (например, попытка сложить " +
                "строковый операнд с числом)."},
            new Pair {Code = 28, Message = "Первый аргумент функции " +
                "REF - нечисловое выражение."},
            new Pair {Code = 51, Message = "Слишком много литералов " +
               "и/или условных команд связано с командой вывода поля."},
            new Pair {Code = 53, Message = "IF команда не завершена " +
               "ключевым словом FI."},
            new Pair {Code = 54, Message = "Знак + не соответствует " +
               "контексту: CDS/ISIS предполагает наличие " +
               "повторяющегося литерала за знаком +."},
            new Pair {Code = 55, Message = "Непарное ключевое слово FI."},
            new Pair {Code = 56, Message = "Переполнение рабочей " +
                "области: формат создает слишком большой выходной " +
                "текст, который система не может обработать."},
            new Pair {Code = 57, Message = "Зацикливание повторяющейся" +
                " группы"},
            new Pair {Code =58,Message = "Один или более аргументов " +
                "функции F - нечисловые выражения."},
            new Pair {Code =60, Message = "Нестроковая функция " +
                "используется как команда (только строковые функции " +
                "могут быть использованы в качестве команды)."},
            new Pair {Code =61, Message = "Аргумент функции A или Р " +
                "- не команда вывода поля."},
            new Pair {Code =99, Message = "Неизвестная команда " +
                "(например, ошибка в правильности написания имени " +
                "функции или команды), возможен также пропуск " +
                "закрывающего ограничителя литерала."},
            new Pair {Code =101, Message = "Переполнение стека " +
                "(возможно из-за наличия слишком сложного выражения)."},
            new Pair {Code =102, Message = "Некорректная работа " +
                "со стеком (может быть из-за непарной открывающей " +
                "скобки)."}
        };

        #endregion
    }
}
