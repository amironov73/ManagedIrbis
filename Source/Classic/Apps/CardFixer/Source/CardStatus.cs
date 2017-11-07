/* CardStatus.cs
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace CardFixer
{
    /// <summary>
    /// Статус карторчи
    /// </summary>
    [PublicAPI]
    public enum CardStatus
    {
        /// <summary>
        /// Начальное состояние.
        /// </summary>
        InitialState = 0,

        /// <summary>
        /// Не вводить, уже есть в каталоге.
        /// </summary>
        AlreadyHave = 1,

        /// <summary>
        /// Не вводить, все экземпляры списаны.
        /// </summary>
        WrittenOff = 2,

        /// <summary>
        /// Не вводить, проблемная карточка.
        /// </summary>
        Problem = 3,

        /// <summary>
        /// Надо вводить.
        /// </summary>
        NeedToEnter = 4,

        /// <summary>
        /// Карточка успешно введена.
        /// </summary>
        Done = 5
    }
}
