// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblCode.cs -- command codes for GBL files
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Command codes for GBL files.
    /// </summary>
    public static class GblCode
    {
        #region Constants

        /// <summary>
        /// Добавление нового повторения поля или подполя
        /// в заданное существующее поле.
        /// </summary>
        public const string Add = "ADD";

        /// <summary>
        /// Удаляет поле или подполе в поле.
        /// </summary>
        public const string Delete = "DEL";

        /// <summary>
        /// Замена целиком поля или подполя.
        /// </summary>
        public const string Replace = "REP";

        /// <summary>
        /// Замена данных в поле или подполе.
        /// </summary>
        public const string Change = "CHA";

        /// <summary>
        /// Замена данных в поле или подполе.
        /// </summary>
        public const string ChangeCase = "CHAC";

        /// <summary>
        /// Удаление записи в целом.
        /// </summary>
        public const string DeleteRecord = "DELR";

        /// <summary>
        /// Восстановление записи в целом.
        /// </summary>
        public const string UndeleteRecord = "UNDELR";

        /// <summary>
        /// Из текущей записи, вызывает на корректировку другие записи, 
        /// отобранные по поисковым терминам  из текущей или другой, 
        /// доступной в системе, базы данных.
        /// </summary>
        public const string CorrectRecords = "CORREC";

        /// <summary>
        /// Создает новую запись в текущей или другой базе данных.
        /// </summary>
        public const string CreateRecord = "NEWMFN";

        /// <summary>
        /// Очищает (опустошает) текущую запись.
        /// </summary>
        public const string EmptyRecord = "EMPTY";

        /// <summary>
        /// Переход к одной из предыдущих копий записи (откат).
        /// </summary>
        public const string Undo = "UNDOR";

        /// <summary>
        /// Комментарий. Может находиться между другими операторами 
        /// и содержать любые тексты в строках (до 4-х) после себя.
        /// </summary>
        public const string Comment = "//";

        /// <summary>
        /// Завершает работу с другой базой данных,
        /// установленной в операторах CORREC или NEWREC.
        /// </summary>
        public const string End = "END";

        /// <summary>
        /// Определяет условие выполнения операторов, 
        /// следующих за ним до оператора FI. 
        /// Состоит из двух строк: первая строка – имя оператора IF; 
        /// вторая строка – формат, результатом которого может быть строка ‘1’, 
        /// что означает разрешение на выполнение последующих операторов, 
        /// или любое другое значение, что означает запрет 
        /// на выполнение последующих операторов.
        /// </summary>
        public const string If = "IF";

        /// <summary>
        /// Завершает действие оператора IF. Состоит из одной строки – FI.
        /// </summary>
        public const string Fi = "FI";

        /// <summary>
        /// Оператор можно использовать в группе операторов после операторов 
        /// NEWMFN или CORREC. Он дополняет записи всеми полями текущей записи. 
        /// Т. е. это способ, например, создать новую запись и наполнить 
        /// ее содержимым текущей записи. Или можно вызвать на корректировку 
        /// другую запись (CORREC), очистить ее (EMPTY) и наполнить содержимым 
        /// текущей записи.
        /// </summary>
        public const string All = "ALL";

        /// <summary>
        /// Формирование пользовательского протокола.
        /// </summary>
        public const string PutLog = "PUTLOG";

        /// <summary>
        /// Операторы REPEAT-UNTIL организуют цикл выполнения группы операторов. 
        /// Группа операторов между ними будет выполняться до тех пор, 
        /// пока формат в операторе UNTIL будет давать значение ‘1’.
        /// </summary>
        public const string Repeat = "REPEAT";

        /// <summary>
        /// Второй строкой оператора должен быть формат, который позволяет 
        /// завершить цикл, если результат форматирования на текущей записи 
        /// отличен от ‘1’.
        /// </summary>
        public const string Until = "UNTIL";

        #endregion
    }
}
