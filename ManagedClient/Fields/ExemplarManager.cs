/* ExemplarManager.cs --
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AM;
using AM.Runtime;
using AM.Text.Output;
using JetBrains.Annotations;

using AM.IO;
using ManagedClient.Readers;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Exemplar manager
    /// </summary>
    public sealed class ExemplarManager
    {
        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public ManagedClient64 Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Brief format name.
        /// </summary>
        [NotNull]
        public string Format { get; set; }

        /// <summary>
        /// List of exemplars.
        /// </summary>
        [NotNull]
        public ReadOnlyCollection<ExemplarInfo> List
        {
            get { return _list.AsReadOnly(); }
        }

        /// <summary>
        /// Output.
        /// </summary>
        [CanBeNull]
        public AbstractOutput Output { get { return _output; } }

        /// <summary>
        /// Prefix.
        /// </summary>
        [NotNull]
        public string Prefix { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExemplarManager()
            : this
            (
                ManagedClientUtility.GetClientFromConfig(),
                null
            )
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExemplarManager
            (
                [NotNull] ManagedClient64 client
            )
            : this (client, null)
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExemplarManager
            (
                [NotNull] ManagedClient64 client,
                [CanBeNull] AbstractOutput output
            )
        {
            _client = client;
            _output = output;
            Prefix = "IN=";
            Format = "@brief";
            _list = new List<ExemplarInfo>();
            _newspapers = new Dictionary<string, bool>();
        }

        #endregion

        #region Private members

        private readonly ManagedClient64 _client;

        private readonly List<ExemplarInfo> _list;

        private readonly Dictionary<string, bool> _newspapers;

        private readonly AbstractOutput _output;

        private static string _GetYear
            (
                IrbisRecord record
            )
        {
            string result = record.FM("210", 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM("461", 'h');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM("461", 'z');
            }
            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            Match match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }

            return result;
        }

        private static string _GetPrice
            (
                IrbisRecord record,
                ExemplarInfo exemplar
            )
        {
            if (!string.IsNullOrEmpty(exemplar.Price))
            {
                return exemplar.Price;
            }
            string price = record.FM("10", 'd');
            if (!string.IsNullOrEmpty(price))
            {
                return price;
            }
            return string.Empty;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add given exemplar to the collection.
        /// </summary>
        [NotNull]
        public ExemplarManager Add
            (
                [NotNull] ExemplarInfo exemplar
            )
        {
            if (string.IsNullOrEmpty(exemplar.Number))
            {
                return this;
            }

            if (Find(exemplar.Number) == null)
            {
                _list.Add(exemplar);
            }

            return this;
        }

        /// <summary>
        /// Add many.
        /// </summary>
        [NotNull]
        public ExemplarManager AddRange
            (
                [NotNull] IEnumerable<ExemplarInfo> exemplars
            )
        {
            foreach (ExemplarInfo exemplar in exemplars)
            {
                Add(exemplar);
            }

            return this;
        }

        /// <summary>
        /// Clear the list of exemplars.
        /// </summary>
        [NotNull]
        public ExemplarManager Clear()
        {
            _list.Clear();

            return this;
        }

        /// <summary>
        /// Get bibliographic description.
        /// </summary>
        [NotNull]
        public string GetDescription
            (
                [NotNull] IrbisRecord record
            )
        {
            string result = record.Description;
            if (string.IsNullOrEmpty(result))
            {
                result = Client.FormatRecord
                    (
                        Format,
                        record.Mfn
                    );
                record.Description = result;
            }

            if (string.IsNullOrEmpty(result))
            {
                throw new ApplicationException();
            }

            return result;
        }

        /// <summary>
        /// Get bibliographic description.
        /// </summary>
        [NotNull]
        public string GetDescription
            (
                [CanBeNull] IrbisRecord record,
                [NotNull] ExemplarInfo exemplar
            )
        {
            string result;

            if (record != null)
            {
                result = GetDescription(record);
            }
            else
            {
                result = Client.FormatRecord
                    (
                        Format,
                        exemplar.Mfn
                    );
            }

            return result;
        }

        /// <summary>
        /// Extend info.
        /// </summary>
        [NotNull]
        public ExemplarInfo Extend
            (
                [NotNull] ExemplarInfo exemplar,
                [CanBeNull] IrbisRecord record
            )
        {
            if (exemplar.Mfn <= 0)
            {
                throw new ApplicationException("MFN <= 0");
            }

            exemplar.Description = GetDescription
                (
                    record,
                    exemplar
                );

            if (record != null)
            {
                exemplar.Year = _GetYear(record);
                exemplar.Price = _GetPrice(record, exemplar);
            }

            return exemplar;
        }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public ExemplarInfo Find
            (
                [CanBeNull] string number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                return null;
            }

            return _list.FirstOrDefault
                (
                    e => e.Number.SameString(number)
                         || e.Barcode.SameString(number)
                );
        }

        /// <summary>
        /// Parses the record for exemplars.
        /// </summary>
        [NotNull]
        public ExemplarInfo[] FromRecord
            (
                [NotNull] IrbisRecord record
            )
        {
            ExemplarInfo[] result = ExemplarInfo.Parse(record);

            foreach (ExemplarInfo exemplar in result)
            {
                Extend(exemplar, record);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the record is newspaper/magazine
        /// or not.
        /// </summary>
        public bool IsNewspaper
            (
                IrbisRecord record
            )
        {
            string worklist = record.FM("920");
            if (string.IsNullOrEmpty(worklist))
            {
                return false;
            }
            if (!worklist.SameString("NJ"))
            {
                return false;
            }
            string index = record.FM("933");
            if (string.IsNullOrEmpty(index))
            {
                return false;
            }

            bool result;
            if (_newspapers.TryGetValue(index, out result))
            {
                return result;
            }

            IrbisRecord main = Client.SearchReadOneRecord
                (
                    "\"I={0}\"",
                    index
                );
            if (ReferenceEquals(main, null))
            {
                return false;
            }

            string kind = main.FM("110", 'b');
            result = kind.SameString("c");
            _newspapers[index] = result;
            return result;
        }

        /// <summary>
        /// List library places.
        /// </summary>
        public ChairInfo[] ListPlaces()
        {
            ChairInfo[] result = ChairInfo.Read
                (
                    Client,
                    "mhr.mnu"
                )
                .Where(item => item.Code != "*")
                .ToArray();

            return result;
        }

        /// <summary>
        /// Load from file.
        /// </summary>
        public void LoadFromFile
            (
                [NotNull] string fileName
            )
        {
            ExemplarInfo[] loaded = SerializationUtility
                .RestoreArrayFromFile<ExemplarInfo>(fileName);

            if (ReferenceEquals(loaded, null))
            {
                throw new ApplicationException();
            }

            foreach (ExemplarInfo exemplar in loaded)
            {
                if (string.IsNullOrEmpty(exemplar.Number))
                {
                    continue;
                }
                ExemplarInfo copy = Find(exemplar.Number);
                if (copy == null)
                {
                    _list.Add(exemplar);
                }
            }
        }

        /// <summary>
        /// Reads exemplar for given number.
        /// </summary>
        [CanBeNull]
        public ExemplarInfo Read
            (
                [NotNull] string number
            )
        {
            IrbisRecord[] records = Client.SearchRead
                (
                    "\"{0}{1}\"",
                    Prefix,
                    number
                );

            ExemplarInfo result = records
                .SelectMany(ExemplarInfo.Parse)
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Read configuration.
        /// </summary>
        public void ReadConfiguration
            (
                [NotNull] string fileName,
                [CanBeNull] Encoding encoding
            )
        {
            throw new NotImplementedException();

#if NOTDEF
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            IniFile ini = IniFile.ParseFile<IniFile>(fileName, encoding);
            IniFile.Section section = ini.GetOrCreateSection("Main");
            Format = section.Get("Format", Format);
            Prefix = section.Get("Prefix", Prefix);
#endif
        }

        /// <summary>
        /// Save the configuration.
        /// </summary>
        public void SaveConfiguration
            (
                [NotNull] string fileName,
                [CanBeNull] Encoding encoding
            )
        {
            throw new NotImplementedException();

#if NOTDEF
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            IniFile ini = File.Exists(fileName)
                ? IniFile.ParseFile<IniFile>(fileName, encoding)
                : new IniFile();
            IniFile.Section section = ini.GetOrCreateSection("Main");
            section["Format"] = Format;
            section["Prefix"] = Prefix;

            ini.Save(fileName, encoding);
#endif
        }

        /// <summary>
        /// Save to the file.
        /// </summary>
        public void SaveToFile
            (
                [NotNull] string fileName
            )
        {
            _list.ToArray().SaveToZipFile(fileName);
        }

        /// <summary>
        /// Read many.
        /// </summary>
        [NotNull]
        [StringFormatMethod("format")]
        public ExemplarManager ReadRange
            (
                [CanBeNull] string place,
                [NotNull] string format,
                params object[] args
            )
        {
            throw new NotImplementedException();

#if NOTDEF
            BatchRecordReader reader = new BatchRecordReader
                (
                    Client,
                    format,
                    args
                );
            foreach (IrbisRecord record in reader)
            {
                ExemplarInfo[] exemplars = FromRecord(record);
                if (!string.IsNullOrEmpty(place))
                {
                    exemplars = exemplars
                        .Where(e => e.Place.SameString(place))
                        .ToArray();
                }
                foreach (ExemplarInfo exemplar in exemplars)
                {
                    Add(exemplar);
                }
            }

            return this;
#endif
        }

        /// <summary>
        /// Remove
        /// </summary>
        [NotNull]
        public ExemplarManager Remove
            (
                [CanBeNull] string number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                return this;
            }

            ExemplarInfo found = Find(number);
            if (found != null)
            {
                _list.Remove(found);
            }

            return this;
        }

        /// <summary>
        /// Write line.
        /// </summary>
        [StringFormatMethod("format")]
        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            if (!ReferenceEquals(_output, null))
            {
                _output.WriteLine(format, args);
            }
        }

        /// <summary>
        /// Write delimiter.
        /// </summary>
        public void WriteDelimiter()
        {
            WriteLine
                (
                    new string('=', 60)
                );
        }

        #endregion
    }
}
