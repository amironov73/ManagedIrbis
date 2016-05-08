/* IrbisInputMode.cs -- input mode constants
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedClient
{
    /// <summary>
    /// Режимы ввода
    /// </summary>
    public enum IrbisInputMode
    {
        /// <summary>
        /// Ввод без дополнительной обработки.
        /// Простая строка ввода
        /// </summary>
        Default = 0,

        /// <summary>
        /// Ввод через простое (не иерархическое)
        /// меню (справочник).
        /// </summary>
        Menu = 1,

        /// <summary>
        /// Ввод через поисковый словарь.
        /// </summary>
        Dictionary = 2,

        /// <summary>
        /// Ввод через рубрикатор ГРНТИ.
        /// </summary>
        Rubricator = 3,

        /// <summary>
        /// Ввод через оконный редактор
        /// </summary>
        Multiline = 4,

        /// <summary>
        /// Ввод через вложенный рабочий лист.
        /// </summary>
        SubSheet = 5,

        /// <summary>
        /// Ввод через иерархический справочник
        /// </summary>
        Tree = 6,

        /// <summary>
        /// Ввод с использованием переключателей.
        /// </summary>
        Switch = 7,

        /// <summary>
        /// Ввод с использованием внешней программы.
        /// </summary>
        ExternalProgram = 8,

        /// <summary>
        /// Ввод на основе маски (шаблона).
        /// </summary>
        Template = 9,

        /// <summary>
        /// Ввод через авторитетный файл.
        /// </summary>
        AuthorityFile = 10,

        /// <summary>
        /// Ввод через тезаурус.
        /// </summary>
        Thesaurus = 11,

        /// <summary>
        /// Ввод через обращение к внешнему файлу.
        /// </summary>
        ExternalFile = 12,

        /// <summary>
        /// Ввод на основе ИРБИС-Навигатора.
        /// </summary>
        Navigator = 13,

        /// <summary>
        /// Ввод с помощью режима (функции) пользователя.
        /// </summary>
        ExternalDll = 14
    }
}
