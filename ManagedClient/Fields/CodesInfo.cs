/* CodesInfo.cs -- коды (поле 900)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedClient.Mapping;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Коды (поле 900).
    /// </summary>
    public sealed class CodesInfo
    {
        #region Properties

        /// <summary>
        /// Тип документа. Подполе t.
        /// </summary>
        [SubField('t')]
        public string DocumentType { get; set; }

        /// <summary>
        /// Вид документа. Подполе b.
        /// </summary>
        [SubField('b')]
        public string DocumentKind { get; set; }

        /// <summary>
        /// Характер документа. Подполе c.
        /// </summary>
        [SubField('c')]
        public string DocumentCharacter1 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 2.
        /// </summary>
        [SubField('2')]
        public string DocumentCharacter2 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 3.
        /// </summary>
        [SubField('3')]
        public string DocumentCharacter3 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 4.
        /// </summary>
        [SubField('4')]
        public string DocumentCharacter4 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 5.
        /// </summary>
        [SubField('5')]
        public string DocumentCharacter5 { get; set; }

        /// <summary>
        /// Характер документа. Подполе 6.
        /// </summary>
        [SubField('6')]
        public string DocumentCharacter6 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе x.
        /// </summary>
        [SubField('7')]
        public string PurposeCode1 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе y.
        /// </summary>
        [SubField('y')]
        public string PurposeCode2 { get; set; }

        /// <summary>
        /// Код целевого назначения. Подполе 9.
        /// </summary>
        [SubField('9')]
        public string PurposeCode3 { get; set; }

        /// <summary>
        /// Возрастные ограничения. Подполе z.
        /// </summary>
        [SubField('z')]
        public string AgeRestrictions { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static CodesInfo Parse
            (
                RecordField field
            )
        {
            CodesInfo result = new CodesInfo
                {
                    DocumentType = field.GetFirstSubFieldValue('t'),
                    DocumentKind = field.GetFirstSubFieldValue('b'),
                    DocumentCharacter1 = field.GetFirstSubFieldValue('c'),
                    DocumentCharacter2 = field.GetFirstSubFieldValue('2'),
                    DocumentCharacter3 = field.GetFirstSubFieldValue('3'),
                    DocumentCharacter4 = field.GetFirstSubFieldValue('4'),
                    DocumentCharacter5 = field.GetFirstSubFieldValue('5'),
                    DocumentCharacter6 = field.GetFirstSubFieldValue('6'),
                    PurposeCode1 = field.GetFirstSubFieldValue('x'),
                    PurposeCode2 = field.GetFirstSubFieldValue('y'),
                    PurposeCode3 = field.GetFirstSubFieldValue('9'),
                    AgeRestrictions = field.GetFirstSubFieldValue('z')
                };

            return result;
        }

        /// <summary>
        /// Transform back to field.
        /// </summary>
        public RecordField ToField()
        {
            RecordField result = new RecordField("900")
                .AddNonEmptySubField('t', DocumentType)
                .AddNonEmptySubField('b', DocumentKind)
                .AddNonEmptySubField('c', DocumentCharacter1)
                .AddNonEmptySubField('2', DocumentCharacter2)
                .AddNonEmptySubField('3', DocumentCharacter3)
                .AddNonEmptySubField('4', DocumentCharacter4)
                .AddNonEmptySubField('5', DocumentCharacter5)
                .AddNonEmptySubField('6', DocumentCharacter6)
                .AddNonEmptySubField('x', PurposeCode1)
                .AddNonEmptySubField('y', PurposeCode2)
                .AddNonEmptySubField('9', PurposeCode3)
                .AddNonEmptySubField('z', AgeRestrictions);

            return result;
        }

        #endregion
    }
}
