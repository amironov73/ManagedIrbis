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
        /// Ошибка выделения памяти.
        /// </summary>
        MemoryAllocationError = -100,

        /// <summary>
        /// Размер полки меньше размера записи.
        /// </summary>
        ShelfSizeError = -101,

        /// <summary>
        /// Номер полки больше числа полок.
        /// </summary>
        ShelfNumberError = -102,

        /// <summary>
        /// Заданный MFN вне пределов БД.
        /// </summary>
        WrongMfn = -140,

        /// <summary>
        /// Ошибка чтения записи, она требует физического удаления.
        /// </summary>
        ReadRecordError = -141,

        /// <summary>
        /// Заданного поля нет.
        /// isisfldrep irbisfldadd = пустое поле.
        /// </summary>
        FieldNotExist = -200,

        /// <summary>
        /// Нет предыдущей версии записи.
        /// </summary>
        PreviousVersionNotExist = -201,

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
        /// Блокировка ввода - не используется в IRBIS64.
        /// </summary>
        DatabaseLockedForEdit = -301,

        /// <summary>
        /// Ошибка при открытии файла MST или XRF.
        /// </summary>
        OpenMstError = -400,

        /// <summary>
        /// Ошибка при открытии файлов поискового индекса.
        /// </summary>
        OpenIndexError = -401,

        /// <summary>
        /// Ошибка при записи в файл.
        /// </summary>
        WriteError = -402,

        /// <summary>
        /// Ошибка при актуализации.
        /// </summary>
        ActualizationError = -403,

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
        /// Ошибка в Autoin.gbl.
        /// </summary>
        AutoinError = -607,

        /// <summary>
        /// При записи обнаружено несоответствие версий.
        /// </summary>
        VersionError = -608,

        /// <summary>
        /// Ошибка в GUID. Появилась в IRBIS64+.
        /// </summary>
        GuidError = -609,

        /// <summary>
        /// Ошибка при создании страховой копии.
        /// </summary>
        BackupCreationError = -700,

        /// <summary>
        /// Ошибка при восстановлении из страховой копии.
        /// </summary>
        BackupRestoreError = -701,

        /// <summary>
        /// Ошибка при сортировке.
        /// </summary>
        ErrorWhileSorting = -702,

        /// <summary>
        /// Ошибка при отборе терминов словаря.
        /// </summary>
        TermCreationError = -703,

        /// <summary>
        /// Ошибка при разгрузке словаря.
        /// </summary>
        LinkCreationError = -704,

        /// <summary>
        /// Ошибка при загрузке словаря.
        /// </summary>
        LinkLoadError = -705,

        /// <summary>
        /// Количество параметров GBL не число.
        /// </summary>
        GblParameterError = -800,

        /// <summary>
        /// Повторение задано не числом.
        /// </summary>
        GblOccurrenceError = -801,

        /// <summary>
        /// Метка задана не числом.
        /// </summary>
        GblTagError = -802, 

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
