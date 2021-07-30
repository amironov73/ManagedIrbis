// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Technology.cs -- информация о создании и внесении модификаций в запись
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация о создании и внесении изменений в библиографическую запись.
    /// Поле 907.
    /// </summary>
    public sealed class Technology
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 907;

        #endregion

        #region Properties

        /// <summary>
        /// Этап работы, подполе C. См. <see cref="WorkPhase"/>.
        /// </summary>
        [SubField('c')]
        public string Phase { get; set; }

        /// <summary>
        /// Дата, подполе A.
        /// </summary>
        [SubField('a')]
        public string Date { get; set; }

        /// <summary>
        /// Ответственное лицо, ФИО.
        /// </summary>
        [SubField('b')]
        public string Responsible { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение информации к полю.
        /// </summary>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', Date)
                .ApplySubField('b', Responsible)
                .ApplySubField('c', Phase);
        }

        /// <summary>
        /// Получение даты последней модификации записи.
        /// </summary>
        [CanBeNull]
        public static string GetLatestDate
            (
                [NotNull] MarcRecord record
            )
        {
            string result = null;

            foreach (var field in record.Fields.GetField(Tag))
            {
                var candidate = field.GetFirstSubFieldValue('a');
                if (!string.IsNullOrEmpty(candidate))
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result = candidate;
                    }
                    else
                    {
                        result = string.CompareOrdinal(result, candidate) < 0
                            ? candidate
                            : result;
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// Разбор записи <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static Technology[] ParseRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            var result = new List<Technology>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    var tech = ParseField(field);
                    result.Add(tech);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор заданного поля <see cref="RecordField"/>.
        /// </summary>
        [NotNull]
        public static Technology ParseField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            var result = new Technology
            {
                Date = field.GetFirstSubFieldValue('a'),
                Responsible = field.GetFirstSubFieldValue('b'),
                Phase = field.GetFirstSubFieldValue('c')
            };

            return result;
        }

        /// <summary>
        /// Преобразование информации в поле.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            var result = new RecordField(Tag)
                .AddNonEmptySubField('a', Date)
                .AddNonEmptySubField('b', Responsible)
                .AddNonEmptySubField('c', Phase);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Date = reader.ReadNullableString();
            Responsible = reader.ReadNullableString();
            Phase = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Date)
                .WriteNullable(Responsible)
                .WriteNullable(Phase);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Technology>(this, throwOnError);

            verifier.Assert
            (
                !string.IsNullOrEmpty(Date)
                || !string.IsNullOrEmpty(Responsible)
                || !string.IsNullOrEmpty(Phase)
            );

            return verifier.Result;
        }

        #endregion


        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return string.Format("{0}: {1}: {2}", Phase, Date, Responsible);
        }

        #endregion

    } // class Technology

} // namespace ManagedIrbis.Fields
