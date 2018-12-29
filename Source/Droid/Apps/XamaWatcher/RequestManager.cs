// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RequestManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Linq;

using AM;

using ManagedIrbis;
using ManagedIrbis.Readers;

#endregion

// ReSharper disable StringLiteralTypo

namespace XamaWatcher
{
    class RequestManager
        : IDisposable
    {
        public RequestManager
            (
                Transport transport
            )
        {
            _transport = transport;
            _places = _transport.Place.Split(',', ';');
            _client = _transport.CreateClient();
        }

        private readonly Transport _transport;
        private readonly string[] _places;
        private readonly IrbisConnection _client;

        public BookRequest[] GetRequests()
        {
            int[] found = _client.Search("I=0");

            return found
                .Select(mfn => _client.ReadRecord(mfn))
                .Select(BookRequest.Parse)
                .NonNullItems()
                .ToArray();
        }

        public void GetAdditionalInfo
            (
                BookRequest request
            )
        {
            MarcRecord record = ReadCatalog(request.BookCode);
            if (ReferenceEquals(record, null))
            {
                return;
            }

            request.BookRecord = record;
            request.Reader = ReadReader(request.ReaderId);
            if (ReferenceEquals(request.Reader, null))
            {
                return;
            }

            request.FreeNumbers = ExtractInventoryNumbers(record);
            request.MyNumbers = FilterMyNumbers(request.FreeNumbers);
            if (string.IsNullOrEmpty(request.ReaderDescription))
            {
                string readerDescription = _client.FormatRecord
                    (
                        _transport.ReaderDb,
                        "@brief",
                        request.Reader.Mfn
                    );
                int index = readerDescription.IndexOf
                    (
                        "Записан в",
                        StringComparison.InvariantCulture
                    );
                if (index > 0)
                {
                    readerDescription = readerDescription
                        .Substring(0, index)
                        .TrimEnd();
                }

                request.ReaderDescription = readerDescription;
            }
        }

        public MarcRecord ReadCatalog
            (
                string bookCode
            )
        {
            try
            {
                _client.PushDatabase(_transport.CatalogDb);
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
            if (!ReferenceEquals(record, null))
            {
                _client.WriteRecord(record, false, true);
            }

            record = request.BookRecord;
            if (!ReferenceEquals(record, null) && record.Modified)
            {
                _client.WriteRecord(record, false, true);
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

            return _places.Any(p => p.SameString(place));
        }

        public string[] ExtractInventoryNumbers
            (
                MarcRecord record
            )
        {
            if (ReferenceEquals(record, null))
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
                _client.PushDatabase(_transport.ReaderDb);
                int[] found = _client.Search($"RI={readerId}");
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
            NumberFilter filter = NumberFilter.ParseNumbers(_transport.My);
            return filter.FilterNumbers(numbers);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}