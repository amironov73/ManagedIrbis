/* CardStatusWrapper.cs
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace CardFixer
{
    [PublicAPI]
    internal sealed class CardStatusWrapper
    {
        #region Properties

        public CardStatus Value { get; set; }

        public string Description { get; set; }

        #endregion

        #region Construction

        public CardStatusWrapper
            (
                CardStatus value, 
                string description
            )
        {
            Value = value;
            Description = description;
        }

        #endregion

        #region Public methods

        public static CardStatusWrapper[] GetValues()
        {
            return new []
            {
                new CardStatusWrapper
                    (
                        CardStatus.InitialState, 
                        "Не разобранные"
                    ), 
                new CardStatusWrapper
                    (
                        CardStatus.AlreadyHave, 
                        "Уже есть в каталоге"
                    ), 
                new CardStatusWrapper
                    (
                        CardStatus.WrittenOff, 
                        "Все экземпляры списаны"
                    ), 
                new CardStatusWrapper
                    (
                        CardStatus.Problem, 
                        "Проблемные"
                    ), 
                new CardStatusWrapper
                    (
                        CardStatus.NeedToEnter, 
                        "На ввод"
                    ), 
                new CardStatusWrapper
                    (
                        CardStatus.Done, 
                        "Ввведены"
                    ), 
            };
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
