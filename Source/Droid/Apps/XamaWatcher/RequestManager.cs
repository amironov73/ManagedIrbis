using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ManagedIrbis;
using ManagedIrbis.Readers;

namespace XamaWatcher
{
    public class RequestManager
        : IDisposable
    {
        public RequestManager(Activity activity)
        {
            ReadSettings(activity);
        }

        private string _host;
        private string _login;
        private string _password;
        private string _requestDb;
        private string _catalogDb;
        private string _readerDb;
        private string _place;
        private string _my;

        private int _port, _auto;
        bool _sound;
        private string[] _places;

        private IrbisConnection _client;

        private void ReadSettings
            (
                Activity activity
            )
        {
            using (StreamReader reader
                = new StreamReader (activity.Assets.Open ("settings.txt")))
            {
                _host = reader.ReadLine ();
                _port = int.Parse (reader.ReadLine ().ThrowIfNull("port"));
                _login = reader.ReadLine ();
                _password = reader.ReadLine ();
                _requestDb = reader.ReadLine ();
                _catalogDb = reader.ReadLine ();
                _readerDb = reader.ReadLine ();
                _place = reader.ReadLine ().ThrowIfNull("place");
                _my = reader.ReadLine ();
                _auto = int.Parse (reader.ReadLine ().ThrowIfNull("auto"));
                _sound = Convert.ToBoolean
                    (
                        int.Parse (reader.ReadLine().ThrowIfNull ("sound"))
                    );

                _places = _place.Split (',', ';');

                _client = _CreateClient ();
            }
        }

        private IrbisConnection _CreateClient()
        {
            IrbisConnection result = new IrbisConnection
            {
                Host = _host,
                Port = _port,
                Database = _requestDb,
                Username = _login,
                Password = _password
            };
            result.Connect();
            return result;
        }

        public BookRequest[] GetRequests()
        {
            int[] found = _client.Search("I=0");

            return found
                .Select(mfn => _client.ReadRecord(mfn))
                .Select(record => BookRequest.Parse(record))
                .Where(request => request != null)
                .ToArray();
        }

        public void GetAdditionalInfo
            (
                BookRequest request
            )
        {
            MarcRecord record = ReadCatalog(request.BookCode);
            request.BookRecord = record;
            request.FreeNumbers = ExtractInventoryNumbers(record);
            //request.Reader = ReadReader(request.ReaderID);
            request.MyNumbers = FilterMyNumbers(request.FreeNumbers);
        }

        public MarcRecord ReadCatalog
            (
                string bookCode
            )
        {
            try
            {
                _client.PushDatabase(_catalogDb);
                int[] found = _client.Search($"\"I={bookCode}\"");
                if (found.Length == 0)
                {
                    return null;
                }
                MarcRecord result = _client.ReadRecord(found[0]);
                return result;
            }
            finally
            {
                _client.PopDatabase();
            }
        }

        public void WriteRequest
            (
                BookRequest request
            )
        {
            MarcRecord record = request.Record;
            if (record != null)
            {
                _client.WriteRecord(record,false,true);
            }
        }

        public bool IsOurPlace
            (
                RecordField field
            )
        {
            if (_places.Contains("*"))
            {
                return true;
            }

            string place = field.GetFirstSubFieldValue('d');

            return _places.Any(p=>string.Compare(p,place,
                StringComparison.OrdinalIgnoreCase) == 0);
        }

        public string[] ExtractInventoryNumbers
            (
                MarcRecord record
            )
        {
            if (record == null)
            {
                return new string[0];
            }

            RecordField[] allFields = record.Fields
                .GetField(910)
                .GetField('a', "0");

            RecordField[] ourFields = allFields
                .Where(IsOurPlace)
                .ToArray();

            if (allFields.Length > ourFields.Length)
            {
                return new string[0];
            }

            return ourFields
                .GetSubField('b')
                .GetSubFieldValue();
        }

        public ReaderInfo ReadReader
            (
                string readerId
            )
        {
            try
            {
                _client.PushDatabase(_readerDb);
                int[] found = _client.Search($"I={readerId}");
                if (found.Length == 0)
                {
                    return null;
                }
                MarcRecord record = _client.ReadRecord(found[0]);
                ReaderInfo result = ReaderInfo.Parse(record);
                return result;
            }
            finally
            {
                _client.PopDatabase();
            }
        }

        public string[] FilterMyNumbers
            (
                string[] numbers
            )
        {
            NumberFilter filter = NumberFilter.ParseNumbers(_my);
            return filter.FilterNumbers(numbers);
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose ();
                _client = null;
            }
        }
    }
}