/* RequestManager.cs -- управление читательскими заказами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;
using CodeJam;
using JetBrains.Annotations;

using MoonSharp.Interpreter;

#if PocketPC
using CM=OpenNETCF.Configuration.ConfigurationSettings;
#else
using CM = System.Configuration.ConfigurationManager;
#endif

#endregion

namespace ManagedClient.Requests
{
    using Readers;

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
        public IrbisConnection Client { get; private set; }

        /// <summary>
        /// Host name.
        /// </summary>
        [NotNull]
        public string Host
        {
            get
            {
                return _GetSetting
                    (
                        "host",
                        IrbisConnection.DefaultHost
                    );
            }
        }

        /// <summary>
        /// Port number.
        /// </summary>
        public int Port
        {
            get
            {
                return int.Parse(_GetSetting
                    (
                        "port",
                        IrbisConnection.DefaultPort.ToInvariantString()
                    ));
            }
        }

        /// <summary>
        /// Request database name.
        /// </summary>
        [NotNull]
        public string RequestDatabase
        {
            get
            {
                return _GetSetting
                    (
                        "request-db",
                        "RQST"
                    );
            }
        }

        /// <summary>
        /// Catalog database name.
        /// </summary>
        [NotNull]
        public string CatalogDatabase
        {
            get
            {
                return _GetSetting
                    (
                        "catalog-db",
                        "IBIS"
                    );
            }
        }

        /// <summary>
        /// Reader database name.
        /// </summary>
        [NotNull]
        public string ReaderDatabase
        {
            get
            {
                return _GetSetting
                    (
                        "reader-db",
                        "RDR"
                    );
            }
        }

        /// <summary>
        /// User login.
        /// </summary>
        [NotNull]
        public string Login
        {
            get
            {
                return _GetSetting
                    (
                        "login",
                        "1"
                    );
            }
        }

        /// <summary>
        /// User password.
        /// </summary>
        [NotNull]
        public string Password
        {
            get
            {
                return _GetSetting
                    (
                        "password",
                        "1"
                    );
            }
        }

        /// <summary>
        /// Filter specification.
        /// </summary>
        [NotNull]
        public string FilterSpecification
        {
            get
            {
                return _GetSetting
                    (
                        "my",
                        "*"
                    );
            }
        }

        /// <summary>
        /// Allow debug.
        /// </summary>
        public bool AllowDebug
        {
            get
            {
                return bool.Parse(_GetSetting
                    (
                        "debug",
                        "false"
                    ));
            }
        }

        /// <summary>
        /// Places.
        /// </summary>
        [NotNull]
        public string[] Places { get; set; }

        #endregion

        #region Private members

        private string _GetSetting
            (
                string name,
                string defaultValue
            )
        {
            string result = CM.AppSettings[name];
            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }
            return result;
        }

        private IrbisConnection _CreateClient()
        {
            IrbisConnection result = new IrbisConnection
                                         {
                                             Host = Host,
                                             Port = Port,
                                             Database = RequestDatabase,
                                             Username = Login,
                                             Password = Password
                                         };
            if (AllowDebug)
            {
                //result.DebugWriter = File.AppendText("Watcher.log");
                //result.AllowHexadecimalDump = true;
            }
            result.Connect();

            return result;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RequestManager()
        {
            Client = _CreateClient();
            Places = _GetSetting("place", "*").Split(',', ';');
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получение списка новых заказов.
        /// </summary>
        [NotNull]
        public BookRequest[] GetRequests()
        {
            int[] found = Client.Search("I=0");

            return found
                .Select(mfn => Client.ReadRecord(mfn))
                .Where(record => record != null)
                .Select(record => BookRequest.Parse(record))
                .Where(request => request != null)
                .ToArray();
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

            if (!ReferenceEquals(request.BookCode, null))
            {
                IrbisRecord record = ReadCatalog(request.BookCode);
                request.BookRecord = record;
                request.FreeNumbers = ExtractInventoryNumbers(record);
                //request.Reader = ReadReader(request.ReaderID);
                request.MyNumbers = FilterMyNumbers(request.FreeNumbers);
            }
        }

        /// <summary>
        /// Read info from catalog.
        /// </summary>
        [CanBeNull]
        public IrbisRecord ReadCatalog
            (
                [NotNull] string bookCode
            )
        {
            try
            {
                Client.PushDatabase(CatalogDatabase);
                int[] found = Client.Search
                    (
                        "\"I={0}\"",
                        bookCode
                    );
                if (found.Length == 0)
                {
                    return null;
                }
                IrbisRecord result = Client.ReadRecord(found[0]);

                return result;
            }
            finally
            {
                Client.PopDatabase();
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
            IrbisRecord record = request.RequestRecord;
            if (record != null)
            {
                Client.WriteRecord(record, false, true);
            }
        }

        /// <summary>
        /// Determine, whether is our place?
        /// </summary>
        public bool IsOurPlace
            (
                [NotNull] RecordField field
            )
        {
            if (Places.Contains("*"))
            {
                return true;
            }

            string place = field.GetFirstSubFieldValue('D');

            return Places.Any(p => string.Compare(p, place,
                StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        /// Extract inventory numbers.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] ExtractInventoryNumbers
            (
                [CanBeNull] IrbisRecord record
            )
        {
            if (record == null)
            {
                return new string[0];
            }

            RecordField[] allFields = record.Fields
                .GetField("910", "A", "0");

            RecordField[] ourFields = allFields
                .Where(IsOurPlace)
                .ToArray();

            if (allFields.Length > ourFields.Length)
            {
                return new string[0];
            }

            return ourFields
                .GetSubField('B')
                .GetSubFieldValue();
        }

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
                Client.PushDatabase(ReaderDatabase);
                int[] found = Client.Search
                    (
                        "I={0}",
                        readerID
                    );
                if (found.Length == 0)
                {
                    return null;
                }
                IrbisRecord record = Client.ReadRecord(found[0]);
                ReaderInfo result = ReaderInfo.Parse(record);

                return result;
            }
            finally
            {
                Client.PopDatabase();
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
            throw new NotImplementedException();
#if NOTDEF
            NumberFilter filter = NumberFilter.ParseNumbers(FilterSpecification);
            return filter.FilterNumbers(numbers);
#endif
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (Client != null)
            {
                //if (Client.DebugWriter != null)
                //{
                //    Client.DebugWriter.Dispose();
                //    Client.DebugWriter = null;
                //}

                Client.Dispose();
            }
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        #endregion
    }
}
