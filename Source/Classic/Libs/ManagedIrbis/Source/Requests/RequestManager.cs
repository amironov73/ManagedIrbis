// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RequestManager.cs -- управление читательскими заказами
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if !UAP

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

using MoonSharp.Interpreter;

#if WINMOBILE || PocketPC

using CM=OpenNETCF.Configuration.ConfigurationSettings;

#elif !ANDROID

using CM = System.Configuration.ConfigurationManager;

#endif

#endregion

namespace ManagedIrbis.Requests
{

    /// <summary>
    /// Управление читательскими заказами.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RequestManager
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// ИРБИС-клиент
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Reader database name.
        /// </summary>
        public NonNullValue<string> ReaderDatabase { get; set; }

        /// <summary>
        /// Request database name.
        /// </summary>
        public NonNullValue<string> RequestDatabase { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestManager
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
            ReaderDatabase = "IBIS";
            RequestDatabase = "RQST";
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Получение списка заказов согласно указанному условию.
        /// </summary>
        [NotNull]
        public BookRequest[] GetRequests
            (
                [NotNull] string searchCriteria
            )
        {
            Code.NotNullNorEmpty(searchCriteria, "searchCriteria");

            IEnumerable<MarcRecord> found = BatchRecordReader.Search
                (
                    Connection,
                    RequestDatabase,
                    searchCriteria,
                    500
                );

            // ReSharper disable ConvertClosureToMethodGroup
            BookRequest[] result = found
                .Select
                (
                    record => BookRequest.Parse(record)
                )
                .NonNullItems()
                .ToArray();
            // ReSharper restore ConvertClosureToMethodGroup

            return result;
        }

        /// <summary>
        /// Get additional info.
        /// </summary>
        public void GetAdditionalInfo
            (
                [NotNull] BookRequest request
            )
        {
            Code.NotNull(request, "request");

            string database = request.Database;
            string bookCode = request.BookCode;
            if (!string.IsNullOrEmpty(database)
                && !string.IsNullOrEmpty(bookCode))
            {
                MarcRecord record = ReadCatalog
                    (
                        database,
                        bookCode
                    );
                request.BookRecord = record;
                //request.FreeNumbers = ExtractInventoryNumbers(record);
                //request.Reader = ReadReader(request.ReaderID);
                request.MyNumbers = FilterMyNumbers(request.FreeNumbers);
            }
        }

        /// <summary>
        /// Read info from catalog.
        /// </summary>
        [CanBeNull]
        public MarcRecord ReadCatalog
            (
                [NotNull] string catalogDatabase,
                [NotNull] string bookCode
            )
        {
            try
            {
                Connection.PushDatabase(catalogDatabase);
                int[] found = Connection.Search
                    (
                        "\"I={0}\"",
                        bookCode
                    );
                if (found.Length == 0)
                {
                    return null;
                }
                MarcRecord result = Connection.ReadRecord(found[0]);

                return result;
            }
            finally
            {
                Connection.PopDatabase();
            }
        }

        /// <summary>
        /// Write request.
        /// </summary>
        public void WriteRequest
            (
                [NotNull] BookRequest request
            )
        {
            MarcRecord record = request.RequestRecord;
            if (record != null)
            {
                Connection.WriteRecord(record, false, true);
            }
        }

        ///// <summary>
        ///// Determine, whether is our place?
        ///// </summary>
        //public bool IsOurPlace
        //    (
        //        [NotNull] RecordField field
        //    )
        //{
        //    if (Places.Contains("*"))
        //    {
        //        return true;
        //    }

        //    string place = field.GetFirstSubFieldValue('D');

        //    return Places.Any(p => string.Compare(p, place,
        //        StringComparison.OrdinalIgnoreCase) == 0);
        //}

        ///// <summary>
        ///// Extract inventory numbers.
        ///// </summary>
        //[NotNull]
        //[ItemNotNull]
        //public string[] ExtractInventoryNumbers
        //    (
        //        [CanBeNull] MarcRecord record
        //    )
        //{
        //    if (record == null)
        //    {
        //        return StringUtility.EmptyArray;
        //    }

        //    RecordField[] allFields = record.Fields
        //        .GetField("910", "A", "0");

        //    RecordField[] ourFields = allFields
        //        .Where(IsOurPlace)
        //        .ToArray();

        //    if (allFields.Length > ourFields.Length)
        //    {
        //        return StringUtility.EmptyArray;
        //    }

        //    return ourFields
        //        .GetSubField('B')
        //        .GetSubFieldValue();
        //}

        /// <summary>
        /// Загрузка сведений о читателе.
        /// </summary>
        [CanBeNull]
        public ReaderInfo ReadReader
            (
                [NotNull] string readerID
            )
        {
            Code.NotNullNorEmpty(readerID, "readerID");

            try
            {
                Connection.PushDatabase(ReaderDatabase);
                int[] found = Connection.Search
                    (
                        "\"I={0}\"",
                        readerID
                    );
                if (found.Length == 0)
                {
                    return null;
                }
                MarcRecord record = Connection.ReadRecord(found[0]);
                ReaderInfo result = ReaderInfo.Parse(record);

                return result;
            }
            finally
            {
                Connection.PopDatabase();
            }
        }

        /// <summary>
        /// Filter my numbers.
        /// </summary>
        [NotNull]
        public string[] FilterMyNumbers
            (
                [NotNull] string[] numbers
            )
        {
            Log.Error
                (
                    "RequestManager::FilterMyNumbers: "
                    + "not implemented"
                );

            throw new NotImplementedException();
#if NOTDEF
            NumberFilter filter = NumberFilter.ParseNumbers(FilterSpecification);
            return filter.FilterNumbers(numbers);
#endif
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (Connection != null)
            {
                //if (Client.DebugWriter != null)
                //{
                //    Client.DebugWriter.Dispose();
                //    Client.DebugWriter = null;
                //}

                Connection.Dispose();
            }
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        #endregion
    }
}

#endif
