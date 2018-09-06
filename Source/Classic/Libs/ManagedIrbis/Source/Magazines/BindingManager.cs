// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BindingManager.cs -- менеджер подшивок
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;
using AM.Text.Ranges;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Fields;
using ManagedIrbis.Search;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Менеджер подшивок.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BindingManager
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IIrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BindingManager
            (
                [NotNull] IIrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create or update binding according the specification.
        /// </summary>
        public void BindMagazines
            (
                [NotNull] BindingSpecification specification
            )
        {
            Code.NotNull(specification, "specification");

            if (string.IsNullOrEmpty(specification.MagazineIndex)
                || string.IsNullOrEmpty(specification.Year)
                || string.IsNullOrEmpty(specification.IssueNumbers)
                || string.IsNullOrEmpty(specification.Description)
                || string.IsNullOrEmpty(specification.BindingNumber)
                || string.IsNullOrEmpty(specification.Inventory)
                || string.IsNullOrEmpty(specification.Fond)
                || string.IsNullOrEmpty(specification.Complect))
            {
                throw new IrbisException("Empty binding specification");
            }

            NumberRangeCollection collection = NumberRangeCollection.Parse(specification.IssueNumbers);
            string longDescription = string.Format("Подшивка N{0} {1} ({2})", specification.BindingNumber,
                specification.Description, specification.IssueNumbers);
            string bindingIndex = string.Format("{0}/{1}/{2}", specification.MagazineIndex, specification.Year,
                longDescription);

            MarcRecord mainRecord = Connection.ByIndex(specification.MagazineIndex);

            MagazineInfo magazine = MagazineInfo.Parse(mainRecord);

            foreach (NumberText numberText in collection)
            {
                // Создание записей, если их еще нет.
                MarcRecord issueRecord = new MarcRecord
                {
                    Database = Connection.Database
                };

                string issueIndex = string.Format
                    (
                        "{0}/{1}/{2}",
                        specification.MagazineIndex,
                        specification.Year,
                        numberText
                    );
                issueRecord
                    .AddField(933, specification.MagazineIndex)
                    .AddField(903, issueIndex)
                    .AddField(934, specification.Year)
                    .AddField(936, numberText)
                    .AddField(920, "NJP")
                    .AddField
                        (
                            new RecordField(910)
                                .AddSubField('a', "0")
                                .AddSubField('b', specification.Complect)
                                .AddSubField('c', "?")
                                .AddSubField('d', specification.Fond)
                                .AddSubField('p', bindingIndex)
                                .AddSubField('i', specification.Inventory)
                        )
                    .AddField
                        (
                            new RecordField(463)
                                .AddSubField('w', bindingIndex)
                        );

                Connection.WriteRecord(issueRecord);
            }

            // Создание записи подшивки, если ее еще нет
            MarcRecord bindingRecord = new MarcRecord
            {
                Database = Connection.Database
            };

            bindingRecord
                .AddField(933, specification.MagazineIndex)
                .AddField(903, bindingIndex)
                .AddField(904, specification.Year)
                .AddField(936, longDescription)
                .AddField(931, specification.IssueNumbers)
                .AddField(920, "NJK")
                .AddField
                    (
                        new RecordField(910)
                            .AddSubField('a', "0")
                            .AddSubField('b', specification.Inventory)
                            .AddSubField('c', "?")
                            .AddSubField('d', specification.Fond)
                    );

            Connection.WriteRecord(bindingRecord);

            // Обновление кумуляции
            mainRecord.AddField
                (
                    new RecordField(909)
                        .AddSubField('q', specification.Year)
                        .AddSubField('d', specification.Fond)
                        .AddSubField('k', specification.Complect)
                        .AddSubField('h', specification.IssueNumbers)
                );
            Connection.WriteRecord(mainRecord);
        }

        /// <summary>
        /// Расшитие и удаление подшивки по ее индексу.
        /// </summary>
        public void Unbind
            (
                [NotNull] string bindingIndex
            )
        {
            Code.NotNullNorEmpty(bindingIndex, "bindingIndex");
        }

        #endregion
    }
}
