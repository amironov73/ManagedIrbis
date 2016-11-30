// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisReturnCode.cs -- return code of IRBIS64 server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Коды возвратов из Irbis64.
    /// </summary>
    [PublicAPI]
    public enum IrbisReturnCode
    // : int (implied)
    {
        /// <summary>
        /// Успешное завершение, нет ошибки.
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Успешное завершение, нет ошибки.
        /// </summary>
        Zero = 0,

        /// <summary>
        /// Прервано пользователем или общая ошибка.
        /// </summary>
        UserError = -1,

        /// <summary>
        /// Не завершена обработка предыдущего запроса.
        /// </summary>
        Busy = -2,

        /// <summary>
        /// Неизвестная ошибка.
        /// </summary>
        Unknown = -3,

        /// <summary>
        /// Выходной буфер мал.
        /// </summary>
        BadBufferSize = -4,

        /// <summary>
        /// Заданный MFN вне пределов БД.
        /// </summary>
        WrongMfn = -140,

        /// <summary>
        /// Нет запрошенного значения в поисковом индексе.
        /// </summary>
        TermNotExist = -202,

        /// <summary>
        /// Была считана последняя запись в поисковом индексе.
        /// </summary>
        LastTermInList = -203,

        /// <summary>
        /// Возвращена первая подходящая запись в поисковом
        /// индексе вместо запрошенного значения.
        /// </summary>
        FirstTermInList = -204,

        /// <summary>
        /// Монопольная блокировка БД.
        /// </summary>
        DatabaseLocked = -300,

        /// <summary>
        /// Запись заблокирована на ввод.
        /// </summary>
        RecordLocked = -602,

        /// <summary>
        /// Запись логически удалена.
        /// </summary>
        RecordDeleted = -603,

        /// <summary>
        /// Запись физически удалена.
        /// </summary>
        PhysicallyDeleted = -605,

        /// <summary>
        /// При записи обнаружено несоответствие версий.
        /// </summary>
        VersionError = -608,

        /// <summary>
        /// Ошибка в клиентском файле формата.
        /// </summary>
        ClientFormatError = 999,

        /// <summary>
        /// Ошибка выполнения на сервере.
        /// </summary>
        ServerExecutionError = -1111,

        /// <summary>
        /// Несоответствие полученной и реальной длины.
        /// </summary>
        AnswerLengthError = -1112,

        /// <summary>
        /// Неверный протокол.
        /// </summary>
        WrongProtocol = -2222,

        /// <summary>
        /// Незарегистрированный клиент.
        /// </summary>
        ClientNotInList = -3333,

        /// <summary>
        /// Клиент не выполнил регистрацию.
        /// </summary>
        ClientNotInUse = -3334,

        /// <summary>
        /// Неправльный идентификатор клиента.
        /// </summary>
        WrongClientIdentifier = -3335,

        /// <summary>
        /// Зарегистрировано максимально допустимое 
        /// количество клиентов.
        /// </summary>
        ClientListOverload = -3336,

        /// <summary>
        /// Клиент уже зарегистрирован.
        /// </summary>
        ClientAlreadyExist = -3337,

        /// <summary>
        /// Нет доступа к командам АРМ.
        /// </summary>
        ClientNotAllowed = -3338,

        /// <summary>
        /// Неверный пароль.
        /// </summary>
        WrongPassword = -4444,

        /// <summary>
        /// Файл не существует.
        /// </summary>
        FileNotExist = -5555,

        /// <summary>
        /// Сервер перегружен: достигнуто максимальное число
        /// потоков обработки.
        /// </summary>
        ServerOverload = -6666,

        /// <summary>
        /// Не удалось запустить или прервать поток или процесс.
        /// </summary>
        ProcessError = -7777,

        /// <summary>
        /// Обрушение при выполнении глобальной корректировки.
        /// </summary>
        GlobalError = -8888,
    }
}
