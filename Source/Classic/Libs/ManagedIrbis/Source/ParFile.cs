/* ParFile.cs -- PAR files handling
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    // Official documentation:
    //
    // Каждой базе данных ИРБИС соответствует один .par-файл.
    // Этот файл содержит набор путей к файлам базы данных ИРБИС.
    // Имя .par-файла соответствует имени базы данных.
    //
    // .par-файл представляет собой текстовый файл, состоящий
    // из 11 строк. Каждая строка представляет собой путь,
    // указывающий местонахождение соответствующих файлов базы данных.
    // Примечание: до версии 2011.1 включительно .par-файлы включают
    // в себя 10 строк. 11-я строка добавлена в версии 2012.1.
    //
    // В исходном состоянии системы .par-файл содержит относительные
    // пути размещения файлов базы данных – относительно основной
    // директории системы <IRBIS_SERVER_ROOT>.
    //
    // Фактически в ИРБИС принят принцип хранения всех файлов
    // базы данных в одной папке, поэтому .par-файлы содержат
    // один и тот же путь, повторяющийся в каждой строке.

    // Как правило, PAR-файлы располагаются в подпапке DataI внутри
    // папки IRBIS64, в которую установлен сервер ИРБИС
    // (но их расположение может быть переопределено параметром
    // DataPath в irbis_server.ini).

    // Пример файла IBIS.PAR:
    //
    // 1=.\datai\ibis\
    // 2=.\datai\ibis\
    // 3=.\datai\ibis\
    // 4=.\datai\ibis\
    // 5=.\datai\ibis\
    // 6=.\datai\ibis\
    // 7=.\datai\ibis\
    // 8=.\datai\ibis\
    // 9=.\datai\ibis\
    // 10=.\datai\ibis\
    // 11=f:\webshare\

    // Параметр | Назначение
    //        1 | Путь к файлу XRF
    //        2 | MST
    //        3 | CNT
    //        4 | N01
    //        5 | N02 (только для ИРБИС32)
    //        6 | L01
    //        7 | L02 (только для ИРБИС32)
    //        8 | IFP
    //        9 | ANY
    //       10 | FDT, FST, FMT, PFT, STW, SRT
    //       11 | появился в версии 2012:
    //          | расположение внешних объектов (поле 951)


    /// <summary>
    /// PAR files handling
    /// </summary>
    [PublicAPI]
    [XmlRoot("par")]
    [MoonSharpUserData]
    public sealed class ParFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Путь к файлу XRF
        /// </summary>
        [CanBeNull]
        [XmlAttribute("xrf")]
        [JsonProperty("xrf")]
        public string XrfPath { get; set; }

        /// <summary>
        /// Путь к файлу MST
        /// </summary>
        [CanBeNull]
        [XmlAttribute("mst")]
        [JsonProperty("mst")]
        public string MstPath { get; set; }

        /// <summary>
        /// Путь к файлу CNT
        /// </summary>
        [CanBeNull]
        [XmlAttribute("cnt")]
        [JsonProperty("cnt")]
        public string CntPath { get; set; }

        /// <summary>
        /// Путь к файлу N01
        /// </summary>
        [CanBeNull]
        [XmlAttribute("n01")]
        [JsonProperty("n01")]
        public string N01Path { get; set; }

        /// <summary>
        /// Путь к файлу N02
        /// </summary>
        [CanBeNull]
        [XmlAttribute("n02")]
        [JsonProperty("n02")]
        public string N02Path { get; set; }

        /// <summary>
        /// Путь к файлу L01
        /// </summary>
        [CanBeNull]
        [XmlAttribute("l01")]
        [JsonProperty("l01")]
        public string L01Path { get; set; }

        /// <summary>
        /// Путь к файлу L02
        /// </summary>
        [CanBeNull]
        [XmlAttribute("l02")]
        [JsonProperty("l02")]
        public string L02Path { get; set; }

        /// <summary>
        /// Путь к файлу IFP
        /// </summary>
        [CanBeNull]
        [XmlAttribute("ifp")]
        [JsonProperty("ifp")]
        public string IfpPath { get; set; }

        /// <summary>
        /// Путь к файлу ANY
        /// </summary>
        [CanBeNull]
        [XmlAttribute("any")]
        [JsonProperty("any")]
        public string AnyPath { get; set; }

        /// <summary>
        /// Путь к файлам PFT
        /// </summary>
        [CanBeNull]
        [XmlAttribute("pft")]
        [JsonProperty("pft")]
        public string PftPath { get; set; }

        /// <summary>
        /// Расположение внешних объектов (поле 951)
        /// </summary>
        /// <remarks>Параметр появился в версии 2012</remarks>
        [CanBeNull]
        [XmlAttribute("ext")]
        [JsonProperty("ext")]
        public string ExtPath { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор PAR-файла на строчки вида 1=.\datai\ibis.
        /// </summary>
        [NotNull]
        public static Dictionary<string, string> ReadDictionary
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            string line;
            char[] separator = { '=' };
            Dictionary<string, string> result
                = new Dictionary<string, string>(11);
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

#if !WINMOBILE && !PocketPC && !SILVERLIGHT

                string[] parts = line.Split(separator, 2);

#else

                // TODO Implement properly

                string[] parts = line.Split(separator);

#endif

                if (parts.Length != 2)
                {
                    throw new FormatException();
                }
                string key = parts[0];
                string value = parts[1];
                if (string.IsNullOrEmpty(key)
                    || string.IsNullOrEmpty(value))
                {
                    throw new FormatException();
                }
                key = key.Trim();
                value = value.Trim();
                if (string.IsNullOrEmpty(key)
                    || string.IsNullOrEmpty(value))
                {
                    throw new FormatException();
                }
                result.Add(key, value);
            }

            foreach (string key in Enumerable.Range(1, 10)

#if !WINMOBILE && !PocketPC

                .Select(NumericUtility.ToInvariantString)

#else

                .Select(n => n.ToInvariantString())

#endif

                )
            {
                if (!result.ContainsKey(key))
                {
                    throw new FormatException();
                }
            }

            return result;
        }

#if !WIN81

        /// <summary>
        /// Разбор файла.
        /// </summary>
        [NotNull]
        public static ParFile ParseFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (StreamReader reader = new StreamReader
                    (
                        File.OpenRead(fileName),
                        IrbisEncoding.Ansi
                    ))
            {
                return ParseText(reader);
            }
        }

#endif

        /// <summary>
        /// Разбор текста.
        /// </summary>
        [NotNull]
        public static ParFile ParseText
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Dictionary<string, string> dictionary
                = ReadDictionary(reader);

            ParFile result = new ParFile
            {
                XrfPath = dictionary["1"],
                MstPath = dictionary["2"],
                CntPath = dictionary["3"],
                N01Path = dictionary["4"],
                N02Path = dictionary["5"],
                L01Path = dictionary["6"],
                L02Path = dictionary["7"],
                IfpPath = dictionary["8"],
                AnyPath = dictionary["9"],
                PftPath = dictionary["10"]
            };
            if (dictionary.ContainsKey("11"))
            {
                result.ExtPath = dictionary["11"];
            }

            return result;
        }

        /// <summary>
        /// Преобразование в словарь.
        /// </summary>
        [NotNull]
        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> result
                = new Dictionary<string, string>(11)
                {
                    {"1", XrfPath},
                    {"2", MstPath},
                    {"3", CntPath},
                    {"4", N01Path},
                    {"5", N02Path},
                    {"6", L01Path},
                    {"7", L02Path},
                    {"8", IfpPath},
                    {"9", AnyPath},
                    {"10", PftPath},
                    {"11", ExtPath}
                };

            return result;
        }

#if !WIN81

        /// <summary>
        /// Запись в файл
        /// </summary>
        public void WriteFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (StreamWriter writer 
                = new StreamWriter
                    (
                        File.Create(fileName),
                        IrbisEncoding.Ansi
                    ))
            {
                WriteText(writer);
            }
        }

#endif

        /// <summary>
        /// Запись в поток.
        /// </summary>
        public void WriteText
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteLine("1={0}", XrfPath);
            writer.WriteLine("2={0}", MstPath);
            writer.WriteLine("3={0}", CntPath);
            writer.WriteLine("4={0}", N01Path);
            writer.WriteLine("5={0}", N02Path);
            writer.WriteLine("6={0}", L01Path);
            writer.WriteLine("7={0}", L02Path);
            writer.WriteLine("8={0}", IfpPath);
            writer.WriteLine("9={0}", AnyPath);
            writer.WriteLine("10={0}", PftPath);
            writer.WriteLine("11={0}", ExtPath);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            XrfPath = reader.ReadNullableString();
            MstPath = reader.ReadNullableString();
            CntPath = reader.ReadNullableString();
            N01Path = reader.ReadNullableString();
            N02Path = reader.ReadNullableString();
            L01Path = reader.ReadNullableString();
            L02Path = reader.ReadNullableString();
            IfpPath = reader.ReadNullableString();
            AnyPath = reader.ReadNullableString();
            PftPath = reader.ReadNullableString();
            ExtPath = reader.ReadNullableString();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(XrfPath)
                .WriteNullable(MstPath)
                .WriteNullable(CntPath)
                .WriteNullable(N01Path)
                .WriteNullable(N02Path)
                .WriteNullable(L01Path)
                .WriteNullable(L02Path)
                .WriteNullable(IfpPath)
                .WriteNullable(AnyPath)
                .WriteNullable(PftPath)
                .WriteNullable(ExtPath);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ParFile> verifier = new Verifier<ParFile>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(XrfPath, "XrfPath")
                .NotNullNorEmpty(MstPath, "MstPath")
                .NotNullNorEmpty(CntPath, "CntPath")
                .NotNullNorEmpty(N01Path, "N01Path")
                .NotNullNorEmpty(N02Path, "N02Path")
                .NotNullNorEmpty(L01Path, "L01Path")
                .NotNullNorEmpty(L02Path, "L02Path")
                .NotNullNorEmpty(IfpPath, "IfpPath")
                .NotNullNorEmpty(AnyPath, "AnyPath")
                .NotNullNorEmpty(PftPath, "PftPath");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return MstPath.ToVisibleString();
        }

        #endregion
    }
}

