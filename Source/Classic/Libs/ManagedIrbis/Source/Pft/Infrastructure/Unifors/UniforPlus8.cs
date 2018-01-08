// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforPlus8.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.PlatformSpecific;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вызвать функцию из внешней DLL – &uf('+8')
    // Вид функции: +8.
    // Назначение: Вызвать функцию из внешней DLL.
    // Присутствует в версиях ИРБИС с 2006.1.
    // Формат (передаваемая строка):
    // +8<имя_DLL>,<имя_функции>,<передаваемые_данные>
    // Внешние функции должны ОБЯЗАТЕЛЬНО иметь следующую структуру:
    // в случае Pascal
    // test_function1(buf1, buf2: Pchar; bufsize: integer): integer; 
    // в случае C
    // int test_function1(char* buf1, char* buf2, int bufsize)
    // где: buf1 – передаваемые данные(входные), buf2 – возвращаемые
    // данные (выходные), bufsize – размер выходного буфера(buf2).
    // В ИРБИС64 данные передаются и возвращаются в UTF8.
    // Возврат функции: 0 – нормальное завершение;
    // любое другое значение – ненормальное.
    // В случае нестандартного вызова функций из DLL (по Pascal-правилам)
    // надо указывать символ* перед именем DLL:
    // &unifor('+8*<имя_DLL>,<имя_функции>,.... 
    // Следует помнить, что имя функции в вызове надо указывать
    // строго в соответствии с тем, как она экспортирована из DLL,
    // большие и маленькие буквы различаются.
    //
    // Примеры:
    //
    // В вызываемую функцию передается заглавие:
    // &unifor('+8test_dll,test_function1,', v200^a)
    //
    // Передаются повторения 910 поля:
    // (&unifor('+8test_dll,test_function2,', v910))
    //
    // Передается вся текущая запись:
    // (&unifor('+8test_dll,test_function2,',&unifor('+0')))
    //

    static class UniforPlus8
    {
        #region Public methods

        public static void ExecuteNativeMethod
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            TextNavigator navigator = new TextNavigator(expression);
            bool winApi = false;
            if (navigator.PeekChar() == '*')
            {
                winApi = true;
                navigator.ReadChar();
            }
            string dllName = navigator.ReadUntil(',');
            if (string.IsNullOrEmpty(dllName))
            {
                return;
            }

            navigator.ReadChar(); // eat the comma
            string methodName = navigator.ReadUntil(',');
            if (string.IsNullOrEmpty(methodName))
            {
                return;
            }

            navigator.ReadChar(); // eat the comma
            string input = navigator.GetRemainingText()
                ?? string.Empty;

            MethodResult result = MethodRunner.RunMethod
                (
                    dllName,
                    methodName,
                    winApi,
                    input
                );

            // TODO: do we need to check return code?
            if (result.ReturnCode == 0)
            {
                context.Write
                    (
                        node,
                        result.Output
                    );
            }
        }

        #endregion
    }
}
