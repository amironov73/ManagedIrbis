/* Card.cs
 */

#region Using directives

using System.IO;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using CM = System.Configuration.ConfigurationManager;

#endregion

namespace CardFixer
{
    [PublicAPI]
    [TableName("cards")]
    public class Card
    {
        #region Properties

        [MapField("id")]
        [PrimaryKey]
        public string Id { get; set; }

        [MapField("boxnumber")]
        public int BoxNumber { get; set; }

        [MapField("status")]
        public CardStatus Status { get; set; }

        [MapField("path")]
        public string Path { get; set; }

        [MapField("operator")]
        public string Operator { get; set; }

        [MapField("remark")]
        public string Remark { get; set; }

        [MapIgnore]
        public string FullPath { get; set; }

        #endregion

        #region Public methods

        public static string CalculateFullPath
            (
                Card card
            )
        {
            string cardPath = CM.AppSettings["cards-path"];
            string cardExtension 
                = CM.AppSettings["cards-extension"];
            string result = System.IO.Path.Combine
                (
                    cardPath,
                    card.Path,
                    card.Id + cardExtension
                );
            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Id;
        }

        #endregion
    }
}
