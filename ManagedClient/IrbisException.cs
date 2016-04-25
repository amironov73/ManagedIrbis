/* IrbisException.cs -- исключения, специфичные для библиотеки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Исключения, специфичные для библиотеки
    /// ManagedClient
    /// </summary>
    [PublicAPI]
    [Serializable]
    //[MoonSharpUserData]
    [DebuggerDisplay("Code={Code}, Message={Message}")]
    public class IrbisException
        : ApplicationException
    {
        #region Properties

        /// <summary>
        /// Код возврата (код ошибки)
        /// </summary>
        public int ErrorCode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public IrbisException()
        {
        }

        /// <summary>
        /// Конструктор с кодом ошибки.
        /// </summary>
        public IrbisException
            (
                int returnCode
            )
            : base(GetErrorDescription(returnCode))
        {
            ErrorCode = returnCode;
        }

        /// <summary>
        /// Конструктор с готовым сообщением
        /// об ошибке.
        /// </summary>
        public IrbisException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Конструктор для десериализации.
        /// </summary>
        public IrbisException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Текстовое описание ошибки.
        /// </summary>
        [NotNull]
        public static string GetErrorDescription
            (
                [NotNull] IrbisException exception
            )
        {
            Code.NotNull(exception, "exception");

            return string.IsNullOrEmpty(exception.Message)
                ? GetErrorDescription(exception.ErrorCode)
                : exception.Message;
        }


        /// <summary>
        /// Текстовое описание ошибки.
        /// </summary>
        [NotNull]
        public static string GetErrorDescription
            (
                IrbisReturnCode code
            )
        {
            return GetErrorDescription((int)code);
        }

        /// <summary>
        /// Текстовое описание ошибки.
        /// </summary>
        [NotNull]
        public static string GetErrorDescription
            (
                int code
            )
        {
            string result = "Неизвестная ошибка";

            if (code > 0)
            {
                result = "Нет ошибки";
            }
            else
            {
                switch (code)
                {
                    case 0:
                        result = "Нормальное завершение";
                        break;
                    case -100:
                        result = "Заданный MFN вне пределов БД";
                        break;
                    case -101:
                        result = "Ошибочный размер полки";
                        break;
                    case -102:
                        result = "Ошибочный номер полки";
                        break;
                    case -140:
                        result = "MFN вне пределов БД";
                        break;
                    case -141:
                        result = "Ошибка чтения";
                        break;
                    case -200:
                        result = "Указанное поле отсутствует";
                        break;
                    case -201:
                        result = "Предыдущая версия записи отсутствует";
                        break;
                    case -202:
                        result = "Заданный термин не найден (термин не существует)";
                        break;
                    case -203:
                        result = "Последний термин в списке";
                        break;
                    case -204:
                        result = "Первый термин в списке";
                        break;
                    case -300:
                        result = "База данных монопольно заблокирована";
                        break;
                    case -301:
                        result = "База данных монопольно заблокирована";
                        break;
                    case -400:
                        result = "Ошибка при открытии файлов MST или XRF (ошибка файла данных)";
                        break;
                    case -401:
                        result = "Ошибка при открытии файлов IFP (ошибка файла индекса)";
                        break;
                    case -402:
                        result = "Ошибка при записи";
                        break;
                    case -403:
                        result = "Ошибка при актуализации";
                        break;
                    case -600:
                        result = "Запись логически удалена";
                        break;
                    case -601:
                        result = "Запись физически удалена";
                        break;
                    case -602:
                        result = "Запись заблокирована на ввод";
                        break;
                    case -603:
                        result = "Запись логически удалена";
                        break;
                    case -605:
                        result = "Запись физически удалена";
                        break;
                    case -607:
                        result = "Ошибка autoin.gbl";
                        break;
                    case -608:
                        result = "Ошибка версии записи";
                        break;
                    case -700:
                        result = "Ошибка создания резервной копии";
                        break;
                    case -701:
                        result = "Ошибка восстановления из резервной копии";
                        break;
                    case -702:
                        result = "Ошибка сортировки";
                        break;
                    case -703:
                        result = "Ошибочный термин";
                        break;
                    case -704:
                        result = "Ошибка создания словаря";
                        break;
                    case -705:
                        result = "Ошибка загрузки словаря";
                        break;
                    case -800:
                        result = "Ошибка в параметрах глобальной корректировки";
                        break;
                    case -801:
                        result = "ERR_GBL_REP";
                        break;
                    case -802:
                        result = "ERR_GBL_MET";
                        break;
                    case -1111:
                        result = "Ошибка исполнения сервера (SERVER_EXECUTE_ERROR)";
                        break;
                    case -2222:
                        result = "Ошибка в протоколе (WRONG_PROTOCOL)";
                        break;
                    case -3333:
                        result = "Незарегистрированный клиент (ошибка входа на сервер) (клиент не в списке)";
                        break;
                    case -3334:
                        result = "Клиент не выполнил вход на сервер (клиент не используется)";
                        break;
                    case -3335:
                        result = "Неправильный уникальный идентификатор клиента";
                        break;
                    case -3336:
                        result = "Нет доступа к командам АРМ";
                        break;
                    case -3337:
                        result = "Клиент уже зарегистрирован";
                        break;
                    case -3338:
                        result = "Недопустимый клиент";
                        break;
                    case -4444:
                        result = "Неверный пароль";
                        break;
                    case -5555:
                        result = "Файл не существует";
                        break;
                    case -6666:
                        result = "Сервер перегружен. Достигнуто максимальное число потоков обработки";
                        break;
                    case -7777:
                        result = "Не удалось запустить/прервать поток администратора (ошибка процесса)";
                        break;
                    case -8888:
                        result = "Общая ошибка";
                        break;
                }
            }

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "ErrorCode: {2}{1}Description: {3}{1}{0}",
                    base.ToString(),
                    Environment.NewLine,
                    ErrorCode,
                    Message
                );
        }

        #endregion
    }
}
