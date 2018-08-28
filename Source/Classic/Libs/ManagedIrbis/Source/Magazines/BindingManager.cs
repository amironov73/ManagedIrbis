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
            string reference = string.Format("{0}/{1}/{2}", specification.MagazineIndex, specification.Year,
                longDescription);

            MarcRecord mainRecord = Connection.SearchReadOneRecord("\"I={0}\"", specification.MagazineIndex);
            if (ReferenceEquals(mainRecord, null))
            {
                throw new IrbisException("Main record not found");
            }

            // MagazineInfo magazine = MagazineInfo.Parse(mainRecord);

            foreach (NumberText numberText in collection)
            {
                // Создание записей, если их еще нет.
                MarcRecord issue = new MarcRecord();
            }

            // Создание записи подшивки, если ее еще нет
            MarcRecord binding = new MarcRecord();

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
