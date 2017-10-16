// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExemplarManager.cs -- manages exemplars of the books/magazines etc
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Manages exemplars of the books/magazines etc.
    /// </summary>
    public sealed class ExemplarManager
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [NotNull]
        public IIrbisConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Brief format name.
        /// </summary>
        [NotNull]
        public string Format { get; set; }

#if WIN81 || PORTABLE

        /// <summary>
        /// List of exemplars.
        /// </summary>
        [NotNull]
        public List<ExemplarInfo> List
        {
            get { return _list; }
        }


#else

        /// <summary>
        /// List of exemplars.
        /// </summary>
        [NotNull]
        public ReadOnlyCollection<ExemplarInfo> List
        {
            get { return _list.AsReadOnly(); }
        }

#endif

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
        public ExemplarManager
            (
                [NotNull] IrbisConnection connection
            )
            : this (connection, null)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExemplarManager
            (
                [NotNull] IIrbisConnection connection,
                [CanBeNull] AbstractOutput output
            )
        {
            _connection = connection;
            _output = output;
            Prefix = "IN=";
            Format = "@brief";
            _list = new List<ExemplarInfo>();
            _newspapers = new Dictionary<string, bool>();
        }

        #endregion

        #region Private members

        private readonly IIrbisConnection _connection;

        private readonly List<ExemplarInfo> _list;

        private readonly Dictionary<string, bool> _newspapers;

        private readonly AbstractOutput _output;

        private static string _GetYear
            (
                [NotNull] MarcRecord record
            )
        {
            string workList = record.FM(920);
            string result = record.FM(210, 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'h');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'z');
            }
            if (string.IsNullOrEmpty(result))
            {
                if (workList.SameString("NJ"))
                {
                    result = record.FM(934);
                }
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
                MarcRecord record,
                ExemplarInfo exemplar
            )
        {
            if (!string.IsNullOrEmpty(exemplar.Price))
            {
                return exemplar.Price;
            }
            string price = record.FM(10, 'd');
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
                [NotNull] MarcRecord record
            )
        {
            string result = record.Description;
            if (string.IsNullOrEmpty(result))
            {
                result = Connection.FormatRecord
                    (
                        Format,
                        record.Mfn
                    );
                record.Description = result;
            }

            if (string.IsNullOrEmpty(result))
            {
                Log.Error
                    (
                        "ExemplarManager::GetDescription: "
                        + "empty description"
                    );

                throw new IrbisException("Empty description");
            }

            return result;
        }

        /// <summary>
        /// Get bibliographic description.
        /// </summary>
        [CanBeNull]
        public string GetDescription
            (
                [CanBeNull] MarcRecord record,
                [NotNull] ExemplarInfo exemplar
            )
        {
            string result;

            if (!ReferenceEquals(record, null))
            {
                result = GetDescription(record);
            }
            else
            {
                result = Connection.FormatRecord
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
                [CanBeNull] MarcRecord record
            )
        {
            if (exemplar.Mfn <= 0)
            {
                Log.Error
                    (
                        "ExemplarManager::Extend: "
                        + "MFN="
                        + exemplar.Mfn
                    );

                throw new IrbisException("MFN <= 0");
            }

            exemplar.Description = GetDescription
                (
                    record,
                    exemplar
                );

            if (!ReferenceEquals(record, null))
            {
                string workList = record.FM(920);

                if (string.IsNullOrEmpty(exemplar.ShelfIndex))
                {
                    exemplar.ShelfIndex = record.FM(906)
                                          ?? record.FM(621)
                                          ?? record.FM(686);
                }

                if (string.IsNullOrEmpty(exemplar.ShelfIndex)
                    && workList.SameString("NJ"))
                {
                    string consolidatedIndex = record.FM(933);
                    if (!string.IsNullOrEmpty(consolidatedIndex))
                    {
                        string expression = string.Format
                            (
                                "\"I={0}\"",
                                consolidatedIndex
                            );
                        MarcRecord consolidatedRecord
                            = Connection.SearchReadOneRecord(expression);
                        if (!ReferenceEquals(consolidatedRecord, null))
                        {
                            exemplar.ShelfIndex = consolidatedRecord.FM(906)
                                ?? consolidatedRecord.FM(621)
                                ?? consolidatedRecord.FM(686);
                        }
                    }
                }

                exemplar.Year = _GetYear(record);
                exemplar.Price = _GetPrice(record, exemplar);
                exemplar.Issue = record.FM(936);
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
                [NotNull] MarcRecord record
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
                [NotNull] MarcRecord record
            )
        {
            string worklist = record.FM(920);
            if (string.IsNullOrEmpty(worklist))
            {
                return false;
            }
            if (!worklist.SameString("NJ"))
            {
                return false;
            }
            string index = record.FM(933);
            if (string.IsNullOrEmpty(index))
            {
                return false;
            }

            bool result;
            if (_newspapers.TryGetValue(index, out result))
            {
                return result;
            }

            MarcRecord main = Connection.SearchReadOneRecord
                (
                    "\"I={0}\"",
                    index
                );
            if (ReferenceEquals(main, null))
            {
                return false;
            }

            string kind = main.FM(110, 'b');
            result = kind.SameString("c");
            _newspapers[index] = result;
            return result;
        }

        /// <summary>
        /// List library places.
        /// </summary>
        [NotNull]
        public ChairInfo[] ListPlaces()
        {
            ChairInfo[] result = ChairInfo.Read
                (
                    Connection,
                    "mhr.mnu",
                    false
                )
                .ToArray();

            return result;
        }

#if !WIN81 && !PORTABLE

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
                Log.Error
                    (
                        "ExemplarManager::LoadFromFile: "
                        + "failed to load from: "
                        + fileName
                    );

                throw new IrbisException
                    (
                        "Failed to load exemplars from file"
                    );
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

#endif

        /// <summary>
        /// Reads exemplar for given number.
        /// </summary>
        [CanBeNull]
        public ExemplarInfo Read
            (
                [NotNull] string number
            )
        {
            MarcRecord[] records = Connection.SearchRead
                (
                    "\"{0}{1}\"",
                    Prefix,
                    number
                );

            ExemplarInfo result = records
                .SelectMany(record => ExemplarInfo.Parse(record))
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Reads exemplar for given number.
        /// </summary>
        [CanBeNull]
        public ExemplarInfo ReadExtend
            (
                [NotNull] string number
            )
        {
            MarcRecord[] records = Connection.SearchRead
                (
                    "\"{0}{1}\"",
                    Prefix,
                    number
                );

            ExemplarInfo result = records
                .SelectMany(r => ExemplarInfo.Parse(r))
                .Tee(exemplar => Extend(exemplar, exemplar.Record))
                .FirstOrDefault
                    (
                        e => e.Barcode.SameString(number)
                            || e.Number.SameString(number)
                    );

            return result;
        }


#if !SILVERLIGHT && !WIN81 && !PORTABLE

        /// <summary>
        /// Read configuration.
        /// </summary>
        public void ReadConfiguration
            (
                [NotNull] string fileName,
                [CanBeNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            if (ReferenceEquals(encoding, null))
            {
                encoding = Encoding.UTF8;
            }

            using (IniFile ini = new IniFile(fileName, encoding, false))
            {
                IniFile.Section section = ini.GetSection("Main");
                if (!ReferenceEquals(section, null))
                {
                    string format = section.GetValue("Format", Format);
                    if (!string.IsNullOrEmpty(format))
                    {
                        Format = format;
                    }
                    string prefix = section.GetValue("Prefix", Prefix);
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        Prefix = prefix;
                    }
                }
            }
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
            Code.NotNullNorEmpty(fileName, "fileName");

            if (ReferenceEquals(encoding, null))
            {
                encoding = Encoding.UTF8;
            }

            using (IniFile ini = File.Exists(fileName)
                ? new IniFile(fileName, encoding, true)
                : new IniFile {Encoding = encoding})
            {
                IniFile.Section section
                    = ini.GetOrCreateSection("Main");
                section["Format"] = Format;
                section["Prefix"] = Prefix;

                ini.Save(fileName);
            }
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

#endif

        /// <summary>
        /// Read many.
        /// </summary>
        [NotNull]
        [StringFormatMethod("format")]
        public ExemplarManager ReadRange
            (
                [CanBeNull] string place,
                [NotNull] string searchExpression
            )
        {
            IEnumerable<MarcRecord> reader = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database,
                    searchExpression,
                    1000
                );
            foreach (MarcRecord record in reader)
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

