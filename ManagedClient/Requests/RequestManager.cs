/* RequestManager.cs -- управление читательскими заказами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using AM;
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
    public sealed class RequestManager
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// ИРБИС-клиент
        /// </summary>
        public ManagedClient64 Client { get; private set; }

        public string Host
        {
            get
            {
                return _GetSetting
                    (
                        "host",
                        ManagedClient64.DefaultHost
                    );
            }
        }

        public int Port
        {
            get
            {
                return int.Parse(_GetSetting
                    (
                        "port",
                        ManagedClient64.DefaultPort.ToInvariantString()
                    ));
            }
        }

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

        private ManagedClient64 _CreateClient()
        {
            ManagedClient64 result = new ManagedClient64
                                         {
                                             Host = Host,
                                             Port = Port,
                                             Database = RequestDatabase,
                                             Username = Login,
                                             Password = Password
                                         };
            if (AllowDebug)
            {
                result.DebugWriter = File.AppendText("Watcher.log");
                result.AllowHexadecimalDump = true;
            }
            result.Connect();
            return result;
        }

        #endregion

        #region Construction

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
        /// <returns></returns>
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

        public void GetAdditionalInfo(BookRequest request)
        {
            IrbisRecord record = ReadCatalog(request.BookCode);
            request.BookRecord = record;
            request.FreeNumbers = ExtractInventoryNumbers(record);
            //request.Reader = ReadReader(request.ReaderID);
            request.MyNumbers = FilterMyNumbers(request.FreeNumbers);
        }

        public IrbisRecord ReadCatalog(string bookCode)
        {
            try
            {
                Client.PushDatabase(CatalogDatabase);
                int[] found = Client.Search(string.Format
                                                (
                                                    "\"I={0}\"",
                                                    bookCode
                                                ));
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

        public void WriteRequest(BookRequest request)
        {
            IrbisRecord record = request.Record;
            if (record != null)
            {
                Client.WriteRecord(record, false, true);
            }
        }

        public bool IsOurPlace(RecordField field)
        {
            if (Places.Contains("*"))
            {
                return true;
            }

            string place = field.GetFirstSubFieldValue('D');

            return Places.Any(p => string.Compare(p, place,
                StringComparison.OrdinalIgnoreCase) == 0);
        }

        public string[] ExtractInventoryNumbers
            (
                IrbisRecord record
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
        /// <param name="readerID"></param>
        /// <returns></returns>
        public ReaderInfo ReadReader(string readerID)
        {
            try
            {
                Client.PushDatabase(ReaderDatabase);
                int[] found = Client.Search(string.Format
                                                (
                                                    "I={0}",
                                                    readerID
                                                ));
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

        public string[] FilterMyNumbers
            (
                string[] numbers
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

        public void Dispose()
        {
            if (Client != null)
            {
                if (Client.DebugWriter != null)
                {
                    Client.DebugWriter.Dispose();
                    Client.DebugWriter = null;
                }

                Client.Dispose();
                Client = null;
            }
        }

        #endregion
    }
}
