/* BinaryResource.cs -- field 953.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Binary resource in field 953.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BinaryResource
    {
        #region Constants

        /// <summary>
        /// Default tag for binary resources.
        /// </summary>
        public const string Tag = "953";

        #endregion

        #region Properties

        /// <summary>
        /// Kind of resource. Subfield a.
        /// </summary>
        /// <remarks>For example, "jpg".</remarks>
        [CanBeNull]
        public string Kind { get; set; }

        /// <summary>
        /// Percent-encoded resource. Subfield b.
        /// </summary>
        [CanBeNull]
        public string Resource { get; set; }

        /// <summary>
        /// Title of resource. Subfield t.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// View method. Subfield p.
        /// </summary>
        [CanBeNull]
        public string View { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Decode the resource.
        /// </summary>
        [NotNull]
        public byte[] Decode()
        {
            if (string.IsNullOrEmpty(Resource))
            {
                return new byte[0];
            }

            byte[] result = IrbisUtility.DecodePercentString(Resource);

            return result;
        }

        /// <summary>
        /// Encode the resource.
        /// </summary>
        public void Encode
            (
                [CanBeNull] byte[] array
            )
        {
            if (array.IsNullOrEmpty())
            {
                Resource = null;
                return;
            }

            Resource = IrbisUtility.EncodePercentString(array);
        }

        /// <summary>
        /// Parse field 953.
        /// </summary>
        [NotNull]
        public static BinaryResource Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            BinaryResource result = new BinaryResource
            {
                Kind = field.GetFirstSubFieldValue('a'),
                Resource = field.GetFirstSubFieldValue('b'),
                Title = field.GetFirstSubFieldValue('t'),
                View = field.GetFirstSubFieldValue('p')
            };

            return result;
        }

        /// <summary>
        /// Parse fields 953 of the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static BinaryResource[] Parse
            (
                [NotNull] MarcRecord record,
                [NotNull] string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");

            RecordField[] fields = record
                .Fields
                .GetField(tag);

            BinaryResource[] result = fields
                .Select(Parse)
                .ToArray();

            return result;
        }

        /// <summary>
        /// Parse fields 953 of the <see cref="MarcRecord"/>.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static BinaryResource[] Parse
            (
                [NotNull] MarcRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        #endregion

        #region Object members

        #endregion
    }
}
