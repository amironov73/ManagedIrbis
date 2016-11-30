// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CommandCode.cs -- command codes for protocol
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
    /// Command codes for protocol.
    /// </summary>
    [PublicAPI]
    public static class CommandCode
    {
        /// <summary>
        /// Получение признака монопольной блокировки базы данных.
        /// </summary>
        public const string ExclusiveDatabaseLock = "#";

        /// <summary>
        /// Получение списка удаленных, неактуализированных
        /// и заблокированных записей.
        /// </summary>
        public const string RecordList = "0";

        /// <summary>
        /// Получение версии сервера.
        /// </summary>
        public const string ServerInfo = "1";

        /// <summary>
        /// Получение статистики по базе данных.
        /// </summary>
        public const string DatabaseStat = "2";

        /// <summary>
        /// IRBIS_FORMAT_ISO_GROUP.
        /// </summary>
        public const string FormatIsoGroup = "3";

        /// <summary>
        /// ???
        /// </summary>
        public const string UnknownCommand4 = "4";

        /// <summary>
        /// Глобальная корректировка.
        /// </summary>
        /// <remarks>IRBIS_GBL</remarks>
        public const string GlobalCorrection = "5";

        /// <summary>
        /// Сохранение группы записей.
        /// </summary>
        public const string SaveRecordGroup = "6";

        /// <summary>
        /// Печать.
        /// </summary>
        public const string Print = "7";

        /// <summary>
        /// Запись параметров в ini-файл, расположенный на сервере.
        /// </summary>
        public const string UpdateIniFile = "8";

        /// <summary>
        /// IRBIS_IMPORT_ISO.
        /// </summary>
        public const string ImportIso = "9";

        /// <summary>
        /// Регистрация клиента на сервере.
        /// </summary>
        /// <remarks>IRBIS_REG</remarks>
        public const string RegisterClient = "A";

        /// <summary>
        /// Разрегистрация клиента.
        /// </summary>
        /// <remarks>IRBIS_UNREG</remarks>
        public const string UnregisterClient = "B";

        /// <summary>
        /// Чтение записи, ее расформатирование.
        /// </summary>
        /// <remarks>IRBIS_READ</remarks>
        public const string ReadRecord = "C";

        /// <summary>
        /// Сохранение записи.
        /// </summary>
        /// <remarks>IRBIS_UPDATE</remarks>
        public const string UpdateRecord = "D";

        /// <summary>
        /// Разблокировка записи.
        /// </summary>
        /// <remarks>IRBIS_RUNLOCK</remarks>
        public const string UnlockRecord = "E";

        /// <summary>
        /// Актуализация записи.
        /// </summary>
        /// <remarks>IRBIS_RECIFUPDATE</remarks>
        public const string ActualizeRecord = "F";

        /// <summary>
        /// Форматирование записи или группы записей.
        /// </summary>
        /// <remarks>IRBIS_SVR_FORMAT</remarks>
        public const string FormatRecord = "G";

        /// <summary>
        /// Получение терминов и ссылок словаря, форматирование записей
        /// </summary>
        /// <remarks>IRBIS_TRM_READ</remarks>
        public const string ReadTerms = "H";

        /// <summary>
        /// Получение ссылок для термина (списка терминов).
        /// </summary>
        /// <remarks>IRBIS_POSTING</remarks>
        public const string ReadPostings = "I";

        /// <summary>
        /// Глобальная корректировка виртуальной записи.
        /// </summary>
        /// <remarks>IRBIS_GBL_RECORD</remarks>
        public const string CorrectVirtualRecord = "J";

        /// <summary>
        /// Поиск записей с опциональным форматированием
        /// (также последовательный поиск).
        /// </summary>
        /// <remarks>IRBIS_SEARCH</remarks>
        public const string Search = "K";

        /// <summary>
        /// Получение/сохранение текстового файла, расположенного
        /// на сервере (группы текстовых файлов).
        /// </summary>
        public const string ReadDocument = "L";

        /// <summary>
        /// IRBIS_BACKUP.
        /// </summary>
        public const string Backup = "M";

        /// <summary>
        /// Пустая операция. Периодическое подтверждение
        /// соединения с сервером.
        /// </summary>
        /// <remarks>IRBIS_NOOP</remarks>
        public const string Nop = "N";

        /// <summary>
        /// Получение максимального MFN для базы данных.
        /// </summary>
        /// <remarks>IRBIS_MAXMFN</remarks>
        public const string GetMaxMfn = "O";

        /// <summary>
        /// Получение терминов и ссылок словаря в обратном порядке.
        /// </summary>
        public const string ReadTermsReverse = "P";

        /// <summary>
        /// Разблокирование записей.
        /// </summary>
        public const string UnlockRecords = "Q";

        /// <summary>
        /// Полнотекстовый поиск.
        /// </summary>
        /// <remarks>IRBIS_FULLTEXT_SEARCH</remarks>
        public const string FullTextSearch = "R";

        /// <summary>
        /// Опустошение базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_EMPTY</remarks>
        public const string EmptyDatabase = "S";

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_NEW</remarks>
        public const string CreateDatabase = "T";

        /// <summary>
        /// Разблокирование базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_UNLOCK</remarks>
        public const string UnlockDatabase = "U";

        /// <summary>
        /// Чтение ссылок для заданного MFN.
        /// </summary>
        /// <remarks>IRBIS_MFN_POSTINGS</remarks>
        public const string GetRecordPostings = "V";

        /// <summary>
        /// Удаление базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_DELETE</remarks>
        public const string DeleteDatabase = "W";

        /// <summary>
        /// Реорганизация мастер-файла.
        /// </summary>
        /// <remarks>IRBIS_RELOAD_MASTER</remarks>
        public const string ReloadMasterFile = "X";

        /// <summary>
        /// Реорганизация словаря.
        /// </summary>
        /// <remarks>IRBIS_RELOAD_DICT</remarks>
        public const string ReloadDictionary = "Y";

        /// <summary>
        /// Создание поискового словаря заново.
        /// </summary>
        /// <remarks>IRBIS_CREATE_DICT</remarks>
        public const string CreateDictionary = "Z";

        /// <summary>
        /// Получение статистики работы сервера.
        /// </summary>
        /// <remarks>IRBIS_STAT</remarks>
        public const string GetServerStat = "+1";

        /// <summary>
        /// ???
        /// </summary>
        public const string UnknownCommandPlus2 = "+2";

        /// <summary>
        /// Получение списка запущенных процессов.
        /// </summary>
        public const string GetProcessList = "+3";

        /// <summary>
        /// ???
        /// </summary>
        public const string UnknownCommandPlus4 = "+4";

        /// <summary>
        /// ???
        /// </summary>
        public const string UnknownCommandPlus5 = "+5";

        /// <summary>
        /// ???
        /// </summary>
        public const string UnknownCommandPlus6 = "+6";

        /// <summary>
        /// Сохранение списка пользователей.
        /// </summary>
        public const string SetUserList = "+7";

        /// <summary>
        /// Перезапуск сервера.
        /// </summary>
        public const string RestartServer = "+8";

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        public const string GetUserList = "+9";

        /// <summary>
        /// Получение списка файлов на сервере.
        /// </summary>
        public const string ListFiles = "!";

    }
}
