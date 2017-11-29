// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WsFile.cs -- рабочий лист
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
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Worksheet
{
    /// <summary>
    /// Рабочий лист.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("worksheet")]
    public sealed class WsFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Имя рабочего листа.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Страницы рабочего листа.
        /// </summary>
        [NotNull]
        [XmlArray("pages")]
        [XmlArrayItem("page")]
        [JsonProperty("pages")]
        public NonNullCollection<WorksheetPage> Pages { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WsFile()
        {
            Pages = new NonNullCollection<WorksheetPage>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор потока.
        /// </summary>
        [NotNull]
        public static WsFile ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            WsFile result = new WsFile();

            int count = int.Parse(reader.RequireLine());

            Pair<string, int>[] pairs = new Pair<string, int>[count];

            for (int i = 0; i < count; i++)
            {
                string name = reader.ReadLine();
                pairs[i] = new Pair<string, int>(name);
            }
            for (int i = 0; i < count; i++)
            {
                string text = reader.ReadLine().ThrowIfNull("text");
                int length = int.Parse(text);
                pairs[i].Second = length;
            }

            for (int i = 0; i < count; i++)
            {
                string name = pairs[i].First.ThrowIfNull("name");
                WorksheetPage page = WorksheetPage.ParseStream
                    (
                        reader,
                        name,
                        pairs[i].Second
                    );
                result.Pages.Add(page);
            }

            return result;
        }

        /// <summary>
        /// Read from server.
        /// </summary>
        [CanBeNull]
        public static WsFile ReadFromServer
            (
                [NotNull] IrbisProvider provider,
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(specification, "specification");

            string content = provider.ReadFile(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            WsFile result;

            using (StringReader reader = new StringReader(content))
            {
                result = ParseStream(reader);
                result.Name = specification.FileName;
            }

            for (int i = 0; i < result.Pages.Count; )
            {
                WorksheetPage page = result.Pages[i];
                string name = page.Name.ThrowIfNull("page.Name");
                if (name.StartsWith("@"))
                {
                    string extension = Path.GetExtension(specification.FileName);
                    FileSpecification nestedSpecification = new FileSpecification
                        (
                            specification.Path,
                            specification.Database,
                            name.Substring(1) + extension
                        );
                    WsFile nestedFile = ReadFromServer
                        (
                            provider,
                            nestedSpecification
                        );
                    if (ReferenceEquals(nestedFile, null))
                    {
                        // TODO: somehow report error
                        i++;
                    }
                    else
                    {
                        result.Pages.RemoveAt(i);
                        for (int j = 0; j < nestedFile.Pages.Count; j++)
                        {
                            result.Pages.Insert
                                (
                                    i + j,
                                    nestedFile.Pages[j]
                                );
                        }
                    }
                }
                else
                {
                    i++;
                }
            }

            return result;
        }

#if !WIN81 && !PORTABLE

        /// <summary>
        /// Fixup nested worksheets for local file.
        /// </summary>
        [NotNull]
        public static WsFile FixupLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding,
                [NotNull] WsFile wsFile
            )
        {
            Code.NotNull(fileName, "fileName");
            Code.NotNull(encoding, "encoding");
            Code.NotNull(wsFile, "wsFile");

            for (int i = 0; i < wsFile.Pages.Count; )
            {
                WorksheetPage page = wsFile.Pages[i];
                string name = page.Name.ThrowIfNull("page.Name");
                if (name.StartsWith("@"))
                {
                    string directory = Path.GetDirectoryName(fileName)
                        ?? string.Empty;
                    string extension = Path.GetExtension(fileName);
                    string nestedName = Path.Combine
                        (
                            directory,
                            name.Substring(1) + extension
                        );
                    WsFile nestedFile = ReadLocalFile
                        (
                            nestedName,
                            encoding
                        );
                    wsFile.Pages.RemoveAt(i);
                    for (int j = 0; j < nestedFile.Pages.Count; j++)
                    {
                        wsFile.Pages.Insert
                            (
                                i + j,
                                nestedFile.Pages[j]
                            );
                    }
                }
                else
                {
                    i++;
                }
            }

            return wsFile;
        }

        /// <summary>
        /// Считывание из локального файла.
        /// </summary>
        [NotNull]
        public static WsFile ReadLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            WsFile result;

            using (StreamReader reader = TextReaderUtility.OpenRead
                (
                    fileName,
                    encoding
                ))
            {
                result = ParseStream(reader);
                result.Name = Path.GetFileName(fileName);
            }

            return result;
        }

        /// <summary>
        /// Считывание из локального файла.
        /// </summary>
        [NotNull]
        public static WsFile ReadLocalFile
            (
                [NotNull] string fileName
            )
        {
            return ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
        }

#endif

        /// <summary>
        /// Should serialize the <see cref="Pages"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializePages()
        {
            return Pages.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            Pages = reader.ReadNonNullCollection<WorksheetPage>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            writer.Write(Pages);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<WsFile> verifier = new Verifier<WsFile>(this, throwOnError);

            foreach (WorksheetPage page in Pages)
            {
                verifier.VerifySubObject(page, "page");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
